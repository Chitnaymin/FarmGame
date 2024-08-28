using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelXPGraph
{

	private int lvlDB;
	private int xpDB;

	public LevelXPGraph(int lvlDB, int xpDB)
	{
		this.SetLvlDB(lvlDB);
		this.SetXpDB(xpDB);
	}

	public int GetLvlDB()
	{
		return lvlDB;
	}

	public void SetLvlDB(int value)
	{
		lvlDB = value;
	}

	public int GetXpDB()
	{
		return xpDB;
	}

	public void SetXpDB(int value)
	{
		xpDB = value;
	}
}
