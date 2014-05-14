using UnityEngine;
using System.Collections;

public class BgDecoratorUnity : UIComponentUnity {
	private UITexture sprite;

	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config,origin);
		sprite = GetComponent<UITexture> ();
	}

	public override void ShowUI () {
		base.ShowUI();
		UIEventListener.Get (gameObject).onClick = OnClickCallback;
		NGUITools.AddWidgetCollider (gameObject);
	}

	public override void HideUI () {
		base.HideUI();

	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	void OnClickCallback(GameObject caller) {
//		Debug.LogError ("origin``" + origin == null + "```type``" + origin.GetType());
		if(origin != null && origin is IUICallback){
			IUICallback callback = origin as IUICallback;
			callback.CallbackView (caller);	
		}
	}
}
