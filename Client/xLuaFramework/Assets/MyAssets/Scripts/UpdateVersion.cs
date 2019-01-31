using UnityEngine;

[XLua.ReflectionUse]
public class UpdateVersion : UIBaseSingleton<UpdateVersion>
{
    public UILabel LabDesc;
    public GameObject BtnConfirm;
    public GameObject BtnCancel;

    protected override void DoRegister()
    {
        base.DoRegister();
        UIEventListener.Get(BtnConfirm).onClick = OnClickBtn;
        UIEventListener.Get(BtnCancel).onClick = OnClickBtn;
    }

    public override void DoOnEnable()
    {
        base.DoOnEnable();
        var updateMessage = ServerInfo.Data.ServerUpdateMessage.Split('@');
        LabDesc.text = "最新版本: " +
                    ServerInfo.Data.ServerVersionCode +
                    "，当前版本: " + Application.version + "\n" + updateMessage[0];
    }

    private void OnClickBtn(GameObject go)
    {
        if (go.name == BtnConfirm.name)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                Application.OpenURL(ServerInfo.Data.UpdateIosUrl);
            else if (Application.platform == RuntimePlatform.Android)
                Application.OpenURL(ServerInfo.Data.UpdateAndroidUrl);
            else
                Application.OpenURL(ServerInfo.Data.UpdateAndroidUrl);

        }
        else if (go.name == BtnCancel.name)
        {
            Application.Quit();
        }
    }
}
