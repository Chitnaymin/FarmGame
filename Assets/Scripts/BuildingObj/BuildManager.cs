using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : SingletonBehaviour<BuildManager> {
    public GameObject[] Block;
    public GameObject BuildingPreFabs;
    public bool LabisBuilding = false;

    List<Building> buildingList = new List<Building>();

    PlayerStorageManager pdbsm; //database storage manager

    public override void Awake() {
        base.Awake();
    }

    void Start() {
        pdbsm = new PlayerStorageManager();
        StartCoroutine(LoadFormation());
    }

    // some level update here
    public void UpdateBuildingLevel(Building buildingInfo) {
        pdbsm.UpdateBuildingUpgradeData(buildingInfo.GetLevel(), buildingInfo.GetProcess(), buildingInfo.GetStartingTime(), buildingInfo.GetBlockID());
    }
    public void DeleteBuilding(GameObject buildingGO) { // block obj
        Destroy(buildingGO.transform.GetChild(0).gameObject);
        pdbsm.DeleteBuildingFromDatabase(FindBlock(buildingGO));
    }
    public void ItemPickUpBuilding(int blockID, int newBuildingID) {
        pdbsm.UpdateBuildingBuilding(blockID, newBuildingID);
    }
    public void SaveFormation(Building buildingInfo) {
        buildingInfo.SetBlockID(FindBlock(buildingInfo.GetBlockRef()));
        pdbsm.InsertBuildingFormation(buildingInfo);
    }
    public void UpdateStartingTime(Building buildingInfo) {
        pdbsm.UpdateBuildingStartingTime(buildingInfo.GetStartingTime(), buildingInfo.GetBlockID());
    }
    public void UpdateProcess(Building buildingInfo) {
        pdbsm.UpdateBuildingProgress(buildingInfo.GetProcess(), buildingInfo.GetBlockID());
    }
    public void UpdateFormation(Building buildingInfo, int oldBlockID) {
        buildingInfo.SetBlockID(FindBlock(buildingInfo.GetBlockRef()));
        pdbsm.UpdateBuildingFormation(buildingInfo.GetBlockID(), oldBlockID);
    }
    
    IEnumerator LoadFormation() { // Load from database 
        yield return new WaitForSeconds(0.5f);
        buildingList = pdbsm.GetBuildingFormationFromDatabase();
        HideButton.Instance().hideLabButton();
        foreach (Building ele in buildingList) {
            BuildingFound(ele);
            CheckLabInfo(ele);
        }
    }
    public void CheckLabInfo(Building ele) {
        if (ele.GetBuildingID() == 7) {
            LabisBuilding = true;
            if (ele.GetProcess() == 1) {
                HideButton.Instance().showLabButton();
            }
        }

    }

    // database building // rebuild by crop
    public void BuildingFound(Building building) {
        GameObject tempBuilding = Instantiate(BuildingPreFabs, Block[building.GetBlockID()].transform.position, Quaternion.identity) as GameObject;

        tempBuilding.GetComponent<Building>().SettingAllData(building.GetBlockID(), building.GetBuildingID(), building.GetStartingTime(), building.GetLevel(), building.GetProcess());
        tempBuilding.GetComponent<SpriteRenderer>().sortingOrder = Block[building.GetBlockID()].GetComponent<SpriteRenderer>().sortingOrder;
        tempBuilding.GetComponent<MovingNewPosition>().SetBlock(Block[building.GetBlockID()]);
        tempBuilding.GetComponent<MovingNewPosition>().CompleteMove();
    }

    public int FindBlock(GameObject inputBlock) {
        int i;
        for (i = 0; i <= Block.Length; i++) {
            if (inputBlock.name == Block[i].name) {
                break;
            }
        }
        return i;
    }
    public GameObject GetBlock(int blockID) {
        return Block[blockID];
    }

    // some more action for building detail panel

    public GameObject ProduceAction;
    public GameObject Upgrade;
    public GameObject MoveAction;
    public GameObject Remove;
    public GameObject Shop;

    public List<GameObject> GetActionButtons(int buildingID, Shop_Building.Category category) {
        List<GameObject> actionBtns = new List<GameObject>();
        if (category == Shop_Building.Category.Build) {
            if (buildingID == 1 || buildingID == 2 || buildingID == 3) {
                actionBtns.Add(Shop);
            } else if (buildingID == 7) {
            } else {
                actionBtns.Add(ProduceAction);
                actionBtns.Add(Upgrade);
            }
            actionBtns.Add(MoveAction);
        } else if (category == Shop_Building.Category.Field) { // နောက်တော့ပြန်သန့်ပါ debughere
            actionBtns.Add(MoveAction);
        } else if (category == Shop_Building.Category.Pergola) {
            actionBtns.Add(MoveAction);
        } else if (category == Shop_Building.Category.Tree) {
            actionBtns.Add(MoveAction);
        }
        if (buildingID != 7) {
            actionBtns.Add(Remove);
        }
        return actionBtns;
    }

}