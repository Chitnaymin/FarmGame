using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RewardAction : MonoBehaviour {
    string[] keycodelist = {"levelTo30", "sold300times", "done30quests", "login30times", "sharedLinksToFriends" };

	public GameObject SerialPanel;
    public GameObject loadingPanel;
	public GameObject LoadCompleteBox;
	public Text serailText;
    string s = "";

    private void Start() {
		SerialPanel.SetActive(false);
		loadingPanel.SetActive(false);
		LoadCompleteBox.SetActive(false);
	}

    void Update() {
        //if (Input.GetKeyDown(KeyCode.A)) {
        //    loadingPanel.SetActive(true);
        //    StartCoroutine(retrieveingSerial(0));
        //}
    }

    IEnumerator retrieveingSerial(string keycode) {
        yield return null;
        s = "";
        GetComponent<SerialData>().GetSerialNumber(keycode);
        while (s == "" ) {
            yield return new WaitForSeconds(0.2f);
            s = GetComponent<SerialData>().GetSerialNo();
            serailText.text = s;
        }
        if (s != "") {
			SerialPanel.SetActive(true);
			loadingPanel.SetActive(false);
			LoadCompleteBox.SetActive(true);
			serailText.text = s;
			AchievementManager.Instance().pdb.UpdateSerial(keycode, s);
		}
    }


    public void btnRetrieveingSerial(string keycode) { // call by btn
		SerialPanel.SetActive(true);
		loadingPanel.SetActive(true);
        StartCoroutine(retrieveingSerial(keycode));
    }

	public void GetSerialForShow(string serial) {
		SerialPanel.SetActive(true);
		loadingPanel.SetActive(false);
		LoadCompleteBox.SetActive(true);
		serailText.text = serial;
	}

    public void CopyToClipboard() {// call by btn
        GUIUtility.systemCopyBuffer = s;
    }
}
