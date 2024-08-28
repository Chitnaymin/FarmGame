using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunLanguageSetting : MonoBehaviour {
    public static int LANGUAGE() {
        return PlayerPrefs.GetInt("Language", 1);
    }
    public static void CHANGELANGUAGE(int LangCode) {
        PlayerPrefs.SetInt("Language", LangCode);
    }
}
