using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BagItem : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
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
        Debug.Log("开始拖物品");
        if (itemInfo != null)
        {
            gameObject.transform.SetParent(gameObject.transform.parent.parent);
            gameObject.transform.position = Input.mousePosition;
            //把能接受射线的功能关掉
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("正在拖物品");
        if (itemInfo != null)
        {
            gameObject.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("拖物品结束");
        GameObject parent = bagPanel.slotsList[slotsIndex];
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = parent.transform.position;
        //把能接受射线的功能开启，保证下次能够拖动
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

}
