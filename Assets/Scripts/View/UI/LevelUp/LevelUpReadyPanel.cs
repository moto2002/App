using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;
public class LevelUpReadyPanel: UIComponentUnity {
	UILabel hpLabel;
	UILabel atkLabel;
	UILabel expNeedLabel;
	UILabel expCurGotLabel;
	UILabel cionNeedLabel;

	GameObject curFocusTab;
	GameObject baseTab;
	GameObject friendTab;
	GameObject materialTab;

	UIImageButton levelUpButton;
	List<GameObject> Tabs = new List<GameObject>();
	Dictionary<int,GameObject> materialPoolDic = new Dictionary<int,GameObject>();

	UITexture baseCollectorTex;
	UITexture friendCollectorTex;
	List<UITexture> materialCollectorTex = new List<UITexture>();
	
	UnitItemInfo baseUnitInfo;
	UnitItemInfo[] unitItemInfo = new UnitItemInfo[4];
	TUserUnit friendUnitInfo;
//	List<TUserUnit> materialUnitInfo = new List<TUserUnit>();

	public override void Init(UIInsConfig config, IUICallback origin){
		InitUI();
		base.Init(config, origin);
	}

	public override void ShowUI(){

		base.ShowUI();
		FoucsOnTab( Tabs[0] );
		AddListener();
		levelUpButton.isEnabled = false;
		levelUpButton.gameObject.SetActive (false);
		ClearTexture();
		ClearLabel();
		ClearData();
	}

	public override void HideUI(){
		base.HideUI();
		RemoveListener();
	}

	void InitUI(){
		InitTab();
		InitButton();
		FindInfoPanelLabel();
		FindCollectorTexture();
	}


	void UpdateBaseInfoView( UnitItemInfo itemInfo){

		UITexture tex = Tabs [0].GetComponentInChildren<UITexture> ();
//		Debug.LogError ("tex : " + tex + " itemInfo : " + itemInfo);
		if (itemInfo == null) {
			tex.mainTexture = null;
			MsgCenter.Instance.Invoke(CommandEnum.BaseAlreadySelect, null);
		} else {
			TUserUnit tuu = itemInfo.userUnitItem;
			TUnitInfo tu = GlobalData.unitInfo[ tuu.UnitID ];
			tex.mainTexture = tu.GetAsset(UnitAssetType.Avatar);			
			int hp = GlobalData.Instance.GetUnitValue(tu.HPType,tuu.Level);
			hpLabel.text = hp.ToString();			
			int atk =  GlobalData.Instance.GetUnitValue(tu.AttackType, tuu.Level);
			atkLabel.text = atk.ToString();			
			expNeedLabel.text = "16918";			
			expCurGotLabel.text = "0";			
			cionNeedLabel.text = "0";
//			Debug.LogError("tex : "+ tex + " maintexture : " + tex.mainTexture);
			MsgCenter.Instance.Invoke(CommandEnum.BaseAlreadySelect, itemInfo);
			FoucsOnTab(Tabs[2]);

		}
	}

	void ClearLabel(){
		hpLabel.text = UIConfig.emptyLabelTextFormat;
		atkLabel.text = UIConfig.emptyLabelTextFormat;
		expNeedLabel.text = UIConfig.emptyLabelTextFormat;
		expCurGotLabel.text = UIConfig.emptyLabelTextFormat;
		cionNeedLabel.text = UIConfig.emptyLabelTextFormat;
	}
 
	void UpdateFriendInfo(TUserUnit unitInfo){
		UITexture tex = Tabs [1].GetComponentInChildren<UITexture> ();
		if (friendUnitInfo == null) {
			friendUnitInfo = unitInfo;
			tex.mainTexture = GlobalData.unitInfo [unitInfo.UnitID].GetAsset (UnitAssetType.Avatar);
		} 
		else if(friendUnitInfo == unitInfo) {
			friendUnitInfo = null;
			tex.mainTexture = null;	
		}
	}

	void FindInfoPanelLabel(){
		hpLabel = FindChild< UILabel >("InfoPanel/Label_Vaule/0");
		atkLabel = FindChild< UILabel >("InfoPanel/Label_Vaule/1");
		expNeedLabel = FindChild< UILabel >( "InfoPanel/Label_Vaule/2");
		expCurGotLabel = FindChild< UILabel >( "InfoPanel/Label_Vaule/3");
		cionNeedLabel = FindChild< UILabel >( "InfoPanel/Label_Vaule/4");
	}
	
