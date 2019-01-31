using Debuger;
using GameShare;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

[XLua.ReflectionUse]
public class ToolsFunc
{
    /// <summary>
    /// 3D坐标到屏幕坐标
    /// </summary>
    /// <returns>The to U.</returns>
    /// <param name="point">Point.</param>
    public static Vector3 WorldToUI(Vector3 point)
    {
        Vector3 pt = Camera.main.WorldToScreenPoint(point);
        //我发现有时候UICamera.currentCamera 有时候currentCamera会取错，取的时候注意一下啊。
        Vector3 ff = UICamera.currentCamera.ScreenToWorldPoint(pt);
        //UI的话Z轴 等于0 
        ff.z = 0;
        return ff;
    }

    /// <summary>
    /// 秒转换成时间
    /// </summary>
    /// <returns>The time.</returns>
    /// <param name="total_time">Total_time.</param>
    public static string getTime(int total_time)
    {
        string str = "";
        str += (total_time / 3600 >= 10 ? (total_time / 3600).ToString() : "0" + (total_time / 3600).ToString()) + ":";
        str += ((total_time % 3600) / 60 >= 10 ? ((total_time % 3600) / 60).ToString() : "0" + ((total_time % 3600) / 60).ToString()) + ":";
        str += (total_time % 3600) % 60 >= 10 ? ((total_time % 3600) % 60).ToString() : "0" + ((total_time % 3600) % 60).ToString();

        return str;
    }
    /// <summary>
    /// 获得首字母
    /// </summary>
    /// <param name="CnChar"></param>
    /// <returns></returns>
    public static string GetCharSpellCode(string CnChar)
    {
        long iCnChar;

        byte[] ZW = System.Text.Encoding.Default.GetBytes(CnChar);

        //如果是字母，则直接返回 
        if (ZW.Length == 1)
        {
            return CnChar.ToUpper();
        }
        else
        {
            // get the array of byte from the single char 
            int i1 = (short)(ZW[0]);
            int i2 = (short)(ZW[1]);
            iCnChar = i1 * 256 + i2;
        }

        //expresstion 
        //table of the constant list 
        // 'A'; //45217..45252 
        // 'B'; //45253..45760 
        // 'C'; //45761..46317 
        // 'D'; //46318..46825 
        // 'E'; //46826..47009 
        // 'F'; //47010..47296 
        // 'G'; //47297..47613 

        // 'H'; //47614..48118 
        // 'J'; //48119..49061 
        // 'K'; //49062..49323 
        // 'L'; //49324..49895 
        // 'M'; //49896..50370 
        // 'N'; //50371..50613 
        // 'O'; //50614..50621 
        // 'P'; //50622..50905 
        // 'Q'; //50906..51386 

        // 'R'; //51387..51445 
        // 'S'; //51446..52217 
        // 'T'; //52218..52697 
        //没有U,V 
        // 'W'; //52698..52979 
        // 'X'; //52980..53640 
        // 'Y'; //53689..54480 
        // 'Z'; //54481..55289 

        // iCnChar match the constant 
        if ((iCnChar >= 45217) && (iCnChar <= 45252))
        {
            return "A";
        }
        else if ((iCnChar >= 45253) && (iCnChar <= 45760))
        {
            return "B";
        }
        else if ((iCnChar >= 45761) && (iCnChar <= 46317))
        {
            return "C";
        }
        else if ((iCnChar >= 46318) && (iCnChar <= 46825))
        {
            return "D";
        }
        else if ((iCnChar >= 46826) && (iCnChar <= 47009))
        {
            return "E";
        }
        else if ((iCnChar >= 47010) && (iCnChar <= 47296))
        {
            return "F";
        }
        else if ((iCnChar >= 47297) && (iCnChar <= 47613))
        {
            return "G";
        }
        else if ((iCnChar >= 47614) && (iCnChar <= 48118))
        {
            return "H";
        }
        else if ((iCnChar >= 48119) && (iCnChar <= 49061))
        {
            return "J";
        }
        else if ((iCnChar >= 49062) && (iCnChar <= 49323))
        {
            return "K";
        }
        else if ((iCnChar >= 49324) && (iCnChar <= 49895))
        {
            return "L";
        }
        else if ((iCnChar >= 49896) && (iCnChar <= 50370))
        {
            return "M";
        }

        else if ((iCnChar >= 50371) && (iCnChar <= 50613))
        {
            return "N";
        }
        else if ((iCnChar >= 50614) && (iCnChar <= 50621))
        {
            return "O";
        }
        else if ((iCnChar >= 50622) && (iCnChar <= 50905))
        {
            return "P";
        }
        else if ((iCnChar >= 50906) && (iCnChar <= 51386))
        {
            return "Q";
        }
        else if ((iCnChar >= 51387) && (iCnChar <= 51445))
        {
            return "R";
        }
        else if ((iCnChar >= 51446) && (iCnChar <= 52217))
        {
            return "S";
        }
        else if ((iCnChar >= 52218) && (iCnChar <= 52697))
        {
            return "T";
        }
        else if ((iCnChar >= 52698) && (iCnChar <= 52979))
        {
            return "W";
        }
        else if ((iCnChar >= 52980) && (iCnChar <= 53640))
        {
            return "X";
        }
        else if ((iCnChar >= 53689) && (iCnChar <= 54480))
        {
            return "Y";
        }
        else if ((iCnChar >= 54481) && (iCnChar <= 55289))
        {
            return "Z";
        }
        else return ("?");
    }
    /// <summary>
    /// 解析URL 获得IP
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetServerIP(string url)
    {
        string str = url.Substring(0, 7);
        if (str == "192.168") return url;
        IPHostEntry ipHost = Dns.GetHostEntry(url);
        string serverIP = "";
        switch (ipHost.AddressList[0].AddressFamily)
        {
            case AddressFamily.InterNetwork:
                ConnServer.m_IsIpv6 = false;
                break;
            case AddressFamily.InterNetworkV6:
                ConnServer.m_IsIpv6 = true;
                break;

        }
        serverIP = ipHost.AddressList[0].ToString();
        return serverIP;
    }

