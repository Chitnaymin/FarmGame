using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvItem : MonoBehaviour {
    [SerializeField]
    private Inventory inv;
    public GameObject selectedImg;
    bool isSelected = false;
    Image image;
    Text itemCount;
    private bool ByProduction = false;

    private void Awake() {
        image = transform.GetChild(0).GetComponent<Image>();
        itemCount = transform.GetChild(1).GetComponent<Text>();
        selectedImg.SetActive(false);
    }

    public void ViewInfo() {
        if (ByProduction) {
            ProductionManager.Instance().ShowItemInfo(GetInv());
        } else {
            InventoryManager.Instance().ShowItemInfo(GetInv());
            if (InventoryManager.Instance().SelectedItem != null) {
                InventoryManager.Instance().SelectedItem.setSelection(false);
                InventoryManager.Instance().SelectedItem = this.GetComponent<InvItem>();
            }
            setSelection(true);
        }
    }

    public void SelectionFun() {
        if (isSelected) {
            selectedImg.SetActive(true);
        } else {
            selectedImg.SetActive(false);
        }
    }

    public void setSelection(bool value) {

        //Debug.Log(value);
        isSelected = value;
        SelectionFun();
    }

    public void BindView(Inventory inv, bool BindByProduction = false) {
        this.SetInv(inv);
        image.sprite = inv.Image;

        ByProduction = BindByProduction;
        if (ByProduction) {

        }else{
            itemCount.text = "x" + inv.GetInventoryCount().ToString();
            //change image
        }
    }

    public void ExtraCountBindView(int count, int playerItemCount) {
        transform.GetChild(1).GetChild(0).GetComponent<Text>().text = count.ToString();
        Text currentItem = transform.GetChild(2).GetComponent<Text>();
        currentItem.text =  "x"+playerItemCount.ToString();
        if (count > playerItemCount) {
            currentItem.color = new Color(255,0,0);
        }
    }

    public Inventory GetInv() {
        return inv;
    }

    public void SetInv(Inventory value) {
        inv = value;
    }
}

