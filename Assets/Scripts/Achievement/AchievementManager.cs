using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour {

	public GameObject achievmentMenu;

	public GameObject achievmentPrefab;
	public GameObject awardPrefab;
	public Sprite[] sprites;
	public GameObject notiImg;
	public Text PlayerTitle;
	//public GameObject ActivityBar;

	public ScrollRect scrollRect;

	public GameObject visualAchievment;
	public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();

	public Dictionary<string, Award> awards = new Dictionary<string, Award>();

	public List<MajorAchData> MajorAchDatas = new List<MajorAchData>();

	List<AwardObj> awardObjs = new List<AwardObj>();

	// public Sprite unlockedSprite;

	private AchievementButton activeButton;
	private int fadeTime = 2;

	private static AchievementManager instance;
	private float activity;

	[HideInInspector]
	public PlayerStorageManager pdb;

	public List<Transform> Tabs = new List<Transform>();

	public static AchievementManager Instance() {
		if (instance == null) {
			instance = GameObject.FindObjectOfType<AchievementManager>();
		}
		return instance;
	}
	private void Start() {
		//achievmentMenu.SetActive(false);
		pdb = new PlayerStorageManager();
		ShowTitle();
		PlayerPrefs.SetInt("Noti", 0);
		IsNoti();
		//activity = PlayerPrefs.GetFloat("Activity");
		//ActivityProgressSet(activity); //For Activity Bar
	}
	public void IncreaseProgress(int ID) {
		Achievement ach;
		if (achievements.TryGetValue(ID.ToString(), out ach)) {

			int curentProgress = ach.GetCurrentProgression();
			int MaxProgress = ach.GetMaxProgression();

			if (curentProgress >= MaxProgress) {
				return;
			}
			curentProgress++;
			ach.SetCurrentProgression(curentProgress);
			//if (curentProgress == MaxProgress) {
			//	float i = curentProgress - (curentProgress - 1);
			//	ActivityProgressSet(i);
			//}
		}
		ach.CheckProgress();

	}
	public void MajorAchievementUpdate(E_AchType AchType, int Progress, bool GiveReward = false) {
		int LastCoin = PlayerData.Instance().GetCoin();
		Achievement ach;
		if (achievements.TryGetValue((4 + ((int)AchType + 1)).ToString(), out ach)) {

			int curentProgress = Progress;
			int MaxProgress = ach.GetMaxProgression();
			int CurrentLevel = pdb.GetClaimedLevel((int)AchType);
			ach.SetCurrentProgression(curentProgress);
			if (curentProgress >= MaxProgress) {

				if (CurrentLevel >= MajorAchDatas[(int)AchType].AchDetails.Count) {
					return;
				}//If you delete,Program will not work
				MajorAchStruct detail = MajorAchDatas[(int)AchType].AchDetails[CurrentLevel];
				if (GiveReward) {
					if (detail.RewardType == E_RewardType.Money) {
						PlayerData.Instance().SetCoin(PlayerData.Instance().GetCoin() + ach.GetPoints(), true);
						ShopManager.Instance().ShowCoin();
						if (detail.TitleTxtID != 0) {
							pdb.InsertTitle(detail.TitleTxtID);
						}
					} else if (detail.RewardType == E_RewardType.SerialCode) {
						if (detail.SerialKeyCode != "") {
							pdb.InsertAward(detail.SerialKeyCode);
						}
					} else if (detail.RewardType == E_RewardType.SerialTitle) {
						if (detail.SerialKeyCode != "" && detail.TitleTxtID != 0) {
							pdb.InsertSerialTitle(detail.SerialKeyCode, detail.TitleTxtID);
						}
					}
					CurrentLevel++;
				}

				if (CurrentLevel >= MajorAchDatas[(int)AchType].AchDetails.Count) {
					//ach.gameObject.SetActive(false);
					PlayerPrefs.SetInt("Noti", 1);
					IsNoti();

				} else {
					MajorAchStruct newDetail = MajorAchDatas[(int)AchType].AchDetails[CurrentLevel];
					MaxProgress = newDetail.MaxProgress;
					ach.SetMaxProgression(newDetail.MaxProgress);
					ach.SetTitle(newDetail.AchNameTextID);
					ach.SetPoints(newDetail.CoinReward);
					ach.gameObject.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = ach.GetPoints().ToString();
					//ach.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = ach.GetTitle().ToString();
					LanguageManager.Instance().SetLangText(ach.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Text>(), ach.GetTitle());
				}
			}
			pdb.UpdateLevelAndProgress((int)AchType, CurrentLevel, curentProgress);
			ach.CheckProgress();
		}
		if (PlayerData.Instance().GetCoin() != LastCoin) {
			MajorAchievementUpdate(E_AchType.Money, PlayerData.Instance().GetCoin());
		}
	}

	public void RefreshLang() {
		Achievement ach;
		if (Tabs[0]) {
			for (int i = 0; i < Tabs[0].childCount; i++) {
				Text txt = Tabs[0].GetChild(i).transform.GetChild(1).GetChild(0).GetComponent<Text>();
				if (achievements.TryGetValue((i + 1).ToString(), out ach)) {
					if (i < 5) {
						LanguageManager.Instance().SetLangText(txt, ach.GetTitle());
					}
				}
			}
		}
		if (Tabs[1]) {
			for (int i = 0; i < Tabs[1].childCount; i++) {
				Text txt = Tabs[1].GetChild(i).transform.GetChild(1).GetChild(0).GetComponent<Text>();
				if (achievements.TryGetValue((i + 5).ToString(), out ach)) {
					LanguageManager.Instance().SetLangText(txt, ach.GetTitle());
				}
			}
		}
	}
	public void IsNoti() {
		if (PlayerPrefs.GetInt("Noti") == 0) {
			notiImg.SetActive(false);
		} else if (PlayerPrefs.GetInt("Noti") == 1) {
			notiImg.SetActive(true);
		}
	}

	public void RefreshTab3() {
		if (Tabs[2]) {
			Option3Display();
		}
	}

	//float temp = 0;  //Activity
	//public void ActivityProgressSet(float currentProgression) {
	//	temp += currentProgression;
	//	PlayerPrefs.SetFloat("Activity", temp);
	//	float width = FunProgressCalculator.PercentageCalculator(temp, 4, 400f);
	//	width = Mathf.Clamp(width, 0, 400f);
	//	temp = Mathf.Clamp(temp, 0, 4);
	//	ActivityBar.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(width, 50);
	//}

	public void Option3Display() {//call by btn
		PlayerPrefs.SetInt("Noti", 0);
		IsNoti();

		Award[] awards = GameObject.FindObjectsOfType<Award>();
		for (int i = 0; i < awards.Length; i++) {
			Destroy(awards[i].gameObject);
		}

		this.awards = new Dictionary<string, Award>();

		//KeyCodeLists = db.GetKeyCodes();
		//titles = db.GetTitlewithSerial();
		//playerTitles = db.GetPlayerTitle();
		awardObjs = pdb.GetAwards();

		for (int i = 0; i < awardObjs.Count; i++) {
			if (awardObjs[i].GetTitleID() == 0) {
				CreateAward(2, i, i.ToString() + "|" + LanguageManager.Instance().SearchTextList(FunKeyToText.KeyCodeToTextID(awardObjs[i].GetKeycode())), "", awardObjs[i].GetKeycode(), awardObjs[i].GetTitleID());
			} else if (awardObjs[i].GetTitleID() != 0 && awardObjs[i].GetSerial() != "0") {
				CreateAward(2, i, i.ToString() + "|" + LanguageManager.Instance().SearchTextList(FunKeyToText.KeyCodeToTextID(awardObjs[i].GetKeycode())), "", awardObjs[i].GetKeycode(), awardObjs[i].GetTitleID());
			} else if(awardObjs[i].GetTitleID() != 0) {
				CreateAward(2, i, i.ToString() + "|" + LanguageManager.Instance().SearchTextList(awardObjs[i].GetTitleID()), "", "0", awardObjs[i].GetTitleID());
			}
		}
	}

	// if (serial == "0") {
	//	foreach (int b in titles) {
	//		for (int a = 0; a < playerTitles.Count; a++) {
	//			if (b == playerTitles[i].GetTitleID()) {
	//				CreateAward(2, a, LanguageManager.Instance().SearchTextList(b), "", "0");
	//			}
	//		}
	//	}
	//}

	public void ShowTitle() {
		if (PlayerData.Instance().GetTitle() == 0) {
			PlayerTitle.text = "";
		} else {
			LanguageManager.Instance().SetLangText(PlayerTitle, PlayerData.Instance().GetTitle());
		}	
	}
	public void DelayStart() {
		//PlayerPrefs.DeleteAll();
		achievmentMenu.SetActive(true); // must be setactive true when finding Button
		activeButton = GameObject.Find("Option1btn").GetComponent<AchievementButton>();
		// input from database (maybe)
		CreateAchievment("Option1", 1, 20101, "Press W now", "Daily", 0, 500, 1);
		CreateAchievment("Option1", 2, 20102, "Press W now", "Daily", 0, 500, 10);
		CreateAchievment("Option1", 3, 20103, "Press W now", "Daily", 0, 500, 5);
		CreateAchievment("Option1", 4, 20104, "Press W now", "Daily", 0, 300, 1);

		//CreateAchievment("Option2", 5, "Press A", "Press W now", "LongTerm", 1, 3, 40);
		//CreateAchievment("Option2", 6, "Press S", "Press W now", "LongTerm", 0, 50, 50);

		for (int i = 0; i < MajorAchDatas.Count; i++) {
			MajorAchData achData = MajorAchDatas[i];
			int CurrentAchLevel = pdb.GetClaimedLevel((int)achData.AchType);
			//int CurrentAchProgress = db.GetCurrentProgress((int)achData.AchType);
			if (CurrentAchLevel < achData.AchDetails.Count) {
				CreateAchievment("Option2", 4 + ((int)achData.AchType + 1), achData.AchDetails[CurrentAchLevel].AchNameTextID, "", "LongTerm", (int)achData.AchDetails[CurrentAchLevel].RewardType, achData.AchDetails[CurrentAchLevel].CoinReward, achData.AchDetails[CurrentAchLevel].MaxProgress);
			} else {
				CurrentAchLevel -= 1;
				if (achData.AchDetails[CurrentAchLevel].RewardType == E_RewardType.SerialTitle) {
					achData.AchDetails[CurrentAchLevel].RewardType -= 1;
					CreateAchievment("Option2", 4 + ((int)achData.AchType + 1), achData.AchDetails[CurrentAchLevel].AchNameTextID, "", "LongTerm", (int)achData.AchDetails[CurrentAchLevel].RewardType, achData.AchDetails[CurrentAchLevel].CoinReward, achData.AchDetails[CurrentAchLevel].MaxProgress);
				}
			}
		}
		foreach (GameObject achievmentList in GameObject.FindGameObjectsWithTag("AchievmentList")) {
			achievmentList.SetActive(false);
		}

		activeButton.Click();
		achievmentMenu.SetActive(false);

	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.E)) {
			PlayerData.Instance().SetLvl(PlayerData.Instance().GetLvl() + 1);
			GameManager.Instance().ShowLvl();
		}
	}

	public void EarnAchievment(string title) {
		achievements[title].EarnAchievment();
	}
	// spriteIndex will be gainer ID
	// achType will be daily or long team
	public void CreateAchievment(string parent, int id, int title, string description, string achType, int gainerID, int gainPoints, int maxprogression) {
		GameObject achievment = Instantiate(achievmentPrefab) as GameObject;
		Achievement newAchievment = new Achievement(id, achievment, title, description, achType, gainerID, gainPoints, maxprogression);
		newAchievment.gameObject = achievment;
		achievements.Add(id.ToString(), newAchievment);

		SetAchievmentInfo(parent, achievment, newAchievment);

		newAchievment.CheckProgress();

		achievment.GetComponentInChildren<Button>().onClick.AddListener(delegate { TakeReward(newAchievment); });

	}

	// bind view
	//string parent is catagory
	public void SetAchievmentInfo(string parent, GameObject achievment, Achievement newAchievment) {
		achievment.transform.SetParent(GameObject.Find(parent).transform);
		achievment.transform.localScale = new Vector3(1, 1, 1);
		achievment.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprites[newAchievment.GetGainerID()];
		achievment.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = newAchievment.GetPoints().ToString();

		LanguageManager.Instance().SetLangText(achievment.transform.GetChild(1).GetChild(0).GetComponent<Text>(), newAchievment.GetTitle());

	}
	public void TakeReward(Achievement ach) {
		if (ach.GetID() < 5) {
			PlayerData.Instance().SetCoin(PlayerData.Instance().GetCoin() + ach.GetPoints());
			ShopManager.Instance().ShowCoin();
			int progress = pdb.GetCurrentProgress((int)E_AchType.Task);
			MajorAchievementUpdate(E_AchType.Task, progress + 1);

			pdb.UpdateUnlocked(ach.GetID(), 1);

		} else {
			E_AchType type = (E_AchType)(ach.GetID() - 5);
			MajorAchievementUpdate(type, pdb.GetCurrentProgress((int)type), true);
		}
		ach.CheckProgress();
	}

	//Award Part
	public void CreateAward(int ParentIndex, int id, string title, string description, string serial,int playertitle) {
		GameObject award = Instantiate(awardPrefab) as GameObject;
		Award newAward = award.GetComponent<Award>();
		newAward.SetAwardData(id, award, title, description, serial, playertitle);
		awards.Add(title, newAward);
		SetAwardInfo(ParentIndex, award, newAward);
		if (serial == "0") {
			award.transform.GetChild(1).gameObject.SetActive(false);
		}
		if (playertitle == 0) {
			award.transform.GetChild(2).gameObject.SetActive(false);
		}
	}

	private void SetAwardInfo(int ParentIndex, GameObject award, Award newAward) {
		award.transform.SetParent(Tabs[ParentIndex]);
		award.transform.localScale = new Vector3(1, 1, 1);
		string title = newAward.GetTitle();
		string[] sData = title.Split('|');

		award.transform.GetChild(0).GetComponent<Text>().text = sData[1];
	}


	public void ChangeCategory(GameObject button) {
		AchievementButton achievmentButton = button.GetComponent<AchievementButton>();
		scrollRect.content = achievmentButton.archievmentList.GetComponent<RectTransform>();
		achievmentButton.Click();
		activeButton.Click();
		activeButton = achievmentButton;
		//if (button.name == "Option1btn") {
		//	ActivityBar.SetActive(true);
		//} else {
		//	ActivityBar.SetActive(false);
		//}
	}

	private IEnumerator FadeAchievment(GameObject achievment) {
		CanvasGroup canvasGroup = achievment.GetComponent<CanvasGroup>();
		float rate = 1.0f / fadeTime;
		int startAlpha = 0;
		int endAlpha = 1;
		for (int i = 0; i < 2; i++) {
			float progress = 0.0f;
			while (progress < 1.0) {
				canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
				progress += rate * Time.deltaTime;
				yield return null;
			}
			yield return new WaitForSeconds(2);
			startAlpha = 1;
			endAlpha = 0;
		}
		Destroy(achievment);
	}
}
