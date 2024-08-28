using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GameManager : SingletonBehaviour<GameManager> {

	public GameObject buttomPanel;
	public GameObject NamePanel;
	public GameObject GoldPanel;
	public GameObject NameEditPanel;
	public InputField NameBox;
	public Text playerName;
	public Text DefaultName;

	private GameObject tempBuild;
	private GameObject tempSelect;
	public Image expProgress;
	public Text level;
	List<LevelXPGraph> levelXPGraphs = new List<LevelXPGraph>();
	StorageManager db;

	bool moveByBuild = true;
	private GameObject movingOject;
	public enum SelectionState {
		Idle,
		BuildingDetail,
		BuildingMove,
		BuildingDelete
	};
	//[SerializeField]
	public SelectionState selection;

	// SingletonObjectCreation
	public override void Awake() {
		base.Awake();
	}

	void Start() {
		selection = SelectionState.Idle;
		db = new StorageManager();
		string txtLvl = PlayerData.Instance().GetLvl().ToString();
		level.text = "LVL " + txtLvl;
		expProgress.fillAmount = (float)PlayerData.Instance().GetExp() / CheckingExp();
		NameEditPanel.SetActive(false);
		CheckName();
	}
	public bool SelectBuilding() {
		BuildingDeselected();
		RaycastHit2D[] hit2D = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		foreach (RaycastHit2D ele in hit2D) {
			if (ele.collider.tag == "ground" && !EventSystem.current.IsPointerOverGameObject()) {
				if (selection == SelectionState.BuildingDetail && ele.collider.gameObject != tempSelect) {
					return false;
				}
				BuildingDeselected();
				tempSelect = ele.collider.gameObject; // save ground position
				if (tempSelect.transform.childCount == 1) {
					GameObject childTemp = tempSelect.transform.GetChild(0).gameObject;
                    if (childTemp.GetComponent<Building>().FruitCollectable()) {
                        childTemp.GetComponent<Building>().collectingFruit();
                        return false;
                    } else {
                        Camera.main.GetComponent<CameraUpdate>().CameraMoving(childTemp);
                        BuildingSelected();
                        // buildingDetailPanel.GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToViewportPoint(childTemp.transform.position);
                        buttomPanel.SetActive(false);
                        selection = SelectionState.BuildingDetail; // ဒီနားပြင်ရမှာ ရလာတဲ့ building ကို အရင်စစ်ရမှာ ပြီးမှ STATE ချိန်သင့် မချိန်း  
                        return true;
                    }
					
				}
			}
		}
		return false;
	}

	void Update() {
		bool noUIcontrolsInUse = EventSystem.current.currentSelectedGameObject == null;
		switch (selection) {
			case SelectionState.Idle:
				tempBuild = CameraAction.CameraAimingCells();
				break;
			case SelectionState.BuildingDetail:
                tempBuild = CameraAction.CameraAimingCells();
                if (Input.GetMouseButtonUp(0)) {
					if (noUIcontrolsInUse) {
						RaycastHit2D[] hit2D = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
						foreach (RaycastHit2D ele in hit2D) {
							if (ele.collider.tag == "ground" && !EventSystem.current.IsPointerOverGameObject()) {
								//BuildingDeselected();
								if (ele.collider.transform.childCount == 0) {
									buttomPanel.SetActive(true);
									selection = SelectionState.Idle;
								}
							}
						}
					}
				}
				break;
			case SelectionState.BuildingMove:
				if (Input.GetMouseButtonUp(0)) {
					if (noUIcontrolsInUse) {
						RaycastHit2D[] hit2D = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
						foreach (RaycastHit2D ele in hit2D) {
							if (ele.collider.tag == "ground" && !EventSystem.current.IsPointerOverGameObject()) {
								movingOject.GetComponent<MovingNewPosition>().Moving(ele.collider.gameObject, moveByBuild);
								ForceUIInv();
							}
						}
					}
				}
				break;
			case SelectionState.BuildingDelete:
				//may be animatoin
				break;

		}

	}

	public void BuildingSelected() {
		tempSelect.transform.GetChild(0).transform.GetComponent<Building>().ActionSelect();
	}
	public void BuildingDeselected() {
		try {
			tempSelect.transform.GetChild(0).transform.GetComponent<Building>().ActionDeselect();
		} catch (UnityException e) {
			//Debug.Log(e);
		} catch (System.NullReferenceException e) {
			// Debug.Log(e);
		}
	}

	public void ForceIdle() {
		buttomPanel.SetActive(true);
		selection = SelectionState.Idle;
		BuildingDeselected();
		NamePanel.SetActive(true);
		GoldPanel.SetActive(true);
	}
	public void ForceUIInv() {
		buttomPanel.SetActive(false);
		NamePanel.SetActive(false);
		GoldPanel.SetActive(false);
	}

	public void CheckName() {
		if (PlayerPrefs.GetInt("Default") == 0) {
			DefaultName.gameObject.SetActive(true);
			playerName.gameObject.SetActive(false);
		} else {
			DefaultName.gameObject.SetActive(false);
			playerName.gameObject.SetActive(true);
			playerName.text = PlayerData.Instance().GetPlayerName();
		}
	}
	// Building  control by Shop Canvas
	// UI Function
	public void BtnBuild(GameObject building, Shop_Building shop_Building) {
		// To hide Canvas 
		moveByBuild = true;
		//got tempBuild.transform from camera aiming position
		// actural build 
		movingOject = Instantiate(building, tempBuild.transform.position, Quaternion.identity) as GameObject;
		movingOject.GetComponent<SpriteRenderer>().sortingOrder = tempBuild.GetComponent<SpriteRenderer>().sortingOrder;
		movingOject.GetComponent<Building>().BindToPrefab(shop_Building.id, shop_Building.builtSpriteState0,
			 shop_Building.builtSpriteState1, shop_Building.builtSpriteState2, shop_Building.builtSpriteComplete, shop_Building.category_tag,
			shop_Building.price, shop_Building.time, shop_Building.exp, shop_Building.buildingLvl, shop_Building.cropableItem, shop_Building.nameTxtID, shop_Building.ProductionCap, shop_Building.ReduceTheTime, shop_Building.BonusItemID);
		movingOject.GetComponent<MovingNewPosition>().ActionMove(true);
		movingOject.GetComponent<MovingNewPosition>().Moving(tempBuild, moveByBuild);
		// will motify for shop_Building.builtSpriteState0 both 4 state
		selection = SelectionState.BuildingMove;
	}

	// detail panel action start
	public void BtnMove() {
		moveByBuild = false;
		selection = SelectionState.BuildingMove;
		movingOject = tempSelect.transform.GetChild(0).gameObject;
		movingOject.transform.parent = null;
		movingOject.GetComponent<MovingNewPosition>().ActionMove(true); // false by comfirm hide    // panel hide
		ForceUIInv();
	}
	
	public void ProgressUpdate(int buildingXp) {
		int PlayerExp = PlayerData.Instance().GetExp();
		int exp = PlayerExp + buildingXp;
		int difExp;
		if (PlayerData.Instance().GetLvl() < 30) {
			while(exp >= CheckingExp()) { // logic မှန်သွားပြီ
				exp -= CheckingExp();
				PlayerData.Instance().SetLvl(PlayerData.Instance().GetLvl() + 1);
                AudioManager.Instance().PlaySound(5);
			}
		}
		difExp = exp;
		PlayerData.Instance().SetExp(difExp);

		level.text = "LVL " + PlayerData.Instance().GetLvl().ToString();
		expProgress.fillAmount = FunProgressCalculator.PercentageCalculator((float)difExp, (float)CheckingExp());
	}

	public int CheckingExp() {
		int exp = 0;
		levelXPGraphs = db.GetLevelXpGraphFromDB();
		foreach (LevelXPGraph ele in levelXPGraphs) {
			if (PlayerData.Instance().GetLvl() == ele.GetLvlDB()) {
				exp = ele.GetXpDB();
			}
		}
		return exp;
	}

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 CV3 = Camera.main.transform.position;
        Gizmos.DrawLine(CV3, new Vector3(CV3.x, CV3.y, CV3.z + 10));
    }

	public void ShowLvl() {
		level.text = "LVL " + PlayerData.Instance().GetLvl().ToString();
	}

	public void ShowNameEditPanel() {
		NameEditPanel.SetActive(true);
		NameBox.text = "";
		ForceIdle();
	}

	public void BtnOkforNameEdit() {
		string name = NameBox.text;
		if (name == "") {
			Debug.Log("not to be null");
		} else {
			PlayerData.Instance().SetPlayerName(name);
			PlayerPrefs.SetInt("Default", 1);
			CheckName();
		}
		NameEditPanel.SetActive(false);
		ForceIdle();
	}
}