﻿using UnityEngine;
using System.Collections.Generic;



public class SingleMapData {
	private int coordinateX = 0;
	/// <summary>
	/// Gets or sets the coordinate x.
	/// </summary>
	public int CoordinateX 
	{
		get{return coordinateX;}
		set{coordinateX = value;}
	}
	
	private int coordinateY = 0;
	/// <summary>
	/// Gets or sets the coordinate y.
	/// </summary>
	public int CoordinateY {
		get {return this.coordinateY;}
		set {coordinateY = value;}
	}	
	
	private int starLevel;
	/// <summary>
	/// Gets or sets the star level.
	/// </summary>
	public int StarLevel
	{
		get { return starLevel; }
		set { starLevel = value; }
	}

	private MapItemEnum contentType = MapItemEnum.None;
	public MapItemEnum ContentType {
		get { return contentType; }
		set { contentType = value; }
	}

	private uint typeValue = 0;
	public uint TypeValue {
		get { return typeValue; }
		set {typeValue = value; }
	}
	
	private List<uint> monsterID = new List<uint>();
	/// <summary>
	/// Gets or sets the monster ID.
	/// </summary>
	public List<uint> MonsterID {
		get{return monsterID;}
		set{monsterID = value;}
	}
}

public struct Coordinate
{
	public int x ;

	public int y ;

	public Coordinate(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
}

public class TClass<T1,T2>
{
	
	public T1 arg1;
	
	public T2 arg2;
	
	public TClass()
	{
		
	}
	
	public TClass(T1 a1,T2 a2)
	{
		arg1 = a1;
		arg2 = a2;
	}
}

public class TClass<T1,T2,T3>
{
	public T1 arg1;
	
	public T2 arg2;
	
	public T3 arg3;
	
	public TClass()
	{
		
	}

	
	public TClass(T1 a1,T2 a2,T3 a3)
	{
		arg1 = a1;
		arg2 = a2;
		arg3 = a3;
	}
}

