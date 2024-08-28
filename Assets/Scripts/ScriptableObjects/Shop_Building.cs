using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building Data", order = 52)]
public class Shop_Building : ScriptableObject
{
	public enum Category
	{
		Build,
		Field,
		Tree,
		Expansion,
		Pergola
	}

	public Category category_tag;

	public int id;

	public int nameTxtID;

	public int price;

	public int time;

	public int level;

	public int buildingLvl;

	public int exp;

	public int material;

    public int cropableItem;

	public int ProductionCap;

	public int ReduceTheTime;

	public Sprite sprite;

    public Sprite builtSpriteState0;
    public Sprite builtSpriteState1;
    public Sprite builtSpriteState2;
    public Sprite builtSpriteComplete;

    public int[] BonusItemID;

}
