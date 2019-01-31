using Debuger;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[XLua.ReflectionUse]
public class InitGame : SingletonMonoBehaviour<InitGame>
{
    public enum LoadAssetsType
    {
        //电脑端Resources加载
        Resources = 0,
        //AssetBundle加载
        AssetBundle,
    }

    //文件中一行中的数据
    public class DownLoadAssetInfo
    {
        //资源名
        public string SAssetName;
        //资源的MD5码
        public string SMd5;
        //资源大小
        public long LLength;
        //资源占比
        public float FPercent;
    }

    [SerializeField]
    private LoadAssetsType _loadAssetsType;
    public UILabel LabContent;
    public UILabel LabTween;
    public UILabel LabDownLoadLength;
    public UILabel LabDownLoadVelocity;
    public UIProgressBar SliLoading;

    private Coroutine _coroutineTween;
    private Coroutine _coroutineDownLoadProgress;//下载进度的协程
    private string _sDownloadPath;//下载路径
    private long _lTotalAssetsLength = 0;//需要下载的资源总大小
    private long _lCurrAssetsLength = 0;//当前下载的资源大小
    private long _lExistAssetsLength = 0;//已经存在的资源大小
    private bool _bUpdateSucceed = false;//是否更新完成

    public override void DoAwake()
    {
        base.DoAwake();
        _sDownloadPath = PathUtil.GetAssetBundleOutPath();
        GameSetting();
    }

    public override void DoOnEnable()
    {
        base.DoOnEnable();
        LabContent.gameObject.SetActive(false);
        LabTween.gameObject.SetActive(false);
        SliLoading.gameObject.SetActive(false);
        SetDownLoadLength(false);
        SetDownLoadVelocity(false);
    }

    /// <summary>
    /// 游戏设置
    /// </summary>
    private void GameSetting()
    {
        //设置加载方式
        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            _loadAssetsType = LoadAssetsType.AssetBundle;
        }
        LocalData.LoadAssetsType = (int)_loadAssetsType;
        //游戏不休眠
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //后台运行
        Application.runInBackground = true;
        //设置60
        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// 下载IP等服务器信息完毕
    /// </summary>
    public void StartGame()
    {
        SetProgress("正在检测资源更新", 0, true);
        //是否处于审核
        if (ServerInfo.Data.OnAppleCheck)
        {
            CreateGameManager();
            return;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor && LocalData.LoadAssetsType == (int)InitGame.LoadAssetsType.Resources)
            CreateGameManager();
        else
            StartCoroutine(IeCompareAssets());
    }

    private void CreateGameManager()
    {
        GameObject goGameManager = Resources.Load<GameObject>("GameManager");
        GameObject objGameManager = Instantiate(goGameManager);
        objGameManager.transform.localScale = GameObject.Find("2DUIRoot").transform.localScale;
        objGameManager.name = goGameManager.name;
        DontDestroyOnLoad(objGameManager);
    }

