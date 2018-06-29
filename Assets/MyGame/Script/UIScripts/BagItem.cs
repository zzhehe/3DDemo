using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BagItem : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler, IDropHandler
{
    public Image icon;
    public Text number;

    private BagPanel bagPanel;

    [HideInInspector]
    public ItemInfo itemInfo;
    [HideInInspector]
    public int slotsIndex;

    private void Start()
    {
        bagPanel = FindObjectOfType<BagPanel>();
    }


    //设置背包格中的物品信息
    public void SetData(ItemInfo info)
    {
        this.itemInfo = info;
        icon.sprite = Resources.Load<Sprite>("Textures/" + info.icon);
        number.text = itemInfo.count == 0 ? "" : itemInfo.count.ToString();
        if (info.id == -1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public BagItem()
    {
        itemInfo = new ItemInfo();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("开始拖物品"+slotsIndex);
        if (itemInfo != null)
        {
            gameObject.transform.SetParent(gameObject.transform.parent.parent);
            gameObject.transform.position = eventData.position;
            //把能接受射线的功能关掉
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("正在拖物品"+slotsIndex);
        if (itemInfo != null)
        {
            gameObject.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("拖物品结束"+slotsIndex);
        
        GameObject parent = bagPanel.slotsList[slotsIndex];
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = parent.transform.position;

        //把能接受射线的功能开启，保证下次能够拖动
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        BagItem bagItem = eventData.pointerDrag.GetComponent<BagItem>();
        Debug.Log(bagItem.itemInfo.id);
        
        if (bagItem != null && bagItem.slotsIndex != slotsIndex)
        {
            int tempslotindex = slotsIndex;
            //被换的物品信息
            Transform beItem = transform;
            slotsIndex = bagItem.slotsIndex;
            //把被换的物品移动到原来拖动物品的父节点下
            beItem.SetParent(bagPanel.slotsList[bagItem.slotsIndex].transform);
            beItem.position = beItem.parent.position;
            //交换两个物品的信息，拖动物品的位置在OnEndDrag函数中设置
            bagPanel.itemInfoList[bagItem.slotsIndex] = itemInfo;
            bagItem.slotsIndex = tempslotindex;
            bagPanel.itemInfoList[tempslotindex] = bagItem.itemInfo;
        }
    }
}
