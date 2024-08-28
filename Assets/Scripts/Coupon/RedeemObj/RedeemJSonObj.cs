using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedeemJSonObj : MonoBehaviour {
    int  id;
    int event_id;
    string serial_no;
    string create_date;
    int giveaway_id;
    string redeemed_date;
    RedeemEventObj eventObj;
    RedeemGiveawayObj giveawayObj;

    public RedeemJSonObj(int id, int event_id, string serial_no, string create_date, int giveaway_id, string redeemed_date,  RedeemEventObj eventObj, RedeemGiveawayObj giveawayObj) {
        this.id = id;
        this.event_id = event_id;
        this.serial_no = serial_no;
        this.create_date = create_date;
        this.giveaway_id = giveaway_id;
        this.redeemed_date = redeemed_date;
        this.eventObj = eventObj;
        this.giveawayObj = giveawayObj;
    }

    public int GetId() {
        return id;
    }

    public void SetId(int value) {
        id = value;
    }

    public int GetEvent_id() {
        return event_id;
    }

    public void SetEvent_id(int value) {
        event_id = value;
    }

    public string GetSerial_no() {
        return serial_no;
    }

    public void SetSerial_no(string value) {
        serial_no = value;
    }

    public string GetCreate_date() {
        return create_date;
    }

    public void SetCreate_date(string value) {
        create_date = value;
    }

    public int GetGiveaway_id() {
        return giveaway_id;
    }

    public void SetGiveaway_id(int value) {
        giveaway_id = value;
    }

    public string GetRedeemed_date() {
        return redeemed_date;
    }

    public void SetRedeemed_date(string value) {
        redeemed_date = value;
    }

    public RedeemEventObj GetEventObj() {
        return eventObj;
    }

    public void SetEventObj(RedeemEventObj value) {
        eventObj = value;
    }

    public RedeemGiveawayObj GetGiveawayObj() {
        return giveawayObj;
    }

    public void SetGiveawayObj(RedeemGiveawayObj value) {
        giveawayObj = value;
    }
}
