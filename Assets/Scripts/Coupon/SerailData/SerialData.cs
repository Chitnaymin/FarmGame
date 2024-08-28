using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class SerialData : MonoBehaviour {
    public Serial number;
    bool flag = false;
    public void GetSerialNumber(string keycode) {
        flag = false;
        StartCoroutine(Upload(keycode));
    }

    public IEnumerator Upload(string keycode) {
        string c = "{key_code: \"" + keycode + "\"}";

        byte[] myData = System.Text.Encoding.UTF8.GetBytes(c);

        UnityWebRequest www = UnityWebRequest.Post("https://rsn.azurewebsites.net/api/redeem/", "");
        www.uploadHandler = new UploadHandlerRaw(myData);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.method = UnityWebRequest.kHttpVerbPOST;
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        } else {
			JSONNode Node = JSON.Parse(www.downloadHandler.text);

            int maxNum = Node.Count;
            number = new Serial();
            
            string serialNoUntroked = Node["serial_no"].ToString();
            string[] splitArray = serialNoUntroked.Split(char.Parse("\"")); //Here we assing the splitted string to array by that char
            number.serial_no = splitArray[1];
            number.success = Node["success"];
            number.message = Node["message"].ToString();
			Debug.Log("Serial Obtain Complete!");

            flag = true;
        }

        yield return number.serial_no;  
    }
    public string GetSerialNo() {
        if (flag) {

            return number.serial_no ;
        }
        return "";
    }
}


public class Serial {
    public string serial_no;
    public bool success;
    public string message;
}
