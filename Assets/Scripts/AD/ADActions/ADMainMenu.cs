using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "New ADMainMenu", menuName = "AD Data/ADMainMenu", order = 50)]
public class ADMainMenu: ADType {
    
    public override void Reward() {
        base.Reward();
        PlayerData.Instance().SetCoin(PlayerData.Instance().GetCoin() + 1000 );
        ShopManager.Instance().ShowCoin();
    }
}