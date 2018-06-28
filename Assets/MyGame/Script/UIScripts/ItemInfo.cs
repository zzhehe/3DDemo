using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo{

    /// <summary>
    /// 道具名称
    /// </summary>
    public string name;
    /// <summary>
    /// 道具描述
    /// </summary>
    public string description;
    /// <summary>
    /// 道具图标
    /// </summary>
    public string icon;
    /// <summary>
    /// 背景边框
    /// </summary>
    public string bgIcon;
    /// <summary>
    /// 道具数量
    /// </summary>
    public int count;
    /// <summary>
    /// 道具ID
    /// </summary>
    public int id;
    /// <summary>
    /// 道具品质
    /// </summary>
    public int quality;
    /// <summary>
    /// 道具类型
    /// </summary>
    public int type;

    public ItemInfo()
    {
        id = -1;
    }
}

public enum EItemOperation
{
    /// <summary>
    /// 出售
    /// </summary>
    EIO_Sell,
    /// <summary>
    /// 装备
    /// </summary>
    EIO_Equip,
    /// <summary>
    /// 分解
    /// </summary>
    EIO_Resolve,
    /// <summary>
    /// 合成
    /// </summary>
    EIO_Compound,
    EIO_MAX
}

public enum EItemType
{
    /// <summary>
    /// 装备
    /// </summary>
    EIT_Equip,
    /// <summary>
    /// 碎片
    /// </summary>
    EIT_Chip,

    EIT_Prop,
    /// <summary>
    /// 未知道具
    /// </summary>
    EIT_Unknown,
    EIT_MAX
}
