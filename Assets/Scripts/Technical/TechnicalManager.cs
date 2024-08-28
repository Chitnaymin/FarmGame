using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class TechnicalManager : SingletonBehaviour<TechnicalManager> {

    public Text txtCoin;
    public Text ImprovementTxt;
    public Text QualityTxt;
    public Text EvolutionTxt;
    public Text ProductTxt;
    public GameObject PrgressBar;
    public Image PrgressBarImage;
    public Text ProgressText;
    public List<Toggle> ToggleList;
    public Text Title;
    public Text Description;
    public Text Percentage;
    public Text PercentageNo;
    public Text GoldNumber;
    public Button UpgradeBtn;
    public Button FinishBtn;
    public Text textProgress;
    public Technology dataTechnology;
    public int currentLevel;

    private Toggle lastToggle;
    public List<GameObject> selectedImg;
    //List<GameObject> selectedImg = new List<GameObject>();
    [HideInInspector]
    public Technology[] scData;
    private int nextUpdate = 0;
    private string startingTime;
    private DateTime startTechnicalTime;
    private int duration;

    private int originCoin;
    private int technicalProcess = 0;

    void Start() {
        scData = Resources.LoadAll<Technology>("Technical");
        foreach (Toggle tog in ToggleList) {
            tog.onValueChanged.AddListener(delegate {
                ToggleValueChanged(tog);
            });
        }
        technicalProcess = 0;
        RefreshData();
        AssignByDB();
        ClearRightPanel();
        for (int i = 0; i < selectedImg.Count; i++) {
            selectedImg[i].SetActive(false);
        }
        ToggleList[0].isOn = true;
        SelectToggle(ToggleList[0], true);
    }

    public void RefreshLang() {
        if (lastToggle == null) {
            lastToggle = ToggleList[0];
        }
        SelectToggle(lastToggle, true);
        dataTechnology = lastToggle.GetComponent<TechnologyCell>().Data;
        LanguageManager.Instance().SetLangText(Title, dataTechnology.TitleTxtID);
        LanguageManager.Instance().SetLangText(Description, dataTechnology.DescriptionTxtID);
        LanguageManager.Instance().SetLangText(Percentage, dataTechnology.Percentage);
    }

    void Update() {
        if (Time.time >= nextUpdate && technicalProcess != 0) {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }
    }
    private void UpdateEverySecond() {
        TimeSpan differentTime = DateTime.Now - startTechnicalTime;
        if (differentTime.TotalSeconds < duration) {
            float progressLine = FunProgressCalculator.PercentageCalculator((int)differentTime.TotalSeconds, duration);
            int progressInt = Mathf.FloorToInt(progressLine * 100);
            ProgressText.text = progressInt.ToString() + "%";
            PrgressBarImage.fillAmount = progressLine;

            SelectToggle(lastToggle, true);
            LanguageManager.Instance().SetLangText(Title, dataTechnology.TitleTxtID);
            LanguageManager.Instance().SetLangText(Description, dataTechnology.DescriptionTxtID);
            LanguageManager.Instance().SetLangText(Percentage, dataTechnology.Percentage);
            PercentageNo.text = dataTechnology.levelDetails[currentLevel].PercentValue.ToString() + "%";
        } else {
            if (dataTechnology.TechName == "Improvement") {
                PlayerData.Instance().SetImprovement(currentLevel + 1);
            } else if (dataTechnology.TechName == "Quality") {
                PlayerData.Instance().SetQuality(currentLevel + 1);
            } else if (dataTechnology.TechName == "Development") {
                PlayerData.Instance().SetEvolution(currentLevel + 1);
            } else if (dataTechnology.TechName == "Product") {
                PlayerData.Instance().SetProduct(currentLevel + 1);
            }

            UpgradeBtn.gameObject.SetActive(true);
            PrgressBar.SetActive(false);

            technicalProcess = 0;
            PlayerData.Instance().SetCurrentUpgrade(technicalProcess);
            RefreshData();
        }
    }

    public void ClearRightPanel() {
        for (int i = 0; i < selectedImg.Count; i++) {
            selectedImg[i].SetActive(false);
        }
    }

    public void SelectToggle(Toggle toggle, bool whatever = false) {
        if (toggle.isOn || whatever) {
            for (int i = 0; i < selectedImg.Count; i++) {
                if (toggle.name == selectedImg[i].transform.parent.name) {
                    selectedImg[i].SetActive(true);
                } else {
                    selectedImg[i].SetActive(false);
                }
            }
        }
    }

    void ToggleValueChanged(Toggle toggle) {
        Debug.Log("technicalProcess by ToggleValueChanged " + technicalProcess);
        if (technicalProcess == 0) {
            lastToggle = toggle;
            dataTechnology = toggle.GetComponent<TechnologyCell>().Data;
            currentLevel = toggle.GetComponent<TechnologyCell>().CurrentLevel;
            UpgradeBtn.interactable = true;
            SelectToggle(lastToggle);

            LanguageManager.Instance().SetLangText(Title, dataTechnology.TitleTxtID);
            LanguageManager.Instance().SetLangText(Description, dataTechnology.DescriptionTxtID);
            LanguageManager.Instance().SetLangText(Percentage, dataTechnology.Percentage);
            PercentageNo.text = dataTechnology.levelDetails[currentLevel].PercentValue.ToString() + "%";

            if (currentLevel >= dataTechnology.levelDetails.Count) {
                //DImp.text = "Max Level";
                UpgradeBtn.gameObject.SetActive(false);
                FinishBtn.gameObject.SetActive(true);
                return;
            } else {
                // Description.text = dataTechnology.levelDetails[currentLevel].Description;
            }
            if (technicalProcess != 0) {
                UpgradeBtn.gameObject.SetActive(false);
                PrgressBar.SetActive(true);
                return;
            }
            UpgradeBtn.gameObject.SetActive(true);
            FinishBtn.gameObject.SetActive(false);

            int reqCoin = dataTechnology.levelDetails[currentLevel].Price;
            GoldNumber.text = reqCoin.ToString();
            if (reqCoin > originCoin) {
                UpgradeBtn.interactable = false;
            }
        }
    }

    public void UpdateDB() { // called by btn
        int reqCoin = dataTechnology.levelDetails[currentLevel].Price;

        originCoin -= reqCoin;
        PlayerData.Instance().SetCoin(originCoin);
        DisplayGold();

        startTechnicalTime = DateTime.Now;
        startingTime = FunDateTimeAccess.DateTimeToString(startTechnicalTime);

        if (dataTechnology.TechName == "Improvement") {
            technicalProcess = 1;
            duration = dataTechnology.levelDetails[currentLevel].ImprovingTime;
        } else if (dataTechnology.TechName == "Quality") {
            technicalProcess = 2;
            duration = dataTechnology.levelDetails[currentLevel].ImprovingTime;
        } else if (dataTechnology.TechName == "Development") {
            technicalProcess = 3;
            duration = dataTechnology.levelDetails[currentLevel].ImprovingTime;
        } else if (dataTechnology.TechName == "Product") {
            technicalProcess = 4;
            duration = dataTechnology.levelDetails[currentLevel].ImprovingTime;
        }

        PlayerData.Instance().SetCurrentUpgrade(technicalProcess);
        PlayerData.Instance().SetUpgradeStartTime(startingTime);
        int Progress = AchievementManager.Instance().pdb.GetCurrentProgress((int)E_AchType.LabUpgrade);
        AchievementManager.Instance().MajorAchievementUpdate(E_AchType.LabUpgrade, Progress + 1);

        UpgradeBtn.gameObject.SetActive(false);
        PrgressBar.SetActive(true);

    }

    void AssignByDB() {
        technicalProcess = PlayerData.Instance().GetCurrentUpgrade();
        if (technicalProcess != 0) { // currently running some upgrade
            lastToggle = ToggleList[technicalProcess - 1];
            //SelectToggle(lastToggle, true);
            FinishBtn.gameObject.SetActive(false);
            UpgradeBtn.gameObject.SetActive(false);
            PrgressBar.SetActive(true);

            startingTime = PlayerData.Instance().GetUpgradeStartTime();
            startTechnicalTime = FunDateTimeAccess.StringToDateTime(startingTime);
            dataTechnology = SearchByTechID(technicalProcess);
            currentLevel = 0;
            switch (technicalProcess) {
                case 1:
                    currentLevel = PlayerData.Instance().GetImprovement();
                    break;
                case 2:
                    currentLevel = PlayerData.Instance().GetQuality();
                    break;
                case 3:
                    currentLevel = PlayerData.Instance().GetEvolution();
                    break;
                case 4:
                    currentLevel = PlayerData.Instance().GetProduct();
                    break;
            }
            duration = dataTechnology.levelDetails[currentLevel].ImprovingTime;

        } else {
            FinishBtn.gameObject.SetActive(false);
            UpgradeBtn.gameObject.SetActive(true);
            PrgressBar.SetActive(false);
        }

    }

    Technology SearchByTechID(int id) {
        return scData[id - 1];
    }

    public float GetImprovementCurrentPercentage() {
        if (PlayerData.Instance().GetImprovement() == 0) {
            return 0;
        }
        float percent = SearchByTechID(1).levelDetails[PlayerData.Instance().GetImprovement() - 1].PercentValue;
        return percent;
    }
    public float GetQualityCurrentPercentage() {
        if (PlayerData.Instance().GetQuality() == 0) {
            return 0;
        }
        float percent = SearchByTechID(2).levelDetails[PlayerData.Instance().GetQuality() - 1].PercentValue;
        return percent;
    }
    public float GetEvolutionCurrentPercentage() {
        if (PlayerData.Instance().GetEvolution() == 0) {
            return 0;
        }
        float percent = SearchByTechID(3).levelDetails[PlayerData.Instance().GetEvolution() - 1].PercentValue;
        return percent;
    }
    public int GetProductCurrentPercentage() {
        if (PlayerData.Instance().GetProduct() == 0) {
            return 5; // basic drop percentage
        }
        float percent = SearchByTechID(4).levelDetails[PlayerData.Instance().GetProduct() - 1].PercentValue;
        return (int)percent;
    }

    void RefreshData() {
        DisplayGold();

        int ImprovementLevel = PlayerData.Instance().GetImprovement();
        int QualityLevel = PlayerData.Instance().GetQuality();
        int EvolutionLevel = PlayerData.Instance().GetEvolution();
        int ProductLevel = PlayerData.Instance().GetProduct();

        ToggleList[0].GetComponent<TechnologyCell>().RefreshData(scData[0], ImprovementLevel);
        ToggleList[1].GetComponent<TechnologyCell>().RefreshData(scData[1], QualityLevel);
        ToggleList[2].GetComponent<TechnologyCell>().RefreshData(scData[2], EvolutionLevel);
        ToggleList[3].GetComponent<TechnologyCell>().RefreshData(scData[3], ProductLevel);

        int process = PlayerData.Instance().GetCurrentUpgrade();

        if (lastToggle != null) {
            ToggleValueChanged(lastToggle);
        }
    }

    void DisplayGold() {
        originCoin = PlayerData.Instance().GetCoin();
        txtCoin.text = originCoin.ToString();
    }
}

