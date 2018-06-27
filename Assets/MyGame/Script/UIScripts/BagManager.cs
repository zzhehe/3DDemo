using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
/// <summary>
/// 背包管理器，管理背包数据的读写
/// </summary>
public class BagManager{

    private static BagManager instance;
    private static readonly object locker = new object();
    public static BagManager Instance
    {
        get {
            if (instance == null)
            {
                //锁防止两个任务同时执行
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new BagManager();
                    }
                }
            }
            return instance;
        }
    }

    private BagManager()
    {
        CreateItems();
    }

    //背包中的所有物品list
    public List<ItemInfo> items = new List<ItemInfo>();

    Dictionary<int, ItemInfo> itemPairs = new Dictionary<int, ItemInfo>();

    void CreateItems()
    {
        //从json文件中获取数据
        GetDataFromJson();
    }

    void GetDataFromJson()
    {
        //先清除list
        //items.Clear();

        string filePath = Application.dataPath + @"/Data/Bagjson.json";
        if (!File.Exists(filePath)) return;

        FileStream file = new FileStream(filePath, FileMode.OpenOrCreate);
        StreamReader reader = new StreamReader(file);
        string strData = reader.ReadToEnd();

        List<ItemInfo> jsondata = JsonMapper.ToObject<List<ItemInfo>>(strData);
        //  items.Add(jsondata);
        //  itemPairs.Add(jsondata.id, jsondata);

        foreach (var item in jsondata)
        {
            if (item != null && item.id != 0)
            {
                if (!items.Contains(item))
                {
                    items.Add(item);
                    itemPairs.Add(item.id, item);
                }
            }
        }

    }

    void SetDataToData()
    {
        string filePath = Application.dataPath + @"/Data/Bagjson.json";
        if (!File.Exists(filePath)) return;

        //找到当前路径
        FileInfo file = new FileInfo(filePath);
        //判断有没有文件，有则打开文件，，没有创建后打开文件
        StreamWriter sw = file.CreateText();

        string jsondata = JsonMapper.ToJson(items);
        sw.Write(jsondata);
        sw.Flush();
        sw.Close();
        
    }

    public ItemInfo GetItemInfoByID(int ID)
    {
        if (itemPairs.ContainsKey(ID))
        {
            return itemPairs[ID];
        }
        return null;
    }

    public void DelItemInfoByID(int ID)
    {
        if (itemPairs.ContainsKey(ID))
        {
            itemPairs.Remove(ID);
        }
    }
}
