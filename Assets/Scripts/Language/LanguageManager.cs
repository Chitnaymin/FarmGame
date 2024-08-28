using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : SingletonBehaviour<LanguageManager> {
    public Text[] texts;
    public LanguageButton[] LanguageButtons;
    public Font[] fonts;

    StorageManager dbsm;
    List<TextID> TextsList = new List<TextID>();
    string[] lang = { "English", "Taiwanese", "Chinese" };
    FontStyle[] sft = { FontStyle.Bold, FontStyle.Normal, FontStyle.Normal };
    // application restart might need for changing langauage

    void Start() {
        dbsm = new StorageManager();
        FillText();
    }
    void FillText() {
        TextsList = dbsm.LanguageData(lang[FunLanguageSetting.LANGUAGE()]);
        for (int i = 0; i < texts.Length; i++) {
            try {
                texts[i].text = SearchTextList(texts[i].GetComponent<TextID>().TextIDNumber);
                texts[i].font = GetFontBySeletion();
                texts[i].fontStyle = GetFontStyle();
            } catch (NullReferenceException ne) {
                Debug.Log("Game Manager Element ID missing in : " + i);
            }
        }
    }

    internal void SetLangText(Text text, int txtID, int fontsize) {
        if (FunLanguageSetting.LANGUAGE() == 0) {
            text.fontSize = fontsize;
        } else {
            text.fontSize = (fontsize / 4) * 5;
        }

        SetLangText(text, txtID);
    }

    internal void SetLangText(Text text, int txtID) {
        text.text = SearchTextList(txtID);
        text.font = GetFontBySeletion();
        text.fontStyle = GetFontStyle();
    }

    public string SearchTextList(int id) {
        for (int i = 0; i < TextsList.Count; i++) {
            if (TextsList[i].TextIDNumber == id) {
                return TextsList[i].TextString;
            }
        }
        return "ID not Found: " + id;
    }

    public Font GetFontBySeletion() {
        return fonts[FunLanguageSetting.LANGUAGE()];
    }

    public FontStyle GetFontStyle() {
        return sft[FunLanguageSetting.LANGUAGE()];
    }

    public void ChangeLanguage(int code) { // call by btn
        FunLanguageSetting.CHANGELANGUAGE(code);
        for (int i = 0; i < LanguageButtons.Length; i++) {
            if (code == LanguageButtons[i].LanguageCode) {
                LanguageButtons[i].Click();
            } else {
                LanguageButtons[i].UnClick();
            }
        }
        FillText();
        ShopManager.Instance().RefreshTab();
        AchievementManager.Instance().RefreshLang();
        TechnicalManager.Instance().RefreshLang();
        GameManager.Instance().ForceIdle();
	}
}
