using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Production : MonoBehaviour {

    private int dummyBlockID;

    private int productionProcess; // 0 means nothing // 1 means in progress // 2 mains complete havnt pick up
    private int productionItem;
    private int productionCount;
    private string productionStartTime;
    private int productionDuration; // in sec
    private float calculatedProductionDuration;

    public GameObject Item;
    public Image ItemImage;
    public GameObject ProgressBarCanvas;
    public GameObject ADButton;

    public Image ProgressImage;
    public Text ProgressText;
    public Text PercentageText;
    public Inventory InventoryItem;

    private int nextUpdate = 0;
    DateTime startProduceTime;

    // to create object from database
    public Production(int dummyBlockID, int productionProcess, int productionItem, int productionCount, string productionStartTime) {
        this.dummyBlockID = dummyBlockID;
        this.productionProcess = productionProcess;
        this.productionItem = productionItem;
        this.productionCount = productionCount;
        this.productionStartTime = productionStartTime;
    }
    // call  by database
    public void SettingAllData(Production production) {
        this.dummyBlockID = production.GetDummyBlockID();
        this.productionProcess = production.productionProcess;
        this.productionItem = production.productionItem;
        this.productionCount = production.productionCount;
        this.productionStartTime = production.productionStartTime;
        if (productionItem != 0) {
            InventoryItem = ProductionManager.Instance().InvFind(productionItem);
            ItemImage.sprite = InventoryItem.Image;
            productionDuration = InventoryItem.ProductionTime;
           
            calculatedProductionDuration = (productionDuration - (productionDuration * TechnicalManager.Instance().GetImprovementCurrentPercentage() / 100)) * UpgradeMultiplier();
        }
        startProduceTime = FunDateTimeAccess.StringToDateTime(productionStartTime);
        if (productionProcess == 0) {
            ProgressBarCanvas.SetActive(false);
            ADButton.SetActive(false);
        } else {
            if (productionProcess == 1) {
                ProgressBarCanvas.SetActive(true);
                ADButton.SetActive(true);
            } else if (productionProcess == 2) {
                Item.SetActive(true);
                ProgressBarCanvas.SetActive(false);
                ADButton.SetActive(false);
            }
            GetComponent<Building>().BuildingActionAllow(false);
            Debug.Log("productionProcess " + productionProcess);
        }
    }

    void Start() {
        productionProcess = 0;
        HideNothingtoProduce();
    }

    void Update() {
        if (Time.time >= nextUpdate && productionProcess == 1) {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }
    }
    private void UpdateEverySecond() {
        TimeSpan differentTime = DateTime.Now - startProduceTime;
        if (differentTime.TotalSeconds < calculatedProductionDuration / ProdutionTimeMultiplyer()) {
            float progressLine = FunProgressCalculator.PercentageCalculator((int)differentTime.TotalSeconds, calculatedProductionDuration / ProdutionTimeMultiplyer());
            ProgressImage.fillAmount = progressLine;

            ProgressText.text = FunProgressCalculator.HourMinSecTransform(differentTime, calculatedProductionDuration / ProdutionTimeMultiplyer());
            PercentageText.text = Mathf.RoundToInt(progressLine * 100).ToString();
            // show progress bar
        } else {
            productionProcess = 2;
            Item.SetActive(true);
            ProgressBarCanvas.SetActive(false);
            ADButton.SetActive(false);
            Debug.Log("productionCount in just done" + productionCount);
            ProductionManager.Instance().ProductionStatusChange(GetDummyBlockID(), productionProcess, productionItem, productionCount, productionStartTime);
            AudioManager.Instance().PlaySound(0);
            // complate to pickupable action
        }
    }
    // start produce 
    public void Produce(int BlockID, Inventory inv, int count) {
        InventoryItem = inv;
        productionCount = count;
        SetDummyBlockID(BlockID);
        startProduceTime = DateTime.Now;
        productionItem = inv.ID;
        productionDuration = inv.ProductionTime;

        calculatedProductionDuration = (productionDuration - (productionDuration * TechnicalManager.Instance().GetImprovementCurrentPercentage() / 100)) * UpgradeMultiplier();

        ItemImage.sprite = inv.Image;
        productionStartTime = FunDateTimeAccess.DateTimeToString(startProduceTime);
        productionProcess = 1;
        ProgressBarCanvas.SetActive(true);
        ADButton.SetActive(true);
        ProductionManager.Instance().ProductionStatusChange(GetDummyBlockID(), productionProcess, productionItem, productionCount, productionStartTime);

        GetComponent<Building>().BuildingActionAllow(false);
    }

    void HideNothingtoProduce() {
        Item.SetActive(false);
        ProgressBarCanvas.SetActive(false);
        ADButton.SetActive(false);
    }
    public void ItemPickUp() {
        if (productionProcess == 2) {
            productionProcess = 0;
            ProductionManager.Instance().ProductionStatusChange(GetDummyBlockID(), productionProcess, productionItem, productionCount, productionStartTime);
            Debug.Log("This item is picked up " + productionItem + "  Count : " + productionCount);
            AchievementManager.Instance().IncreaseProgress(2);
            int Progress = AchievementManager.Instance().pdb.GetCurrentProgress((int)E_AchType.FactoryHarvest);
            GameManager.Instance().ProgressUpdate(InventoryManager.Instance().InvFind(productionItem).Experience);
            AchievementManager.Instance().MajorAchievementUpdate(E_AchType.FactoryHarvest, Progress + 1);
            // item increase in database
            Item.SetActive(false);
            ProgressBarCanvas.SetActive(false);
            ADButton.SetActive(false);
        } else {
            Debug.Log("Unable to pick up: Process = " + productionProcess);
        }
        GetComponent<Building>().BuildingActionAllow(true);
    }

    public int ProdutionTimeMultiplyer() {
        int buildinID = GetComponent<Building>().GetBuildingID();
        int i = 0;
        if (buildinID == 4) { //make fast for cleaner 
            i = 11;
        } else if (buildinID == 5) {//make fast for fermentation 
            i = 12;
        } else if (buildinID == 6) {//make fast for subcontractor 
            i = 13;
        }
        if (ADManager.Instance().ActionWorking(i)) {
            return 2;
        } else {
            return 1;
        }
    }

    float UpgradeMultiplier() {
        float lvlMultiplayer = 1;
        if (GetComponent<Building>().GetLevel() > 1) {
            Shop_Building sbItem = UpgradeManager.Instance().SearchingUpgradedBuilding(GetComponent<Building>().GetBuildingID(), GetComponent<Building>().GetLevel());

            lvlMultiplayer = 1.00f - (sbItem.ReduceTheTime / 100.00f);
            Debug.Log("in side " + sbItem.ReduceTheTime);
        }
        Debug.Log("GetComponent<Building>().GetLevel()" + GetComponent<Building>().GetLevel());
        Debug.Log("lvlMultiplayer" + lvlMultiplayer);
        return lvlMultiplayer;
    }

    public int GetDummyBlockID() {
        return dummyBlockID;
    }

    public void SetDummyBlockID(int value) {
        dummyBlockID = value;
    }

    public int GetProductionProcess() {
        return productionProcess;
    }

    public void SetStartProduceTime(DateTime value) {
        SetProductionStartTime(FunDateTimeAccess.DateTimeToString(value));
        startProduceTime = value;
    }
    public DateTime GetStartProduceTime() {
        return startProduceTime; ;
    }

    public void SetProductionStartTime(string value) {
        productionStartTime = value;
        ProductionManager.Instance().UpdateProductionStartingTime(this);
    }
    public string GetProductionStartTime() {
        return productionStartTime;
    }

    public int GetProductionDuration() {
        return productionDuration;
    }

}

