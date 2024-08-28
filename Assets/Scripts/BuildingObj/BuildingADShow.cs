using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingADShow : MonoBehaviour {

    public void BuildingADShowBtn(){
        GameObject BuildingGameObject = transform.parent.gameObject.transform.parent.gameObject;
        ADManager.Instance().ADShowBtn(3);
        ADManager.Instance().SettingGameObject(BuildingGameObject);
    }
}