	void FindCollectorTexture(){
		baseCollectorTex = FindChild<UITexture>("Tab_Base/role");
		friendCollectorTex = FindChild<UITexture>("Tab_Friend/role");

		for( int i = 1; i <= 4; i++ ){
			string path = string.Format( "Tab_Material/Material{0}/Avatar", i );
			//Debug.Log("Ready Panel,FindAvatarTexture, Path is " + path);
			UITexture tex = FindChild< UITexture >( path );
			materialCollectorTex.Add( tex );
		}
	}

	void ClearTexture(){
		baseCollectorTex.mainTexture = null;
		friendCollectorTex.mainTexture = null;
		foreach (var item in materialCollectorTex)
			item.mainTexture = null;
	}

	void ClearData(){
		baseUnitInfo = null;
	
		friendUnitInfo = null;
		for (int i = 0; i < unitItemInfo.Length; i++) {
			unitItemInfo[i] = null;
		}
	}
	
	void InitTab()	{
		GameObject tab;

		tab = FindChild("Tab_Base");
		Tabs.Add(tab);
		
		tab = FindChild("Tab_Friend");
		Tabs.Add(tab);

		tab = FindChild("Tab_Material");
		Tabs.Add(tab);

		for (int i = 1; i < 5; i++){
			GameObject item = tab.transform.FindChild("Material" + i.ToString()).gameObject;
                        materialPoolDic.Add( i, item);
        }
                
        foreach (var item in Tabs)
                UIEventListener.Get(item.gameObject).onClick = ClickTab;
	}


	void ClickTab(GameObject tab){
		FoucsOnTab(tab);
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
	}

	void CheckCanLevelUp() {
		if(!levelUpButton.gameObject.activeSelf) {
			levelUpButton.gameObject.SetActive(true);
		}

		bool baseBool = baseUnitInfo != null;
		bool materialBool = false;
		foreach (var item in unitItemInfo) {
			if(item != null) {
				materialBool = true;
				break;
			}
		}
		bool firendBool = friendUnitInfo != null;
//		Debug.LogError (baseBool + " -- " + materialBool + " -- " + firendBool);
		if (baseBool && materialBool && firendBool) {

			levelUpButton.isEnabled = true;
//			Debug.LogError(levelUpButton.isEnabled );
		} else {
			levelUpButton.isEnabled = false;
		}
	}

	void FoucsOnTab(GameObject focus){
		if(focus.name == "Tab_Friend") {
			if(!levelUpButton.gameObject.activeSelf)
				levelUpButton.gameObject.SetActive(true);
			CheckCanLevelUp();
		}
		else{ 
			if(levelUpButton.gameObject.activeSelf)
				levelUpButton.gameObject.SetActive(false);
		}


		foreach (var tab in Tabs){
			tab.transform.FindChild("Light_Frame").gameObject.SetActive(false);
			tab.transform.FindChild("Label_Title").GetComponent< UILabel >().color = Color.white;
		}

		//activate focus tab
		curFocusTab = focus.gameObject;
		curFocusTab.transform.FindChild("Light_Frame").gameObject.SetActive(true);
		curFocusTab.transform.FindChild("Label_Title").GetComponent< UILabel >().color = Color.yellow;
		//activate focus tab content= 
//		Debug.LogError ("CommandEnum.PanelFocus : " + focus.name);
		MsgCenter.Instance.Invoke(CommandEnum.PanelFocus, curFocusTab.name );
//		Debug.Log("FoucsOnTab() :  ");
	}
	
	void AddListener(){
		MsgCenter.Instance.AddListener(CommandEnum.PickBaseUnitInfo, PickBaseUnitInfo );
		MsgCenter.Instance.AddListener(CommandEnum.PickFriendUnitInfo, PickFriendUnitInfo );
//		MsgCenter.Instance.AddListener(CommandEnum.TryEnableLevelUp, EnableLevelUp);

	}
	
	void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.PickBaseUnitInfo, PickBaseUnitInfo );
		MsgCenter.Instance.RemoveListener(CommandEnum.PickFriendUnitInfo, PickFriendUnitInfo );
//		MsgCenter.Instance.RemoveListener(CommandEnum.TryEnableLevelUp, EnableLevelUp);
	}
	
