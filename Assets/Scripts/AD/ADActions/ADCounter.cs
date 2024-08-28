using UnityEngine;

[CreateAssetMenu(fileName = "New ADCounter", menuName = "AD Data/ADCounter", order = 65)]
public class ADCounter : ADType {
    public override void Reward() {
        base.Reward();
        ADManager.Instance().SaveAddCounterInDB(base.id);
    }
}