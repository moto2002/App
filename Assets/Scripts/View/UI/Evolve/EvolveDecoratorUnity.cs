﻿using UnityEngine;
using System.Collections.Generic;

public class EvolveDecoratorUnity : UIComponentUnity {
	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.PickFriendUnitInfo, PickFriendUnitInfo);
	}
	
	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.PickFriendUnitInfo, PickFriendUnitInfo);
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void Callback (object data) {
		Dictionary<string, object> dataDic = data as Dictionary<string, object>;
		List<KeyValuePair<string,object>> datalist = new List<KeyValuePair<string, object>> ();

		foreach (var item in dataDic) {
			datalist.Add(new KeyValuePair< string, object >( item.Key, item.Value));
		}
		for (int i = datalist.Count - 1; i > -1; i--) {
			DisposeCallback(datalist[i]);
		}
	}

	//==========================================interface end ==========================

	public const string BaseData = "SelectData";
	public const string MaterialData = "MaterialData";
	private const string preAtkLabel = "PrevAtkLabel";
	private const string preHPLabel = "PrevHPLabel";
	private const string evolveAtkLabel = "NextAtkLabel";
	private const string evolveHPLabel = "NextHPLabel";
	private const string needLabel = "NeedLabel";
	private string rootPath = "Window";
	private Dictionary<string,UILabel> showInfoLabel = new Dictionary<string, UILabel>();
	/// <summary>
	/// 1: base. 2, 3, 4: material. 5: friend
	/// </summary>
	private Dictionary<GameObject,EvolveItem> evolveItem = new Dictionary<GameObject, EvolveItem> ();
	private Dictionary<int,EvolveItem> materialItem = new Dictionary<int, EvolveItem> ();
	private UIImageButton evolveButton ;
	private EvolveItem baseItem;
	private EvolveItem friendItem;
	private TFriendInfo friendInfo;
	private EvolveItem prevItem = null;
