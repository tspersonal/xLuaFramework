using System.Collections;
using Debuger;
using LitJson;
using UnityEngine;

[XLua.ReflectionUse]
public class GetServerInfo : ExtendMonoBehaviour
{
    public enum VersionType
    {
        Beta,
        Online,
    }

    [SerializeField]
    private VersionType _versionType;

    public override void DoStart()
    {
        base.DoStart();
        StartCoroutine(GetServerConfig());
    }

    IEnumerator GetServerConfig()
    {
        string url = "";
        string version = Application.version;
        if (_versionType == VersionType.Beta)
        {
            version = "9.0.0";
            url = "http://game2.youthgamer.com:93/ClientQuery.aspx?method=QueryServerAddress&gameId={0}&clientVersion={1}";
        }
        else if (_versionType == VersionType.Online)
        {
            url = "http://suoha.renrenhui88.cn:93/ClientQuery.aspx?method=QueryServerAddress&gameId={0}&clientVersion={1}";
        }
        url = string.Format(url, "suoha", version);
        DebugerHelper.Log("域名: " + url, DebugerHelper.LevelType.Debug);
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            DebugerHelper.Log("服务器配置: " + www.text, DebugerHelper.LevelType.Info);
            JsonData jsonData = JsonMapper.ToObject(www.text);
            if (jsonData["GateServers"] == null || jsonData["GateServers"].Count == 0)
            {
                DebugerHelper.Log("链接服务器失败，配置数据为空", DebugerHelper.LevelType.Except);
                InitGame.Instance.SetProgress("链接服务器失败，请稍后重试", 0, false);
                InitGame.Instance.SetColor(LocalData.ColRed, LocalData.ColGrey);
                yield break;
            }
            else
            {
                InitGame.Instance.SetProgress("正在链接服务器", 0, true);
            }
            if (jsonData["WebResultCode"].ToString() == "0")
            {
                string jsonString = jsonData["GateServers"][0].ToJson();
                ServerInfo.Data = JsonUtility.FromJson<ServerInfo>(jsonString);
                if (ServerInfo.Data.ServerVersionCode == version)
                {
                    InitGame.Instance.StartGame();
                }
                else
                {
                    GameObject preObj = Resources.Load<GameObject>("UpdateVersion");
                    GameObject obj = Instantiate(preObj);
                    obj.transform.parent = GameObject.Find("2DUIRoot").transform;
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = Vector3.zero;
                }
            }
        }
        else
        {
            DebugerHelper.Log("下载服务器配置异常: " + www.error, DebugerHelper.LevelType.Except);
            InitGame.Instance.SetProgress("链接服务器失败，请稍后重试", 0, false);
            InitGame.Instance.SetColor(LocalData.ColRed, LocalData.ColGrey);
        }
        yield break;
    }
}
