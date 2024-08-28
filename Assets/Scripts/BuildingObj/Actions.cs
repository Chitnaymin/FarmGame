using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour {

    public void Move() {
        GameManager.Instance().BtnMove();
    }

    public void Produce() {
        GameObject getSelectedBlock = transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject;
        ProductionManager.Instance().ShowProductionView(getSelectedBlock);
    }

    public void Upgrade() {
        Building getSelectedBuilding = transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Building>();
        UpgradeManager.Instance().ShowUpgradeView(getSelectedBuilding); // building data ရရုံနဲ့ လုံလောက်
    }

    public void Shop() {
		Building building = transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Building>();
		ShopManager.Instance().ShowShopCanvas();
        ShopManager.Instance().ChechFromManager();
		ShopManager.Instance().CustomizedTab(building);
        DirectComfirmation.SETDIRECTBUILD(true, building.GetBlockID());
        GameManager.Instance().BuildingDeselected();
    }

    public virtual void BtnRemoveConfirm() {
        GameObject getSelectedBlock = transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject;
        BuildManager.Instance().DeleteBuilding(getSelectedBlock);
    }
    public virtual void BtnRemoveCancel() {
        GameManager.Instance().ForceIdle();
    }
}
