﻿using UnityEngine;
using System.Collections;

public class MapItem : UIBaseUnity
{
	private Coordinate coor;

	public Coordinate Coor
	{
		get{ return coor; }
		set{ coor = value; }
	}

	private UITexture mapItemTexture;

	public int  Width
	{
		get{return mapItemTexture.width;}
	}
		
	public int Height
	{
		get{return mapItemTexture.height;}
	}

	public Vector3 InitPosition
	{
		get{return transform.localPosition;}
	}

	private bool isOld = false;

	public bool IsOld
	{
		set
		{ 
			if(!isOld)
			{
				isOld = value; 
				mapItemTexture.color = Color.red;
			}
		}

		get{return isOld;}
	}

	private UITexture alreayQuestTexture;
	
	public override void Init (string name)
	{
		base.Init (name);

		mapItemTexture = FindChild<UITexture>("MapItem");

		mapItemTexture.mainTexture = LoadAsset.Instance.LoadAssetFromResources("CardCreaterBg",ResourceEuum.Image) as Texture2D;
	}

	public void Around(bool isAround)
	{
		if(isOld)
			return;

		if(isAround)
			mapItemTexture.color = Color.yellow;
		else
			mapItemTexture.color = Color.white;
	}
}