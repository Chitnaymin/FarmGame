using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionManager : SingletonBehaviour<ProductionManager> {

    public GameObject ItemPrefab;// prefab with ProductionViewInfo
    public GameObject ItemPrefabForReq;// prefab with ProductionViewInfo
    public Transform ProductionList;

    public GameObject ProductionCanvas;
    public Text Title;
    public Image ItemImage;
    public Image[] Rank;
    public Text ProductionTime;
    public Text Experiance;

    public Text Deception;
    public Transform RequirementHolder;
    public Text Cost;
    public Button ManufactureBtn;
    public Sprite nullImage;
    public Text CounterText;

    private int requireItemCounter = 1;
    private Building selectedBuilding;
    private GameObject selectedBlock;
    private Inventory[] inventoryItems;
    private List<Inventory> FullItemList = new List<Inventory>();  // Load from Building when clicking
    private List<Production> ProductionsList = new List<Production>();
    private int maxProductionCount = 1;
    Inventory selectedInv;
    PlayerStorageManager pdbsm;
    StorageManager db;

    public override void Awake() {
        base.Awake();
    }

    void Start() {
        inventoryItems = Resources.LoadAll<Inventory>("ItemObjects");
        pdbsm = new PlayerStorageManager();
        db = new StorageManager();
        ProductionsList = pdbsm.CheckingProgressProduction();
        StartCoroutine(CreateProduction());
        //if (ProductionsList.Count > 0) { ShowItemInfo(inventoryItems[0]); }
    }

    IEnumerator CreateProduction() {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < ProductionsList.Count; i++) {
            GameObject block = BuildManager.Instance().GetBlock(ProductionsList[i].GetDummyBlockID());
            block.transform.GetChild(0).gameObject.GetComponent<Production>().SettingAllData(ProductionsList[i]);
        }
    }

    public void ProductionStatusChange(int BlockID, int ProductionProcess, int ProductionItem, int ProductionCount, string ProductionStartTime) {
        if (ProductionProcess == 0) {
            InventoryManager.Instance().AddingItems(ProductionItem, ProductionCount);
        } else if (ProductionProcess == 1) {
            // not allow to produce any more
        }
        pdbsm.UpdateStatusProduction(BlockID, ProductionProcess, ProductionItem, ProductionCount, ProductionStartTime);
    }
    public void UpdateProductionStartingTime(Production productionInfo) {
        pdbsm.UpdateProductionStartingTime(productionInfo.GetProductionStartTime(), productionInfo.GetDummyBlockID());
    }

    public void ShowProductionView(GameObject Block) { // called by btn
        resetingCounters();

        if (Block.transform.GetChild(0).GetComponent<Building>().GetLevel() > 1) {
            Shop_Building sbItem = UpgradeManager.Instance().SearchingUpgradedBuilding(Block.transform.GetChild(0).GetComponent<Building>().GetBuildingID(), Block.transform.GetChild(0).GetComponent<Building>().GetLevel());
            maxProductionCount = sbItem.ProductionCap;
        } else {
            Shop_Building sbItem = ShopManager.Instance().SearchingBuilding(Block.transform.GetChild(0).GetComponent<Building>().GetBuildingID());
            maxProductionCount = sbItem.ProductionCap;
        }

        selectedBlock = Block;
        selectedBuilding = Block.transform.GetChild(0).gameObject.GetComponent<Building>();
        ProductionCanvas.SetActive(true);
        DestoryItem();
        FullItemList.Clear();
        List<int> IDlst = db.GetProductableItems(selectedBuilding.GetBuildingID());   // obtain from productatable itemlist 
        for (int i = 0; i < IDlst.Count; i++) {
            FullItemList.Add(new Inventory(IDlst[i]));
        }
        CreateItem();
        ShowItemInfo(FullItemList[0]); //augument exception တတ်တာကဘာမှထုတ်ဖို့မပေးထားလို့
    }

    private void SelectionShow(int id) {
        for (int i = 0; i < ProductionList.childCount; i++) {
            if (id == i) {
                ProductionList.GetChild(i).GetComponent<InvItem>().setSelection(true);
            } else {
                ProductionList.GetChild(i).GetComponent<InvItem>().setSelection(false);
            }
        }
    }
    List<GameObject> itemGoList = new List<GameObject>(); // created gameobject list dynamic create
    private void CreateItem() {
        foreach (Inventory ele in FullItemList) {
            ele.SetInventoryItem(InvFind(ele.ID));
            GameObject itemGO = Instantiate(ItemPrefab) as GameObject;
            itemGO.transform.SetParent(ProductionList, false);
            itemGoList.Add(itemGO);
            BindtoItems(itemGO, ele);
        }
    }

    private void DestoryItem() {
        itemGoList.Clear();
        for (int j = 0; j < ProductionList.childCount; j++) {
            Destroy(ProductionList.GetChild(j).gameObject);
        }
        HideItemInfo();
    }

    // bind view
    public void ShowItemInfo(Inventory inv) {
        selectedInv = inv;
        LanguageManager.Instance().SetLangText(Title, selectedInv.ItemNameID);

        ItemImage.sprite = selectedInv.Image;
        settingRank(selectedInv.Rank);

        ProductionTime.text = selectedInv.ProductionTime.ToString();
        Experiance.text = selectedInv.Experience.ToString();
        LanguageManager.Instance().SetLangText(Deception, selectedInv.DesceptionTextID);

        settingRequirement(selectedInv.ID);
        SelectionShow(FullItemListFind(inv.ID));
    }
    void HideItemInfo() {
        selectedInv = null;
        Title.text = "";
        ItemImage.sprite = nullImage;
        settingRank(0);

        ProductionTime.text = "0";
        Experiance.text = "0";

        Deception.text = "";
        settingRequirement(0);
        Cost.text = "0";
        ManufactureBtn.interactable = false;
    }
    void settingRank(int lvl) {
        for (int i = 0; i < Rank.Length; i++) {
            Rank[i].enabled = false;
        }
        for (int i = 0; i < lvl; i++) {
            Rank[i].enabled = true;
        }
    }
    List<DependentItem> DependentItemlist;
    bool Producable = true;
    void settingRequirement(int itemID) {
        Producable = true;
        for (int j = 0; j < RequirementHolder.childCount; j++) {
            Destroy(RequirementHolder.GetChild(j).gameObject);
        }
        DependentItemlist = db.GetItemDependencyList(itemID);
        if (DependentItemlist[0].GetRequireID() == 0 || DependentItemlist == null) {
            DependentItemlist.Clear();
            // hide all requirement list
        } else {
            for (int i = 0; i < DependentItemlist.Count; i++) {
                GameObject itemGO = Instantiate(ItemPrefabForReq) as GameObject;
                itemGO.transform.SetParent(RequirementHolder, false);

                BindtoItems(itemGO, InvFind(DependentItemlist[i].GetRequireID()));
                int haveingItem = 0;
                try {
                    haveingItem = InventoryManager.Instance().FullitemFind(DependentItemlist[i].GetRequireID()).GetInventoryCount();
                } catch (System.ArgumentOutOfRangeException e) {
                    haveingItem = 0;
                }
                int showReqItemCount = DependentItemlist[i].GetRequireCount() * requireItemCounter;
                itemGO.GetComponent<InvItem>().ExtraCountBindView(showReqItemCount, haveingItem);

                // hold to all true that depened item
                if (Producable) {
                    Producable = InventoryManager.Instance().CheckingToProduce(DependentItemlist[i].GetRequireID(), showReqItemCount);
                }

                double calculatedUnitPrice = selectedInv.UnitPrice - (selectedInv.UnitPrice * TechnicalManager.Instance().GetEvolutionCurrentPercentage() / 100);
                calculatedUnitPrice = Math.Round(calculatedUnitPrice, 2);
                Cost.text = (calculatedUnitPrice * requireItemCounter).ToString();
                if (calculatedUnitPrice * requireItemCounter > PlayerData.Instance().GetCoin()) { // ပိုက်ဆံစစ်ပြီး မလောက်ရင် fasle Producable
                    Producable = false;
                }
                ManufactureBtn.interactable = Producable;
            }
        }
    }

    void BindtoItems(GameObject item, Inventory inv) {
        item.GetComponent<InvItem>().BindView(inv, true);
    }

    public Inventory InvFind(int id) { // this id get from db
        int i;
        for (i = 0; i < inventoryItems.Length; i++) {
            if (inventoryItems[i].ID == id) {
                break;
            }
        }
        return inventoryItems[i];
    }
    public int FullItemListFind(int id) { // searching in related building
        int i;
        for (i = 0; i < FullItemList.Count; i++) {
            if (FullItemList[i].ID == id) {
                break;
            }
        }
        return i;
    }
    public void Plus() { // btn called 
        if (requireItemCounter < maxProductionCount ) { // Check condition form level of buiding
            requireItemCounter += 1;
            CounterText.text = requireItemCounter.ToString();
            settingRequirement(selectedInv.ID);
            // btn စစ်ပါ လိုအပ်သလောက်မှရိ

        }
    }
    public void Minus() {// btn called  
        if (requireItemCounter > 1) {
            requireItemCounter -= 1;
            CounterText.text = requireItemCounter.ToString();
            settingRequirement(selectedInv.ID);
        }
    }
    //UI button click 
    public void Manufacturing() {
        selectedBlock.transform.GetChild(0).gameObject.GetComponent<Production>().Produce(selectedBuilding.GetBlockID(), selectedInv, requireItemCounter);
        if (DependentItemlist.Count > 0) {
            for (int i = 0; i < DependentItemlist.Count; i++) {
                RemovingItems(DependentItemlist[i].GetRequireID(), DependentItemlist[i].GetRequireCount() * requireItemCounter);
            }
        }
        double calculatedUnitPrice = selectedInv.UnitPrice - (selectedInv.UnitPrice * TechnicalManager.Instance().GetEvolutionCurrentPercentage() / 100);
        calculatedUnitPrice = Math.Round(calculatedUnitPrice, 2);
        PlayerData.Instance().SetCoin(PlayerData.Instance().GetCoin() - (int)(calculatedUnitPrice * requireItemCounter));
        ShopManager.Instance().ShowCoin();
        ProductionCanvas.SetActive(false);
        resetingCounters();
    }

    void RemovingItems(int itemID, int count) {
        InventoryManager.Instance().RemovingItems(itemID, count);
    }

    public void DeleteClone() { // call by btn
        for (int i = 0; i < ProductionList.childCount; i++) {
            Destroy(ProductionList.GetChild(i).gameObject);
        }
        resetingCounters();
    }

    public void resetingCounters() { // call for closing actions close btn and manufacturing btn
        CounterText.text = "1";
        requireItemCounter = 1;
    }
}
