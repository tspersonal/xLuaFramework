using System.Collections;
using Debuger;
using UnityEngine;

[XLua.ReflectionUse]
public class YzResultHelper : MonoBehaviour
{
    //返回相册路径
    public void onImagePath(string msg)
    {
        DebugerHelper.Log("图像保存成功", DebugerHelper.LevelType.Normal);
        StartCoroutine(LoadImage(msg));
    }

    IEnumerator LoadImage(string imageName)
    {
        WWW www = new WWW("file://" + Application.persistentDataPath + "/image.jpg");
        yield return www;
        if (www.error == null)
        {
            DebugerHelper.Log("读取头像成功", DebugerHelper.LevelType.Normal);
            byte[] bytes = www.texture.EncodeToJPG();
            WWWForm form = new WWWForm();
            form.AddField("path", "CustomHead");
            form.AddField("id", imageName);
            form.AddBinaryData("Photo", bytes, "photo.jpg");

            WWW uploadWWW = new WWW(ServerInfo.Data.UpLoadPictureUrl, form);
            yield return uploadWWW;
            if (uploadWWW.error == null)
            {
                DebugerHelper.Log("上传头像完成", DebugerHelper.LevelType.Debug);
                if (YzHelper.photoLuaFunction != null)
                {
                    DebugerHelper.Log("图像传回", DebugerHelper.LevelType.Debug);
                    YzHelper.photoLuaFunction.Call(www.texture);
                }
            }
            else
            {
                DebugerHelper.Log(uploadWWW.error, DebugerHelper.LevelType.Error);
            }
        }
    }
}
