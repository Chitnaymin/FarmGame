using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunPercentToSelect : MonoBehaviour {
    public static ADType PercentToSelect(ADType[] aDType) {
        int random = Random.Range(1, 100);
        int pointA = 0, pointB = 0;
        pointB = aDType[0].percentage;
        for (int i = 0; i < aDType.Length; i++) {
            if (random > pointA && random <= pointB) {
                return aDType[i];
            } 
            pointA += aDType[i].percentage;
            pointB = (i == aDType.Length - 1) ? 100 : (pointA + aDType[i + 1].percentage);
        }
        return aDType[0];
    }
}
