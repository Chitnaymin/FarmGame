using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class UnityADSAction : MonoBehaviour, IUnityAdsListener {
    private string gameId = "3455917";
    bool testMode = true;
    string myPlacementId = "rewardedVideo";

    public Button ADshowBtn;

    bool isFinished = false;
    bool isSkipped = false;

    private void Start() {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);

        ADshowBtn.interactable = Advertisement.IsReady(myPlacementId);
    }

    public void AdShower() {
        isFinished = false;
        isSkipped = false;
        if (Advertisement.IsReady()) {
            Advertisement.Show(myPlacementId);
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        if (showResult == ShowResult.Finished) {
            isFinished = true;
        } else if (showResult == ShowResult.Skipped) {
            isSkipped = true;
            Debug.Log("Skipped");
        } else if (showResult == ShowResult.Failed) {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public bool GetIsFinished() {
        return isFinished;
    }

    public bool GetIsSkipped() {
        return isSkipped;
    }

    public void OnUnityAdsReady(string placementId) {
        if (placementId == myPlacementId) {
            Debug.Log("Ready");
            ADshowBtn.interactable = true;
        }
    }

    public void OnUnityAdsDidError(string message) {
        Debug.LogError(message);
    }

    public void OnUnityAdsDidStart(string placementId) {
        Debug.Log("ADStarted");
    }

    public override bool Equals(object obj) {
        return obj is UnityADSAction aDS &&
               base.Equals(obj) &&
               GetIsFinished() == aDS.GetIsFinished() &&
               GetIsSkipped() == aDS.GetIsSkipped();
    }
}
