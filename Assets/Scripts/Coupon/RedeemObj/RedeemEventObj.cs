using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedeemEventObj {

    int id;
    string key_code;
    string eventName;
    string start_date;
    string endDate;
    RedeemGiveawayObj[] redeemGiveawayObjs;

    public RedeemEventObj(int id, string key_code, string eventName, string start_date, string endDate, RedeemGiveawayObj[] redeemGiveawayObjs) {
        this.id = id;
        this.key_code = key_code;
        this.eventName = eventName;
        this.start_date = start_date;
        this.endDate = endDate;
        this.redeemGiveawayObjs = redeemGiveawayObjs;
    }
}
