using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Building : MonoBehaviour {
    
    private int blockID;
    private int buildingID;
    private int nameTxtID;
    private string startingTime;
    private int level = 0;
    private int process = 0;
    private bool builingInProgress = false; // false = 1 in database
    private GameObject blockRef;
    private DateTime startBuildTime;
    private int duration; // in secc
    private int price;
    private int exp;
    private int productioncap;
    private int reducetime;
    private Sprite builtSpriteState0; // unbuilt
    private Sprite builtSpriteState1; // Start Build 0.1%
    private Sprite builtSpriteState2; // 50% Building
    private Sprite builtSpriteComplete; // 100%
    private Shop_Building.Category category;
    private int cropableItem;
	private GameObject cropAnim;
    public Text txtBuildingLvl;
    public Text txtBuildingName;

    public GameObject progressionCanvas;
    public GameObject ADButton;
    public Image image;
    public Text progressText;
    public Text percentageText;
    public GameObject halo;
    public GameObject BuildingDetailPanel;
    private SpriteRenderer spriteRD;
    public int[] BonusItemID;

    private int nextUpdate = 0;

    public Building(int blockID, int buildingID, string startingTime, int level, int process) {
        this.blockID = blockID;
        this.buildingID = buildingID;
        this.startingTime = startingTime;
        this.level = level;
        this.process = process;
    }

    void Awake() {
        progressionCanvas.SetActive(false);
        ADButton.SetActive(false);
        spriteRD = GetComponent<SpriteRenderer>();
        halo.GetComponent<SpriteRenderer>().sortingOrder = 100;
    }

    private void Start() {
        ActionDeselect();
        if (process == 0) {
            spriteRD.sprite = builtSpriteState0;
        } else {
            spriteRD.sprite = builtSpriteComplete;
        }
        txtBuildingLvl.text = "";
        txtBuildingName.text = "";
		cropAnim = transform.GetChild(9).gameObject;
    }

    void Update() {
        if (Time.time >= nextUpdate && builingInProgress) {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }
    }

    private void UpdateEverySecond() { // calculate for level
        TimeSpan differentTime = DateTime.Now - startBuildTime;
        if (differentTime.TotalSeconds < duration) {
            float progressLine = FunProgressCalculator.PercentageCalculator((int)differentTime.TotalSeconds, duration);
            if (progressLine > 0.000001f && progressLine <= 0.5f) {
                spriteRD.sprite = builtSpriteState1;
            } else if (progressLine > 0.5 && progressLine < 1) {
                spriteRD.sprite = builtSpriteState2;
            }
            image.fillAmount = progressLine;
            progressText.text = FunProgressCalculator.HourMinSecTransform(differentTime, duration);
            percentageText.text = Mathf.RoundToInt(progressLine * 100).ToString();
            BuildingActionAllow(false);
            process = 0;
        } else {
            spriteRD.sprite = builtSpriteComplete;
            progressionCanvas.SetActive(false);
            ADButton.SetActive(false);
            builingInProgress = false;
            BuildingActionAllow(true);
            process = 1;
            if (buildingID == 7) {
                HideButton.Instance().showLabButton();
            }
            BuildManager.Instance().UpdateProcess(this); // update to database that is already build, which is not in progress
        }
    }

    // Set id here
    public void Build() { // this function is called by building setup click
        builingInProgress = true;
        progressionCanvas.SetActive(true);
        ADButton.SetActive(true);
        startBuildTime = DateTime.Now;
        startingTime = FunDateTimeAccess.DateTimeToString(startBuildTime);
        BuildManager.Instance().SaveFormation(this);  // save to database start here
        spriteRD.sprite = builtSpriteState1;
    }

    // For Upgrade Fun
    public void Upgrade() { // this function is called by building setup click
        builingInProgress = true;
        progressionCanvas.SetActive(true);
        ADButton.SetActive(true);
        startBuildTime = DateTime.Now;
        startingTime = FunDateTimeAccess.DateTimeToString(startBuildTime);
        BuildManager.Instance().UpdateBuildingLevel(this);  // save to database start here
        spriteRD.sprite = builtSpriteState1;
    }

    public void Move(int oldBlockID) {
        ActionDeselect();
        BuildManager.Instance().UpdateFormation(this, oldBlockID);
    }
    public void SettingAllData(int blockID, int buildingID, string startingTime, int level, int process) {
        this.blockID = blockID;
        this.buildingID = buildingID;
        this.startingTime = startingTime;
        this.level = level;
        this.process = process;

        if (level == 1 || level == 0) {
            duration = ShopManager.Instance().GetBuildingDuration(buildingID);
        } else {
            duration = UpgradeManager.Instance().GetBuildingDuration(buildingID, level);
        }

        builingInProgress = process == 0 ? true : false;// set up duration
        StartCoroutine(progressionCanvasActive()); // ခဏစောင့်ပြီးမှ လုပ်မယ်
        startBuildTime = FunDateTimeAccess.StringToDateTime(startingTime);
        insertingBuildingSprites(buildingID);
        AssigningCategory();
        spriteRD.sprite = builtSpriteState0; //calculation UPDATE မှာ လုပ်ပြီးသွားရင် ထပ်ထည့်စ၇ာမလိုတော့ဘူး
        // adding sprite by searching building id from shop data .. return sprite  or ob
    }
    void insertingBuildingSprites(int buildingID) {
        Shop_Building buildingData = ShopManager.Instance().SearchingBuilding(buildingID);
        builtSpriteState0 = buildingData.builtSpriteState0;
        builtSpriteState1 = buildingData.builtSpriteState1;
        builtSpriteState2 = buildingData.builtSpriteState2;
        builtSpriteComplete = buildingData.builtSpriteComplete;
        exp = buildingData.exp;
        category = buildingData.category_tag; // shop type data တစ်ခု ထပ်ပြီးထည့်ရမယ် အဲ့ဒီအတွက် category ပြန်ရှာရမယ် 
        cropableItem = buildingData.cropableItem;
        nameTxtID = buildingData.nameTxtID;
        SetProductioncap(buildingData.ProductionCap);
        SetReducetime(buildingData.ReduceTheTime);
        BonusItemID = buildingData.BonusItemID;
    }

    public void BindToPrefab(int id, Sprite builtSpriteState0, Sprite builtSpriteState1, Sprite builtSpriteState2, Sprite builtSpriteComplete, Shop_Building.Category category, int price, int time, int exp, int lvl, int cropableItem, int nameID, int productioncap, int reducetime, int[] BonusItemID) {
        this.buildingID = id;
        this.exp = exp;
        this.price = price;
        this.level = lvl;
        this.nameTxtID = nameID;
        this.SetProductioncap(productioncap);
        this.SetReducetime(reducetime);
        this.builtSpriteState0 = builtSpriteState0;
        this.builtSpriteState1 = builtSpriteState1;
        this.builtSpriteState2 = builtSpriteState2;
        this.builtSpriteComplete = builtSpriteComplete;
        this.category = category;
        AssigningCategory();
        duration = time;
        this.cropableItem = cropableItem;
        this.BonusItemID = BonusItemID;
    }

    public void BuyFromClick() { // တကယ် အမှန်ခြစ် နှိပ်ပြီးဆောက်လုပ်တဲ့ နေရာ
        if (ADManager.Instance().GetRelatedAction(8) && (buildingID == 4 || buildingID == 5 || buildingID == 6)) {// AD က ထွက်လာတဲ့ တန်ဖိုးပေါ်မူတည်ပြီး စျေး ကို free လား တဝက်လား ဆုံးဖြတ်ပေးရမယ် 0 or 0.5
            ShopManager.Instance().BuyItem(price, exp, 0.5f);
            ADManager.Instance().SetRelatedActionUse(8);
        } else if (ADManager.Instance().GetRelatedAction(5) && (category == Shop_Building.Category.Field
               || category == Shop_Building.Category.Pergola
               || category == Shop_Building.Category.Tree)) {
            ShopManager.Instance().BuyItem(price, exp, 0);
            ADManager.Instance().SetRelatedActionUse(5);
        } else {
            ShopManager.Instance().BuyItem(price, exp); // buy animation might show ( money decreasing                
        }

        if (buildingID == 7) {
            BuildManager.Instance().LabisBuilding = true;
        }
        ShopManager.Instance().RefreshTab();
    }

    IEnumerator progressionCanvasActive() {
        yield return new WaitForSeconds(0.2f);
        progressionCanvas.SetActive(builingInProgress);
        ADButton.SetActive(builingInProgress); // Database က ထွက်လာရင် AD btn မပေါ်တာ Production က လည်း ကိုင်ပြီးဖျောက်ထားလို့ --- ၂ ခု ထားလိုက်ပါ 

    }

    public void ActionSelect() {
        halo.SetActive(true);
        BuildingDetailPanel.SetActive(true);
        
        // ပြင်ပြီးသွားပြီ 
        // collectingFruit(); // collection လုပ်လို့ရလား မရလား ကို game manager က စစ်ပြီးတော့ မှ state တခု အနေနဲ့ ဝင်သင့် မဝင်သင့် ပြန် စစ် 

        if (buildingID == 4 || buildingID == 5 || buildingID == 6) {
            txtBuildingLvl.text = "Lv." + GetLevel().ToString(); // စတာနဲ့ ထည့်ပြီးတော့  Set active ရေးတာ ပိုဖြစ်သင့်တယ်။ အဲ့လိုရေးရင်လည်း Language ပြောင်းရင် နာမည် ပြန်ပေးရတာတွေရှိလာနိုင်တယ် 
        }
        if (buildingID == 1 || buildingID == 2 || buildingID == 3) {
            AudioManager.Instance().PlaySound(4);
        }
        if (buildingID == 4) {
            AudioManager.Instance().PlaySound(9);
        }
        if (buildingID == 6) {
            AudioManager.Instance().PlaySound(4);
        }

        LanguageManager.Instance().SetLangText(txtBuildingName, GetNameTxtID());
    }
    public void ActionDeselect() {
        halo.SetActive(false);
        BuildingDetailPanel.SetActive(false);
        txtBuildingLvl.text = "";
        txtBuildingName.text = "";
    }

    void DeleteCatagory() {
        for (int i = 0; i < BuildingDetailPanel.transform.childCount; i++) {
            Destroy(BuildingDetailPanel.transform.GetChild(i).gameObject);
        }
    }
    void AssigningCategory() {
        List<GameObject> actionBtns = BuildManager.Instance().GetActionButtons(buildingID, GetCategory());
        for (int i = 0; i < actionBtns.Count; i++) {
            GameObject itemGO = Instantiate(actionBtns[i]) as GameObject;
            itemGO.transform.SetParent(BuildingDetailPanel.transform, false);
        }
    }

    public void BuildingActionAllow(bool allow) {
        Transform DetailInfoPanel = transform.GetChild(6).GetChild(0);
        for (int i = 0; i < DetailInfoPanel.childCount; i++) {
            if (DetailInfoPanel.GetChild(i).name.ToString() == "UIMove(Clone)"
                || DetailInfoPanel.GetChild(i).name.ToString() == "UIProduce(Clone)"
                || DetailInfoPanel.GetChild(i).name.ToString() == "UIUpgrade(Clone)"
                || DetailInfoPanel.GetChild(i).name.ToString() == "UIRemove(Clone)"
                ) {
                DetailInfoPanel.GetChild(i).gameObject.SetActive(allow);
            }
        }
    }

    public bool FruitCollectable() {
        if (category != Shop_Building.Category.Build) {
            if (process == 1) {
                return true;
            }
        }
        return false;
    }

    public void collectingFruit() {
        if (category != Shop_Building.Category.Build) {
            if (process == 1) {
                int tempBuildingID = 0;
                if (category == Shop_Building.Category.Field) {
                    tempBuildingID = 1;
                } else if (category == Shop_Building.Category.Tree) {
                    tempBuildingID = 2;
                } else if (category == Shop_Building.Category.Pergola) {
                    tempBuildingID = 3;
                }
                StartCoroutine(cropping(cropableItem));
                StartCoroutine(reportCollecting(cropableItem, blockID, tempBuildingID));
				buildingID = tempBuildingID;
                insertingBuildingSprites(buildingID);
                DeleteCatagory();
                AssigningCategory();
                spriteRD.sprite = builtSpriteState0;
                AchievementManager.Instance().IncreaseProgress(3);
                int Progress = AchievementManager.Instance().pdb.GetCurrentProgress((int)E_AchType.CropHarvesting);
                AchievementManager.Instance().MajorAchievementUpdate(E_AchType.CropHarvesting, Progress + 1);
                AudioManager.Instance().PlaySound(1);
			}
        }
    }

	IEnumerator cropping(int cropItemID) {
        cropAnim.GetComponent<SpriteRenderer>().sprite = InventoryManager.Instance().InvFind(cropItemID).Image;
        cropAnim.GetComponent<Animator>().SetTrigger("Crop"); //animation 2 ခုအတွက် ၁ စက္ကန့်စောင့်ပါ
        yield return null;
	}

    IEnumerator reportCollecting(int cropableItem, int blockID, int tempBuildingID) { // smooth မဖြစ်တဲ့ ပြသနာရှိတယ် database ကို camera animation အချိန်မှာ ပိတ်လိုက်ဖွင့်လိုက် ၂ ခါလုပ်ထားတယ်
        randomItemDrop();
        yield return new WaitForSeconds(1);
        GameManager.Instance().ProgressUpdate(InventoryManager.Instance().InvFind(cropableItem).Experience); // ရိတ်လိုက်၇င် lvl တတ်မယ် မတတ်ချင်ရင် ပြန်ဖျက်ပါ
        InventoryManager.Instance().AddingItems(cropableItem, 1);
        BuildManager.Instance().ItemPickUpBuilding(blockID, tempBuildingID);
    }

    void randomItemDrop() { // for 21 22 23 items
        int percent = TechnicalManager.Instance().GetProductCurrentPercentage();// calculate by int
        if (FunProgressCalculator.PossibilityCalculator(percent)) {
            Debug.Log("BonusItemID.Length randomDrop " + BonusItemID.Length);
            int dropItem = BonusItemID[UnityEngine.Random.Range(0, BonusItemID.Length)]; // field ပေါ်မူတည်ပြီး item drop ကျပါမယ်
            if (ADManager.Instance().ActionWorking(10)) {
                InventoryManager.Instance().AddingItems(dropItem, 1);
            }
            InventoryManager.Instance().AddingItems(dropItem, 1);
            StartCoroutine(doubleCropAnimation(dropItem));
        }
    }
    IEnumerator doubleCropAnimation(int dropItem) {
        yield return new WaitForEndOfFrame();
            //new WaitForSeconds(1);//animation 2 ခုအတွက် ၁ စက္ကန့်စောင့်ပါ
        cropAnim.GetComponent<Animator>().SetTrigger("AnotherCrop");
        Debug.Log("Another Croping");
        cropAnim.GetComponent<SpriteRenderer>().sprite = InventoryManager.Instance().InvFind(dropItem).Image;
        cropAnim.GetComponent<Animator>().SetTrigger("Crop"); 
    }
    
    public int GetBlockID() {
        return blockID;
    }

    public void SetBlockID(int value) {
        blockID = value;
    }

    public int GetBuildingID() {
        return buildingID;
    }

    public void SetBuildingID(int value) {
        buildingID = value;
    }

    public int GetNameTxtID() {
        return nameTxtID;
    }

    public void SetNameTxtID(int value) {
        nameTxtID = value;
    }

    public DateTime GetStartBuildTime() {
        return startBuildTime;
    }

    public void SetStartBuildTime(DateTime value) {
        // directly update to starting time string and the database
        SetStartingTime(FunDateTimeAccess.DateTimeToString(value));
        startBuildTime = value;
    }

    public string GetStartingTime() {
        return startingTime;
    }

    public void SetStartingTime(string value) {
        startingTime = value;
        BuildManager.Instance().UpdateStartingTime(this);
    }

    public int GetLevel() {
        return level;
    }

    public void SetLevel(int value) {
        level = value;
    }

    public int GetProcess() {
        return process;
    }

    public void SetProcess(int value) {
        process = value;
    }

    public void SetBlockRef(GameObject value) {
        blockRef = value;
    }

    public GameObject GetBlockRef() {
        return blockRef;
    }

    public int GetDuration() {
        return duration;
    }

    public void SetDuration(int value) {
        duration = value;
    }

    public int GetProductioncap() {
        return productioncap;
    }

    public void SetProductioncap(int value) {
        productioncap = value;
    }

    public int GetReducetime() {
        return reducetime;
    }

    public void SetReducetime(int value) {
        reducetime = value;
    }

    public Shop_Building.Category GetCategory() {
        return category;
    }
}