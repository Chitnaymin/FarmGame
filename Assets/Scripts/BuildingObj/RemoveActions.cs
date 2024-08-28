using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveActions : Actions {
    // remove actions
    public GameObject removeConfirm;
    public GameObject removeConfirmBlank;

    public void Remove() {
        removeConfirm.SetActive(true);
        removeConfirmBlank.SetActive(true);
    }

    public override void BtnRemoveConfirm() {
        base.BtnRemoveConfirm();
        BtnRemoveCancel();
    }
    public override void BtnRemoveCancel() {
        removeConfirm.SetActive(false);
        removeConfirmBlank.SetActive(false);
        base.BtnRemoveCancel();
    }
    private void Update() {
        if (removeConfirm != null) {
            if (removeConfirm.activeSelf) {
                removeConfirm.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
            }
        }
    }

}
