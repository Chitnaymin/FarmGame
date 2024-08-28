using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item Data", order = 51)]
public class Inventory : ScriptableObject {

    public int ID;
    public int ItemNameID;
    public int Rank;
    public int UnitPrice;
    public bool allowSell;
    public int DesceptionTextID;
    public string Category;
    public Sprite Image;

    public int ProductionTime;
    public int Experience;

    private int inventoryCount;
	public Inventory() { }
    public Inventory(int ID) {
        this.ID = ID;
    }
    public Inventory(int ID, int count) {
        this.ID = ID;
        this.inventoryCount= count;
    }

    public void SetInventoryItem(Inventory inv) { // to fill all the information to the item list from 
        this.ItemNameID = inv.ItemNameID;
        this.Rank = inv.Rank;
        this.UnitPrice = inv.UnitPrice;
        this.allowSell = inv.allowSell;
        this.DesceptionTextID = inv.DesceptionTextID;
        this.Category = inv.Category;
        this.Image = inv.Image;
        this.ProductionTime = inv.ProductionTime;
        this.Experience = inv.Experience;
    }

    public int GetInventoryCount() {
        return inventoryCount;
    }

    public void SetInventoryCount(int value) {
        inventoryCount = value;
    }

    public void IncreasingItem(int value) {
        inventoryCount += value;
    }
    public void DecreasingItem(int value) {
        inventoryCount -= value;
    }
}