    /// <summary>
    /// 对比资源 文件相等：不处理，不存在：下载，不相等：下载，多余：删除
    /// </summary>
    IEnumerator IeCompareAssets()
    {
        //string sUrl = ServerInfo.Data.DownLoadResourceUrl + "/" + PathUtil.GetPlatformName() + "/";
        string sUrl = "http://game2.youthgamer.com:92/UpLoadBinarys/QQDP/" + PathUtil.GetPlatformName() + "/";
        string sFilesUrl = sUrl + "files.txt";
        DebugerHelper.Log(sFilesUrl, DebugerHelper.LevelType.Critical);

        #region 1.下载资源记录
        //1.下载资源记录
        WWW www = new WWW(sFilesUrl);
        yield return www;
        if (www.error != null)
        {
            string sError = "下载文件失败: " + www.error;
            DebugerHelper.Log(sError, DebugerHelper.LevelType.Except);
            SetProgress(sError, 0, false);
            SetColor(LocalData.ColRed, LocalData.ColGrey);
            yield break;
        }
        //创建本地AB目录
        if (!Directory.Exists(_sDownloadPath))
        {
            Directory.CreateDirectory(_sDownloadPath);
        }
        byte[] arrFiles = www.bytes;
        #endregion

        #region 2.对比本地资源记录
        //2.对比本地资源记录
        string sFilesPath = _sDownloadPath + "/files.txt";
        List<string> listLocalInfo = new List<string>();
        Dictionary<string, DownLoadAssetInfo> dicLocalInfo = new Dictionary<string, DownLoadAssetInfo>();
        if (!File.Exists(sFilesPath))
        {
            //直接下载
            File.WriteAllBytes(sFilesPath, arrFiles);
        }
        else
        {
            //进行下载、更新、删除
            string sLocalContent = File.ReadAllText(sFilesPath);
            string[] arrLocalAssets = sLocalContent.Split('\n');
            for (var i = 0; i < arrLocalAssets.Length; i++)
            {
                string[] strs = arrLocalAssets[i].Split('|');
                DownLoadAssetInfo info = new DownLoadAssetInfo();
                info.SAssetName = strs[0];
                info.SMd5 = strs[1];
                info.LLength = long.Parse(strs[2]);
                //info.FPercent = float.Parse(strs[3]);
                listLocalInfo.Add(info.SAssetName);
                dicLocalInfo.Add(info.SAssetName, info);
            }
        }

        //读取服务器资源记录的内容
        string sContent = www.text;
        string[] arrAssets = sContent.Split('\n');
        List<string> listServerInfo = new List<string>();
        Dictionary<string, DownLoadAssetInfo> dicServerInfo = new Dictionary<string, DownLoadAssetInfo>();
        for (var i = 0; i < arrAssets.Length; i++)
        {
            string[] strs = arrAssets[i].Split('|');
            DownLoadAssetInfo info = new DownLoadAssetInfo();
            info.SAssetName = strs[0];
            info.SMd5 = strs[1];
            info.LLength = long.Parse(strs[2]);
            //info.FPercent = float.Parse(strs[3]);
            listServerInfo.Add(info.SAssetName);
            dicServerInfo.Add(info.SAssetName, info);
        }

        List<DownLoadAssetInfo> listDelete = new List<DownLoadAssetInfo>();//需要删除本地资源
        List<DownLoadAssetInfo> listUpdate = new List<DownLoadAssetInfo>();//需要更新服务器资源

        //先筛选出需要更新、删除的资源
        for (int i = 0; i < listLocalInfo.Count; i++)
        {
            string sAssetName = listLocalInfo[i];
            DownLoadAssetInfo localInfo = dicLocalInfo[sAssetName];
            if (dicServerInfo.ContainsKey(sAssetName))
            {
                string localPath = (_sDownloadPath + "/" + sAssetName).Trim();
                if (File.Exists(localPath))
                {
                    string localMd5 = ToolsFunc.GetMd5HashFromFile(localPath);
                    DownLoadAssetInfo updateInfo = dicServerInfo[sAssetName];
                    string serverMd5 = updateInfo.SMd5;
                    if (serverMd5 == localMd5)
                    {
                        //无需更新
                        listServerInfo.Remove(sAssetName);
                        dicServerInfo.Remove(sAssetName);
                    }
                    else
                    {
                        //需要更新
                        listDelete.Add(updateInfo);
                    }
                }
                else
                {
                    //需要下载
                }
            }
            else
            {
                //需要删除
                listDelete.Add(localInfo);
            }
        }
        //剩下的都是需要创建的资源
        for (int i = 0; i < listServerInfo.Count; i++)
        {
            DownLoadAssetInfo createInfo = dicServerInfo[listServerInfo[i]];
            listUpdate.Add(createInfo);
        }
        #endregion

        #region 3.删除无效资源
        //3.删除无效资源
        for (int i = 0; i < listDelete.Count; i++)
        {
            DownLoadAssetInfo deleteInfo = listDelete[i];
            string sDeletePath = (_sDownloadPath + "/" + deleteInfo.SAssetName).Trim();
            if (File.Exists(sDeletePath))
            {
                File.Delete(sDeletePath);
                DebugerHelper.Log("删除无效资源: " + deleteInfo.SAssetName, DebugerHelper.LevelType.Warning);
            }
            //SetProgress("删除无效资源中", (float)i / listDelete.Count, false);
        }
        #endregion

        #region 4.更新服务器资源
        //4.更新服务器资源
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        //计算需要下载的资源总大小
        _lTotalAssetsLength = 0;
        for (int i = 0; i < listUpdate.Count; i++)
        {
            _lTotalAssetsLength += listUpdate[i].LLength;
        }
        //计算每一个资源的比例
        for (int i = 0; i < listUpdate.Count; i++)
        {
            listUpdate[i].FPercent = (float)listUpdate[i].LLength / _lTotalAssetsLength;
        }
        //开始下载资源
        float fDownLoadPercent = 0f;
        //计算当前下载的资源大小、已经存在的资源大小
        _lCurrAssetsLength = 0;
        _lExistAssetsLength = 0;
        //设置下载的进度
        _bUpdateSucceed = false;
        _coroutineDownLoadProgress = StartCoroutine(IeSetDownLoadProgress());
        for (int i = 0; i < listUpdate.Count; i++)
        {
            DownLoadAssetInfo updateInfo = listUpdate[i];
            string sUpdatePath = (_sDownloadPath + "/" + updateInfo.SAssetName).Trim();
            //再根据本地文件加重判断
            bool bUpdate = true;
            if (File.Exists(sUpdatePath))
            {
                string localMd5 = ToolsFunc.GetMd5HashFromFile(sUpdatePath);
                if (localMd5 == updateInfo.SMd5)
                {
                    bUpdate = false;
                }
                else
                {
                    DebugerHelper.Log("强制更新，文件【" + updateInfo.SAssetName + "】MD5不匹配", DebugerHelper.LevelType.Normal);
                    File.Delete(sUpdatePath);
                    bUpdate = true;
                }
            }
            else
            {
                DebugerHelper.Log("正常更新，文件【" + updateInfo.SAssetName + "】不存在", DebugerHelper.LevelType.Normal);
                bUpdate = true;
            }
            //是否需要下载
            if (bUpdate)
            {
                string dir = Path.GetDirectoryName(sUpdatePath);
                if (dir != null) Directory.CreateDirectory(dir);
                //开始网络下载
                string sUpdateUrl = sUrl + updateInfo.SAssetName;
                //DebugerHelper.Log(sUpdateUrl, DebugerHelper.LevelType.Normal);
                www = new WWW(sUpdateUrl);
                //记录下载此资源之前的总下载量
                long lCurrLength = _lCurrAssetsLength;
                //记录下载此资源之前的总下载百分比
                float fCurrPercent = fDownLoadPercent;
                while (!www.isDone)
                {
                    yield return wait;
                    //计算当前下载量
                    long lLength = (long)(updateInfo.LLength * www.progress);
                    _lCurrAssetsLength = lCurrLength + lLength;
                    //计算当前进度
                    float fProgress = updateInfo.FPercent * www.progress;
                    fDownLoadPercent = fCurrPercent + fProgress;
                    SetProgress("资源更新中", fDownLoadPercent, false);
                }
                if (www.error != null)
                {
                    string sError = "下载资源文件失败: " + www.error;
                    DebugerHelper.Log(sError, DebugerHelper.LevelType.Except);
                    SetProgress(sError, 0, false);
                    SetColor(LocalData.ColRed, LocalData.ColGrey);
                    if (_coroutineDownLoadProgress != null)
                        StopCoroutine(_coroutineDownLoadProgress);
                    SetDownLoadLength(false);
                    SetDownLoadVelocity(false);
                    yield break;
                }
                else
                {
                    DebugerHelper.Log(
                        "下载资源文件成功(" + ToolsFunc.GetBytesLimitLength(updateInfo.LLength, 4) + "，" + (updateInfo.FPercent * 100).ToString("0.00") + "%): " + updateInfo.SAssetName,
                        DebugerHelper.LevelType.Debug);
                    _lCurrAssetsLength = lCurrLength + updateInfo.LLength;
                    fDownLoadPercent = fCurrPercent + updateInfo.FPercent;
                    SetProgress("资源更新中", fDownLoadPercent, false);
                    File.WriteAllBytes(sUpdatePath, www.bytes);
                }
            }
            else
            {
                DebugerHelper.Log(
                    "文件已存在(" + ToolsFunc.GetBytesLimitLength(updateInfo.LLength, 4) + "，" + (updateInfo.FPercent * 100).ToString("0.00") + "%): " + updateInfo.SAssetName,
                    DebugerHelper.LevelType.Warning);
                _lCurrAssetsLength += updateInfo.LLength;
                _lExistAssetsLength += updateInfo.LLength;
                fDownLoadPercent += updateInfo.FPercent;
                SetProgress("资源更新中", fDownLoadPercent, false);
            }
        }
        SetProgress("资源更新中", 1, false);
        yield return new WaitForEndOfFrame();
        #endregion

        #region 5.开始游戏
        //5.开始游戏
        DebugerHelper.Log("更新完成，开始游戏", DebugerHelper.LevelType.Critical);
        _bUpdateSucceed = true;
        SetProgress("更新完成", 1, false);
        if (_coroutineDownLoadProgress != null)
            StopCoroutine(_coroutineDownLoadProgress);
        SetDownLoadLength(false);
        SetDownLoadVelocity(false);
        File.WriteAllBytes(sFilesPath, arrFiles);
        CreateGameManager();
        #endregion
    }