//	private EvolveState clickState = EvolveState.BaseState;
	private List<TUserUnit> materialUnit = new List<TUserUnit>();
	private int ClickIndex = 0;

	void PickFriendUnitInfo(object data) {
		TFriendInfo tuu = data as TFriendInfo;
		friendInfo = tuu;
		friendItem.Refresh (tuu.UserUnit);
		CheckCanEvolve ();
	}

	void CheckCanEvolve () {
		bool haveBase = baseItem.userUnit != null; 
		bool haveFriend = friendItem.userUnit != null;
		bool haveMaterial = false;
		foreach (var item in materialItem) {
			if(item.Value.userUnit != null) {
				haveMaterial = true;
				break;
			}
		}
		if (haveBase && haveFriend && haveMaterial) {
			evolveButton.isEnabled = true;
		} else {
			evolveButton.isEnabled = false;
		}
	}

	void DisposeCallback (KeyValuePair<string, object> keyValue) {
		switch (keyValue.Key) {
		case BaseData:
			TUserUnit tuu = keyValue.Value as TUserUnit;
			DisposeSelectData(tuu);
			break;
		case MaterialData:
			List<TUserUnit> itemInfo = keyValue.Value as List<TUserUnit>;
			if(itemInfo != null) {
				DisposeMaterial(itemInfo);
			}
			else{
				TUserUnit userUnit = keyValue.Value as TUserUnit;
				materialItem[state].Refresh(userUnit);
				List<TUserUnit> temp = new List<TUserUnit>();
				for (int i = 2; i < 5; i++) {
					temp.Add(materialItem[i].userUnit);
				}
				MsgCenter.Instance.Invoke(CommandEnum.UnitMaterialList, temp);
			}
			break;
		default:
				break;
		}
	}

	void DisposeMaterial (List<TUserUnit> itemInfo) {
		if (itemInfo == null || baseItem == null) {
			return;	
		}

		for (int i = 0; i < itemInfo.Count; i++) {
			materialItem[i + 2].Refresh(itemInfo[i]);
		}
	}

	void DisposeSelectData (TUserUnit tuu) {
		if(tuu == null ) {
			return;
		}

		if (baseItem.userUnit != null && baseItem.userUnit.ID == tuu.ID) {
			return;	
		}
	
		if (state == 1 && tuu.UnitInfo.evolveInfo != null) {
			baseItem.Refresh(tuu);
			showInfoLabel[preAtkLabel].text = tuu.Attack.ToString();
			showInfoLabel[preHPLabel].text = tuu.Hp.ToString();
			MsgCenter.Instance.Invoke(CommandEnum.UnitDisplayBaseData, tuu);
		}
	}

	void ClearMaterial () {
		foreach (var item in evolveItem.Values) {
			item.Refresh(null);
		}
		materialUnit.Clear();
	}
 
	void LongPress (GameObject go) {
//		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
//		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);
	}

	int state = 0;
	void ClickItem (GameObject go) {
		if (baseItem.userUnit == null) {
			return;	
		}

		ClickIndex = System.Int32.Parse (go.name);
//		Debug.LogError ("state :" + state + " go.name : " + go.name);
		switch (go.name) {
		case "1":
			if(state == 1) {
				return;
			}
//			if(evolveButton.gameObject.activeSelf) {
//				evolveButton.gameObject.SetActive(false);
//			}
			ShieldEvolveButton(false);
			state = 1;
			break;
		case "2":
//			if(evolveButton.gameObject.activeSelf) {
//				evolveButton.gameObject.SetActive(false);
//			}
			ShieldEvolveButton(false);
			if(baseItem == null) {
				return;
			}
			state =2;
			break;
		case "3":
//			if(evolveButton.gameObject.activeSelf) {
//				evolveButton.gameObject.SetActive(false);
//			}
			ShieldEvolveButton(false);
			if(baseItem == null) {
				return;
			}
			state =3;
			break;
		case "4":
//			if(evolveButton.gameObject.activeSelf) {
//				evolveButton.gameObject.SetActive(false);
//			}
			ShieldEvolveButton(false);
			if(baseItem == null) {
				return;
			}
			state =4;
			break;
		case "5":
			if(state == 5) {
				return;
			}
			CheckCanEvolve();
			TUserUnit tuu = null;
			if(baseItem != null) {
				tuu = baseItem.userUnit;
			}
			ShieldEvolveButton(true);

//			if(!evolveButton.gameObject.activeSelf) {
//				evolveButton.gameObject.SetActive(true);
//			}
			state =5;
			MsgCenter.Instance.Invoke(CommandEnum.EvolveFriend, tuu);
			break;
		}
		if (prevItem != null) {
			prevItem.highLight.enabled = false;	
		}
		EvolveItem ei = evolveItem [go];
		ei.highLight.enabled = true;
		prevItem = ei;
		MsgCenter.Instance.Invoke (CommandEnum.UnitDisplayState, state);
	}


	void InitUI () {
		InitItem ();
		InitLabel ();
	}
	
	void InitItem () {
		string path = rootPath + "/title/";
		for (int i = 1; i < 6; i++) {
			GameObject go = FindChild(path + i);
			UIEventListenerCustom ui = UIEventListenerCustom.Get(go);
			ui.LongPress = LongPress;
			ui.onClick = ClickItem;
			EvolveItem ei = new EvolveItem();
			ei.index = i;
			ei.itemObject = go;
			ei.showTexture = go.transform.Find("Texture").GetComponent<UITexture>();
			ei.highLight = go.transform.Find("Light").GetComponent<UISprite>();
			ei.highLight.enabled = false;
			evolveItem.Add(ei.itemObject, ei);
			if(i == 1 ) {
				baseItem = ei;
				ei.highLight.enabled = true;
//				clickState = EvolveState.BaseState;
				state = 1;
				prevItem = ei;
				continue;
			}
			else if (i == 5) {
				friendItem = ei;
				continue;
			}
			else{
				materialItem.Add(i,ei);
			}
			
			ei.haveLabel = go.transform.Find("HaveLabel").GetComponent<UILabel>();
		}
	}

	void InitLabel () {
		string path = rootPath + "/info_panel/";

		UILabel temp = transform.Find(path + preAtkLabel).GetComponent<UILabel>();
		showInfoLabel.Add (preAtkLabel, temp);

		temp = transform.Find(path + preHPLabel).GetComponent<UILabel>();
		showInfoLabel.Add (preHPLabel, temp);

		temp = transform.Find(path + evolveAtkLabel).GetComponent<UILabel>();
		showInfoLabel.Add (evolveAtkLabel, temp);

		temp = transform.Find(path + evolveHPLabel).GetComponent<UILabel>();
		showInfoLabel.Add (evolveHPLabel, temp);

		temp = transform.Find(path + needLabel).GetComponent<UILabel>();
		showInfoLabel.Add (needLabel, temp);

		evolveButton = FindChild<UIImageButton> ("Window/Evolve");
		ShieldEvolveButton (false);
		UIEventListener.Get (evolveButton.gameObject).onClick = Evolve;
	}

	void Evolve(GameObject go) {
		List<ProtobufDataBase> evolveInfoList = new List<ProtobufDataBase> ();
		evolveInfoList.Add (baseItem.userUnit);
		evolveInfoList.Add (friendInfo);
		foreach (var item in materialItem.Values) {
			TUserUnit tuu = item.userUnit;
			if(tuu != null) {
				evolveInfoList.Add(tuu);
			}
		}

		ExcuteCallback (evolveInfoList);
	}

	void ShieldEvolveButton (bool b) {
		if (evolveButton.gameObject.activeSelf == !b) {
			evolveButton.gameObject.SetActive(b);
		}
	}
}

//public enum EvolveState {
//	BaseState = 0,
//	MaterialState = 1,
//	FriendState = 2,
//}

public class EvolveItem {
	public GameObject itemObject;
	public TUserUnit userUnit;
	public UITexture showTexture;
	public UILabel haveLabel;
	public UISprite highLight;
	public int index;

	public void Refresh (TUserUnit tuu) {
		userUnit = tuu;
		if (tuu == null) {
			showTexture.mainTexture = null;
		} else {
			Texture2D tex = userUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
			showTexture.mainTexture = tex;
		}
	}
}