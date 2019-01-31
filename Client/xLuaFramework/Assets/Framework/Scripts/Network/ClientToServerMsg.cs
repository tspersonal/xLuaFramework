using Debuger;
using GameFrame;

[XLua.ReflectionUse]
public class ClientToServerMsg
{
    //public static void Send(Opcodes op, string jsonString)
    //{
    //    DebugerHelper.Log("send:--->" + jsonString);
    //    NetworkMessage message = NetworkMessage.Create((ushort)op, 256);
    //    message.writeString(jsonString);
    //    SendMsg(message);
    //}
    public static void Send(Opcodes op, params object[] pms)
    {
        if (pms.Length > 0)
            DebugerHelper.Log("send:---> " + op + ", msg:---> " + pms[0], DebugerHelper.LevelType.Info);
        else
            DebugerHelper.Log("send:---> " + op + ", msg:---> Null", DebugerHelper.LevelType.Info);

        NetworkMessage message = NetworkMessage.Create((ushort)op, 256);
        for (int i = 0; i < pms.Length; i++)
        {
            if (pms[i].GetType() == typeof(string)) message.writeString((string)pms[i]);
            else if (pms[i].GetType() == typeof(bool)) message.writeBool((bool)pms[i]);
            else if (pms[i].GetType() == typeof(int)) message.writeInt32((int)pms[i]);
            else if (pms[i].GetType() == typeof(long)) message.writeInt64((long)pms[i]);
            else if (pms[i].GetType() == typeof(byte)) message.writeUInt8((byte)pms[i]);
            else if (pms[i].GetType() == typeof(uint)) message.writeUInt32((uint)pms[i]);
            else if (pms[i].GetType() == typeof(ulong)) message.writeUInt64((ulong)pms[i]);

        }
        SendMsg(message);
    }

    // 把数据发给服务器
    static void SendMsg(NetworkMessage message)
    {
        ConnServer.m_WaitServerMsgCount++;
        Connection conn = ConnServer.global.Tcp_gateway[0];
        if (conn != null) conn.send(message);
    }
}
