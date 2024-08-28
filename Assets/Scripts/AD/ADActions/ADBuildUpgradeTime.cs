using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New ADBuildUpgradeTime", menuName = "AD Data/ADBuildUpgradeTime", order = 66)]
public class ADBuildUpgradeTime : ADType {

    GameObject GObuilding;

    public override void Reward() {
        base.Reward();
        Debug.Log("Here is reward");
        ChangeStartTime();
    }

    public void SetGameObject(GameObject GObuilding) {
        this.GObuilding = GObuilding;
    }

    public void ChangeStartTime() {
        if (GObuilding.GetComponent<Building>().GetProcess() == 0) {
            //int durationSeconds = GObuilding.GetComponent<Building>().GetDuration();
            //durationSeconds = (int)(durationSeconds * 0.5f);
            //TimeSpan interval = TimeSpan.FromSeconds(durationSeconds);
            TimeSpan interval = new TimeSpan(0, 0, 1, 0, 0);
            DateTime dt = GObuilding.GetComponent<Building>().GetStartBuildTime() - interval;
            GObuilding.GetComponent<Building>().SetStartBuildTime(dt);
        } else if (GObuilding.GetComponent<Production>().GetProductionProcess() == 1) {
            //int durationSeconds = GObuilding.GetComponent<Production>().GetProductionDuration();
            //durationSeconds = (int)(durationSeconds * 0.5f);
            //TimeSpan interval = TimeSpan.FromSeconds(durationSeconds);
            TimeSpan interval = new TimeSpan(0, 0, 1, 0, 0);
            DateTime dt = GObuilding.GetComponent<Production>().GetStartProduceTime() - interval;
            GObuilding.GetComponent<Production>().SetStartProduceTime(dt);
        }
    }
}
