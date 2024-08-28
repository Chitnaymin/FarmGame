using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansionManager : SingletonBehaviour<ExpansionManager> {

    public GameObject[] Maps;

    int ExpensionLevel;
    PlayerStorageManager pdbsm;
    public override void Awake() {
        base.Awake();
    }

    void Start() {
        pdbsm = new PlayerStorageManager();
        Expend();
    }

    public void BuyExpension() {
        pdbsm.BuyExpansion(ExpensionLevel + 1);
        Expend();
    }

    // on start and on buy 
    void Expend() {
        ExpensionLevel = pdbsm.Expansion();
        for (int i = 0; i <= ExpensionLevel; i++) {
            Maps[i].SetActive(true);
        }
		Camera.main.GetComponent<Panning>().ExpanstionDataSet();
	}

    public int GetExpensionLevel() {
        return ExpensionLevel;
    }
}
