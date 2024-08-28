using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : SingletonBehaviour<InventoryManager> {

    public GameObject InventoryCanvas;

    public GameObject itemPrefab;
    public ScrollRect scrollRect;

    public Text ItemName;
    public Image ItemImage;
    public Text Rank;
    public Text Price;
    public Text Count;
    public Text Desception;
    public Text Increasing;
    public GameObject SellingPanel;
    public Text SellingInfotxt;
    public GameObject[] OptionListGO;
    public GameObject[] OptionBtnGO;

    public Image[] stars;
    public Text Adjustment;
    public Button BtnSellGameObject;

    public Sprite Selection;
    public Sprite NullImage;

    public InvItem SelectedItem;

    Inventory[] inventoryItems; // Load from Scriptable Object
    Inventory selectedInv;

    List<Inventory> FullItemList; // Load from Database or dynamic Create
    List<GameObject> itemGoList = new List<GameObject>(); // created gameobject list dynamic create
    List<Inventory> categorizedInv;

    Buttonbtn activeButton;
    GameObject clickekBtn;
    bool isClickedAll;
    PlayerStorageManager pdbsm = new PlayerStorageManager();

    void Start() {
        InventoryCanvas.SetActive(true);
        inventoryItems = Resources.LoadAll<Inventory>("ItemObjects");

        FullItemList = ItemObtainFromDatabase();
        isClickedAll = true;
        CreateItem();
        if (FullItemList.Count > 0) { ShowItemInfo(FullItemList[0]); }
        activeButton = OptionBtnGO[0].GetComponent<Buttonbtn>();
        activeButton.Click();
        ItemInfoSetup(true);
        InventoryCanvas.SetActive(false);
    }


    private List<Inventory> ItemObtainFromDatabase() {
        List<Inventory> IVlst = new List<Inventory>();
        IVlst = pdbsm.GetInventoryFromDatabase();
        return IVlst;
    }

    private void CreateItem() {
        foreach (GameObject ele in OptionListGO) {
            ele.SetActive(true);
        }
        categorizedInv = new List<Inventory>();
        if (isClickedAll) {
            categorizedInv = FullItemList;
        } else {
            for (int i = 0; i < FullItemList.Count; i++) {
                if (clickekBtn.name.ToString() == "Option2btn") {
                    if (FullItemList[i].Category == "Material") {
                        categorizedInv.Add(FullItemList[i]);
                    }
                } else if (clickekBtn.name.ToString() == "Option3btn") {
                    if (FullItemList[i].Category == "Product") {
                        categorizedInv.Add(FullItemList[i]);
                    }
                } else {
                    if (FullItemList[i].Category == "Other") {
                        categorizedInv.Add(FullItemList[i]);
                    }
                }
            }
        }

        foreach (Inventory ele in categorizedInv) {
            if (ele.GetInventoryCount() > 0) {
                ele.SetInventoryItem(InvFind(ele.ID));
                GameObject itemGO = Instantiate(itemPrefab) as GameObject;
                BindtoItems(itemGO, ele);
                itemGoList.Add(itemGO);
                if (isClickedAll) {
                    itemGO.transform.SetParent(GameObject.Find("All").transform, false);
                } else {
                    itemGO.transform.SetParent(GameObject.Find(ele.Category).transform, false);// set item to the related parent }
                }
            }
        }

        foreach (GameObject ele in OptionListGO) {
            ele.SetActive(false);
        }
        foreach (GameObject ele in OptionBtnGO) {
            ele.GetComponent<Buttonbtn>().ReClicked();
        }
    }

    void DeleteAllItem() {
        foreach (GameObject ele in itemGoList) {
            ele.GetComponent<InvItem>().setSelection(false);
        }
        itemGoList.Clear();
        string[] OptionList = { "All", "Material", "Product", "Other" };
        for (int i = 0; i < OptionList.Length; i++) {
            GameObject gt = OptionListGO[i];
            for (int j = 0; j < gt.transform.childCount; j++) {
                Destroy(gt.transform.GetChild(j).gameObject);
            }
        }
    }

    public Inventory InvFind(int id) { // this id get from db to find all scriptable object
        int i;
        for (i = 0; i < inventoryItems.Length; i++) {
            if (inventoryItems[i].ID == id) {
                break;
            }
        }
        return inventoryItems[i];
    }
    public Inventory FullitemFind(int id) {
        int i;
        for (i = 0; i < FullItemList.Count; i++) {
            if (FullItemList[i].ID == id) {
                break;
            }
        }
        return FullItemList[i]; // ရှာလိုက်တဲ့ item က မတွေ့ရင် arguemnt exception တတ်တယ်
    }
    Inventory FullitemFindByCatagory(string catagory) {
        if (catagory == "All" && FullItemList.Count > 0) {
            return FullItemList[0];
        }

        int i;
        for (i = 0; i < FullItemList.Count; i++) {
            if (FullItemList[i].Category == catagory) {
                break;
            }
        }
        try {
            return FullItemList[i];
        } catch (System.ArgumentOutOfRangeException e) {
            return null;
        }
    }

    void BindtoItems(GameObject item, Inventory inv) {
        item.GetComponent<InvItem>().BindView(inv);
    }

    void ItemInfoSetup(bool start = false) {
        if (FullItemList.Count > 0) {
            if (start) {
                ShowItemInfo(FullItemList[0]);
            } else {
                ShowItemInfo(FullitemFindByCatagory(activeButton.GetComponent<Buttonbtn>().optionList.name));
            }
            if (categorizedInv.Count > 0) {
                if (SelectedItem != null) {
                    SelectedItem.setSelection(false);
                }
                SelectedItem = itemGoList[0].GetComponent<InvItem>();
                itemGoList[0].GetComponent<InvItem>().setSelection(true);
            }
        } else { HideItemInfo(); }
    }
    // public called by single items
    public void ShowItemInfo(Inventory inv) {
        if (inv == null) { HideItemInfo(); return; }
        selectedInv = inv;
        BtnSellGameObject.interactable = selectedInv.allowSell;

        Adjustment.text = "0";
        LanguageManager.Instance().SetLangText(ItemName, selectedInv.ItemNameID);
        SettingRank(selectedInv.Rank);
        
        string bonusMultiplyerStr = "";
        float bonusMultiplyerFlt = 1.0f;

        if (selectedInv.Category == "Product" && ADManager.Instance().GetRelatedAction(9)) {
            bonusMultiplyerStr = " 120%UP";
            bonusMultiplyerFlt = 1.2f;
        }

        if (selectedInv.allowSell) {
            float calculatedSellPrice = (selectedInv.UnitPrice + (selectedInv.UnitPrice * TechnicalManager.Instance().GetQualityCurrentPercentage() / 100));
            float ADCalculatedSellPrice = calculatedSellPrice * bonusMultiplyerFlt;
            int rountedCalculatedSellPrice = (int)Mathf.Round(ADCalculatedSellPrice);
            Price.text = LanguageManager.Instance().SearchTextList(10212) + " " + rountedCalculatedSellPrice.ToString() + bonusMultiplyerStr;
        } else {
            Price.text = LanguageManager.Instance().SearchTextList(10212) + " 0";
        }

        ItemImage.sprite = selectedInv.Image;
        Count.text = LanguageManager.Instance().SearchTextList(10213) + " " + selectedInv.GetInventoryCount().ToString();
        LanguageManager.Instance().SetLangText(Desception,selectedInv.DesceptionTextID);
    }
    void HideItemInfo() {
        selectedInv = null;
        Adjustment.text = "0";
        ItemName.text = "No Item Selected";
        SettingRank(0);
        Price.text = "0 $";
        ItemImage.sprite = NullImage;
        Count.text = "Count: 0";
        Desception.text = "";
    }
    void SettingRank(int lvl) {
        for (int i = 0; i < stars.Length; i++) {
            stars[i].enabled = false;
        }
        for (int i = 0; i < lvl; i++) {
            stars[i].enabled = true;
        }
    }
    // UI buttons action events
    public void Minus() {
        int textCount = int.Parse(Adjustment.text.ToString());
        if (textCount > 0) {
            Adjustment.text = (textCount - 1).ToString();
        }
    }
    public void Plus() {
        int textCount = int.Parse(Adjustment.text.ToString());
        if (textCount < selectedInv.GetInventoryCount()) {
            Adjustment.text = (textCount + 1).ToString();
        }
    }
    public bool CheckingToProduce(int itemID, int count) {
        Inventory item = new Inventory();
        try {
            item = FullitemFind(itemID);
        } catch (System.ArgumentOutOfRangeException e) {
            return false;
        }

        if (item.GetInventoryCount() < count) {
            return false;
        }
        return true;
    }

    public void AddingItems(int itemID, int count) {
        Inventory item = new Inventory(itemID, 0);
        try {
            item = FullitemFind(itemID);
        } catch (System.ArgumentOutOfRangeException e) {
            //Debug.Log("InventoryManager: ArgumentOutOfRangeException: " + e); // checking item or not 
        }
        bool noItemleft = false;
        if (item.GetInventoryCount() == 0) {
            noItemleft = true;
        }
        item.IncreasingItem(count);
        InventoryUpdate(item, noItemleft);
    }
    public void RemovingItems(int itemID, int count) {
        InventoryCanvas.SetActive(true);
        Inventory item = FullitemFind(itemID);
        item.DecreasingItem(count);
        InventoryUpdate(item);
        refresh();
        InventoryCanvas.SetActive(false);
    }

    public void Sell() {
        int textCount = int.Parse(Adjustment.text.ToString());
        selectedInv.DecreasingItem(textCount);
        Adjustment.text = "0";

        float bonusMultiplyerFlt = 1.0f;
        if (selectedInv.Category == "Product" && ADManager.Instance().GetRelatedAction(9)) {
            bonusMultiplyerFlt = 1.2f;
        }

        float calculatedSellPrice = (selectedInv.UnitPrice + (selectedInv.UnitPrice * TechnicalManager.Instance().GetQualityCurrentPercentage() / 100)) * textCount;
        float ADCalculatedSellPrice = calculatedSellPrice * bonusMultiplyerFlt;
        int rountedCalculatedSellPrice = (int)Mathf.Round(ADCalculatedSellPrice);
        PlayerData.Instance().SetCoin(PlayerData.Instance().GetCoin() + rountedCalculatedSellPrice);
        ShopManager.Instance().ShowCoin();
        ADManager.Instance().SetRelatedActionUse(9);

        SellingPanel.SetActive(false);
        SellingInfotxt.text = "";
        InventoryUpdate(selectedInv);
        refresh();
        AchievementManager.Instance().IncreaseProgress(4);
        int Progress = AchievementManager.Instance().pdb.GetCurrentProgress((int)E_AchType.Sell);
        AchievementManager.Instance().MajorAchievementUpdate(E_AchType.Sell, Progress + 1);
        AudioManager.Instance().PlaySound(3);
    }

    public void btnSell() {
        int textCount = int.Parse(Adjustment.text.ToString());
        if (textCount != 0) {
            SellingPanel.SetActive(true);

            float bonusMultiplyerFlt = 1.0f;
            if (selectedInv.Category == "Product" && ADManager.Instance().GetRelatedAction(9)) { 
                bonusMultiplyerFlt = 1.2f;
            }

            float calculatedSellPrice = (selectedInv.UnitPrice + (selectedInv.UnitPrice * TechnicalManager.Instance().GetQualityCurrentPercentage() / 100)) * textCount;
            float ADCalculatedSellPrice = calculatedSellPrice * bonusMultiplyerFlt;
            int rountedCalculatedSellPrice = (int)Mathf.Round(ADCalculatedSellPrice);
            SellingInfotxt.text = LanguageManager.Instance().SearchTextList(5)+ " <color=red>" + textCount +
                                    "</color> <color=yellow>" + LanguageManager.Instance().SearchTextList(selectedInv.ItemNameID) +
                                    "</color><color=white>(" + rountedCalculatedSellPrice + LanguageManager.Instance().SearchTextList(10)+")</color>?";
        } else {
            Debug.Log("Sorry!");
        }
    }

    public void btnCancel() {
        SellingPanel.SetActive(false);
    }

    public void InventoryUpdate(Inventory itemToUpdate, bool insertItem = false) {
        if (itemToUpdate.GetInventoryCount() == 0) {
            FullItemList.Remove(selectedInv);
            pdbsm.DeleteInventoryData(selectedInv.ID);
            refresh();
        } else {
            if (insertItem) {
                pdbsm.InsertInventoryData(itemToUpdate.ID, itemToUpdate.GetInventoryCount());
				FullItemList = pdbsm.GetInventoryFromDatabase();
				// db ထဲမှာလိုင်းအသစ် တိုးရင် list ကို ထည့်ဖြစ် ပေးရမယ် 
			} else {
                pdbsm.UpdateInventoryData(itemToUpdate.ID, itemToUpdate.GetInventoryCount());
            }
        }
    }
    public void refresh() {
        DeleteAllItem(); //remove form list
        CreateItem(); // recreate all items
        ItemInfoSetup();
    }

    public void ChangeCategory(GameObject button) { // calledby btn
                                                    // if clicked btn is all == > all = true other false 
        clickekBtn = button;
        if (button.name == "Option1btn") {
            isClickedAll = true;
        } else {
            isClickedAll = false;
        }
        refresh();
        Buttonbtn highlightableButton = button.GetComponent<Buttonbtn>();
        scrollRect.content = highlightableButton.optionList.GetComponent<RectTransform>();
        highlightableButton.Click();
        activeButton.Click();
        activeButton = highlightableButton;
        ShowItemInfo(FullitemFindByCatagory(activeButton.GetComponent<Buttonbtn>().optionList.name));
    }
}