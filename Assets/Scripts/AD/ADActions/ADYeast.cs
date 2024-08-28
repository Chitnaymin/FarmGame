using UnityEngine;
// DB ID 2
[CreateAssetMenu(fileName = "New ADYeast", menuName = "AD Data/ADYeast", order = 52)]
public class ADYeast : ADType {
    public override void Reward() {
        base.Reward();
        InventoryManager.Instance().AddingItems(22,3); // yeast id = 22
    }
}