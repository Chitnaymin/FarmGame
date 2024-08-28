using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement {

	public GameObject gameObject;

    private int id;
    private int titleTextID;
    private string description;
    private string achType; // long team or short
    private int gainerID; // coin or coupon
    private int gainPoint;
    private string startDate;
    private int currentProgression;
    private int maxProgression;
    private bool unlocked;
    private int achieveingID;

    private int spriteIndex;

    private GameObject achievmentRef;

    private PlayerStorageManager pdbsm = new PlayerStorageManager();
    public Achievement(int id, GameObject achievmentRef, int titleTextID, string description, string achType, int gainerID, int gainPoint, int maxProgression, int achieveingID = 0) {
        this.id = id;
        this.achievmentRef = achievmentRef;
        this.titleTextID = titleTextID;
        this.description = description;
        this.achType = achType;
        this.gainerID = gainerID;
        this.gainPoint = gainPoint;
        this.startDate = FunDateTimeAccess.GetTodayDate();

        this.maxProgression = maxProgression;
        this.unlocked = false;
        this.achieveingID = achieveingID;
        LoadAchievment();
        //SavetoDB();
    }

    void SavetoDB() {
        pdbsm.InsertInitialAchievement(id, startDate,currentProgression,maxProgression, unlocked ? 1 : 0);
    }

    public bool EarnAchievment() {
        if (!unlocked && CheckProgress()) {
            unlocked = true;
            //changing image to show done or not 
            // achievmentRef.GetComponent<Image>().sprite = AchievmentManager.Instance().unlockedSprite;
            SaveAchievment(true);
            /*
            if (child != null) {
                AchievmentManager.Instance().EarnAchievment(child);
            }
            */
            return true;
        }
        return false;
    }

    // database link will be here
    public void SaveAchievment(bool value) {
        unlocked = value;

        int tmpPoints = PlayerPrefs.GetInt("Points", 0);

		if(id<5) {
			pdbsm.UpdateAchievementProgress(id, currentProgression);
		}
        PlayerPrefs.SetInt("Points", tmpPoints + gainPoint);
        PlayerPrefs.SetInt("Progression" + titleTextID, currentProgression);

        PlayerPrefs.SetInt(titleTextID.ToString(), value ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void LoadAchievment() 
	{
		if(id<5) 
		{
			currentProgression = pdbsm.GetAchievementFromDatabase(id);
		}
        else 
		{
			currentProgression = pdbsm.GetCurrentProgress(id-5);
		}
        unlocked = maxProgression<= currentProgression ? true : false;
        ProgressBarSet();

        if (unlocked) {
            //AchievmentManager.Instance().textPoints.text = "point: " + PlayerPrefs.GetInt("Points");

            //changing image to show done or not  when load
            //achievmentRef.GetComponent<Image>().sprite = AchievmentManager.Instance().unlockedSprite;
        }
    }
    public bool CheckProgress() {

		//semi bind function applied
		ProgressBarSet();

        SaveAchievment(false);
        if (maxProgression == 0) {
            return true;
        }
		if(id<5) {
			if (currentProgression >= maxProgression && AchievementManager.Instance().pdb.GetUnlocked(id) == 0) {
				gameObject.GetComponentInChildren<CanvasGroup>().alpha = 1;
				gameObject.GetComponentInChildren<CanvasGroup>().interactable = true;
				return true;
			}
		}
		else {

			int MaxLevel=AchievementManager.Instance().MajorAchDatas[id - 5].AchDetails.Count;

			if (currentProgression >= maxProgression && pdbsm.GetClaimedLevel(id-5)<MaxLevel) {
				gameObject.GetComponentInChildren<CanvasGroup>().alpha = 1;
				gameObject.GetComponentInChildren<CanvasGroup>().interactable = true;
				return true;
			}
		}
        
		gameObject.GetComponentInChildren<CanvasGroup>().alpha = 0.5f;
		gameObject.GetComponentInChildren<CanvasGroup>().interactable = false;
		return false;
    }

    private void ProgressBarSet() {
		float width = FunProgressCalculator.PercentageCalculator(currentProgression, maxProgression, 740.0f);
		width = Mathf.Clamp(width, 0, 740.0f);
		currentProgression = Mathf.Clamp(currentProgression,0,maxProgression);
		achievmentRef.transform.GetChild(1).GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(width, 45); // will calculate via function of the progress
        achievmentRef.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = FunProgressCalculator.ProgressToString(currentProgression, maxProgression); // current and max progression
    }

	public int GetID() 
	{
		return id;
	}
    // getter and setter
    public int GetTitle() {
        return titleTextID;
    }

    public void SetTitle(int value) {
        titleTextID = value;
    }

    public int GetGainerID() {
        return gainerID;
    }

    public void SetGainerID(int value) {
        gainerID = value;
    }
    public string GetDescription() {
        return description;
    }
    public void SetDescription(string value) {
        description = value;
    }

    public bool GetUnlocked() {
        return unlocked;
    }

    public void SetUnlocked(bool value) {
        unlocked = value;
    }

    public int GetPoints() {
        return gainPoint;
    }

    public void SetPoints(int value) {
        gainPoint = value;
    }

    public int GetSpriteIndex() {
        return spriteIndex;
    }

    public void SetSpriteIndex(int value) {
        spriteIndex = value;
    }


    public int GetCurrentProgression() {
        return currentProgression;
    }

    public void SetCurrentProgression(int value) {
        currentProgression = value;
    }

    public int GetMaxProgression() {
        return maxProgression;
    }

    public void SetMaxProgression(int value) {
        maxProgression = value;
    }
}
