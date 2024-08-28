using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : SingletonBehaviour<PlayerData> {
    //public Image ProfileImg;
    private string playerName;
    private int coin;
    private int lvl;
    private int exp;
    private int rank;
    private int title;
    private int expansion;
    private int improvement;
    private int quality;
    private int evolution;
    private int product;
    private int currentUpgrade;
    private string upgradeStartTime;
    PlayerStorageManager pdb;

    public PlayerData(string playerName, int coin, int lvl, int exp, int rank, int title, int expansion, int improvement, int quality, int evolution,
    int product, int currentUpgrade, string upgradeStartTime) {
        this.playerName = playerName;
        this.coin = coin;
        this.lvl = lvl;
        this.exp = exp;
        this.rank = rank;
        this.title = title;
        this.expansion = expansion;
        this.improvement = improvement;
        this.quality = quality;
        this.evolution = evolution;
        this.product = product;
        this.currentUpgrade = currentUpgrade;
        this.upgradeStartTime = upgradeStartTime;
    }

    private void Start() {
        pdb = new PlayerStorageManager();
        PlayerData pl = pdb.GetPlayerData();
        playerName = pl.playerName;
        coin = pl.coin;
        lvl = pl.lvl;
        exp = pl.exp;
        rank = pl.rank;
        title = pl.title;
        expansion = pl.expansion;
        improvement = pl.improvement;
        quality = pl.quality;
        evolution = pl.evolution;
        product = pl.product;
        currentUpgrade = pl.currentUpgrade;
        upgradeStartTime = pl.upgradeStartTime;
    }

    public string GetPlayerName() {
        return playerName;
    }

    public void SetPlayerName(string value) {
        pdb.UpdatePlayerData("PlayerName", value);
        playerName = value;
    }

    public int GetCoin() {
        return coin;
    }

    public void SetCoin(int value, bool NotAllowRecursive = false) {
        pdb.UpdatePlayerData("Coin", value);
        coin = value;
        AudioManager.Instance().PlaySound(6);
        if (!NotAllowRecursive) {
            AchievementManager.Instance().MajorAchievementUpdate(E_AchType.Money, coin);
        }
    }

    public int GetLvl() {
        return lvl;
    }

    public void SetLvl(int value) {
        pdb.UpdatePlayerData("Level", value);
        lvl = value;

        AchievementManager.Instance().MajorAchievementUpdate(E_AchType.Level, lvl);
    }

    public int GetExp() {
        return exp;
    }

    public void SetExp(int value) {

        if (ADManager.Instance().ActionWorking(14)) {
            value = value * 2;
        }

        pdb.UpdatePlayerData("Experience", value);
        exp = value;
    }

    public int GetRank() {
        return rank;
    }

    public void SetRank(int value) {
        pdb.UpdatePlayerData("Rank", value);
        rank = value;
    }

    public int GetTitle() {
        return title;
    }

    public void SetTitle(int value) {
        pdb.UpdatePlayerData("Title", value);
        title = value;
    }

    public int GetExpansion() {
        return expansion;
    }

    public void SetExpansion(int value) {
        pdb.UpdatePlayerData("Expansion", value);
        expansion = value;
    }
    public int GetImprovement() {
        return improvement;
    }
    public void SetImprovement(int value) {
        pdb.UpdatePlayerData("Improvement", value);
        improvement = value;
    }
    public int GetQuality() {
        return quality;
    }
    public void SetQuality(int value) {
        pdb.UpdatePlayerData("Quality", value);
        quality = value;
    }
    public int GetEvolution() {
        return evolution;
    }

    public void SetEvolution(int value) {
        pdb.UpdatePlayerData("Evolution", value);
        evolution = value;
    }
    public int GetProduct() {
        return product;
    }

    public void SetProduct(int value) {
        pdb.UpdatePlayerData("Product", value);
        product = value;
    }
    public int GetCurrentUpgrade() {
        return currentUpgrade;
    }

    public void SetCurrentUpgrade(int value) {
        pdb.UpdatePlayerData("CurrentUpgrade", value);
        currentUpgrade = value;
    }
    public string GetUpgradeStartTime() {
        return upgradeStartTime;
    }

    public void SetUpgradeStartTime(string value) {
        pdb.UpdatePlayerData("UpgradeStartTime", value);
        upgradeStartTime = value;
    }
}


