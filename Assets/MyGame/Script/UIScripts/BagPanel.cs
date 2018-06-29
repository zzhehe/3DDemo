using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BagPanel : MonoBehaviour {
    //物品模板
    public GameObject itemPanel;
    //物品格子模板
    public GameObject itemTemp;
    //物品的父节点
    public Transform itemParent;
    
    
    //存储背包slot（对象池）
    public List<GameObject> slotsList;
    //存储背包slot对应的item信息
    private List<ItemInfo> itemInfoList;
    //物品详细页面
    public ItemDetailPanel detailPanel;

    //
    public RectTransform leftPoint;
    public RectTransform rightPoint;

    //要移动到的点
    public float leftToPos;
    public float rightToPos;

    //面板
    public RectTransform leftPanel;
    public RectTransform rightPanel;
    

    private void Awake()
    {

        leftToPos = leftPanel.localPosition.x;
        rightToPos = rightPanel.localPosition.x;

        leftPanel.gameObject.SetActive(false);
        rightPanel.gameObject.SetActive(false);

        leftPanel.localPosition = leftPoint.localPosition;
        rightPanel.localPosition = rightPoint.localPosition;
    }
    // Use this for initialization
    void Start () {
        Tweener leftTweener = leftPanel.DOLocalMoveX(leftToPos, 0.5f);
        leftTweener.OnStart(() => { leftPanel.gameObject.SetActive(true); });

        Tweener rightTweener = rightPanel.DOLocalMoveX(rightToPos, 0.5f);
        rightTweener.OnStart(() => { rightPanel.gameObject.SetActive(true); });

        //防止自动销毁
        leftTweener.SetAutoKill(false);
        leftTweener.Pause();

        rightTweener.SetAutoKill(false);
        rightTweener.Pause();

        ShowRightPanel(true);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowRightPanel(false);
            ShowLeftPanel(false);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ShowRightPanel(true);
        }
    }

    public void ShowLeftPanel(bool isShow)
    {
        if (isShow)
        {
            leftPanel.DOPlayForward();
        }
        else
        {
            leftPanel.DOPlayBackwards();
        }
    }

    public void ShowRightPanel(bool isShow)
    {
        if (isShow)
        {
            rightPanel.DOPlayForward();
        }
        else
        {
            rightPanel.DOPlayBackwards();
        }
    }

    private void OnEnable()
    {
        ShowItemsFromData(EItemType.EIT_Chip);

    }

    public void ShowItemsFromData(EItemType type)
    {
        //数据库中所有的背包物品
        var list = BagManager.Instance.items;
        if (list == null)
        {
            return;
        }
        if (slotsList == null)
        {
            slotsList = new List<GameObject>();
        }
        if (itemInfoList == null)
        {
            itemInfoList = new List<ItemInfo>();
        }
        int itemCount = 0;
        for (int i = 0; i < 30; i++)
        {
            //对象池中如果对象不够则生成对象,简单的对象池
            GameObject itemObj = null;
            if (itemCount < slotsList.Count)
            {
                itemObj = slotsList[itemCount];
            }
            else
            {
                itemObj = CreateItemFromTemplate();
                slotsList.Add(itemObj);
                itemInfoList.Add(new ItemInfo());
            }
            //上面把所有格子都置为id为-1的空格子
            //如果道具为空就不显示
            //var bagItem = itemObj.GetComponentInChildren<BagItem>();
            //itemObj.SetActive(true);
            //if (bagItem == null)
            //{
            //    itemObj.SetActive(false);
            //}
            //可以尝试在这儿加一个Button组件，然后用lambda表达式确定点击事件
            //也可以使用本示例的Event Trigger添加事件

            ++itemCount;
        }

        //对象池中多余的对象置为false
        //for (; itemCount < slotsList.Count; ++itemCount)
        //{
        //    slotsList[itemCount].SetActive(false);
        //}//这里格子数量固定为30个

        //吧list中的物品加入到slotsList对象格子里
        foreach (var item in list)
        {
            AddItemToSlot(item);
        }

    }

    public void AddItemToSlot(ItemInfo itemInfo)
    {
        for (int i = 0; i < 30; i++)
        {
            if (itemInfoList[i].id == -1)
            {
                GameObject item = Instantiate(itemPanel, slotsList[i].transform);
                BagItem bagItem = item.GetComponent<BagItem>();
                bagItem.SetData(itemInfo);
                bagItem.slotsIndex = i;
                itemInfoList[i] = bagItem.itemInfo;
                break;
            }
        }
    }

    /// <summary>
    /// 在父节点下创建预制体
    /// </summary>
    /// <returns></returns>
    public GameObject CreateItemFromTemplate()
    {
        GameObject item = Instantiate(itemTemp, itemParent);
        return item;
    }

    public void ShowItemButton()
    {
        ShowItemsFromData(EItemType.EIT_Chip);
    }

    public void ShowEquipButton()
    {
        ShowItemsFromData(EItemType.EIT_Equip);
    }

    public void ShowChipButton()
    {
        ShowItemsFromData(EItemType.EIT_Chip);
    }



    public void ItemClick(BagItem item)
    {
        detailPanel.SetData(item);
        ShowLeftPanel(true);
    }
}
