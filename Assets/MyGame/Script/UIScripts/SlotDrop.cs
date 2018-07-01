using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDrop : MonoBehaviour, IDropHandler
{
    public int slotIndex;
    //背包的存储所有格子物品列表的类
    private BagPanel bagPanel;
    // Use this for initialization
    void Start () {
        bagPanel = FindObjectOfType<BagPanel>();
    }
	
    

    public void OnDrop(PointerEventData eventData)
    {
        BagItem bagItem = eventData.pointerDrag.GetComponent<BagItem>();
        Debug.Log(bagItem.itemInfo.id);
        if (bagPanel.itemInfoList[slotIndex].id == -1)
        {
            if (bagItem != null)
            {
                bagPanel.itemInfoList[bagItem.slotsIndex] = new ItemInfo();
                bagItem.slotsIndex = slotIndex;
                bagPanel.itemInfoList[slotIndex] = bagItem.itemInfo;
            }
        }
        else if (bagItem.slotsIndex != slotIndex)
        {
            //被换的物品信息
            Transform beItem = transform.GetChild(0);
            beItem.GetComponent<BagItem>().slotsIndex = bagItem.slotsIndex;
            //把被换的物品移动到原来拖动物品的父节点下
            beItem.SetParent(bagPanel.slotsList[bagItem.slotsIndex].transform);
            beItem.position = beItem.parent.position;
            //交换两个物品的信息，拖动物品的位置在OnEndDrag函数中设置
            bagPanel.itemInfoList[bagItem.slotsIndex] = beItem.GetComponent<BagItem>().itemInfo;
            bagItem.slotsIndex = slotIndex;
            bagPanel.itemInfoList[slotIndex] = bagItem.itemInfo;
        }
    }
}