//	void EnableLevelUp(object info){
//		Dictionary<string, object> levelUpInfo = PackLevelUpInfo();
//		if( levelUpInfo == null){	
//			levelUpButton.isEnabled = false;
//		}
//		else{
//			levelUpButton.isEnabled = true;
//		}
//	}
	
	void InitButton(){
		levelUpButton = FindChild<UIImageButton>("Button_LevelUp");
		UIEventListener.Get( levelUpButton.gameObject ).onClick = ClickLevelUpButton;
		levelUpButton.isEnabled = false;
	}

	void ClickLevelUpButton(GameObject go){
		List<TUserUnit> temp = PackUserUnitInfo ();
		ExcuteCallback (temp);
//		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);//before
//		MsgCenter.Instance.Invoke(CommandEnum.LevelUp, PackUserUnitInfo());//after
	}
	
	List<TUserUnit> PackUserUnitInfo(){
		List<TUserUnit> pickedUserUnitInfo = new List<TUserUnit>();
		pickedUserUnitInfo.Add (baseUnitInfo.userUnitItem);
		pickedUserUnitInfo.Add (friendUnitInfo);
		//TODO add base unit info .....
//		pickedUserUnitInfo.Add(baseUnitInfo);
		pickedUserUnitInfo.Add(friendUnitInfo);
		foreach (var item in unitItemInfo){
			if(item != null) {
				pickedUserUnitInfo.Add(item.userUnitItem);
			}
		}
		return pickedUserUnitInfo;
	}

	void PickBaseUnitInfo(object info){
		if( curFocusTab.name == "Tab_Base" ){
			UnitItemInfo uui = info as UnitItemInfo;
			if(baseUnitInfo != null && baseUnitInfo != uui) {
				return;
			}
			if(uui.isSelect) {
				baseUnitInfo = uui;
			}
			else{
				baseUnitInfo = null;
			}
			UpdateBaseInfoView( baseUnitInfo );
		}else{
			UnitItemInfo uui = info as UnitItemInfo;

			if(!CancelMaterialClick(uui)) {
				MaterialClick(uui);
			}

			MsgCenter.Instance.Invoke(CommandEnum.ShieldMaterial, unitItemInfo);
		}
	}

	bool CancelMaterialClick(UnitItemInfo uui) {
		for (int i = 0; i < unitItemInfo.Length; i++) {
			if(unitItemInfo[i] == null) {
				continue;
			}
			if(unitItemInfo[i].Equals(uui)) {
				unitItemInfo[i] = null;
				MsgCenter.Instance.Invoke(CommandEnum.MaterialSelect, false);
				UpdateMaterialInfoView(null,i + 1);
				return true;
			}
		}
		return false;
	}

	bool MaterialClick (UnitItemInfo uui) {
		for (int i = 0; i < unitItemInfo.Length; i++) {
			if(unitItemInfo[i] == null) {
				unitItemInfo[i] = uui;
				MsgCenter.Instance.Invoke(CommandEnum.MaterialSelect, true);
				UpdateMaterialInfoView(uui,i + 1);
				return true;
			}
		}
		return false;
	}

	void UpdateMaterialInfoView( UnitItemInfo uui, int index){
//		Debug.LogError (index + "  materialPoolDic : " + materialPoolDic.Count);
		GameObject materialTab = materialPoolDic[index];
		UITexture tex = materialTab.GetComponentInChildren<UITexture>();
		if (uui == null) {
			tex.mainTexture = null;
		} else {
			tex.mainTexture = GlobalData.unitInfo [uui.userUnitItem.UnitID].GetAsset (UnitAssetType.Avatar);
		}
	}

	void PickFriendUnitInfo(object info){
		//if(friendUnitInfo != null)	return;
//		friendUnitInfo = info as TUserUnit;

		TUserUnit tuu =  info as TUserUnit;
		UpdateFriendInfo(tuu);
		CheckCanLevelUp ();
	}

//	void PickMaterialUnitInfo(object info){
//		if( materialUnitInfo.Count == 4)	return;
//		TUserUnit tempInfo = info as TUserUnit;
//		materialUnitInfo.Add(tempInfo);
//		UpdateMaterialInfoView( tempInfo );
//
//	}

//	Dictionary<string, object> PackLevelUpInfo(){
//		//condition : exist base && material && friend
//		if(baseUnitInfo == null)		
//			return null;
//		if(friendUnitInfo == null)	
//			return null;
//		if( materialUnitInfo.Count < 1)
//			return null;
//		Dictionary<string, object> levelUpInfo = new Dictionary<string, object>();
//		levelUpInfo.Add("BaseInfo", baseUnitInfo);
//		levelUpInfo.Add("FriendInfo", friendUnitInfo);
//		levelUpInfo.Add("MaterialInfo",materialUnitInfo);
//
//		return levelUpInfo;
//	}

}

