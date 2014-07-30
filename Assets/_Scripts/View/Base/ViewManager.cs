using UnityEngine;
using System.Collections.Generic;

public class ViewManager {
	private static ViewManager instance;

	public static ViewManager Instance {
		get {
			if(instance == null)
				instance = new ViewManager();
			return instance;
		}
	}
	
	private GameObject mainUIRoot;
	
	public GameObject MainUIRoot
	{
		get{return mainUIRoot;}
	}

	private GameObject topPanel;

	public GameObject TopPanel
	{
		get{return topPanel;}
	}

	private GameObject bottomPanel;

	public GameObject BottomPanel
	{
		get{return bottomPanel;}
	}
	
	private GameObject centerPanel;
	
	public GameObject CenterPanel
	{
		get{ return centerPanel; }
	}

	private GameObject parentPanel;
	
	public GameObject ParentPanel
	{
		get{ return parentPanel; }
	}

	private GameObject popupPanel;
	
	public GameObject PopupPanel
	{
		get{ return popupPanel; }
	}

	private GameObject bottomLeftPanel;
	public GameObject BottomLeftPanel {
		get { return bottomLeftPanel; }
	}

	public int manualHeight;

	private GameObject effectPanel;
	public GameObject EffectPanel {
		get { return effectPanel; }
	}
	
	private UICamera mainUICamera;
	
	public UICamera MainUICamera
	{
		set{mainUICamera = value;}
		get{return mainUICamera;}
	}

	private UICamera battleCamera;
	public UICamera BattleCamera {
		set { battleCamera = value; }
		get { return battleCamera; }
	}

	private GameObject battleBottom;
	/// <summary>
	/// battle bottom parent object.
	/// </summary>
	/// <value>The battle bottom.</value>
	public GameObject BattleBottom {
		get { return battleBottom; }
		set { battleBottom = value; }
	}

	private Font dynamicFont;

	public Font DynamicFont {
		get{return dynamicFont;}
	}

	private TipsLabelUI tipsLabelUI;

	private GameObject popUpBg;

	public void Init(GameObject ui){
		mainUIRoot = ui;		
		mainUICamera = mainUIRoot.GetComponentInChildren<UICamera>();
		Transform trans = mainUIRoot.transform;
		parentPanel = trans.Find("Bottom").gameObject;
		popupPanel = trans.Find ("PopUp").gameObject;
		popUpBg = trans.Find ("PopUp/BackGround").gameObject;
		popUpBg.SetActive (false);
		topPanel = trans.Find ("Top").gameObject;
		bottomPanel = trans.Find ("Bottom").gameObject;
		effectPanel = trans.Find ("Anchor/EffectPanel").gameObject;
		centerPanel = trans.Find ("Center").gameObject;
		tipsLabelUI = centerPanel.transform.Find ("Panel/LabelPanel/Label").GetComponent<TipsLabelUI> ();

		bottomLeftPanel =  trans.Find ("BottomLeft").gameObject;

		ResourceManager.Instance.LoadLocalAsset("Font/Dimbo Regular", o =>{
			dynamicFont = o as Font;
			manualHeight = mainUIRoot.GetComponent<UIRoot>().manualHeight;
		}
		);
	}

	public void ShowTipsLabel (string content) {
		tipsLabelUI.ShowInfo (content);
	}

	public void ShowTipsLabel (string content, params object[] data) {
		string info = string.Format (content, data);
		tipsLabelUI.ShowInfo (info);
	}

	public void ShowTipsLabel(string content, GameObject target) {
		tipsLabelUI.ShowInfo (content, target);
	}

	private Dictionary<string,UIBaseUnity> uiObjectDic = new Dictionary<string, UIBaseUnity>();

	public void RegistUIBaseUnity(UIBaseUnity obj) {
		if(uiObjectDic.ContainsKey(obj.UIName))
			uiObjectDic[obj.UIName] = obj;
		else
			uiObjectDic.Add(obj.UIName,obj);
	}