    public static string GetServerIP(string url, ref AddressFamily addressType)
    {
        string str = url.Substring(0, 7);
        if (str == "192.168") return url;
        IPHostEntry ipHost = Dns.GetHostEntry(url);
        string serverIP = "";
        switch (ipHost.AddressList[0].AddressFamily)
        {
            case AddressFamily.InterNetwork:
                addressType = AddressFamily.InterNetwork;
                break;
            case AddressFamily.InterNetworkV6:
                addressType = AddressFamily.InterNetworkV6;
                break;

        }
        serverIP = ipHost.AddressList[0].ToString();
        return serverIP;
    }
    /// <summary>
    /// 获得字符串长度
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int GetStringLength(string str)
    {
        int len = 0;
        foreach (char item in str)
        {
            len += (int)item > 127 ? 2 : 1;
        }
        return len;
    }
    /// <summary>
    /// 获得缩略显示
    /// </summary>
    /// <returns></returns>
    public static string GetSuoNueShowText(int number)
    {
        string str = "";
        string suffix = "";
        int integ = 0;
        if (number < 1000)
        {
            return number.ToString();
        }
        else if (number >= 1000 && number < 10000)
        {
            integ = (int)(number / 1000);
            suffix = "K";
        }
        else if (number >= 10000 && number < 1000000)
        {
            integ = (int)(number / 10000);
            suffix = "W";
        }
        else if (number >= 1000000)
        {
            integ = (int)(number / 1000000);
            suffix = "M";
        }
        str += integ.ToString();
        str += suffix;
        return str;
    }
    /// <summary>
    /// 设置分享图标
    /// </summary>
    public static void SetShareIcon()
    {
        Texture2D texture = Resources.Load<Texture2D>("ICON");
        File.WriteAllBytes(Application.persistentDataPath + "/icon.png", texture.EncodeToPNG());
    }

    //删除所有子对象
    public static void DestroyTransformChild(Transform _transform)
    {
        foreach (Transform child in _transform)
        {
            UnityEngine.GameObject.Destroy(child.gameObject);
        }
    }

