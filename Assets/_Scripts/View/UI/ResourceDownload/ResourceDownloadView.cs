﻿using UnityEngine;
using System.Collections;

public class ResourceDownloadView : UIComponentUnity {

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();
	}
	
	public override void ShowUI(){
		base.ShowUI();
//		MsgCenter.Instance.Invoke(CommandEnum.ShowHomeBgMask, false);
//		
//		MsgCenter.Instance.AddListener (CommandEnum.ChangeSceneComplete,OnChangeSceneComplete);
//		
//		MsgCenter.Instance.AddListener (CommandEnum.RefreshRewardList,OnRefreshRewardList);
//		
//		GameTimer.GetInstance ().CheckRefreshServer ();
		
//		ShowRewardInfo ();
	}

	public override void HideUI(){
		base.HideUI ();
	}

	private void InitUI(){
		FindChild<UILabel> ("Title").text = TextCenter.GetText ("Title_ResourceDownload");
	}
}