	public void GetViewObject(string name, ResourceCallback callback) {
//		if(uiObjectDic.ContainsKey(name)) {	
//			return uiObjectDic[name];
//		}
		CreatObject(name, callback);
	}

	public void GetBattleMap (string name, ResourceCallback callback) {
		CreatNoUIObject (name,callback);
	}

	void CreatNoUIObject (string name,ResourceCallback callback) {

		LoadAsset.Instance.LoadAssetFromResources (name, ResourceEuum.Prefab, o=>{
			GameObject go = GameObject.Instantiate (o) as GameObject;
			UIBaseUnity goScript = go.GetComponent<UIBaseUnity>();
			callback(goScript);
		});

//		uiObjectDic.Add(name,goScript);
	}

	void CreatObject(string name,ResourceCallback callback) {	
		LoadAsset.Instance.LoadAssetFromResources (name, ResourceEuum.Prefab, o => {
			GameObject sourceObject = o as GameObject;
			GameObject go = NGUITools.AddChild (centerPanel, sourceObject);
			UIBaseUnity goScript = go.GetComponent<UIBaseUnity> ();
//			uiObjectDic.Add(name,goScript);
			callback (goScript);
		});

	}

	public void DestoryUI(UIBaseUnity ui) {
		RemoveUI(ui.name);
		GameObject.Destroy(ui.gameObject);
	}

	void RemoveUI(string uiName)
	{
		if(uiObjectDic.ContainsKey(uiName))
			uiObjectDic.Remove(uiName);
	}

	//-----------------------------------------------------------------------------------------------------------------------
	// new 
	//-----------------------------------------------------------------------------------------------------------------------

	private IUIComponent temp = null;
	
	private Dictionary<string,IUIComponent> UIComponentDic = new Dictionary<string, IUIComponent>();
	
	private static Vector3 hidePos = new Vector3(0f,10000f,10000f);
	public static Vector3 HidePos {
		get {
			return hidePos;
		}
	}
	
	public void AddComponent(IUIComponent component) {
		if (component == null) {
			return;	
		}

		UIInsConfig config = component.uiConfig;
		string name = config.uiName;
		
		if (UIComponentDic.TryGetValue (name, out temp)) {
			UIComponentDic [name] = component;	
			temp = null;
		}
		else {
			UIComponentDic.Add(name,component);
		}
	}
	
	public IUIComponent GetComponent(string name) {
		if (UIComponentDic.ContainsKey (name)) {
			return UIComponentDic [name];	
		}
		else {
			return null;
		}
	}
	
	public void RemoveComponent(string name) {
		if (!UIComponentDic.ContainsKey (name)) {
			return;
		}	
		UIComponentDic.Remove (name);
	}
	
	public void DeleteComponent(string name) {
		if (UIComponentDic.TryGetValue (name,out temp)) {
			temp.DestoryUI ();
			UIComponentDic.Remove(name);
			temp = null;
		}
	}

	public void CleartComponent () {
		List<ConcreteComponent> cclist = new List<ConcreteComponent> ();
		List<string> ccID = new List<string> ();
		System.Type ty = typeof(MsgWindowLogic);
		System.Type ty1 = typeof(MaskController);
		System.Type ty2 = typeof(NoviceMsgWindowLogic);
		foreach (var item in UIComponentDic) {
			string key = item.Key;
			ConcreteComponent cc = item.Value as ConcreteComponent;
			System.Type tempType = cc.GetType();

			if(tempType == ty || tempType == ty1 || tempType == ty2) {
				continue;
			}

			ccID.Add(key);
			cclist.Add(cc);
		}
		for (int i = 0; i < ccID.Count; i++) {
			UIComponentDic.Remove(ccID[i]);
		}
		for (int i = cclist.Count - 1; i >= 0; i--) {
//			Debug.LogError("CleartComponent : " + cclist[i]);
			cclist[i].DestoryUI();
		}
		cclist.Clear ();
	}

	public void TogglePopUpWindow(bool show){
		popUpBg.SetActive(show);
	}
}