    public static void FindAllTagAndMaterial(string tag, Material material)
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < items.Length; i++)
        {
            items[i].GetComponent<MeshRenderer>().material = material;
        }
    }

    public static string GetUTF8String(string str)
    {
        string newStr = "";
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        newStr = Encoding.UTF8.GetString(bytes);
        return newStr;
    }

    public static Vector3 LookTargetAngle(Transform playerTrans, Vector3 targetPos)
    {
        float dx = targetPos.x - playerTrans.transform.localPosition.x;
        float dy = targetPos.y - playerTrans.transform.localPosition.y;
        float rotationZ = Mathf.Atan2(dy, dx) * 180 / Mathf.PI;
        //得到最终的角度并且确保在 [0, 360) 这个区间内
        rotationZ -= 90;
        //获取增加的角度
        float originRotationZ = playerTrans.localEulerAngles.z;
        float addRotationZ = rotationZ - originRotationZ;
        //超过 180 度需要修改为负方向的角度
        if (addRotationZ > 180)
        {
            addRotationZ -= 360;
        }
        //应用旋转
        return new Vector3(0, 0, playerTrans.localEulerAngles.z + addRotationZ);
    }
    //判断牌型
    public static int GetCardType(List<uint> cards)
    {
        if (cards.Count == 2)
        {
            if (cards[0] % 100 == cards[1] % 100) return 1;
            else return 0;
        }
        else
        {
            PokerInfo pokerInfo = TexasHelper.GetMaxPokerInfo(cards);
            return (int)pokerInfo.PokerType;
        }
    }
    //获得手指操作
    public static int GetTouchIndex()
    {
        return (int)Input.GetTouch(0).phase;
    }

    //计算加注总格子数量
    public static uint JiSuanAddGrid(int needAddChips, int totalChips)
    {
        uint total_count = 0;
        uint min_chips = (uint)needAddChips;
        int max_chips = totalChips;
        if (max_chips <= min_chips)
        {
            return total_count;
        }
        total_count = 1;
        if (min_chips < 10) { if (max_chips <= 10) total_count += (uint)Math.Ceiling((max_chips - min_chips) / 2.0); else { total_count += (uint)Math.Ceiling((10 - min_chips) / 2.0); min_chips = 10; } }
        if (min_chips >= 10 && min_chips < 100) { if (max_chips <= 100) total_count += (uint)Math.Ceiling((max_chips - min_chips) / 5.0); else { total_count += (uint)Math.Ceiling((100 - min_chips) / 5.0); min_chips = 100; } }
        if (min_chips >= 100 && min_chips < 1000) { if (max_chips <= 1000) total_count += (uint)Math.Ceiling((max_chips - min_chips) / 10.0); else { total_count += (uint)Math.Ceiling((1000 - min_chips) / 10.0); min_chips = 1000; } }
        if (min_chips >= 1000 && min_chips < 10000) { if (max_chips <= 10000) total_count += (uint)Math.Ceiling((max_chips - min_chips) / 100.0); else { total_count += (uint)Math.Ceiling((10000 - min_chips) / 100.0); min_chips = 10000; } }
        if (min_chips >= 10000 && min_chips < 100000) { if (max_chips <= 100000) total_count += (uint)Math.Ceiling((max_chips - min_chips) / 1000.0); else { total_count += (uint)Math.Ceiling((100000 - min_chips) / 1000.0); min_chips = 100000; } }
        if (min_chips >= 100000 && min_chips < 1000000) { if (max_chips <= 1000000) total_count += (uint)Math.Ceiling((max_chips - min_chips) / 10000.0); else { total_count += (uint)Math.Ceiling((1000000 - min_chips) / 10000.0); min_chips = 1000000; } }
        return total_count;
    }
    //标准化筹码
    public static uint GetStandardChipValue(int index, int needAddChips, int totalChips)
    {
        int curAddChips = 0;
        int grid2 = 0;
        int grid5 = 0;
        int grid10 = 0;
        int grid100 = 0;
        int grid1000 = 0;
        int grid10000 = 0;

        if (needAddChips < 10)
        {
            int number = 10 - needAddChips;
            grid2 = number % 2 == 0 ? number / 2 : number / 2 + 1;
            if (totalChips > 100)
                grid5 = (100 - 10) / 5;
            else
                grid5 = (totalChips - 10) % 5 == 0 ? (totalChips - 10) / 5 : (totalChips - 10) / 5 + 1;
            if (totalChips > 1000)
                grid10 = 900 / 10;
            else
                grid10 = (totalChips - 100) % 10 == 0 ? (totalChips - 100) / 10 : (totalChips - 100) / 10 + 1;
            if (totalChips > 10000)
                grid100 = 9000 / 100;
            else
                grid100 = (totalChips - 1000) % 100 == 0 ? (totalChips - 1000) / 100 : (totalChips - 1000) / 100 + 1;

            if (totalChips > 100000)
                grid1000 = 90000 / 1000;
            else
                grid1000 = (totalChips - 10000) % 1000 == 0 ? (totalChips - 10000) / 1000 : (totalChips - 10000) / 1000 + 1;

            if (totalChips > 1000000)
                grid10000 = 900000 / 10000;
            else
                grid10000 = (totalChips - 100000) % 10000 == 0 ? (totalChips - 100000) / 10000 : (totalChips - 100000) / 10000 + 1;
        }

        if (needAddChips >= 10 && needAddChips < 100)
        {
            int number = 100 - needAddChips;
            grid5 = number % 5 == 0 ? number / 5 : number / 5 + 1;
            if (totalChips > 1000)
                grid10 = 900 / 10;
            else
                grid10 = (totalChips - 100) % 10 == 0 ? (totalChips - 100) / 10 : (totalChips - 100) / 10 + 1;
            if (totalChips > 10000)
                grid100 = 9000 / 100;
            else
                grid100 = (totalChips - 1000) % 100 == 0 ? (totalChips - 1000) / 100 : (totalChips - 1000) / 100 + 1;

            if (totalChips > 100000)
                grid1000 = 90000 / 1000;
            else
                grid1000 = (totalChips - 10000) % 1000 == 0 ? (totalChips - 10000) / 1000 : (totalChips - 10000) / 1000 + 1;

            if (totalChips > 1000000)
                grid10000 = 900000 / 10000;
            else
                grid10000 = (totalChips - 100000) % 10000 == 0 ? (totalChips - 100000) / 10000 : (totalChips - 100000) / 10000 + 1;
        }

        if (needAddChips >= 100 && needAddChips < 1000)
        {
            int number = 1000 - needAddChips;
            grid10 = number % 10 == 0 ? number / 10 : number / 10 + 1;
            if (totalChips > 10000)
                grid100 = 9000 / 100;
            else
                grid100 = (totalChips - 1000) % 100 == 0 ? (totalChips - 1000) / 100 : (totalChips - 1000) / 100 + 1;

            if (totalChips > 100000)
                grid1000 = 90000 / 1000;
            else
                grid1000 = (totalChips - 10000) % 1000 == 0 ? (totalChips - 10000) / 1000 : (totalChips - 10000) / 1000 + 1;

            if (totalChips > 1000000)
                grid10000 = 900000 / 10000;
            else
                grid10000 = (totalChips - 100000) % 10000 == 0 ? (totalChips - 100000) / 10000 : (totalChips - 100000) / 10000 + 1;
        }

        if (needAddChips >= 1000 && needAddChips < 10000)
        {
            int number = 10000 - needAddChips;
            grid100 = number % 100 == 0 ? number / 100 : number / 100 + 1;
            if (totalChips > 100000)
                grid1000 = 90000 / 1000;
            else
                grid1000 = (totalChips - 10000) % 1000 == 0 ? (totalChips - 10000) / 1000 : (totalChips - 10000) / 1000 + 1;

            if (totalChips > 1000000)
                grid10000 = 900000 / 10000;
            else
                grid10000 = (totalChips - 100000) % 10000 == 0 ? (totalChips - 100000) / 10000 : (totalChips - 100000) / 10000 + 1;
        }

        if (needAddChips >= 10000 && needAddChips < 100000)
        {
            int number = 100000 - needAddChips;
            grid1000 = number % 1000 == 0 ? number / 1000 : number / 1000 + 1;
            if (totalChips > 1000000)
                grid10000 = 900000 / 10000;
            else
                grid10000 = (totalChips - 100000) % 10000 == 0 ? (totalChips - 100000) / 10000 : (totalChips - 100000) / 10000 + 1;
        }

        if (needAddChips > 1000000)
        {
            int number = 1000000 - needAddChips;
            grid10000 = number % 10000 == 0 ? number / 10000 : number / 10000 + 1;
        }
        if (index <= grid2)
            curAddChips = needAddChips + index * 2;
        else if (index > grid2 && index <= grid5 + grid2)
            curAddChips = needAddChips + (index - grid2) * 5 + grid2 * 2;
        else if ((index > grid5 + grid2) && (index <= grid10 + grid5 + grid2))
            curAddChips = needAddChips + grid5 * 5 + grid2 * 2 + (index - grid2 - grid5) * 10;
        else if ((index > grid10 + grid5 + grid2) && (index <= grid10 + grid5 + grid2 + grid100))
            curAddChips = needAddChips + grid5 * 5 + grid2 * 2 + (index - grid2 - grid5 - grid10) * 100 + grid10 * 10;
        else if ((index > grid10 + grid5 + grid2 + grid100) && (index <= grid10 + grid5 + grid2 + grid100 + grid1000))
            curAddChips = needAddChips + grid5 * 5 + grid2 * 2 + (index - grid2 - grid5 - grid10 - grid100) * 1000 + grid10 * 10 + grid100 * 100;
        else if ((index > grid10 + grid5 + grid2 + grid100 + grid1000) && (index <= grid10 + grid5 + grid2 + grid100 + grid1000 + grid10000))
            curAddChips = needAddChips + grid5 * 5 + grid2 * 2 + (index - grid2 - grid5 - grid10 - grid100 - grid1000) * 10000 + grid10 * 10 + grid100 * 100 + grid1000 * 1000;

        return (uint)curAddChips;
    }

    public static bool IsExist(string objName)
    {
        if (GameObject.Find(objName) == null)
            return false;
        else
            return true;
    }

    /// <summary>
    /// double转为string类型
    /// </summary>
    public static string DoubleToString(double d)
    {
        //double dValue = Math.Round(d, 2);
        //string sValue = dValue.ToString(CultureInfo.InvariantCulture);
        string sValue = string.Format("{0:F}", d);
        return sValue;
    }

    /// <summary>
    /// float转为string类型
    /// </summary>
    public static string FloatToString(float f)
    {
        double value = Math.Round(Convert.ToDouble(f), 2);
        return value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 显示Double数字
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static double SetDouble(double d)
    {
        double value = Math.Round(d, 2);
        return value;
    }

    /// <summary>
    /// 字符串转为16进制
    /// </summary>
    /// <param name="sNum"></param>
    /// <returns></returns>
    public static string StringToString16(string sNum)
    {
        DebugerHelper.Log("转为16进制前：" + sNum, DebugerHelper.LevelType.Normal);
        if (string.IsNullOrEmpty(sNum))
        {
            DebugerHelper.Log("StringToString16 ---> 参数为Null", DebugerHelper.LevelType.Warning);
            return "";
        }
        string sValue = "";
        try
        {
            long nNum = long.Parse(sNum);
            sValue = Convert.ToString(nNum, 16);
            sValue = sValue.ToUpper();
        }
        catch (Exception e)
        {
            DebugerHelper.Log(e.Message, DebugerHelper.LevelType.Except);
        }
        DebugerHelper.Log("转为16进制后：" + sValue, DebugerHelper.LevelType.Normal);
        return sValue;
    }

    /// <summary>
    /// 字符串转为10进制数字
    /// </summary>
    /// <param name="sNum"></param>
    /// <returns></returns>
    public static long StringToInt10(string sNum)
    {
        DebugerHelper.Log("转为10进制前：" + sNum, DebugerHelper.LevelType.Normal);
        if (string.IsNullOrEmpty(sNum))
        {
            DebugerHelper.Log("StringToString16 ---> 参数为Null", DebugerHelper.LevelType.Warning);
            return 0;
        }
        long nValue = 0;
        try
        {
            sNum = sNum.ToLower();
            long lValue = long.Parse(sNum, NumberStyles.HexNumber);
            DebugerHelper.Log("转为10进制后：" + lValue, DebugerHelper.LevelType.Normal);
            string sValue = ((ulong)lValue).ToString();

            if (sValue.Length >= 6)
                sValue = sValue.Substring(0, 6);
            else
                return 0;

            nValue = long.Parse(sValue);
        }
        catch (Exception e)
        {
            DebugerHelper.Log(e.Message, DebugerHelper.LevelType.Except);
        }
        return nValue;
    }

    //获取字节长度的显示
    public static string GetBytesLength(double dLength, int nRound)
    {
        string sFormat = "{0:N" + nRound + "}";
        double dBLength = dLength;
        double dKLength = dBLength / 1024;
        if (dKLength <= 1)
        {
            return String.Format(sFormat, dBLength) + " B";
            //Math.Round(dBLength, nRound).ToString(CultureInfo.InvariantCulture) + " B";
        }
        else
        {
            double dMbLength = dKLength / 1024;
            if (dMbLength <= 1)
            {
                return String.Format(sFormat, dKLength) + " KB"; ;
                //return Math.Round(dKLength, nRound).ToString(CultureInfo.InvariantCulture) + " KB";
            }
            else
            {
                double dGbLength = dMbLength / 1024;
                if (dGbLength <= 1)
                {
                    return String.Format(sFormat, dMbLength) + " MB";
                    //return Math.Round(dMbLength, nRound).ToString(CultureInfo.InvariantCulture) + " MB";
                }
                else
                {
                    return String.Format(sFormat, dGbLength) + " GB";
                    //return Math.Round(dGbLength, nRound).ToString(CultureInfo.InvariantCulture) + " GB";
                }
            }
        }
    }

    //获取字节长度的显示 根据整数位进行补全
    public static string GetBytesLimitLength(double dLength, int nlimit)
    {
        double dBLength = dLength;
        double dKLength = dBLength / 1024;
        if (dKLength <= 1)
        {
            string sFormat = "{0:N" + GetReserveCount(dBLength, nlimit) + "}";
            return String.Format(sFormat, dBLength) + " B";
            //Math.Round(dBLength, nRound).ToString(CultureInfo.InvariantCulture) + " B";
        }
        else
        {
            double dMbLength = dKLength / 1024;
            if (dMbLength <= 1)
            {
                string sFormat = "{0:N" + GetReserveCount(dKLength, nlimit) + "}";
                return String.Format(sFormat, dKLength) + " KB"; ;
                //return Math.Round(dKLength, nRound).ToString(CultureInfo.InvariantCulture) + " KB";
            }
            else
            {
                double dGbLength = dMbLength / 1024;
                if (dGbLength <= 1)
                {
                    string sFormat = "{0:N" + GetReserveCount(dMbLength, nlimit) + "}";
                    return String.Format(sFormat, dMbLength) + " MB";
                    //return Math.Round(dMbLength, nRound).ToString(CultureInfo.InvariantCulture) + " MB";
                }
                else
                {
                    string sFormat = "{0:N" + GetReserveCount(dGbLength, nlimit) + "}";
                    return String.Format(sFormat, dGbLength) + " GB";
                    //return Math.Round(dGbLength, nRound).ToString(CultureInfo.InvariantCulture) + " GB";
                }
            }
        }
    }

    //根据传入的数值 根据整数位进行限制位数 返回小数位应该保留几位 1=>0 2=>1 3=>2 4=>3
    public static int GetReserveCount(double dValue, int nlimit = 1)
    {
        int nReserve = 0;
        if (nlimit == 1)
        {
            return 0;
        }
        if (dValue <= 0)
        {
            nReserve = nlimit - 1;
        }
        else
        {
            for (var i = nlimit; i >= 1; i--)
            {
                double dTemp = dValue / Math.Pow(10, i - 1);
                if (dTemp >= 1)
                {
                    nReserve = nlimit - i;
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
        return nReserve;
    }

    /// <summary>
    /// 字符串转为MD5码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string CalculateMd5Hash(string input)
    {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        // step 2, convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }

    /// <summary>
    /// 文件转为MD5码
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetMd5HashFromFile(string fileName)
    {
        try
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
        }
    }

    /// <summary>
    /// 获取换行符
    /// </summary>
    /// <returns></returns>
    public static string GetCharNewline()
    {
        //根据平台不同，替换换行符
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            return "\r\n";
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            return "\r\n";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return "\r";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            return "\n";
        }
        else
        {
            return "\n";
        }
    }
}
