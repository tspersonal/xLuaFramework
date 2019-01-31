using UnityEngine;

[XLua.ReflectionUse]
public class LocalData
{
    #region 游戏配置
    //游戏的加载方式
    public static int LoadAssetsType
    {
        get { return PlayerPrefs.GetInt("LoadAssetsType", 1); }
        set { PlayerPrefs.SetInt("LoadAssetsType", value); }
    }
    //今天的日期
    public static string DateToday
    {
        get { return PlayerPrefs.GetString("DateToday", ""); }
        set { PlayerPrefs.SetString("DateToday", value); }
    }
    #endregion

    #region 颜色配置
    //黄色 E5AF52FF   229 175 82
    public static Color ColYellow = new Color(229 / 255f, 175 / 255f, 82 / 255f, 1);

    //绿色 589d3a 88 157 58
    public static Color ColGreen = new Color(88 / 255f, 157 / 255f, 58 / 255f, 1);
    //蓝色 36A5E4FF  74 129 240
    public static Color ColBlue = new Color(74 / 255f, 129 / 255f, 240 / 255f, 1);
    //青绿色 2ABAB0FF  42 186 176
    public static Color ColDarkGreen = new Color(42 / 255f, 186 / 255f, 176 / 255f, 1);
    //红色 DE4040FF 222 64 64
    public static Color ColRed = new Color(222 / 255f, 64 / 255f, 64 / 255f, 1);
    //灰色 9a9a9a 154 154 154
    public static Color ColGrey = new Color(154 / 255f, 154 / 255f, 154 / 255f, 1);
    //黑色 434343FF 67 67 67  （创建房间）3e3d40 62 61 64
    public static Color ColBlack = new Color(67 / 255f, 67 / 255f, 67 / 255f, 1);
    //紫色 8E65AFFF 142 101 175 
    public static Color ColViolet = new Color(142 / 255f, 101 / 255f, 175 / 255f, 1);

    #endregion

    //是否其他设备登陆
    public static bool isOtherLogin = false;

    //手机号
    public static string GamePhone
    {
        get { return PlayerPrefs.GetString("GamePhone", "null"); }
        set { PlayerPrefs.SetString("GamePhone", value); }
    }
    //密码
    public static string GamePassword
    {
        get { return PlayerPrefs.GetString("GamePassword", "null"); }
        set { PlayerPrefs.SetString("GamePassword", value); }
    }
    //游戏声音
    public static bool GameSound
    {
        get { return bool.Parse(PlayerPrefs.GetString("GameSound", "True")); }
        set { PlayerPrefs.SetString("GameSound", value.ToString()); }
    }
}
