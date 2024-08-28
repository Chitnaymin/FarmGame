using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : SingletonBehaviour<ShopManager> {
    PlayerStorageManager pdb;
    public Text txtCoin;
    private int originExp;
    public GameObject shopUI;
    public GameObject CategoryPanel;
    public ScrollRect scrollRect;
    private ShopButton activeButton;

    //Arrays for scriptable objects
    public static Shop_Building[] buildingItems;
    List<GameObject> child_category = new List<GameObject>();
    public GameObject[] CategoryGOList;

    //Shop Prefab
    public GameObject categoryPrefab;
    public override void Awake() {
        base.Awake();
    }
    void Start() {
        pdb = new PlayerStorageManager();
        originExp = pdb.GetExperienceFromDB();
        txtCoin.text = pdb.GetCoinFromDB().ToString();
        buildingItems = Resources.LoadAll<Shop_Building>("Buildings");
        StartCoroutine(DelayCreate_Building());
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            RefreshTab();
        }
    }

    IEnumerator DelayCreate_Building() { // must wait for database 
        yield return new WaitForSeconds(1);
        shopUI.SetActive(true);
        activeButton = GameObject.Find("btnBuild").GetComponent<ShopButton>();
        Create_Building();
        foreach (GameObject categoryList in CategoryGOList) {
            categoryList.SetActive(false);
        }
        activeButton.Click();
        ChechFromManager();
        shopUI.SetActive(false);
    }

    public Shop_Building SearchingBuilding(int id) {
        for (int i = 0; i < buildingItems.Length; i++) {
            if (buildingItems[i].id == id) {
                return buildingItems[i];
            }
        }
        return null;
    }

    public void ChechFromManager() {//Checking Coin from db 
        for (int i = 0; i < child_category.Count; i++) {
            child_category[i].GetComponent<BuyItemScript>().Check_Price(PlayerData.Instance().GetCoin());
        }
    }// another called by clicking shop btn

    public int GetBuildingDuration(int id) {
        foreach (Shop_Building ele in buildingItems) {
            if (ele.id == id) {
                return ele.time;
            }
        }
        return 0;
    }

    public void Create_Building() {//Building Function
        foreach (GameObject go in CategoryGOList) {
            go.SetActive(true);
        }
        int i;
        buildingItems = buildingItems.OrderBy(x => x.id).ToArray();
        for (i = 0; i < buildingItems.Length; i++) {
            if (buildingItems[i].category_tag == Shop_Building.Category.Build) {
                CreateBuilding("Building", buildingItems[i]);
            }
            if (buildingItems[i].category_tag == Shop_Building.Category.Field) {
                CreateBuilding("Field", buildingItems[i]);
            }
            if (buildingItems[i].category_tag == Shop_Building.Category.Tree) {
                CreateBuilding("Tree", buildingItems[i]);
            }
            if (buildingItems[i].category_tag == Shop_Building.Category.Expansion) {
                CreateBuilding("Expansion", buildingItems[i]);
            }
            if (buildingItems[i].category_tag == Shop_Building.Category.Pergola) {
                CreateBuilding("Pergola", buildingItems[i]);
            }
        }

        foreach (GameObject go in CategoryGOList) {
            go.SetActive(false);
        }
    }
    //Click btn_buy
    public void BuyItem(int price, int exp, float costMuiltiplyer = 1.0f) {
        price = (int)(price * costMuiltiplyer);
        int originCoin = PlayerData.Instance().GetCoin();
        if (price <= originCoin) {
            originCoin -= price;
            PlayerData.Instance().SetCoin(originCoin);
            originExp = exp;
            txtCoin.text = originCoin.ToString();
            GameManager.Instance().ProgressUpdate(originExp);
            ChechFromManager();
        } else if (price > originCoin) {
            shopUI.SetActive(false); // never happen since checked already for btn active အဲဒါများစစ်ထားသေးတယ် 
        }
    }

    public void CloseShopPanel() {
        if (shopUI.activeInHierarchy) {
            shopUI.SetActive(false);
        }
        GameManager.Instance().ForceUIInv();
    }

    public void BtnCloseShopPanel() { // call by btn shopcanvas
        if (shopUI.activeInHierarchy) {
            shopUI.SetActive(false);
        }
    }

    public void CreateBuilding(string parent, Shop_Building building) {
        GameObject category = Instantiate(categoryPrefab) as GameObject;
        category.transform.SetParent(GameObject.Find(parent).transform, false);
        category.GetComponent<BuyItemScript>().BindForBuilding(building);
        child_category.Add(category);
    }

    public void ChangeCategory(GameObject button) {
        ShopButton shopButton = button.GetComponent<ShopButton>();
        scrollRect.content = shopButton.categoryList.GetComponent<RectTransform>();
        shopButton.Click();
        activeButton.Click();
        activeButton = shopButton;
    }
    public void ShowShopCanvas() {
        shopUI.SetActive(true);
        RefreshBuildingTab();
        ForceActive();
    }
    public void ForceActive() {
        activeButton = GameObject.Find("btnBuild").GetComponent<ShopButton>();
        activeButton.Click();
        scrollRect.content = activeButton.categoryList.GetComponent<RectTransform>();
    }

    public void ShowCoin() {
        txtCoin.text = PlayerData.Instance().GetCoin().ToString();
    }

    public void CustomizedTab(Building building) {
        if (building.GetBuildingID() == 1) {
            GameObject go_field = GameObject.Find("btnField") as GameObject;
            ChangeCategory(go_field);
        } else if (building.GetBuildingID() == 2) {
            GameObject go_tree = GameObject.Find("btnTree") as GameObject;
            ChangeCategory(go_tree);
        } else if (building.GetBuildingID() == 3) {
            GameObject go_pergola = GameObject.Find("btnPergola") as GameObject;
            ChangeCategory(go_pergola);
            //RefreshBuildingTab();
            //ShopButton shop_pergola = GameObject.Find("btnPergola").GetComponent<ShopButton>();
            //shop_pergola.Click();
        }
    }

    public void RefreshBuildingTab() {
        ShopButton[] shopButtons = CategoryPanel.GetComponentsInChildren<ShopButton>();
        for (int i = 0; i < shopButtons.Length; i++) {
            shopButtons[i].Refresh();
        }
    }
    public void DeleteShopItems() {
        //Array.Clear(child_category, 0, 20);
        child_category.Clear();
        string[] CategoryList = { "Building", "Field", "Tree", "Expansion", "Pergola" };
        // = GameObject.FindGameObjectsWithTag("CategoryList");
        for (int i = 0; i < CategoryList.Length; i++) {
            GameObject tempGO = CategoryGOList[i];
            for (int j = 0; j < tempGO.transform.childCount; j++) {
                Destroy(tempGO.transform.GetChild(j).gameObject);
            }
        }
    }
    void refresh() {
        DeleteShopItems();
        Create_Building();
        ChechFromManager();
        ForceActive();
    }

    public void RefreshTab() {
        shopUI.SetActive(true);
        refresh();
        CloseShopPanel();
    }

    public void BtnCancelDirectComfirmation() { // call by tab btn and close btn
        DirectComfirmation.DIRECTBUILD = false;
    }
}