    /// <summary>
    /// 设置下载的进度 包括速度与下载量
    /// </summary>
    /// <returns></returns>
    IEnumerator IeSetDownLoadProgress()
    {
        SetDownLoadLength(true, _lCurrAssetsLength, _lTotalAssetsLength);
        SetDownLoadVelocity(true);
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        int nWaitCount = 0;
        long lBeforeRealityDownLoadLength = 0;
        while (!_bUpdateSucceed)
        {
            if (nWaitCount == 0)
            {
                //1s前的实际下载量
                lBeforeRealityDownLoadLength = _lCurrAssetsLength - _lExistAssetsLength;
            }
            yield return wait;
            //每0.1s设置一次 当前下载量
            SetDownLoadLength(true, _lCurrAssetsLength, _lTotalAssetsLength);
            //每过1s设置一次当前1s区间之内的下载速度 1s内的下载速度 = 1s内的实际下载量 = 1s后的实际下载量 - 1s前的实际下载量 = (1s之后的下载量 - 1s后的已存在资源大小) - (1s之前的下载量 - 1s之前已存在的资源大小)
            nWaitCount++;
            if (nWaitCount >= 10)
            {
                //1s后的实际下载量
                long lAfterRealityDownLoadLength = _lCurrAssetsLength - _lExistAssetsLength;
                long lRealityDownLoadLength = lAfterRealityDownLoadLength - lBeforeRealityDownLoadLength;
                //string sTemp = "1s前的实际下载量: {0}, 1s后的实际下载量: {1}";
                //Debuger.Log(string.Format(sTemp, lBeforeRealityDownLoadLength, lRealityDownLoadLength),
                //    Debuger.LevelType.Info);
                SetDownLoadVelocity(true, lRealityDownLoadLength, nWaitCount * 0.1f);
                nWaitCount = 0;
            }
        }
        SetDownLoadLength(false);
        SetDownLoadVelocity(false);
    }

