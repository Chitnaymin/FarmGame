using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToSound : MonoBehaviour {
    public void FinishEnter() {
        AudioManager.Instance().PlaySound(7);
    }
    public void ClickAll() {
        AudioManager.Instance().PlaySound(8);
    }
    
}
