﻿using UnityEngine;
using System.Collections;

//untis
public class NoviceGuideStepI_StateOne:NoviceGuidState{
	
	private static NoviceGuideStepI_StateOne instance;
	
	public static NoviceGuideStepI_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepI_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepI_StateOne ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepI state_one");
		
		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide42_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide42_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
	}

	private void ClickOK(object data){
		GameObject gm = GameObject.Find ("LevelUpUI(Clone)").GetComponent<LevelUpOperateUnity>().GetPartyUnitItem(0).gameObject;
		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,2)});

		UIEventListenerCustom.Get (gm).onClick += OnClickItem;
		//Debug.LogError ("gm : " + gm);
		NoviceGuideUtil.ForceOneBtnClick (gm);
	}

	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		NoviceGuideUtil.RemoveAllArrows ();

		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide43_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide43_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK1;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);

	}

	private void ClickOK1(object data){
		GameObject gm = GameObject.Find ("LevelUpUI(Clone)").GetComponent<LevelUpOperateUnity>().GetPartyUnitItem(-1).gameObject;

		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,2)});
		UIEventListenerCustom.Get (gm).onClick += OnClickItem1;
		NoviceGuideUtil.ForceOneBtnClick (gm);
	}

	private void OnClickItem1(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem1;
		NoviceGuideUtil.RemoveAllArrows ();

		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide44_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide44_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK2;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}

	private void ClickOK2(object data){
		GameObject gm = GameObject.FindWithTag ("level_up_friend");

		UIEventListenerCustom.Get (gm).onClick += OnClickFriend;
		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,3)});
		NoviceGuideUtil.ForceOneBtnClick (gm);

	}

	private void OnClickFriend(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickFriend;
		NoviceGuideUtil.RemoveAllArrows ();
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepI_StateTwo.Instance());
		}
		else{
			
		}
	}
	
}

public class NoviceGuideStepI_StateTwo:NoviceGuidState{
	
	private static NoviceGuideStepI_StateTwo instance;
	
	public static NoviceGuideStepI_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepI_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepI_StateTwo ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepI state_two");
		
		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide45_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide45_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
	}

	private void ClickOK(object data){
		GameObject gm = GameObject.Find ("FriendWindows(Clone)").GetComponent<FriendWindows> ().GetHelperUnitItem(0).gameObject;
		NoviceGuideUtil.ForceOneBtnClick (gm);
		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,3)});
		UIEventListenerCustom.Get (gm).onClick += OnClickFriend;
	}

	private void OnClickFriend(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickFriend;
		NoviceGuideUtil.RemoveAllArrows ();

		JumpToNextState = true;
	}

	public override void Execute(NoviceGuideStepEntity stepEntity)
	{	
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepI_StateThree.Instance());
		}
		else{
			
		}
	}

}

public class NoviceGuideStepI_StateThree:NoviceGuidState{
	
	private static NoviceGuideStepI_StateThree instance;
	
	public static NoviceGuideStepI_StateThree Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepI_StateThree ();
		return instance;
	}
	
	private NoviceGuideStepI_StateThree ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepI state_three");
		
		GameObject gm = GameObject.FindWithTag ("level_up_btn");
		NoviceGuideUtil.ForceOneBtnClick (gm);
		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,4)});
		UIEventListenerCustom.Get (gm).onClick += OnClickLevelUp;
	}

	
	private void OnClickLevelUp(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickLevelUp;
		NoviceGuideUtil.RemoveAllArrows ();
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepI_StateFour.Instance());
		}
		else{
			
		}
	}
	
}

public class NoviceGuideStepI_StateFour:NoviceGuidState{
	
	private static NoviceGuideStepI_StateFour instance;
	
	public static NoviceGuideStepI_StateFour Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepI_StateFour ();
		return instance;
	}
	
	private NoviceGuideStepI_StateFour ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepI state_four");

		UIManager.Instance.forbidChangeScene = true;

		MsgCenter.Instance.AddListener (CommandEnum.levelDone, OnLevelDone);

	}
	
	private void OnLevelDone(object data){
		UIManager.Instance.forbidChangeScene = false;

		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide46_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide46_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}

	private void ClickOK(object data){
		GameObject gm = GameObject.FindWithTag ("scene_back_btn");
		
		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,3)});
		UIEventListenerCustom.Get (gm).onClick += OnClickBack;
		NoviceGuideUtil.ForceOneBtnClick (gm);
	}

	private void OnClickBack(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickBack;
		UIEventListenerCustom.Get (gm).onClick += OnClickBack1;
		NoviceGuideUtil.ForceOneBtnClick (gm);


	}

	private void OnClickBack1(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickBack1;
		NoviceGuideUtil.RemoveAllArrows ();
		JumpToNextState = true;

		NoviceGuideStepEntityManager.CurrentNoviceGuideStage++;
	}

	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (null);
		}
		else{
			
		}
	}

}

//public class NoviceGuideStepI_StateFive:NoviceGuidState{
//	
//	private static NoviceGuideStepI_StateFive instance;
//	
//	public static NoviceGuideStepI_StateFive Instance()
//	{
//		if (instance == null)
//			instance = new NoviceGuideStepI_StateFive ();
//		return instance;
//	}
//	
//	private NoviceGuideStepI_StateFive ():base()	{}
//	
//	public override void Enter(NoviceGuideStepEntity stepEntity)
//	{
//		LogHelper.Log (stepEntity.GetType () + " is execute stepI state_five");
//		
//		GameObject gm = GameObject.FindWithTag ("scene_back_btn");
//		
//		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,3)});
//		UIEventListenerCustom.Get (gm).onClick += OnClickBack;
//		NoviceGuideUtil.ForceOneBtnClick (gm);
//	}
//	
//	private void OnClickBack(GameObject gm){
//		UIEventListenerCustom.Get (gm).onClick -= OnClickBack;
//		NoviceGuideUtil.RemoveAllArrows();
//
//		NoviceGuideStepEntityManager.CurrentNoviceGuideStage++;
//	}
//	
//	public override void Execute(NoviceGuideStepEntity stepEntity)
//	{
//		
//		if (JumpToNextState) {
//			stepEntity.GetStateMachine ().ChangeState (null);
//		}
//		else{
//			
//		}
//	}
//	
//}
