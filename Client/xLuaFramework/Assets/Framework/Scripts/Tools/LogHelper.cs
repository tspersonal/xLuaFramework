using System;
using System.IO;
using UnityEngine;

[XLua.ReflectionUse]
public class LogHelper : ExtendMonoBehaviour
{
    //是否退出程序当异常发生时
    public static bool IsQuitWhenException = false;
    //日志保存路径（文件夹）
    private string _logFolderPath;
    //日志保存路径（文件）
    private string _logFilePath;
    //当前程序运行的时间 yyyy_MM_dd
    private string _sTodayTime;
    //当前程序运行的时间 yyyy_MM_dd_HH_mm_ss
    private string _sNowTime;

    public override void DoAwake()
    {
        base.DoAwake();
        DateTime now = DateTime.Now;
        _sTodayTime = now.ToString("yyyy_MM_dd");
        _sNowTime = now.ToString("yyyy_MM_dd_HH_mm_ss");
        _logFolderPath = PathUtil.GetFilesOutPath() + "/" + "Debug";
        _logFilePath = _logFolderPath + "/" + _sNowTime + ".txt";

        if (LocalData.DateToday == "")
        {
            LocalData.DateToday = _sTodayTime;

            if (Directory.Exists(_logFolderPath))
            {
                DirectoryInfo dir = new DirectoryInfo(_logFolderPath);
                dir.Delete(true);
            }
            Directory.CreateDirectory(_logFolderPath);

        }
        else
        {
            if (LocalData.DateToday != _sTodayTime)
            {
                LocalData.DateToday = _sTodayTime;
                if (Directory.Exists(_logFolderPath))
                {
                    DirectoryInfo dir = new DirectoryInfo(_logFolderPath);
                    dir.Delete(true);
                }
                Directory.CreateDirectory(_logFolderPath);
            }
        }
        if (!File.Exists(_logFilePath))
        {
            File.Create(_logFilePath).Dispose();
        }
    }

    protected override void DoAddListener()
    {
        base.DoAddListener();
        //添加unity日志监听
        Application.logMessageReceivedThreaded += Handler;
    }

    protected override void DoRemoveListener()
    {
        base.DoRemoveListener();
        //清除注册
        Application.logMessageReceivedThreaded -= Handler;
    }

    public override void DoClearData()
    {
        base.DoClearData();
    }

    void Handler(string logString, string stackTrace, LogType type)
    {
        FileStream fs = new FileStream(_logFilePath, FileMode.Append);
        StreamWriter sw = new StreamWriter(fs);
        if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
        {
            sw.WriteLine("[time]:" + _sNowTime);
            sw.WriteLine("[type]:" + ((LogType)type).ToString());
            sw.WriteLine("[exception message]:" + logString);
            sw.WriteLine("[stack trace]:" + stackTrace);
            sw.Close();
            fs.Close();
            //退出程序，bug反馈程序重启主程序
            if (IsQuitWhenException)
            {
                Application.Quit();
            }
        }
        else
        {
            sw.WriteLine(logString);
            sw.Close();
            fs.Close();
        }
    }
}



