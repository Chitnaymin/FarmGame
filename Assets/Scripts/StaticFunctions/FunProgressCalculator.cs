using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunProgressCalculator {
    public static float PercentageCalculator(float currentProgress, float MasProgress, float LayOutMax) {
        return currentProgress * (LayOutMax / MasProgress);
    }

    public static float PercentageCalculator(float currentProgress, float MasProgress) {
        if (currentProgress / MasProgress > 1) {
            return 1;
        }
        return currentProgress / MasProgress;
    }

    public static string ProgressToString(float currentProgress, float MasProgress) {
        return "(" + currentProgress.ToString() + "/" + MasProgress.ToString() + ")";
    }

    public static string HourMinSecTransform(TimeSpan differentTime, float duration) {
        string OverAll = "";
        int differentDurationSec = (int)duration - (int)differentTime.TotalSeconds;
        TimeSpan timeSpanTemp = TimeSpan.FromSeconds(differentDurationSec);

        if (timeSpanTemp.Days != 0) {
            OverAll += timeSpanTemp.Days + "d ";
        }
        if (timeSpanTemp.Hours != 0) {
            OverAll += timeSpanTemp.Hours + "h ";
        }
        if (timeSpanTemp.Minutes != 0) {
            OverAll += timeSpanTemp.Minutes + "m ";
        }
        if (timeSpanTemp.Seconds != 0) {
            OverAll += timeSpanTemp.Seconds + "s";
        }
        return OverAll;
    }

    public static bool PossibilityCalculator(int percentage) {
        int randomNum = UnityEngine.Random.Range(0, 99);
        Debug.Log("randomNum : " + randomNum + " and percent : " + percentage + " if random Num is smaller item will drop");
        if (randomNum < percentage) {
            return true;
        }
        return false;

    }
}
