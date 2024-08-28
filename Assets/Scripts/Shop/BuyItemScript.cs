using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuyItemScript : MonoBehaviour {
	private Shop_Building building;
	public GameObject buildingPrefab;
	public Sprite availableSprite, unavailableSprite;
	//private Image imgBuy;
	//private Button imgBtn;
	private GameObject txtUnlock;
	private GameObject btnBuy;

	void Awake() {
		//imgBuy = btnBuy.GetComponent<Image>();
		//imgBtn = transform.GetChild(1).GetComponent<Button>();
		btnBuy = transform.GetChild(1).gameObject;
		txtUnlock = transform.GetChild(3).gameObject;
	}

	bool firstTimeBind = false;
	public void BindForBuilding(Shop_Building building) {
		this.building = building;
		transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = building.sprite;
		SettingPriceInst();
		LanguageManager.Instance().SetLangText(transform.GetChild(2).GetComponent<Text>(), building.nameTxtID);
	}
	void SettingPriceInst() {
		if (ADManager.Instance().GetRelatedAction(8) && (building.id == 4 || building.id == 5 || building.id == 6)) {
			string s = "50%OFF* " + ADManager.Instance().GetRelatedActionCount(8) + " ";
			float f = building.price * 0.5f;
			transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = s + f.ToString();
		} else if (ADManager.Instance().GetRelatedAction(5) &&
					(building.category_tag == Shop_Building.Category.Field
					|| building.category_tag == Shop_Building.Category.Pergola
					|| building.category_tag == Shop_Building.Category.Tree)) {
			string s = "FREE*" + ADManager.Instance().GetRelatedActionCount(5) + "  ";
			transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = s;
		} else {
			transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = building.price.ToString();
		}

		if (building.id == 7 && BuildManager.Instance().LabisBuilding) {
			transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = LanguageManager.Instance().SearchTextList(10322); // soldout
		}
	}

	public void Click_btnBuy_Building() {//call by btn
		ShopManager.Instance().CloseShopPanel();
		if (building.category_tag == Shop_Building.Category.Expansion) {
			ExpansionManager.Instance().BuyExpension();
			Debug.Log("Buying Item Sc " + building.exp);
			ShopManager.Instance().BuyItem(building.price, building.exp); // buy expansion only
		} else {
			GameManager.Instance().BtnBuild(buildingPrefab, building);
		}
		ShopManager.Instance().RefreshTab();
	}
	public void Clickable() {
		btnBuy.GetComponent<Image>().sprite = availableSprite;
		btnBuy.GetComponent<Button>().enabled = true;
		btnBuy.GetComponent<Button>().interactable = true;
	}

	public void Unclickbale() {
		btnBuy.GetComponent<Image>().sprite = unavailableSprite;
		btnBuy.GetComponent<Button>().enabled = false;
		btnBuy.GetComponent<Button>().interactable = false;
	}

	public void Check_Price(int priceFromDB) {
		if (building.category_tag == Shop_Building.Category.Expansion) {
			if (ExpansionManager.Instance().GetExpensionLevel() == building.level) {
				//imgBuy.sprite = availableSprite;
				//imgBtn.enabled = true;
				txtUnlock.SetActive(false);
				btnBuy.SetActive(true);
				
				if(priceFromDB >= building.price) {
					Clickable();
				} else {
					Unclickbale();
				}
			}
			else if(ExpansionManager.Instance().GetExpensionLevel() > building.level) {
				btnBuy.SetActive(false);
				txtUnlock.SetActive(true);
				LanguageManager.Instance().SetLangText(txtUnlock.GetComponent<Text>(), 18);
			}
			else {
				btnBuy.SetActive(false);
				txtUnlock.SetActive(true);
				LanguageManager.Instance().SetLangText(txtUnlock.GetComponent<Text>(), 19);
			}
		} else {
			if (priceFromDB >= building.price && PlayerData.Instance().GetLvl() >= building.level) {
				txtUnlock.SetActive(false);
				btnBuy.SetActive(true);
				Clickable();

			} else if (PlayerData.Instance().GetLvl() < building.level) {
				btnBuy.SetActive(false);
				txtUnlock.SetActive(true);
				txtUnlock.GetComponent<Text>().text = "LV" + building.level + " " + LanguageManager.Instance().SearchTextList(14);
			} else if(priceFromDB < building.price) {
				txtUnlock.SetActive(false);
				btnBuy.SetActive(true);
				Unclickbale();
			}
		}

		if (building.id == 7 && BuildManager.Instance().LabisBuilding) { // Lab တကြိမ် ဝယ်ပြီး ဆက်ဝယ်ခွင့် မပေး
			btnBuy.SetActive(false);
			txtUnlock.SetActive(true);
			LanguageManager.Instance().SetLangText(txtUnlock.GetComponent<Text>(), 10322);
		}
	}
}
