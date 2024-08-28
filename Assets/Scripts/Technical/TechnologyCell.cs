using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechnologyCell : MonoBehaviour
{   
    public Technology Data;
    public int CurrentLevel;
   // public Text TechName;
    public Text txtLevel;

    public void RefreshData(Technology _data,int _currentLevel)
    {
        Data=_data;
        CurrentLevel=_currentLevel;
        //TechName.text=Data.TechName;
        txtLevel.text="Lv."+CurrentLevel;
        if(CurrentLevel == 5){
            txtLevel.text = "Lv.max";
        }
    }
}