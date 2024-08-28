using UnityEngine;
// DB ID 3
[CreateAssetMenu(fileName = "New ADDietaryFiber", menuName = "AD Data/ADDietaryFiber", order = 53)]
public class ADDietaryFiber : ADType {
    public override void Reward() {
        base.Reward();
        Debug.Log("Here is reward " + base.id);
        InventoryManager.Instance().AddingItems(23, 1); // DietaryFiber id = 23
    }
}
