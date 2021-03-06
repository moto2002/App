using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainBackgroundView : ViewBase {
	private UISprite background;
	private UITexture otherBg;
	public override void Init (UIConfigItem config, Dictionary<string, object> data = null){
		base.Init (config, data);
		background = transform.FindChild("HomeBG").GetComponent<UISprite>();
		otherBg = FindChild<UITexture> ("OtherBG");
		otherBg.enabled = false;
	}

	public override void ShowUI () {
		base.ShowUI();
//		UIEventListenerCustom.Get (gameObject).onClick = OnClickCallback;
//		NGUITools.AddWidgetCollider (gameObject);

		MsgCenter.Instance.AddListener(CommandEnum.ShowHomeBgMask, ShowMask);
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowHomeBgMask, ShowMask);
	}

	public override void DestoryUI () {
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowHomeBgMask, ShowMask);
		base.DestoryUI ();
	}

//	void OnClickCallback(GameObject caller) {
//		if(origin != null && origin is IUICallback){
//			IUICallback callback = origin as IUICallback;
//			callback.CallbackView (caller);	
//		}
//	}

	private void ShowMask(object msg){
		bool isMask = (bool)msg;
//		Debug.LogError ("ShowMask : " + isMask);
		otherBg.enabled = isMask;
		background.enabled = !isMask;
	}
}
