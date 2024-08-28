using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour {
    public int LanguageCode;
    Image image;
    void Start() {
        image = GetComponent<Image>();
        if (LanguageCode == FunLanguageSetting.LANGUAGE()) {
            image.color = new Color32(255, 0, 0, 255);
        } else {
            image.color = new Color32(255, 255, 225, 255);
        }
    }

    public void Click() {
        image.color = new Color32(255, 0, 0, 255);
    }
    public void UnClick() {
        image.color = new Color32(255, 255, 225, 255);
    }
}
