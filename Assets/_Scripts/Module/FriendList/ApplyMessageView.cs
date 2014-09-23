using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ApplyMessageView : ViewBase{
	GameObject rootPanel;

	UILabel titleLabel;
	UILabel noteLabel;
	UILabel nameLabel;
	UILabel rankLabel;
	UILabel timeLabel;
	UILabel idLabel;

	UIButton sureButton;
	UIButton cancelButton;

//	UITexture avatarTexture;
	
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config,data);
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
//		ShowSelf(false);

//		HideUI ();
	}

	public override void HideUI(){
		base.HideUI();
//		ShowSelf(false);  
	}

	public override void CallbackView(params object[] args){
//		base.CallbackView(data);
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (args[0].ToString()){
			case "StylizeTitle": 
				ShowTitle(args[1]);
				break;
			case "StylizeNote":
				ShowNote(args[1]);
				break;
			case "RefreshContent":
				ShowCenterContent(args[1]);
				ShowSelf(true);
				break;
			case "HidePanel":
				HidePanel(args[1]);
				break;
			default:
				break;
		}
	}

	void HidePanel(object args){
		ShowSelf(false);
	}

	void InitUIElement(){
		rootPanel = FindChild("Window");

		titleLabel = FindChild<UILabel>("Window/Label_Title");
		noteLabel = FindChild<UILabel>("Window/Label_Note");
		nameLabel = FindChild<UILabel>("Window/Label_Vaule/Name");
		rankLabel = FindChild<UILabel>("Window/Label_Vaule/Rank");
		timeLabel = FindChild<UILabel>("Window/Label_Vaule/LastLogin");
		idLabel = FindChild<UILabel>("Window/Label_Vaule/ID");
//		avatarTexture = FindChild<UITexture>("Window/Avatar/Texture");
		sureButton = FindChild<UIButton>("Window/Button_Sure");
		cancelButton = FindChild<UIButton>("Window/Button_Cancel");
	
		UIEventListener.Get(sureButton.gameObject).onClick = ClickSure;
		UIEventListener.Get(cancelButton.gameObject).onClick = ClickCancel;  

		FindChild<UILabel> ("Window/Label_Text/Name").text = TextCenter.GetText ("Text_Name_Colon");
		FindChild<UILabel> ("Window/Label_Text/Rank").text = TextCenter.GetText ("Text_Rank_Colon");
		FindChild<UILabel> ("Window/Label_Text/LastLogin").text = TextCenter.GetText ("Text_LastLogin_Colon");
		FindChild<UILabel> ("Window/Button_Sure/Label").text = TextCenter.GetText ("OK");
		FindChild<UILabel> ("Window/Button_Cancel/Label").text = TextCenter.GetText ("Cancel");
		FindChild<UILabel> ("Window/Label_Note").text = TextCenter.GetText ("ApplyFriendNote");
	}


	void ClickSure(GameObject btn){
//		Debug.LogError("ApplyMessageView.ClickSure(),  click...");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickSure", null);
//		ExcuteCallback(cbdArgs);
		ModuleManager.SendMessage (ModuleEnum.ApplyMessageModule, "ClickSure");
	}

	
	void ClickCancel(GameObject btn){
//		Debug.LogError("ApplyMessageView.ClickCancel(),  click...");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickCancel", null);
//		ExcuteCallback(cbdArgs);
		ModuleManager.SendMessage (ModuleEnum.ApplyMessageModule, "ClickCancel");
	}

	void ShowSelf(bool canShow){
//		this.gameObject.SetActive(canShow);
		if (canShow){
			ShowUI();
//			MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.BriefInfoWindow, true));
			rootPanel.transform.localScale = new Vector3(1f, 0f, 1f);
			iTween.ScaleTo(rootPanel, iTween.Hash("y", 1, "time", 0.4f, "easetype", iTween.EaseType.easeOutBounce));
		}
		else{
//			MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.BriefInfoWindow, false));     
			HideUI();
		}
	}

	void ShowTitle(object args){
		titleLabel.text = args as string;
	}

	void ShowNote(object args){
		noteLabel.text = args as string;
	}

	void ShowCenterContent(object args){
		FriendInfo tfi = args as FriendInfo;

//		tfi.UserUnit.UnitInfo.GetAsset(UnitAssetType.Avatar, o=>{
//			avatarTexture.mainTexture = o as Texture2D;
//		});
		if(tfi.nickName == string.Empty)
			nameLabel.text = "NoName";
		else
			nameLabel.text = tfi.nickName;

		rankLabel.text = tfi.rank.ToString();
		timeLabel.text = Utility.TimeHelper.GetLatestPlayTime(tfi.lastPlayTime);
		idLabel.text = tfi.userId.ToString();

		FriendUnitItem fuv = FriendUnitItem.Inject(FindChild("Window/Avatar"));
		fuv.Init(tfi);

		UIEventListenerCustom.Get (FindChild("Window/Avatar")).LongPress = null;
//		fuv.callback = ClickItem;
	}
      
//	private void ClickItem(FriendUnitItem item){
//		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		MsgCenter.Instance.Invoke(CommandEnum.ViewApplyInfo, item.FriendInfo);
//	}
}