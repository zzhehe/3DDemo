using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailPanel : MonoBehaviour {
    public Text ItemName;
    public Text ItemDesc;

    public Button sellBtn;
    public Button equipbtn;
    public Button resolveBtn;
    public Button compoundBtn;

    public ItemInfo item;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetData(BagItem info)
    {
        item = info.itemInfo;
        ItemName.text = item.name;
        ItemDesc.text = item.description;
        if ((EItemType)item.type==EItemType.EIT_Equip)
        {
            sellBtn.gameObject.SetActive(true);
            equipbtn.gameObject.SetActive(true);
            resolveBtn.gameObject.SetActive(true);
            compoundBtn.gameObject.SetActive(false);
        }
        if ((EItemType)item.type == EItemType.EIT_Chip)
        {
            sellBtn.gameObject.SetActive(true);
            equipbtn.gameObject.SetActive(false);
            resolveBtn.gameObject.SetActive(false);
            compoundBtn.gameObject.SetActive(true);
        }
    }

}
