using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADCanvases : MonoBehaviour {
    public void BtnVideoStart() { // call by btn
        ADManager.Instance().MenuAD();
        Destroy(gameObject);
    }

    public void BtnCancel() {
        Destroy(gameObject);
    }
}
