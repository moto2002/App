using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoviceGuideUtil {

	private static LayerMask camLastLayer;

	private static int oneBtnClickLayer;

	private static Dictionary<string ,GameObject> arrows = new Dictionary<string,GameObject>();

	private static GameObject tipText;

	private static UIEventListenerCustom.VoidDelegate clickDelegate;

	private static UIEventListenerCustom.LongPressDelegate longPressDelegate;

	private static UIEventListenerCustom.LongPressDelegate pressDelegate;

	private static UICallback clickCallback;

	private static GameObject[] multiBtns;

	private static GameObject arrowPrefab;

	private static UICamera mainCam;

	// posAndDir:the x,y stand for the position, the z stands for direction
	public static void ShowArrow(GameObject[] parents,Vector3[] posAndDir, bool showTap = true, bool forceClick = true, UICallback callback = null){

		Vector3 dir;
		int i = 0,len = posAndDir.Length;
		ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/NoviceGuide/NoviceGuideArrow", o => {
			GameObject obj = o as GameObject;
			foreach (GameObject parent in parents) {
				//			GameObject arrow = GameObject.Instantiate(obj,new Vector3(pos.x,pos.y,0),dir) as GameObject;
				GameObject arrow = NGUITools.AddChild (parent, obj);
				GameObject tap = arrow.transform.FindChild("Sprite/Sprite").gameObject;
				tap.layer = arrow.transform.FindChild("Sprite").gameObject.layer = arrow.layer;
				if(showTap){
					tap.GetComponent<UISprite>().enabled = true;
				}else{
					tap.GetComponent<UISprite>().enabled = false;
				}
				TweenPosition tPos = arrow.transform.FindChild("Sprite").GetComponent<TweenPosition> ();

				Vector3 size = Vector3.zero;
//				try {
				if(parent.GetComponent<BoxCollider> () != null){
					size = parent.GetComponent<BoxCollider> ().size;
				}else{
					size = parent.transform.localPosition;
				}
					
//				} catch (MissingComponentException e) {
//					LogHelper.LogWarning (e.ToString ());
//					
//				}

				switch (i < len ? (int)posAndDir [i].z : 0) {
				//point to the top
				case 3:
					dir = new Vector3 (0.0f, 0.0f, 180.0f);// = Quaternion.FromToRotation(new Vector3(1,0,0),Vector3.zero);
					tPos.to.y = -size.y / 2 - 12 + posAndDir [i].y;
					tPos.from.y = -size.y / 2 - 32.0f + posAndDir [i].y;
					tPos.to.x = posAndDir [i].x;
					tPos.from.x = posAndDir [i].x;
					break;
				//point to the right
				case 4:
					dir = new Vector3 (0f, 0f, 90f);
//					dir = Quaternion.FromToRotation(new Vector3(-1,0,0),Vector3.zero);
					tPos.to.x = -size.x / 2 - 12 + posAndDir [i].x;
					tPos.from.x = -size.x / 2 - 32.0f + posAndDir [i].x;
					tPos.to.y = posAndDir [i].y;
					tPos.from.y = posAndDir [i].y;
					break;
				//point to the bottom
				case 1:
//					dir = Quaternion.FromToRotation(new Vector3(0,1,0),Vector3.zero);
					dir = new Vector3 (0f, 0f, 0f);
					tPos.to.y = size.y / 2 + 12 + posAndDir [i].y;
					tPos.from.y = size.y / 2 + 32.0f + posAndDir [i].y;
					tPos.to.x = posAndDir [i].x;
					tPos.from.x = posAndDir [i].x;
					break;
				case 2:
	//point to the left
//					dir = Quaternion.FromToRotation(new Vector3(0,-1,0),Vector3.zero);
					dir = new Vector3 (0f, 0f, 270f);
					tPos.to.x = size.x / 2 + 12 + posAndDir [i].x;
					tPos.from.x = size.x / 2 + 32.0f + posAndDir [i].x;
					tPos.to.y = posAndDir [i].y;
					tPos.from.y = posAndDir [i].y;
					break;
				default:
					dir = Vector3.zero;
					break;
				}

				arrow.transform.FindChild("Sprite").transform.Rotate (dir);
				tap.transform.Rotate (-dir);
				NGUITools.AdjustDepth (arrow, 20);
				//			if(obj.transform.parent != null)
				//			{
				//LogHelper.Log("-------///-......parent is not null: " + obj.transform.parent);
				//			}
				LogHelper.Log ("=====add arrow dic key: " + parent.GetInstanceID () + parent.name);

				arrows.Add (parent.GetInstanceID () + parent.name, arrow);
				i++;
			}
		});

	}

	public static void ShowArrow(GameObject parent,Vector3 posAndDir, bool showTap = true, bool forceClick = true, UICallback callback = null){

		Vector3 dir;
		if (arrowPrefab == null) {
			arrowPrefab = ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/NoviceGuide/NoviceGuideArrow",null) as GameObject;	
		}
		GameObject arrow = NGUITools.AddChild (parent, arrowPrefab);
		GameObject tap = arrow.transform.FindChild("Sprite/Sprite").gameObject;
		tap.layer = arrow.transform.FindChild("Sprite").gameObject.layer = arrow.layer;
		if(showTap){
			tap.GetComponent<UISprite>().enabled = true;
		}else{
			tap.GetComponent<UISprite>().enabled = false;
		}
		TweenPosition tPos = arrow.transform.FindChild("Sprite").GetComponent<TweenPosition> ();
		
		Vector3 size = Vector3.zero; 
		Vector3 center = Vector3.zero;
		if(parent.GetComponent<BoxCollider> () != null){
			size = parent.GetComponent<BoxCollider> ().size;
			center = parent.GetComponent<BoxCollider> ().center;
		}

		switch ( (int)posAndDir .z) {
			//point to the top
		case 3:
			dir = new Vector3 (0.0f, 0.0f, 180.0f);// = Quaternion.FromToRotation(new Vector3(1,0,0),Vector3.zero);
			tPos.to.y = - 1.0f;
			tPos.from.y = - 50.0f;
			tPos.from.x = tPos.to.x = posAndDir.x = 0;
			arrow.transform.localPosition = new Vector3(center.x + posAndDir.x , center.y - size.y / 2 + posAndDir.y,0);
			break;
			//point to the right
		case 4:
			dir = new Vector3 (0f, 0f, 90f);
			tPos.to.x =  - 1.0f;
			tPos.from.x = - 50.0f;
			tPos.to.y = tPos.from.y = 0;
			arrow.transform.localPosition = new Vector3(center.x - size.x / 2 + posAndDir.x , center.y + posAndDir.y, 0);
			break;
			//point to the bottom
		case 1:
			//					dir = Quaternion.FromToRotation(new Vector3(0,1,0),Vector3.zero);
			dir = new Vector3 (0f, 0f, 0f);
			tPos.to.y = 1.0f;
			tPos.from.y = 50.0f;
			tPos.from.x = tPos.to.x = 0f;
			arrow.transform.localPosition = new Vector3(center.x + posAndDir.x , center.y + size.y / 2 + posAndDir.y,0);
			break;
		case 2:
			//point to the left
			//					dir = Quaternion.FromToRotation(new Vector3(0,-1,0),Vector3.zero);
			dir = new Vector3 (0f, 0f, 270f);
			tPos.to.x = 1f;
			tPos.from.x = 50.0f;
			tPos.from.y = tPos.to.y = 0f;

			arrow.transform.localPosition = new Vector3(center.x + size.x / 2 + posAndDir.x , center.y + posAndDir.y, 0);
			break;
		default:
			dir = Vector3.zero;
			break;
		}
		
		arrow.transform.FindChild("Sprite").transform.Rotate (dir);
		tap.transform.Rotate (-dir);
		NGUITools.AdjustDepth (arrow, 20);

		//don't use the parent gameobject ref.
		arrows.Add (parent.GetInstanceID () + parent.name, arrow);

		if (forceClick) {
			ForceOneBtnClick(parent,callback);
		}
	}

	public static void RemoveAllArrows(){
		Debug.Log ("arrow count: " + arrows.Count);

		foreach (string key in arrows.Keys) {
			GameObject.Destroy(arrows[key]);
			Debug.Log ("===/////===remove arrow: "+key);
		}
		arrows.Clear ();
	}

	public static void RemoveArrow(GameObject obj)
	{
		LogHelper.Log ("arrow count: " + arrows.Count);
		string key = obj.GetInstanceID () + obj.name;
		if (arrows.ContainsKey (key)) {
			GameObject obj1 = arrows[key];
			GameObject.Destroy(obj1);
			arrows.Remove(key);	

			LogHelper.Log ("===/////===remove arrow: "+ key);
		}
	}

	public static void showTipText(string text,Vector2 pos = default(Vector2)){
		Debug.Log ("--------------///////tip text: " + text);
		if (tipText == null) {
			ResourceManager.Instance.LoadLocalAsset("Prefabs/TipText", o => {
				GameObject tip = o as GameObject;
				tipText = GameObject.Instantiate (tip) as GameObject;
				Transform trans = tipText.transform;
				trans.parent = ViewManager.Instance.CenterPanel.transform;
				trans.localPosition = Vector3.zero;
				//trans.position =Vector3.zero;
				trans.gameObject.layer = ViewManager.Instance.CenterPanel.layer;
				trans.localScale = Vector3.one;
				
				NGUITools.AdjustDepth (tipText, 100);
				
				tipText.SetActive (true);
				
				tipText.transform.localPosition = new Vector3 (0, pos.y, 0);
				LogHelper.Log ("tip text position: " + tipText.transform.position);
				
				tipText.GetComponent<TipText> ().SetText (text);
			});

		} else {
			tipText.SetActive (true);
			
			tipText.transform.localPosition = new Vector3 (0, pos.y, 0);
			LogHelper.Log ("tip text position: " + tipText.transform.position);
			
			tipText.GetComponent<TipText>().SetText(text);
		}



	}

	public static void HideTipText(){
		tipText.SetActive (false);
	}

	public static void ForceOneBtnClick(GameObject obj, UICallback callback, bool isExecuteBefore = true)
	{
		if(mainCam == null)
			mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
		camLastLayer = mainCam.eventReceiverMask;

		//TODO:Change the execute order....this may be different in different platform
		if (isExecuteBefore) {
			clickDelegate = UIEventListenerCustom.Get (obj).onClick;
			UIEventListenerCustom.Get (obj).onClick = null;
			UIEventListenerCustom.Get (obj).onClick += BtnClick;
			UIEventListenerCustom.Get (obj).onClick += clickDelegate;
			clickDelegate = null;
		
		} else {
			UIEventListenerCustom.Get (obj).onClick += BtnClick;	
		}
 		
		clickCallback = callback;
		longPressDelegate = UIEventListenerCustom.Get (obj).LongPress;
		UIEventListenerCustom.Get (obj).LongPress = null;

		oneBtnClickLayer = obj.layer;
		LayerMask mask =  1 << LayerMask.NameToLayer ("NoviceGuide");
//		mainCam.eventReceiverMask = mask;
		obj.layer = LayerMask.NameToLayer ("NoviceGuide");
//		Debug.Log ("force click: "+ obj.ToString() + " layer: " + obj.layer);

		InputManager.Instance.SetBlockWithinLayer (BlockerReason.NoviceGuide, true);

		LogHelper.Log ("main cam layer(force click): " + mainCam.eventReceiverMask.value);
	}



	private static void BtnClick(GameObject btn)
	{
		InputManager.Instance.SetBlockWithinLayer (BlockerReason.NoviceGuide, false);
		
		btn.layer = oneBtnClickLayer;


		UIEventListenerCustom.Get (btn).onClick -= BtnClick;
		UIEventListenerCustom.Get (btn).LongPress = longPressDelegate;
		longPressDelegate = null;
//		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
		if (clickCallback != null) {
			UICallback clickTemp = clickCallback;
			clickCallback = null;
			clickTemp (btn);
			clickTemp = null;

		}

//		LogHelper.Log ("btn layer: " + oneBtnClickLayer + ", mainCam layer: " + mainCam.eventReceiverMask.value);
	}

	public static void ForceBtnsClick(GameObject[] objs,UIEventListenerCustom.VoidDelegate clickCalback){
		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
		camLastLayer = mainCam.eventReceiverMask;
//		LayerMask mask =  1 << LayerMask.NameToLayer ("NoviceGuide");
//		mainCam.eventReceiverMask = mask;
		InputManager.Instance.SetBlockWithinLayer (BlockerReason.NoviceGuide, true);

		multiBtns = objs;
		
		
		clickDelegate = clickCalback;
		foreach (GameObject item in objs) {
			clickDelegate = UIEventListenerCustom.Get (item).onClick;
			UIEventListenerCustom.Get (item).onClick = null;
			UIEventListenerCustom.Get (item).onClick += MultiBtnClick;
			UIEventListenerCustom.Get (item).onClick += clickCalback;
			UIEventListenerCustom.Get (item).onClick += clickDelegate;
			clickDelegate = clickCalback;
			oneBtnClickLayer = item.layer;
			item.layer = LayerMask.NameToLayer ("NoviceGuide");
		}
		
		LogHelper.Log ("main cam layer(force click): " + mainCam.eventReceiverMask.value);
	}

	private	static void MultiBtnClick(GameObject btn){

		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
//		mainCam.eventReceiverMask = camLastLayer;

		InputManager.Instance.SetBlockWithinLayer (BlockerReason.NoviceGuide, false);

		foreach (GameObject item in multiBtns) {
			UIEventListenerCustom.Get (item).onClick -= MultiBtnClick;
			UIEventListenerCustom.Get (item).onClick -= clickDelegate;
			item.layer = oneBtnClickLayer;
			LogHelper.Log("======multi btns layer: "+item.layer);
		}
		multiBtns = null;
		clickDelegate = null;

		LogHelper.Log ("btn layer: " + oneBtnClickLayer + ", mainCam layer: " + mainCam.eventReceiverMask.value);
	}

	public static void ForceOneBtnPress(GameObject obj)
	{
		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
		camLastLayer = mainCam.eventReceiverMask;
		
		//TODO:Change the execute order....this may be different in different platform
		pressDelegate = UIEventListenerCustom.Get (obj).LongPress;
		UIEventListenerCustom.Get (obj).LongPress = null;
		UIEventListenerCustom.Get (obj).LongPress += BtnPress;
		UIEventListenerCustom.Get (obj).LongPress += pressDelegate;
		pressDelegate = null;
		
		oneBtnClickLayer = obj.layer;
		LayerMask mask =  1 << LayerMask.NameToLayer ("NoviceGuide");
		InputManager.Instance.SetBlockWithinLayer (BlockerReason.NoviceGuide, true);

		obj.layer = LayerMask.NameToLayer ("NoviceGuide");
		LogHelper.Log ("main cam layer(force click): " + mainCam.eventReceiverMask.value);
	}
	
	private static void BtnPress(GameObject btn)
	{
		UIEventListenerCustom.Get (btn).LongPress -= BtnPress;
		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();

		InputManager.Instance.SetBlockWithinLayer (BlockerReason.NoviceGuide, false);

		btn.layer = oneBtnClickLayer;
		LogHelper.Log ("btn layer: " + oneBtnClickLayer + ", mainCam layer: " + mainCam.eventReceiverMask.value);
		
	}

	public static void showTipTextAnimation(){
		tipText.GetComponent<TipText> ().ShowAnimation ();
	}
	
}
