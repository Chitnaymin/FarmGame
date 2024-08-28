using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedeemGiveawayObj {
    int id;
    int eventId;
    string eventName;
    string amount;
    int sequence;

    public RedeemGiveawayObj(int id, int eventId, string eventName, string amount, int sequence) {
        this.id = id;
        this.eventId = eventId;
        this.eventName = eventName;
        this.amount = amount;
        this.sequence = sequence;
    }
}
