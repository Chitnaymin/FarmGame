using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginAgain : MonoBehaviour {
	
	public void BtnLogin() {
		int i = PlayerPrefs.GetInt("firstTime", 0);
		if (i == 0) {
			PlayerPrefs.SetInt("Default", 0);
			GameManager.Instance().CheckName();
			PlayerData.Instance().SetPlayerName(LanguageManager.Instance().SearchTextList(21));
			PlayerPrefs.SetInt("firstTime", 1);
			transform.parent.gameObject.GetComponent<LoginManager>().LoginStart();

		} else {
			transform.parent.gameObject.GetComponent<LoginManager>().LoginStart();
		}
	}
}
