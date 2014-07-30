﻿using UnityEngine;
using System.Collections.Generic;

public class MapConfig : IOriginModel {
//	public int mapXLength;
//	public int mapYLength;
	
	public SingleMapData[,] mapData;

	//=========== useful start===================

	public const int characterInitCoorX = 2;
	public const int characterInitCoorY = 0;
	public static Coordinate endCoor = new Coordinate (2, 4);
	public const int MapWidth = 5;
	public const int MapHeight = 5;

	//=========== useful end===================

	public const int endPointX = 2;
	public const int endPointY = 4;
	public List<uint> BossID = new List<uint> () {3};
	private List<string> mapItemPath = new List<string>();
	public int floor = 2;

	private int mapID;

	public string GetMapPath() {
		int index = Random.Range (1, mapItemPath.Count);
		return mapItemPath [index];
	}

	public ErrorMsg SerializeData (object instance) {
		throw new System.NotImplementedException ();
	}

	public object DeserializeData () {
		throw new System.NotImplementedException ();

	}

	public MapConfig () {
//		ConfigTrap ct = new ConfigTrap ();
//		floor = 2;
////		mapXLength = 5;
////		mapYLength = 5;
//		mapID = 1;
////		mapData = new SingleMapData[mapXLength,mapYLength];
////		for (int i = 0; i < mapXLength; i++) {
////			for (int j = 0; j < mapYLength; j++) {
////				SingleMapData smd = new SingleMapData();
////				smd.StarLevel = Random.Range(0,5);
////				smd.CoordinateX = i;
////				smd.CoordinateY = j;
//				smd.ContentType = MapItemEnum.Enemy;
////				for (int k = 0; k < smd.StarLevel; k++) 
////				{
//					smd.MonsterID.Add(1);
//					smd.MonsterID.Add(2);
//					smd.MonsterID.Add(3);
//					smd.MonsterID.Add(4);
//					smd.MonsterID.Add(5);
//				//				}
//				
//				mapData[i,j] = smd;
//			}	
//		}
//
//		SingleMapData singleMapItem = mapData [1, 0];
//		singleMapItem.ContentType = MapItemEnum.Trap;
//		singleMapItem.TypeValue = 4;
//		singleMapItem = mapData [1, 1];
//		singleMapItem.ContentType = MapItemEnum.Trap;
//		singleMapItem.TypeValue = 3;
//		singleMapItem = mapData [2, 1];
//		singleMapItem.ContentType = MapItemEnum.Coin;
//		singleMapItem.TypeValue = 0;
//		singleMapItem = mapData [2, 0];
//		singleMapItem.ContentType = MapItemEnum.Start;
//		singleMapItem = mapData [2, 2];
//		singleMapItem.ContentType = MapItemEnum.Exclamation;
//		singleMapItem = mapData [2, 3];
//		singleMapItem.ContentType = MapItemEnum.key;
//		mapData[characterInitCoorX,characterInitCoorY].MonsterID.Clear();   
////		mapData [2, 4].MonsterID.Clear ();
////		mapData [2, 4].MonsterID.Add (100);
//		for (int i = 1; i < 4; i++) {
//			mapItemPath.Add("Texture/fight_sprites/map_"+mapID+"_"+i);
//		}

	}
}
