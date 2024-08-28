using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfSetText : MonoBehaviour {

    public GameObject[] texts;
    private void OnEnable() {
        for (int i = 0; i < texts.Length; i++) {
            LanguageManager.Instance().SetLangText(texts[i].GetComponent<Text>(), texts[i].GetComponent<TextID>().TextIDNumber);
        }
    }
    private void Start() {
        //StartCoroutine(setLang());
        for (int i = 0; i < texts.Length; i++) {
            LanguageManager.Instance().SetLangText(texts[i].GetComponent<Text>(), texts[i].GetComponent<TextID>().TextIDNumber);
        }
    }

    IEnumerator setLang() {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < texts.Length; i++) {
            LanguageManager.Instance().SetLangText(texts[i].GetComponent<Text>(), texts[i].GetComponent<TextID>().TextIDNumber);
        }
    }
}
