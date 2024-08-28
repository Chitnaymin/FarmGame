using UnityEngine;
using UnityEngine.UI;

public class HideButton : SingletonBehaviour<HideButton> {
    public Button btnLab;
    public GameObject LabNoti;
    
    public void showLabButton() {
        btnLab.interactable = true;
    }
    public void hideLabButton() {
        btnLab.interactable = false;
    }

    public void EventTriggerOnButton() {
        if(btnLab.interactable) {
            return;
        } else {
            LabNoti.SetActive(true);
        }
    }

    public void closeBtn() {
        LabNoti.SetActive(false);
    }
}