    /// <summary>
    /// 设置进度条
    /// </summary>
    /// <param name="sContent">文字内容</param>
    /// <param name="fProress">进度</param>
    /// <param name="bTween">是否需要动画</param>
    public void SetProgress(string sContent, float fProress, bool bTween)
    {
        LabContent.gameObject.SetActive(true);
        SliLoading.gameObject.SetActive(true);
        SliLoading.value = fProress;
        if (bTween)
        {
            LabTween.gameObject.SetActive(true);
            LabContent.text = sContent;
            _coroutineTween = StartCoroutine(PlayProgressTween());
        }
        else
        {
            if (_coroutineTween != null)
                StopCoroutine(_coroutineTween);
            LabTween.gameObject.SetActive(false);
            if (fProress > 0)
                LabContent.text = sContent + "..." + (fProress * 100).ToString("0.0") + "%";
            else
                LabContent.text = sContent;
        }
    }

    /// <summary>
    /// 设置下载速度 fLength单位：b    fTime单位：秒s
    /// </summary>
    public void SetDownLoadVelocity(bool bOpen, long lLength = 0, float fTime = 1)
    {
        if (bOpen)
        {
            if (!LabDownLoadVelocity.gameObject.activeSelf)
                LabDownLoadVelocity.gameObject.SetActive(true);

            float fVelocity = lLength / fTime;
            string sVelocity = ToolsFunc.GetBytesLimitLength(fVelocity, 4);
            LabDownLoadVelocity.text = sVelocity + " /s";
            //Debuger.Log(">>>>>下载流量为: " + lLength + ", 下载时间为: " + fTime + ", 下载速度为: " + sVelocity + " /s", Debuger.LevelType.Info);
        }
        else
        {
            if (LabDownLoadVelocity.gameObject.activeSelf)
                LabDownLoadVelocity.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置下载大小 单位：b
    /// </summary>
    public void SetDownLoadLength(bool bOpen, long lLength = 0, long lTotalLength = 0)
    {
        if (bOpen)
        {
            if (!LabDownLoadLength.gameObject.activeSelf)
                LabDownLoadLength.gameObject.SetActive(true);
            string sLength = ToolsFunc.GetBytesLimitLength(lLength, 4);
            string sTotalLength = ToolsFunc.GetBytesLimitLength(lTotalLength, 4);
            LabDownLoadLength.text = sLength + "/" + sTotalLength;
            //Debuger.Log("*****下载流量为: " + sLength + ", 总大小为: " + sTotalLength, Debuger.LevelType.Normal);
        }
        else
        {
            if (LabDownLoadLength.gameObject.activeSelf)
                LabDownLoadLength.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置文本...的动画
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayProgressTween()
    {
        int type = 0;
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (true)
        {
            if (type == 0)
            {
                LabTween.text = "";
                type = 1;
            }
            else if (type == 1)
            {
                LabTween.text = ".";
                type = 2;
            }
            else if (type == 2)
            {
                LabTween.text = "..";
                type = 3;
            }
            else if (type == 3)
            {
                LabTween.text = "...";
                type = 0;
            }
            yield return wait;
        }
    }

    /// <summary>
    /// 设置加载界面的进度颜色 
    /// </summary>
    public void SetColor(Color foreColor, Color backColor)
    {
        LabContent.color = foreColor;
        LabTween.color = foreColor;
        SliLoading.foregroundWidget.color = foreColor;
        SliLoading.backgroundWidget.color = backColor;
    }
}
