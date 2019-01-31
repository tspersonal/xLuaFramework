using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Debuger;
using UnityEngine;
using XLua;

[XLua.ReflectionUse]
public class RecordManager : MonoBehaviour
{
    static Dictionary<string, OnVoiceResult> saveUpdates = new Dictionary<string, OnVoiceResult>();
    public OnVoiceResult OnVoice;

    //等待播放的AudioClip
    Queue<string> waitPlayAudioClips = new Queue<string>();

    AudioSource audioSource;
    AudioClip recordedClip;
    int maxClipLength = 10;
    int frequency = 8000;
    int sampleWindow = 128;
    float recordedTimer = 0.0f;
    bool isRecording = false;

    string fileName;
    uint roomID;
    LuaFunction onFunction;
    bool isPlay;

    // Use this for initialization
    void Start()
    {
        string funcName = gameObject.name + "Voice";
        if (saveUpdates.ContainsKey(funcName))
        {
            OnVoice = saveUpdates[funcName];
        }
        else
        {
            OnVoice = LuaManager.luaEnv.Global.GetInPath<OnVoiceResult>(funcName);
            saveUpdates.Add(funcName, OnVoice);
        }
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void AddSoundPath(string fileName)
    {
        waitPlayAudioClips.Enqueue(fileName);
    }
    private void Update()
    {
        if (isRecording)
        {
            recordedTimer += Time.deltaTime;
            if (recordedTimer >= maxClipLength)
            {
                //StopRecord();
                UploadingWWWAudioClip();
            }
        }
        //队列中有数据就播放
        if (waitPlayAudioClips.Count > 0 && !isPlay)
        {
            isPlay = true;
            PlayWWWAudioClip(waitPlayAudioClips.Dequeue());
        }
    }
    //开始录音
    public void StartRecord(string _fileName, uint _roomID, LuaFunction f)
    {
        recordedTimer = 0.0f;
        isRecording = true;
        roomID = _roomID;
        fileName = _fileName;
        onFunction = f;
        string[] str = Microphone.devices;
        recordedClip = Microphone.Start(null, true, maxClipLength, frequency);
    }
    //停止录音
    public AudioClip StopRecord()
    {
        isRecording = false;
        if (recordedTimer < 0.5f) return null;
        int position = Microphone.GetPosition(null);
        float[] soundData = new float[recordedClip.samples * recordedClip.channels];
        recordedClip.GetData(soundData, 0);

        float[] newData = new float[position * recordedClip.channels];

        for (int i = 0; i < newData.Length; i++)
        {
            newData[i] = soundData[i];
        }
        recordedClip = AudioClip.Create(recordedClip.name, position, recordedClip.channels, recordedClip.frequency, false);
        recordedClip.SetData(newData, 0);
        Microphone.End(null);
        DebugerHelper.Log("record:" + recordedClip.length, DebugerHelper.LevelType.Critical);
        return recordedClip;
    }
    //取消录音
    public void CancelRecord()
    {
        if (!Microphone.IsRecording(null)) return;
        StopRecord();
    }
    //获得麦克分音量
    public float GetLevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (sampleWindow + 1);
        if (micPosition < 0) return 0;
        recordedClip.GetData(waveData, micPosition);

        for (int i = 0; i < sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }
    //播放音频
    public void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    //播放网络声音
    public void PlayWWWAudioClip(string filePath)
    {
        StartCoroutine(DownloadAndPlay(filePath));
    }
    //下载播放
    // AudioClip clip;
    IEnumerator DownloadAndPlay(string filePath)
    {
        string[] strs = filePath.Split('@');
        //string path = strs[1] + "@" + strs[2] + "@" + strs[3] + "@" + strs[4];
        WWW ww = new WWW(ServerInfo.Data.DownLoadSoundUrl + "/" + strs[1] + "/" + filePath + ".sound");
        yield return ww;
        if (ww.error == null)
        {
            AudioClip audioClip = ww.GetAudioClip(false, false, AudioType.WAV);
//            TODO:SoundManager.Instance.PlaySound(audioClip);
            OnVoice(filePath, audioClip.length);
            yield return new WaitForSeconds(audioClip.length);
        }
        isPlay = false;
        yield break;
    }


    //上传当前存在的AudioClip
    public void UploadingWWWAudioClip()
    {
        if (!Microphone.IsRecording(null)) return;
        StopRecord();
        StartCoroutine(UploadingAudioClip());
    }
    //上次到WEB服务器
    IEnumerator UploadingAudioClip()
    {
        if (recordedTimer > 0.5f)
        {
            WWWForm form = new WWWForm();
            form.AddField("path", roomID.ToString());
            form.AddField("id", fileName);
            form.AddBinaryData("Sound", EncodeToWAV(recordedClip), "sound.ogg");
            WWW www = new WWW(ServerInfo.Data.UpLoadSoundUrl, form);
            yield return www;
            if (www.error == null)
            {
                if (onFunction != null)
                    onFunction.Call(fileName, true);
            }
        }
        else
        {
            onFunction.Call("cancal", false);
        }
        yield break;
    }

    public byte[] GetData16(AudioClip clip)
    {
        var data = new float[clip.samples * clip.channels];

        clip.GetData(data, 0);

        byte[] bytes = new byte[data.Length * 2];

        int rescaleFactor = 32767;

        for (int i = 0; i < data.Length; i++)
        {
            short value = (short)(data[i] * rescaleFactor);
            BitConverter.GetBytes(value).CopyTo(bytes, i * 2);
        }

        return bytes;
    }

    public byte[] EncodeToWAV(AudioClip clip)
    {
        byte[] bytes = null;

        using (var memoryStream = new MemoryStream())
        {
            memoryStream.Write(new byte[44], 0, 44);//预留44字节头部信息

            byte[] bytesData = GetData16(clip);

            memoryStream.Write(bytesData, 0, bytesData.Length);

            memoryStream.Seek(0, SeekOrigin.Begin);

            byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
            memoryStream.Write(riff, 0, 4);

            byte[] chunkSize = BitConverter.GetBytes(memoryStream.Length - 8);
            memoryStream.Write(chunkSize, 0, 4);

            byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
            memoryStream.Write(wave, 0, 4);

            byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
            memoryStream.Write(fmt, 0, 4);

            byte[] subChunk1 = BitConverter.GetBytes(16);
            memoryStream.Write(subChunk1, 0, 4);

            UInt16 one = 1;

            byte[] audioFormat = BitConverter.GetBytes(one);
            memoryStream.Write(audioFormat, 0, 2);

            byte[] numChannels = BitConverter.GetBytes(clip.channels);
            memoryStream.Write(numChannels, 0, 2);

            byte[] sampleRate = BitConverter.GetBytes(clip.frequency);
            memoryStream.Write(sampleRate, 0, 4);

            byte[] byteRate = BitConverter.GetBytes(clip.frequency * clip.channels * 2); // sampleRate * bytesPerSample*number of channels
            memoryStream.Write(byteRate, 0, 4);

            UInt16 blockAlign = (ushort)(clip.channels * 2);
            memoryStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

            UInt16 bps = 16;
            byte[] bitsPerSample = BitConverter.GetBytes(bps);
            memoryStream.Write(bitsPerSample, 0, 2);

            byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
            memoryStream.Write(datastring, 0, 4);

            byte[] subChunk2 = BitConverter.GetBytes(clip.samples * clip.channels * 2);
            memoryStream.Write(subChunk2, 0, 4);

            bytes = memoryStream.ToArray();
        }
        DebugerHelper.Log(bytes.Length, DebugerHelper.LevelType.Normal);
        return bytes;
    }
}
