﻿using UnityEngine;
using System.Collections;

public class SceneInfoDecoratorUnity : UIComponentUnity ,IUICallback, IUISetBool{
	
	private UILabel labelSceneName;
	private UIImageButton btnBackScene;

	private IUICallback iuiCallback; 
	private bool temp = false;
	
	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);
		InitUI();

		temp = origin is IUICallback;
	}
	
	public override void ShowUI () {

		base.ShowUI ();
		ShowTweenPostion(0.2f);

	}
	
	public override void HideUI () {
		base.HideUI ();
		ShowTweenPostion();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		labelSceneName = FindChild< UILabel >( "ImgBtn_Back_Scene/Label_Scene_Name" );
		btnBackScene =  FindChild< UIImageButton >( "ImgBtn_Back_Scene" );

		UIEventListener.Get( btnBackScene.gameObject ).onClick = BackPreScene;
	}
	
	public void Callback (object data)
	{
		string info = string.Empty;
		try {
			info = (string)data;
		} 
		catch (System.Exception ex) {
		}
		if(!string.IsNullOrEmpty(info)){
			labelSceneName.text = info;
		}
	}

	public void SetEnable (bool b)
	{
		btnBackScene.isEnabled = b;
	}

	void BackPreScene (GameObject go)
	{
		if( UIManager.Instance.baseScene.CurrentScene == SceneEnum.UnitDetail )
		{
			SceneEnum preScene = UIManager.Instance.baseScene.PrevScene;
			UIManager.Instance.ChangeScene( preScene );
			return;
		}

		if(temp) {
			IUICallback call = origin as IUICallback;
			call.Callback(go);
		}
	}

	private void ShowTweenPostion( float mDelay = 0f, UITweener.Method mMethod = UITweener.Method.Linear ) 
	{
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		
		if( list == null )
			return;
		
		foreach( var tweenPos in list)
		{		
			if( tweenPos == null )
				continue;
			
			Vector3 temp;
			temp = tweenPos.to;
			tweenPos.to = tweenPos.from;
			tweenPos.from = temp;
			
			tweenPos.delay = mDelay;
			tweenPos.method = mMethod;
			
			tweenPos.Reset();
			tweenPos.PlayForward();
			
		}
	}
}
