using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour {
    private static GameObject tempLatestGO;
    public static GameObject CameraAimingCells() {
        Camera cam = Camera.main;
        RaycastHit2D[] hit2D = Physics2D.RaycastAll(cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane)), Vector2.zero);
        foreach (RaycastHit2D ele in hit2D) {
            if (ele.collider.tag == "ground") {
                tempLatestGO = ele.collider.gameObject;
                return ele.collider.gameObject;
            }
        }
        return tempLatestGO;
    }
}
