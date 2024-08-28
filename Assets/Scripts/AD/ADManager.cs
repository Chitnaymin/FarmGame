using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADManager : SingletonBehaviour<ADManager> {

    public GameObject videoObject;

    public GameObject AreYouSure;
    public GameObject ItemObtained;

    private ADType MainMenuAD;
    private ADType[] adsRewards;
    private ADType BuildingAD;

    private PlayerStorageManager pdbsm;

    private ADType currentADShow;
    private int ADTypeID;
    private GameObject go;

    private List<ADObtain> ADObtains;

    private bool flag; // update realted
    private int nextUpdate = 0;
    private ADTimer TempITimerRunning;

    private void Start() {
        MainMenuAD = Resources.Load<ADType>("ADMainMenu/1 ADMainMenu");
        adsRewards = Resources.LoadAll<ADType>("AD");
        BuildingAD = Resources.Load<ADType>("ADBuildingToHalved/15 ADBuildUpgradeTime");
        pdbsm = new PlayerStorageManager();
        ADObtains = pdbsm.GetADObtainTime();
        CheckTimer();
    }

    private void Update() {
        if (Time.time >= nextUpdate) {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }
    }

    private void UpdateEverySecond() {
        if (flag) {
            if (TempITimerRunning.Action(FindADDateTimeWithID(TempITimerRunning.id))) {
                // ဘာထည်မို့လဲ
            } else {
                flag = false;
                SaveTimeInDB(TempITimerRunning.id, 0);
            }
        }
    }

    public bool ActionWorking(int ADID) { // call by any other class by their ID for time
        if (flag) {
            return (TempITimerRunning.Action(FindADDateTimeWithID(TempITimerRunning.id))) ? true : false;
        }
        return false;
    }

    public void SaveAddCounterInDB(int ADID) { // add 1
        int i = FindADCountWithID(ADID);
        pdbsm.UpdateADObtainTime(ADID, i + 1, FunDateTimeAccess.DateTimeToString(DateTime.Now));
        ADObtains = pdbsm.GetADObtainTime();
    }
    public void SaveMinusCounterInDB(int ADID) { // sub 1
        int i = FindADCountWithID(ADID);
        pdbsm.UpdateADObtainTime(ADID, i - 1, FunDateTimeAccess.DateTimeToString(DateTime.Now));
        ADObtains = pdbsm.GetADObtainTime();
    }
    public void SaveTimeInDB(int ADID, int ready) {// အချိန် ကျော်သွားရင် ready = 0 ဖြစ်သွား
        pdbsm.UpdateADObtainTime(ADID, ready, FunDateTimeAccess.DateTimeToString(DateTime.Now));
        ADObtains = pdbsm.GetADObtainTime();
    }

    public void CheckTimer() {
        List<ADTimer> iTimer = new List<ADTimer>();
        for (int i = 0; i < adsRewards.Length; i++) {
            ADTimer TempITimer = adsRewards[i] as ADTimer;
            if (TempITimer != null) {
                iTimer.Add(TempITimer);
            }
        }
        for (int i = 0; i < iTimer.Count; i++) {
            if (FindADCountWithID(iTimer[i].id) == 1) {
                flag = iTimer[i].Action(FindADDateTimeWithID(iTimer[i].id));
                if (flag) {
                    TempITimerRunning = iTimer[i];
                } else {
                    SaveTimeInDB(iTimer[i].id, 0);
                }
            }
        }
    }

    public void SaveTimeToDB(int ADTypeID, string timeString) { // for ProcessTime
        pdbsm.UpdateADtime(ADTypeID, timeString);
    }
    public void ADShowBtn(int ADTypeID) { // call by btn 
        this.ADTypeID = ADTypeID;
        SetCurrentADShowID(ADTypeID);

        if (CheckingMenuAD(ADTypeID)) {
            Instantiate(AreYouSure);
        } else {
            GameObject obtain = Instantiate(ItemObtained);
            if (ADTypeID == 1) {
                LanguageManager.Instance().SetLangText(obtain.transform.GetChild(2).GetComponent<Text>(), 25004); // 25004 = "Your AD is not Ready";
            } else if (ADTypeID == 2) {
                LanguageManager.Instance().SetLangText(obtain.transform.GetChild(2).GetComponent<Text>(), 25005); // 25004 = "Your AD is not Ready";
            }
        }
    }
    public void SettingGameObject(GameObject go) {
        this.go = go;
    }
    public void MenuAD() { // call by btn
        if (ADTypeID == 3) {
            ADBuildUpgradeTime adBuildUpgradeTime = (ADBuildUpgradeTime)BuildingAD;
            adBuildUpgradeTime.SetGameObject(go);
        }
        PlayerVideo();
    }

    public bool CheckingMenuAD(int ADTypeID) {
        int i = CalculateDuration(pdbsm.GetADtime(ADTypeID));
        return currentADShow.CheckingAllow(i);
    }

    public void PlayerVideo() {
        int Progress = AchievementManager.Instance().pdb.GetCurrentProgress((int)E_AchType.Ads);
        AchievementManager.Instance().MajorAchievementUpdate(E_AchType.Ads, Progress + 1);
        GameManager.Instance().ForceUIInv();
        AudioManager.Instance().ADPause();

        if (ADTypeID == 2 || ADTypeID == 3) {
            GetComponent<UnityADSAction>().AdShower();
            StartCoroutine(WaitingForDone());
        } else {
            GameObject video = Instantiate(videoObject, transform.position, Quaternion.identity);
            StartCoroutine(Rewarding(5));
        }
    }
    public IEnumerator WaitingForDone() {
        while (!GetComponent<UnityADSAction>().GetIsFinished() && !GetComponent<UnityADSAction>().GetIsSkipped()) {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Showing AD");
        }
        Debug.Log("Showing AD Done");
        if (GetComponent<UnityADSAction>().GetIsFinished()) {
            StartCoroutine(Rewarding(0));
        } else if (GetComponent<UnityADSAction>().GetIsSkipped()) {
            StartCoroutine(Rewarding(0, false));
        }
    }

    public IEnumerator Rewarding(int time, bool rewarding = true) {
        yield return new WaitForSeconds(time);
        if (ADTypeID != 3) {
            GameObject obtain = Instantiate(ItemObtained);
            LanguageManager.Instance().SetLangText(obtain.transform.GetChild(2).GetComponent<Text>(), 25003);// 25003 = Congratulations on getting it.
            LanguageManager.Instance().SetLangText(obtain.transform.GetChild(3).GetComponent<Text>(), currentADShow.DesceptionTxtID);

            if (currentADShow == MainMenuAD) {
                LanguageManager.Instance().SetLangText(obtain.transform.GetChild(2).GetComponent<Text>(), 26012);// 26012 = Thank you for your support of JIALIAN ENZYME, please receive funds 1000
                obtain.transform.GetChild(3).GetComponent<Text>().text = "";
            }
        }
        AudioManager.Instance().RunMainSnap();
        GameManager.Instance().ForceIdle();
        if (rewarding) {
            currentADShow.Reward();
        }
    }

    public bool GetRelatedAction(int ADID) { // call by other class for counter is availae or not
        if (FindADCountWithID(ADID) > 0) {
            return true;
        }
        return false;
    }
    public int GetRelatedActionCount(int ADID) { // counter နံပါတ် ပါ တယ်
        if (FindADCountWithID(ADID) > 0) {
            return FindADCountWithID(ADID);
        }
        return 0;
    }
    public void SetRelatedActionUse(int ADID) { // decreasing the chance count when use 
        if (FindADCountWithID(ADID) > 0) {
            SaveMinusCounterInDB(ADID);
        }
    }

    ADType FindADTypeWithID(int id) { // retrun from array
        for (int i = 0; i < adsRewards.Length; i++) {
            if (adsRewards[i].id == id) {
                return adsRewards[i];
            }
        }
        return null;
    }
    public int FindADCountWithID(int id) { // retrun from list
        for (int i = 0; i < ADObtains.Count; i++) {
            if (ADObtains[i].GetADID() == id) {
                return ADObtains[i].GetCounter();
            }
        }
        return 0;
    }
    public DateTime FindADDateTimeWithID(int id) { // retrun from list
        for (int i = 0; i < ADObtains.Count; i++) {
            if (ADObtains[i].GetADID() == id) {
                return ADObtains[i].GetDataTime();
            }
        }
        return DateTime.Now;
    }

    void SetCurrentADShowID(int ADTypeID) {
        if (ADTypeID == 1) {
            currentADShow = MainMenuAD;
        } else if (ADTypeID == 2) {
            currentADShow = FunPercentToSelect.PercentToSelect(adsRewards);
        } else if (ADTypeID == 3) {
            currentADShow = BuildingAD;
        }
    }

    int CalculateDuration(string startStr) {
        DateTime startTime = FunDateTimeAccess.StringToDateTime(startStr);
        TimeSpan timeSpan = DateTime.Now - startTime;
        return (int)timeSpan.TotalSeconds;
    }
}
