using UnityEngine;

[CreateAssetMenu(fileName = "New ADCrops", menuName = "AD Data/ADCrops", order = 57)]
public class ADCrops : ADType {
    public override void Reward() {
        base.Reward();
        Debug.Log("Here is reward");
        for (int i = 0; i < 10; i++) {
            InventoryManager.Instance().AddingItems(Random.Range(1, 10), 1);
        }
    }
}