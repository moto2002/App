using UnityEngine;
using System.Collections.Generic;

public class OthersWindow : UIComponentUnity {

//	GameObject scrollerItem;
//	DragPanel othersScroller;
//	UILabel titleLabel;

//	GameObject nickNamePanel;
//	UIButton okButton;
//	UIInput nickNameInput;
	
//	GameObject musicPanel;
//	UIButton bgmOnBtn;
//	UIButton bgmOffBtn;
//	UISprite maskOn;
//	UISprite maskOff;
//
//    GameObject resetOption;

	Dictionary< GameObject, GameObject > options = new Dictionary< GameObject, GameObject>();

	public override void Init ( UIInsConfig config, IUICallback origin ){
		FindUIElement();
//		SetOption();
		base.Init (config, origin);
	}
	
	public override void ShowUI(){
		base.ShowUI ();
//		SetUIElement();

		ShowUIAnimation ();
	}
	
	public override void HideUI(){
		base.HideUI ();
//		ResetUIElement();
		iTween.Stop (gameObject);
	}
	
	public override void DestoryUI(){
		UIEventListenerCustom.Get (FindChild ("OptionItems/Music")).onClick = null;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Nickname")).onClick = null;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Raider")).onClick = null;
#if UNITY_ANDROID
		UIEventListenerCustom.Get (FindChild ("OptionItems/ResetData")).onClick = null;
#endif
		UIEventListenerCustom.Get (FindChild ("OptionItems/Notice")).onClick = null;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Contact")).onClick = null;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Reward")).onClick = null;

		base.DestoryUI ();
	}

	void FindUIElement(){
		FindChild<UILabel> ("OptionItems/Music/Label").text = TextCenter.GetText ("Game_Setting_Option_Music");
		FindChild<UILabel> ("OptionItems/Nickname/Label").text = TextCenter.GetText ("Game_Setting_Option_NickName");
		FindChild<UILabel> ("OptionItems/Raider/Label").text = TextCenter.GetText ("Game_Setting_Option_Raider");
		FindChild<UILabel> ("OptionItems/Notice/Label").text = TextCenter.GetText ("Game_Setting_Option_Notice");
		FindChild<UILabel> ("OptionItems/Reward/Label").text = TextCenter.GetText ("Game_Setting_Option_Reward");
		FindChild<UILabel> ("OptionItems/Contact/Label").text = TextCenter.GetText ("Game_Setting_Option_Contact");

#if UNITY_EDITOR
		FindChild<UILabel> ("OptionItems/ResetData/Label").text = TextCenter.GetText ("Text_ResetData");
#elif UNITY_ANDROID
		FindChild("OptionItems/ResetData").SetActive(false);
#else
		FindChild("OptionItems/ResetData").SetActive(false);
#endif

		UIEventListenerCustom.Get (FindChild ("OptionItems/Music")).onClick = ClicItems;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Nickname")).onClick = ClicItems;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Raider")).onClick = ClicItems;
		UIEventListenerCustom.Get (FindChild ("OptionItems/ResetData")).onClick = ClicItems;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Reward")).onClick = ClicItems;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Notice")).onClick = ClicItems;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Contact")).onClick = ClicItems;
	}

	void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
	}

	void ClicItems(GameObject obj){
		switch (obj.name) {
		case "Music":
			UIManager.Instance.ChangeScene(SceneEnum.Music);
			break;
		case "Nickname":
			UIManager.Instance.ChangeScene(SceneEnum.NickName);
			break;
		case "Raider":
			UIManager.Instance.ChangeScene(SceneEnum.Raider);
			break;
		case "ResetData":
			GameDataStore.Instance.StoreData(GameDataStore.UUID, "");
			GameDataStore.Instance.StoreData(GameDataStore.USER_ID, 0);
			UIManager.Instance.ChangeScene(SceneEnum.Loading);
			break;
		case "Reward":
			UIManager.Instance.ChangeScene(SceneEnum.Reward);
			break;
		case "Contact":
			ShowContact();
			break;
		case "Notice":
			UIManager.Instance.ChangeScene(SceneEnum.OperationNotice);
			break;
		}
	}

	private void ShowContact(){
		MsgWindowParams mwp = new MsgWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("ContactUs");
		mwp.contentText = TextCenter.GetText("ContactUsContent");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
	}

//	void SetOption() {
//		string itemPath = "Prefabs/UI/Others/OtherOptions";
//		ResourceManager.Instance.LoadLocalAsset( itemPath ,CallbackFunc);
//	}
//
//	void CallbackFunc(Object o){
//		GameObject item = o as GameObject;
//		othersScroller = new DragPanel ( "OthersScroller", scrollerItem );
//		othersScroller.CreatUI ();
//		
//		GameObject musicOption = othersScroller.AddScrollerItem( item );
//		musicOption.name = "MusicOption";
//		musicOption.GetComponentInChildren<UILabel>().text = "Music";
//		options.Add( musicOption, musicPanel );
//		
//		GameObject nickNameOption = othersScroller.AddScrollerItem( item );
//		nickNameOption.name = "NickNameOption";
//		nickNameOption.GetComponentInChildren<UILabel>().text = "NickName";
//		options.Add( nickNameOption, nickNamePanel );
//		
//		//raider
//		GameObject raiderOption = othersScroller.AddScrollerItem( item );
//		raiderOption.name = "RaiderOption";
//		raiderOption.GetComponentInChildren<UILabel>().text = "Raider";
//		options.Add( raiderOption, nickNamePanel );
//		
//		//currency
//		GameObject currencyOption = othersScroller.AddScrollerItem( item );
//		currencyOption.name = "CurrencyOption";
//		currencyOption.GetComponentInChildren<UILabel>().text = "Currency";
//		options.Add( currencyOption, nickNamePanel );
//		
//		resetOption = othersScroller.AddScrollerItem( item );
//		resetOption.name = "ResetOption";
//		resetOption.GetComponentInChildren<UILabel>().text = "Reset Data";
//		options.Add( resetOption, nickNamePanel );
//		
//		
//		
//		Transform parentTrans = FindChild("OptionItems").transform;
//		othersScroller.DragPanelView.SetScrollView(ConfigDragPanel.OthersDragPanelArgs, parentTrans);
//		
//		for(int i = 0; i < othersScroller.ScrollItem.Count; i++)
//			UIEventListener.Get( othersScroller.ScrollItem[ i ].gameObject ).onClick = ClickOption;
//	}



       



}