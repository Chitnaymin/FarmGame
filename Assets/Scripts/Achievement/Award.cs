using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Award : MonoBehaviour {
    [SerializeField]
    public int id;
    [SerializeField]
    private string title;
    [SerializeField]
    private string desception;
    [SerializeField]
    private string serialCode;
	[SerializeField]
	private int playerTitle;

	private GameObject awardRef;

	public string GetSerialCode() {
		return serialCode;
	}

	public void SetSerialCode(string value) {
		serialCode = value;
	}

	public string GetTitle() {
        return title;
    }

    public void SetTitle(string value) {
        title = value;
    }

	public int GetPlayerTitle() {
		return playerTitle;
	}

	public void SetPlayerTitle(int value) {
		playerTitle = value;
	}

	public void SetAwardData(int id, GameObject awardRef,string title, string desception, string serial,int playerTitle) {
        this.id = id;
        this.awardRef = awardRef;
        this.title = title;
        this.desception = desception;
        this.serialCode = serial;
		this.playerTitle = playerTitle;
    }

    public void BtnCoupon() {// call by btn
		string serial = AchievementManager.Instance().pdb.GetSerial(serialCode);
		if (serial == "_") {
			AchievementManager.Instance().GetComponent<RewardAction>().btnRetrieveingSerial(serialCode);
		} else {
			AchievementManager.Instance().GetComponent<RewardAction>().GetSerialForShow(serial);
		}
    }
    public void BtnUseTitle() {
		PlayerData.Instance().SetTitle(GetPlayerTitle());
		AchievementManager.Instance().ShowTitle();
	}
}
