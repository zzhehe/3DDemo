using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : MonoBehaviour
{

    public Image icon;
    public Image bgicon;
    public Text number;

    [HideInInspector]
    public ItemInfo itemInfo;

    public void SetData(ItemInfo info)
    {
        this.itemInfo = info;
        icon.sprite = Resources.Load<Sprite>("Textures/" + info.icon);
        bgicon.sprite = Resources.Load<Sprite>("Textures/" + info.bgIcon);
        number.text = itemInfo.count.ToString();
    }
}
