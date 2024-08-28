using UnityEngine;

[CreateAssetMenu(fileName = "New ADIMO", menuName = "AD Data/ADIMO", order = 54)]
public class ADIMO : ADType {
    public override void Reward() {
        base.Reward();
        Debug.Log("you get IMO");
        InventoryManager.Instance().AddingItems(23, 3); // IMO id = 23
    }
}