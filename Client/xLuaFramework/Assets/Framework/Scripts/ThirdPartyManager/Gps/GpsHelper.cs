using System.Collections;
using UnityEngine;

[XLua.ReflectionUse]
public class GpsHelper : MonoBehaviour
{
    [HideInInspector]
    public string gps_info = "";
    [HideInInspector]
    public int flash_num = 1;

    public static GpsHelper Instance = null;
    // Use this for initialization  
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitGps();
    }

    public void InitGps()
    {
        StartCoroutine(IeStartGps());
    }

    void StopGps()
    {
        Input.location.Stop();
    }

    IEnumerator IeStartGps()
    {
        // Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置  
        // LocationService.isEnabledByUser 用户设置里的定位服务是否启用  
        if (!Input.location.isEnabledByUser)
        {
            this.gps_info = "isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString() + " Please turn on the GPS";
        }
        else
        {
            Input.location.Start(10.0f, 10.0f);
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                // 暂停协同程序的执行(1秒)  
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            if (maxWait < 1)
            {
                this.gps_info = "Init GPS service time out";
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                this.gps_info = "位置服务失败（用户拒绝访问位置服务）";
            }
            else
            {
                //Debuger.Log(this.gps_info = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude);
                //Debuger.Log(this.gps_info = this.gps_info + " Time:" + Input.location.lastData.timestamp);
                yield return new WaitForSeconds(100);
            }
        }
        yield break;
    }
	
    //纬度
    public float GetLatitude()
    {
        return Input.location.lastData.latitude;
    }
	
    //经度
    public float GetLongitude()
    {
        return Input.location.lastData.longitude;
    }
}
