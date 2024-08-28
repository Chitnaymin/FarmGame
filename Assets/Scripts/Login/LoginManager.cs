using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : SingletonBehaviour<LoginManager> {
    public GameObject LoginCanvas;
    public GameObject tutorialCanvas;

    PlayerStorageManager pdb;
    GameObject logCanvasClone;

	private void Start() {
        //PlayerPrefs.DeleteAll();
        logCanvasClone = Instantiate(LoginCanvas);
        logCanvasClone.transform.SetParent(transform);
    }
    /*
    public Text debugt;
    private void Update() {
        debugt.text = PlayerPrefs.GetInt("tutorialFirstTime", 0).ToString();
    }*/

    public void LoginStart() { 
        pdb = new PlayerStorageManager();
        System.DateTime lastLoginTime = FunDateTimeAccess.StringToDateTime(pdb.GetLoginDate());
        System.DateTime currentTime = System.DateTime.Now;
        int DayDif = (currentTime - lastLoginTime).Days;
        if (DayDif > 0) {
			ResetDB(currentTime);
            AchievementManager.Instance().DelayStart();
			int Progress = AchievementManager.Instance().pdb.GetCurrentProgress((int)E_AchType.Login);
			AchievementManager.Instance().MajorAchievementUpdate(E_AchType.Login, Progress +1);
			DailyLogin();
        } else {
            AchievementManager.Instance().DelayStart();
        }
		AudioManager.Instance().PlaySound(1, true);
        ShowTutorialForFirstTime();
        GameManager.Instance().ForceIdle();
        Destroy(gameObject);
    }
    void ResetDB(System.DateTime lastLoginTime) {
        string s = FunDateTimeAccess.DateTimeToString(lastLoginTime.Date);
        Debug.Log(s);
        pdb.ResetLogin(0, 0, s);

	}


    void DailyLogin() {
        AchievementManager.Instance().IncreaseProgress(1);
    }

    void ShowTutorialForFirstTime() {
        int i = PlayerPrefs.GetInt("tutorialFirstTime", 0);
        if (i == 0) {
            tutorialCanvas.SetActive(true);
            PlayerPrefs.SetInt("tutorialFirstTime", 1);
        }
    }
}
