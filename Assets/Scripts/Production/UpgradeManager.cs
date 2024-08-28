using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : SingletonBehaviour<UpgradeManager> {

	public GameObject UpgradeManagerCanvas;

	public GameObject LowerLevelPanel;
	public Text Title;
	public Text BuildingName;
	public Text ProductionCapBefore;
	public Text ReduceTheTimeBefore;
	public Text ProductionCapAfter;
	public Text ReduceTheTimeAfter;
	public Text BeforeUpdateLvl;
	public Text BeforeBuildingName;
	public Text AfterBuildingName;
	public Text AfterUpdateLvl;
	public Text Time;
	public Button btnUpgrade;
	public Sprite availableSprite, unavailableSprite;

	public GameObject MaxLevelPanel;
	public Text CompleteUpdate;

	public Text UpgardePrice;

	private Building buildingInfo;
	StorageManager dbsm;

	Shop_Building[] upgradeBuildings;
	private Shop_Building building;
	private GameObject buildingGO;

	void Start() {
		dbsm = new StorageManager();
		upgradeBuildings = Resources.LoadAll<Shop_Building>("UpgradeBuildings");
		HideItemInfo();
	}

	public void ShowUpgradeView(Building buidlingInfo) {
		this.buildingInfo = buidlingInfo;
		Debug.Log("Pro" + buidlingInfo.GetProductioncap());
		Debug.Log("Reduce" + buidlingInfo.GetReducetime());
		UpgradeManagerCanvas.SetActive(true);
		foreach (Shop_Building ele in upgradeBuildings) {
			if (buidlingInfo.GetBuildingID() == ele.id) {
				if (buidlingInfo.GetLevel() + 1 == ele.buildingLvl) {
					BindToUpgrade(ele);
					Title.text = "Level " + buidlingInfo.GetLevel();
					LanguageManager.Instance().SetLangText(BuildingName, buildingInfo.GetNameTxtID());
					LanguageManager.Instance().SetLangText(BeforeBuildingName, buidlingInfo.GetNameTxtID());
					LanguageManager.Instance().SetLangText(AfterBuildingName, buidlingInfo.GetNameTxtID());
					BeforeUpdateLvl.text = "Lv." + buidlingInfo.GetLevel();
					AfterUpdateLvl.text = "Lv." + ele.buildingLvl;
					ProductionCapBefore.text = LanguageManager.Instance().SearchTextList(27001) + " (" + buidlingInfo.GetProductioncap() + ")";
					ReduceTheTimeBefore.text = LanguageManager.Instance().SearchTextList(27002) + " (" + buidlingInfo.GetReducetime() + "%)";
					ProductionCapAfter.text = LanguageManager.Instance().SearchTextList(27001) + " (" + ele.ProductionCap + ")";
					ReduceTheTimeAfter.text = LanguageManager.Instance().SearchTextList(27002) + " (" + ele.ReduceTheTime + "%)";
					CheckLvlAndPrice(ele);
				} else if (buidlingInfo.GetLevel() > 4) {
					MaxLevelPanel.SetActive(true);
					CompleteUpdate.text = "Level Max";
				}
			}
		}
		//ShowItemInfo(buidlingInfo);
	}
	// bind view
	private void BindToUpgrade(Shop_Building shop_Building) {
		this.building = shop_Building;
		UpgardePrice.text = building.price.ToString();
		Time.text = building.time.ToString();
	}

	//Bind building
	private void bindToBuilding(Building buildingInfo, Shop_Building shop_Building) {
		this.buildingInfo = buildingInfo;
		this.buildingInfo.SetBlockID(buildingInfo.GetBlockID());
		this.buildingInfo.SetLevel(shop_Building.buildingLvl);
		this.buildingInfo.SetProcess(0);
		this.buildingInfo.SetDuration(shop_Building.time);

	}
	void HideItemInfo() {
		LowerLevelPanel.SetActive(true); // if nothing to upgarde
		MaxLevelPanel.SetActive(false);
		Title.text = "";
		BuildingName.text = "";
		ProductionCapBefore.text = "";
		ReduceTheTimeBefore.text = "";
		ProductionCapAfter.text = "";
		ReduceTheTimeAfter.text = "";
		BeforeUpdateLvl.text = "";
		BeforeBuildingName.text = "";
		AfterBuildingName.text = "";
		AfterUpdateLvl.text = "";
		Time.text = "";
		UpgardePrice.text = "";
	}

	public void Upgrade() {
		bindToBuilding(buildingInfo, building);
		//Debug.Log(buildingInfo.GetDuration());
		BuildManager.Instance().UpdateBuildingLevel(buildingInfo);
		UpgradeManagerCanvas.SetActive(false);

		//price
		int coin = PlayerData.Instance().GetCoin() - building.price;
		PlayerData.Instance().SetCoin(coin);
		ShopManager.Instance().txtCoin.text = coin.ToString();
		buildingInfo.Upgrade();//call from building
							   // MAY BE RE UPDATE FOR READY TO NEXT LEVEL
	}

	//Checking Level and Price
	public void CheckLvlAndPrice(Shop_Building shop_Building) {
		if (PlayerData.Instance().GetLvl() >= shop_Building.level && PlayerData.Instance().GetCoin() >= shop_Building.price) {
			btnUpgrade.GetComponent<Image>().sprite = availableSprite;
			btnUpgrade.interactable = true;
		} else {
			btnUpgrade.GetComponent<Image>().sprite = unavailableSprite;
			btnUpgrade.interactable = false;
		}
	}

    public Shop_Building SearchingUpgradedBuilding(int id, int level) {
        foreach (Shop_Building ele in upgradeBuildings) {
            if (ele.id == id) {
                if (ele.buildingLvl == level) {
                    return ele;
                }
            }
        }
        return null;
    }

    public int GetBuildingDuration(int id, int level) {
		foreach (Shop_Building ele in upgradeBuildings) {
			if (ele.id == id) {
				if (ele.buildingLvl == level) {
					return ele.time;
				}
			}
		}
		return 0;
	}
}
