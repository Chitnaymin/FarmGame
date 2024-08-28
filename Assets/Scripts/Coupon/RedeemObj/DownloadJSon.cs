using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;

public class DownloadJSon : MonoBehaviour {

    public GameObject LoadingImage;

    List<RedeemJSonObj> RedeemJSonObjs;
    bool isDownloadComplete = false;
    void Start() {
        RedeemJSonObjs = new List<RedeemJSonObj>();
        StartCoroutine(DownloadData());
    }

   public IEnumerator DownloadData() {
        LoadingImage.SetActive(true);
        using (UnityWebRequest www = UnityWebRequest.Get("https://rsn.azurewebsites.net/api/redeem/list")) {
            isDownloadComplete = false;
            yield return www.SendWebRequest();

            if(!www.isNetworkError && !www.isHttpError)
            {
                LoadingImage.SetActive(true);
                
                
                JSONNode Node = JSON.Parse(www.downloadHandler.text);
                isDownloadComplete = true;
                int maxNum = Node.Count;
                // By Looping 
                for (int i = 0; i < maxNum; i++)
                {

                    RedeemGiveawayObj[] newRedeemGiveaway = new RedeemGiveawayObj[Node[i]["event"]["giveaways"].Count];
                    for (int j = 0; j < newRedeemGiveaway.Length; j++)
                    {
                        newRedeemGiveaway[j] = new RedeemGiveawayObj(
                            Node[i]["event"]["giveaway"][j]["id"]
                            , Node[i]["event"]["giveaway"][j]["event_id"]
                            , Node[i]["event"]["giveaway"][j]["name"]
                            , Node[i]["event"]["giveaway"][j]["amount"]
                            , Node[i]["event"]["giveaway"][j]["seq"]
                            );
                    }

                    RedeemJSonObjs.Add(new RedeemJSonObj(
                        (int)Node[i]["id"],
                        (int)Node[i]["event_id"],
                        Node[i]["serial_no"].ToString(),
                        Node[i]["create_date"].ToString(),
                        (int)Node[i]["giveaway_id"],
                        Node[i]["redeemed_date"].ToString(),
                        new RedeemEventObj(
                            (int)Node[i]["event"]["id"],
                            Node[i]["event"]["key_code"].ToString(),
                            Node[i]["event"]["name"].ToString(),
                            Node[i]["event"]["start_date"].ToString(),
                            Node[i]["event"]["end_date"].ToString(),
                            newRedeemGiveaway),
                        new RedeemGiveawayObj(
                            (int)Node[i]["giveaway"]["id"],
                            (int)Node[i]["giveaway"]["event_id"],
                            Node[i]["giveaway"]["name"].ToString(),
                            Node[i]["giveaway"]["amount"].ToString(),
                            (int)Node[i]["giveaway"]["seq"]
                            )
                        ));
                   
                }
            } else
            {
                LoadingImage.SetActive(true);
                //yield return new WaitForSeconds(3);
                //StartCoroutine(DownloadData());
            }

            LoadingImage.SetActive(false);
            Debug.Log("Download Complete");
        }
    }

    public string SearchSerailNumber(int id) {
        if (isDownloadComplete) {
            for (int i = 0; i < RedeemJSonObjs.Count; i++) {
                if (RedeemJSonObjs[i].GetId() == id) {
                    return RedeemJSonObjs[i].GetSerial_no();
                    
                }
               
            }
        } else {
            return "No Internet Connection";
        }
        return "";
    }
}