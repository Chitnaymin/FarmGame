using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADActionManager : SingletonBehaviour<ADActionManager> {
    PlayerStorageManager pdbsm;

    List<ADObtain> ADObtains;
    private void Start() {
        pdbsm = new PlayerStorageManager();

        List<ADObtain> ADObtains;
        ADObtains = pdbsm.GetADObtainTime();
    }

    
}
