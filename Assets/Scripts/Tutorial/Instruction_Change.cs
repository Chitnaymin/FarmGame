using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instruction_Change : MonoBehaviour {
    public GameObject TutorialCanvas;
    public Image Instruct;
    public Text TextLeft;
    public Text TextRight;
    public Text ButtonText;

    public Instruction[] Nice;

    int currentIndex = 0;

    private void Start() {
        ButtonText.text = LanguageManager.Instance().SearchTextList(11); // next
        ChangeInstruction1(0);
    }
    public void BTNChange() {
        TutorialCanvas.SetActive(true);
        currentIndex++;
        if (currentIndex == Nice.Length) {
            ButtonText.text = LanguageManager.Instance().SearchTextList(11); // next
            TutorialCanvas.SetActive(false);
            BtnSetCurrentIndexToZero();
        } else {
            ButtonText.text = LanguageManager.Instance().SearchTextList(11); // next
            ChangeInstruction1(currentIndex);
        }
        if (currentIndex == Nice.Length - 1) {
            ButtonText.text = LanguageManager.Instance().SearchTextList(12); // close
        }
    }
    public void ChangeInstruction1(int id) {
        Instruct.sprite = Nice[id].image;
        LanguageManager.Instance().SetLangText(TextLeft, Nice[id].leftText, 32);
        LanguageManager.Instance().SetLangText(TextRight, Nice[id].rightText, 32);
    }
    public void BtnSetCurrentIndexToZero() { // also called by btn
        currentIndex = 0;
        ChangeInstruction1(0);
        ButtonText.text = LanguageManager.Instance().SearchTextList(11); // next
    }
}
[System.Serializable]
public class Instruction {
    public Sprite image;
    public int leftText;
    public int rightText;
    public int buttonText;
}
