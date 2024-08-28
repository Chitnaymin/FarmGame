using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "New Technical", menuName = "Technical Data", order = 53)]
public class Technology : ScriptableObject {
    public E_TechnologyType Type;
    public string TechName;
	public int DescriptionID;
	public int TitleTxtID;
	public int DescriptionTxtID;
	public int Percentage;
    public List<LevelDetails> levelDetails = new List<LevelDetails>();
}
public enum E_TechnologyType : int {
    FertilizerImprovement = 1,
    QualityUpgrade = 2,
    TechnologyDevelopment = 3,
    ProductUpgrade = 4,
}
[System.Serializable]
public class LevelDetails {
    public int Price;
    public float PercentValue;
    public int ImprovingTime; // sec
}