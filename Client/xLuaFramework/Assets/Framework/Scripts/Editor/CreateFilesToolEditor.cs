using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CreateFilesToolEditor
{
    [MenuItem("Tools/Create Files Tool/Android")]
    public static void CreateFilesAndroid()
    {
        string path = Application.dataPath + "/AssetBundleAssets/Android";
        CreateFiles(path);
        Debug.Log("Create Files Android 生成完成");
    }

    [MenuItem("Tools/Create Files Tool/Iphone")]
    public static void CreateFilesIos()
    {
        string path = Application.dataPath + "/AssetBundleAssets/Iphone";
        CreateFiles(path);
        Debug.Log("Create Files Iphone 生成完成");
    }

    [MenuItem("Tools/Create Files Tool/Windows")]
    public static void CreateFilesWindows()
    {
        string path = Application.dataPath + "/AssetBundleAssets/Windows";
        CreateFiles(path);
        Debug.Log("Create Files Windows 生成完成");
    }

    [MenuItem("Tools/Create Files Tool/Lua To Txt")]
    public static void LuaToTxt()
    {
        string path = Application.dataPath + "/Lua";

        ChangeFileEx(new DirectoryInfo(path));

        AssetDatabase.Refresh();
        Debug.Log("LuaToTxt 完成");
    }

    //生成版本对比文件
    private static void CreateFiles(string path)
    {
        CopyLuaFiles(Application.dataPath + "/Lua", path + "/Lua");
        string outPath = path;//PathUtil.GetAssetBundleOutPath();
        //校验文件的路径
        string filePath = outPath + "/files.txt";
        if (File.Exists(filePath))
            File.Delete(filePath);

        //遍历这个文件夹下面的所有文件 
        List<string> fileList = new List<string>();
        TraverseFiles(new DirectoryInfo(outPath), ref fileList);

        //获取所有文件中的有效文件
        List<string> validFileList = new List<string>();
        for (int i = 0; i < fileList.Count; i++)
        {
            string file = fileList[i];
            string ext = Path.GetExtension(file);
            if (ext.EndsWith(".meta") || ext.EndsWith(".ab.manifest"))
                continue;

            validFileList.Add(file);
        }

        //开始写入文件
        FileStream fs = new FileStream(filePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        string sFile = "";
        string sLastLine = "";

        for (int i = 0; i < validFileList.Count; i++)
        {
            string file = validFileList[i];
            //            string ext = Path.GetExtension(file);
            //            if (ext.EndsWith(".meta") || ext.EndsWith(".ab.manifest"))
            //               continue;
            FileInfo fileInfo = new FileInfo(file);
            long lLength = fileInfo.Length;
            //生成这个文件对应的md5值 
            string md5 = GetFileMd5(file);
            //scene1.assetbundle
            string value = file.Replace(outPath + "/", string.Empty);
            //记录写入到文件的数据
            if (i == validFileList.Count - 1)
                sFile += (value + "|" + md5 + "|" + lLength);
            else
                sFile += (value + "|" + md5 + "|" + lLength + "\r\n");

            //if (i == fileList.Count - 1)
            //    sFile += (value + "|" + md5 + string.Empty);
            //else
            //    sFile += (value + "|" + md5 + "\r\n");

            //sw.WriteLine(value + "|" + md5);
        }
        sw.Write(sFile);
        sw.Close();
        fs.Close();

        AssetDatabase.Refresh();
    }

    //拷贝Lua文件
    private static void CopyLuaFiles(string fromPath, string toPath)
    {
        CopyDir(fromPath, toPath);
        AssetDatabase.Refresh();
    }

    //拷贝文件夹
    private static void CopyDir(string srcPath, string aimPath)
    {
        try
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加
            if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
            {
                aimPath += System.IO.Path.DirectorySeparatorChar;
            }
            // 判断目标目录是否存在如果不存在则新建
            if (!System.IO.Directory.Exists(aimPath))
            {
                System.IO.Directory.CreateDirectory(aimPath);
            }
            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
            // string[] fileList = Directory.GetFiles（srcPath）；
            string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
            // 遍历所有的文件和目录
            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                if (System.IO.Directory.Exists(file))
                {
                    CopyDir(file, aimPath + System.IO.Path.GetFileName(file));
                }
                // 否则直接Copy文件
                else
                {
                    System.IO.File.Copy(file, aimPath + System.IO.Path.GetFileName(file), true);
                }
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <summary>
    /// 遍历文件夹下的所有文件
    /// </summary>
    /// <param name="fileSystemInfo">文件夹的路径</param>
    /// <param name="fileList"></param>
    private static void TraverseFiles(FileSystemInfo fileSystemInfo, ref List<string> fileList)
    {
        DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
        //获取所有的文件系统
        if (directoryInfo != null)
        {
            FileSystemInfo[] infos = directoryInfo.GetFileSystemInfos();

            foreach (var info in infos)
            {
                FileInfo fileInfo = info as FileInfo;
                //如果是文件 就成功了
                if (fileInfo != null)
                {
                    fileList.Add(fileInfo.FullName.Replace("\\", "/"));
                }
                //如果是文件夹就不成功 null
                else
                {
                    //递归
                    TraverseFiles(info, ref fileList);
                }
            }
        }
        else
        {
            Debug.LogError("获取文件系统错误");
        }
    }

    /// <summary>
    /// 修改文件后缀
    /// </summary>
    /// <param name="fileSystemInfo"></param>
    private static void ChangeFileEx(FileSystemInfo fileSystemInfo)
    {
        DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
        //获取所有的文件系统
        if (directoryInfo != null)
        {
            FileSystemInfo[] infos = directoryInfo.GetFileSystemInfos();

            foreach (var info in infos)
            {
                FileInfo fileInfo = info as FileInfo;
                //如果是文件 就成功了
                if (fileInfo != null)
                {
                    //fileList.Add(fileInfo.FullName.Replace("\\", "/"));
                    string fileName = fileInfo.FullName.Replace("\\", "/");
                    if (Path.GetExtension(fileName).EndsWith(".lua"))
                    {
                        string changeName = Path.ChangeExtension(fileName, ".lua.txt");
                        //string newFileName = fileInfo.Name.Replace(".lua",".txt");
                        //string newDirectoryName = Path.Combine(fileName, newFileName);
                        fileInfo.MoveTo(changeName);
                    }
                    else if (Path.GetExtension(fileName).EndsWith(".txt"))
                    {
                        if (!fileName.Contains(".lua.txt"))
                        {
                            string changeName = Path.ChangeExtension(fileName, ".lua.txt");
                            fileInfo.MoveTo(changeName);
                        }
                    }
                }
                //如果是文件夹就不成功 null
                else
                {
                    //递归
                    ChangeFileEx(info);
                }
            }
        }
    }

    /// <summary>
    /// 获取文件的md5值
    /// </summary>
    /// <param name="filePath"></param>
    private static string GetFileMd5(string filePath)
    {
        FileStream fs = new FileStream(filePath, FileMode.Open);
        MD5 md5 = new MD5CryptoServiceProvider();

        byte[] result = md5.ComputeHash(fs);
        fs.Close();

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < result.Length; i++)
        {
            sb.Append(result[i].ToString("x2"));
        }
        return sb.ToString();
    }
}
