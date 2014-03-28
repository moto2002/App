﻿using UnityEngine;
using System.Collections;

public class MapItem : UIBaseUnity {
	private Coordinate coor; 
	public Coordinate Coor {
		get{ return coor; }
		set{ coor = value; }
	}

	private GameObject mapBack;
	private UISprite mapBackSprite;
	private FloorRotate floorRotate;
	private UISprite mapItemSprite;
	string spriteName = "";
	string backSpriteName = "";

	private Vector3 initPosition = Vector3.zero;
	private Vector3 initRotation = Vector3.zero;

	private TQuestGrid gridItem ;

	public int  Width {
		get{ return mapItemSprite.width; }
	}
		
	public int Height {
		get{return mapItemSprite.height;}
	}

	public Vector3 InitPosition {
		get { return transform.localPosition; }
	}

	private bool isOld = false;
	public bool IsOld {
		set {
			isOld = value; 
		}
		get{return isOld;}
	}

	private bool isRotate = false;

	public Vector3 GetBoxPosition () {
		return floorRotate.currentPoint;
	}

	private UITexture alreayQuestTexture;
	public override void Init (string name) {
		base.Init (name);
		initPosition = transform.localPosition;
		initRotation = transform.rotation.eulerAngles;
		mapBackSprite = FindChild<UISprite>("Floor/MapItem/Texture");
		mapBack = mapBackSprite.gameObject;
		mapItemSprite = FindChild<UISprite>("Sprite");
		floorRotate = GetComponent<FloorRotate> ();
		floorRotate.Init ();
		if (name == "SingleMap") {
			return;
		}
		string[] info = name.Split('|');
		int x = System.Int32.Parse (info[0]);
		int y = System.Int32.Parse (info [1]);
		gridItem = BattleQuest.questDungeonData.GetSingleFloor (new Coordinate (x, y));
		if (gridItem != null) {
			switch (gridItem.Star) {
			case bbproto.EGridStar.GS_KEY:
//				mapBackSprite.enabled = true;

				spriteName = "key";
//				Destroy(mapItemTexture);
				break;
			case bbproto.EGridStar.GS_QUESTION:
				break;
			case bbproto.EGridStar.GS_EXCLAMATION:
//				mapBackSprite.enabled = true;

				spriteName = "d";
//				Destroy(mapItemTexture);
				break;
			default:
				break;
			}
			mapItemSprite.spriteName = spriteName;
//			spriteName = "";
			backSpriteName = "";
			switch (gridItem.Type) {
			case bbproto.EQuestGridType.Q_NONE:
//				Destroy(mapItemTexture);
				break;
			case bbproto.EQuestGridType.Q_KEY:
//				Destroy(mapItemTexture);
				break;
			case bbproto.EQuestGridType.Q_EXCLAMATION:
//				Destroy(mapItemTexture);
				break;
			case bbproto.EQuestGridType.Q_ENEMY:

				uint unitID = gridItem.Enemy [0].UnitID;
				TUnitInfo tui = DataCenter.Instance.GetUnitInfo (unitID);
				if (tui != null) {
					UITexture tex = mapBack.AddComponent<UITexture>();
					tex.depth = -1;
//					mapItemTexture.enabled = true;
					Destroy(mapBackSprite);
					tex.mainTexture = tui.GetAsset (UnitAssetType.Avatar);
					tex.width = 110;
					tex.height = 110;
				}
				break;
			case bbproto.EQuestGridType.Q_TRAP:
//				Destroy(mapItemTexture);
//				mapBackSprite.enabled = true;
				backSpriteName = TrapBase.GetTrapSpriteName(gridItem.TrapInfo);
				break;
			case bbproto.EQuestGridType.Q_TREATURE:
//				Destroy(mapItemTexture);
//				mapBackSprite.enabled = true;
				backSpriteName = "s";
				break;
			default:
				break;
			}
			if(mapBackSprite != null) {
				mapBackSprite.spriteName = backSpriteName;
			}

			mapBack.SetActive(false);
		}
	}

	public override void ShowUI() {
		isOld = false;
	}

	public void HideEnvirment(bool b) {
		if (!isOld) {
			if(b) {
				mapItemSprite.spriteName = "6";
			}else{
				mapItemSprite.spriteName = spriteName;
			}
		}
	}

	public void RotateOneCircle() {
		if (!isRotate) {
			isRotate = true;
			floorRotate.RotateOne ();
		}
	}

	public void RotateAnim() {
		if (!isRotate) {
			isRotate = true;
			floorRotate.RotateFloor (RotateEnd);	
			if(!mapBack.activeSelf) {
				mapBack.SetActive(true);
			}
		}
	}     

	void RotateEnd () {
		mapBack.SetActive(false);
	}

	public void ShowBox() {
		floorRotate.isShowBox = true;
	}

	public void Reset () {
		gameObject.transform.localPosition = initPosition;
		gameObject.transform.rotation = Quaternion.Euler (initRotation);
	}

	public void Around(bool isAround)
	{
		if(isOld)
			return;

//		if(isAround)
//			mapItemTexture.color = Color.yellow;
//		else
//			mapItemTexture.color = Color.white;
	}
}
