using UnityEngine;
using System;

public class ADType : ScriptableObject {
    [SerializeField]
    public int id, ADTypeID, duration, percentage, DesceptionTxtID; // duration is refresh duration 

    public virtual bool CheckingAllow(int totalSec) {
        return (totalSec > duration) ? true : false;
    }

    public virtual void StoreTime(int ADTypeID) {
        ADManager.Instance().SaveTimeToDB(ADTypeID, FunDateTimeAccess.DateTimeToString(DateTime.Now));
    }

    public virtual void Reward() {
        StoreTime(ADTypeID);
    }
}
