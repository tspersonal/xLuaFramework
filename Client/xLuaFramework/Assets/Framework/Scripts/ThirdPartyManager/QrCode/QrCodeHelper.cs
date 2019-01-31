using UnityEngine;
using ZXing;
using ZXing.QrCode;

[XLua.ReflectionUse]
public class QrCodeHelper
{
    //定义方法生成二维码  
    public static Color32[] QrCodeEncode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }
}
