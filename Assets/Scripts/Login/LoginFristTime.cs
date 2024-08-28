using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginFristTime : MonoBehaviour {
    public InputField idText;
    public void BtnLogin1stTime() { // call by btn
        string idName = idText.text;
        if (idName == "") {
            Debug.Log("not to be null");
        } else {
            Debug.Log(idName);
            PlayerData.Instance().SetPlayerName(idName);
            PlayerPrefs.SetInt("firstTime",1);
            transform.parent.gameObject.GetComponent<LoginManager>().LoginStart();
        }

    }
}
