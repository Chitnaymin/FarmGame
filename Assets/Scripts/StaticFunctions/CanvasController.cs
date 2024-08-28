using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {
		
    public void ShowCanvas(GameObject canvas) {
        canvas.SetActive(true);
    }
    public void HideCanvas(GameObject canvas) {
        canvas.SetActive(false);
    }
}
