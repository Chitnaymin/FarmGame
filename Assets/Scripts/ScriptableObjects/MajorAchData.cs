using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MajorAchievement", menuName = "MajorAchievement", order = 53)]
public class MajorAchData : ScriptableObject
{
	public E_AchType AchType;
	public List<MajorAchStruct> AchDetails;
}
[System.Serializable]
public class MajorAchStruct 
{
	public int AchNameTextID;
	public int MaxProgress;
	public E_RewardType RewardType;
	public int CoinReward;
	public string SerialKeyCode;
	public int TitleTxtID;
}
public enum E_AchType 
{
	Level=0,
	Building=1,
	CropHarvesting=2,
	Money=3,
	Sell=4,
	Task=5,
	FactoryHarvest=6,
	LabUpgrade=7,
	Ads=8,
	Login=9,
	Share=10
}
public enum E_RewardType 
{
	Money=0,
	SerialCode=1,
	SerialTitle=2
}

