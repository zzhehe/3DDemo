using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicInput : MonoBehaviour {

    //声音大小
    public float volume;
    //存储声音片段
    AudioClip micRecord;
    //背景音乐
    public AudioSource BgmMusic;

    string device;

    public int dataLength = 128;
	// Use this for initialization
	void Start () {
        //设备选择，0是默认的
        device = Microphone.devices[0];
        //
        micRecord = Microphone.Start(device, true, 10, 44100);

    }
	
	// Update is called once per frame
	void Update () {

        //volume = GetMaxVolume();

        volume = GetMaxVolumeByAudiosource();

    }

    float GetMaxVolume()
    {
        float maxVolume = 0f;

        float[] volumeData = new float[dataLength];
        //从哪里开始截取数据
        int offset = Microphone.GetPosition(device) - dataLength + 1;
        if (offset < 0)
        {
            return 0;
        }
        micRecord.GetData(volumeData, offset);

        for (int i = 0; i < dataLength; i++)
        {
            float tempMax = volumeData[i];
            if (maxVolume < tempMax)
            {
                maxVolume = tempMax;
            }
        }

        return maxVolume;
    }

    float GetMaxVolumeByAudiosource()
    {
        float maxVolume = 0f;

        float[] volumeData = new float[dataLength];

        BgmMusic.clip.GetData(volumeData, BgmMusic.timeSamples);

        for (int i = 0; i < dataLength; i++)
        {
            float tempMax = volumeData[i];
            if (maxVolume < tempMax)
            {
                maxVolume = tempMax;
            }
        }

        return maxVolume;
    }
}
