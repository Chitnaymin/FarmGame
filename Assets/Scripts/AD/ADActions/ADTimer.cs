using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New ADTimer", menuName = "AD Data/ADTimer", order = 66)]
public class ADTimer : ADType, ITimerable {
    public int TimerDuration;

    public override void Reward() {
        base.Reward();
        ADManager.Instance().SaveTimeInDB(base.id, 1);
        ADManager.Instance().CheckTimer();
    }
    public bool Action(DateTime StartRewardedTime) {
        TimeSpan differentTime = DateTime.Now - StartRewardedTime;
        if (differentTime.TotalSeconds < TimerDuration) { // action allow to work 
            return true;
        }
        return false;
    }
}
