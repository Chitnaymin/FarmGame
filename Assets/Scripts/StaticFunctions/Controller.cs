using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : SingletonBehaviour<Controller> {
    public float MiniumDragDistance = 20f; // the bigger the shorter
    Vector3 touchStart;
    bool allowUp = false;

    private Vector3 StartPoint;
    public GameObject[] canvas;

    public bool allowClick = true;

    void Update() {
        if (allowClick) {
            if (Input.GetMouseButtonDown(0)) {
                bool noUIcontrolsInUse = EventSystem.current.currentSelectedGameObject == null;
                if (!noUIcontrolsInUse) {
                    return;
                } else {
                    bool flagCanvasOn = true;
                    for (int i = 0; i < canvas.Length; i++) {
                        if (canvas[i].activeSelf == true) {
                            flagCanvasOn = false;
                            break;
                        }
                    }
                    if (flagCanvasOn) {
                        touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        allowUp = false;
                        StartPoint = Input.mousePosition;
                    }
                }
            } else if (Input.GetMouseButton(0)) {
                bool noUIcontrolsInUse = EventSystem.current.currentSelectedGameObject == null;
                if (!noUIcontrolsInUse) {
                    return;
                } else {
                    bool flagCanvasOn = true;
                    for (int i = 0; i < canvas.Length; i++) {
                        if (canvas[i].activeSelf == true) {
                            flagCanvasOn = false;
                            break;
                        }
                    }
                    if (flagCanvasOn) {
                        Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Camera.main.transform.position += direction;
                    }
                }
            } else if (Input.GetMouseButtonUp(0)) {
                if (GameManager.Instance().selection == GameManager.SelectionState.Idle || GameManager.Instance().selection == GameManager.SelectionState.BuildingDetail) {
                    bool noUIcontrolsInUse = EventSystem.current.currentSelectedGameObject == null;
                    if (!noUIcontrolsInUse) {
                        return;
                    }
                    if (Vector3.Distance(Input.mousePosition, StartPoint) < Screen.width / MiniumDragDistance) {
                        allowUp = GameManager.Instance().SelectBuilding();
                        Camera.main.GetComponent<CameraUpdate>().isLerping = allowUp;
                        if (!allowUp) {
                            GameManager.Instance().ForceIdle();
                        }
                    } else {
                        GameManager.Instance().ForceIdle();
                    }
                }
            }

            if (allowUp == false) {
                Camera.main.GetComponent<CameraUpdate>().isLerping = false;
            }
        }
    }

}
