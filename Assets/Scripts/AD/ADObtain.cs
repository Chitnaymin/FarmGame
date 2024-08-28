using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADObtain : MonoBehaviour {

    int ADID;
    int Counter;
    string StartTime;
    DateTime dataTime;

    

    public ADObtain(int aDID, int counter, string startTime) {
        ADID = aDID;
        Counter = counter;
        StartTime = startTime;
    }

    public int GetADID() {
        return ADID;
    }

    public void SetADID(int value) {
        ADID = value;
    }

    public int GetCounter() {
        return Counter;
    }

    public void SetCounter(int value) {
        Counter = value;
    }

    public string GetStartTime() {
        return StartTime;
    }

    public void SetStartTime(string value) {
        StartTime = value;
    }

    public DateTime GetDataTime() {
        dataTime = FunDateTimeAccess.StringToDateTime(StartTime);
        return dataTime;
    }

    public void SetDataTime(DateTime value) {
        dataTime = value;
    }
}
