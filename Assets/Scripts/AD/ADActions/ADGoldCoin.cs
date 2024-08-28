using UnityEngine;

[CreateAssetMenu(fileName = "New ADGoldCoin", menuName = "AD Data/ADGoldCoin", order = 58)]
public class ADGoldCoin : ADType {
    public override void Reward() {
        base.Reward();
        int i = Random.Range(400, 800);
        PlayerData.Instance().SetCoin(PlayerData.Instance().GetCoin() + i);
        Debug.Log("Gold Gained : " + i);
        ShopManager.Instance().ShowCoin();
    }
}