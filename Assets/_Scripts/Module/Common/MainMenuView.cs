using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class MainMenuView : ViewBase{
	private UITexture leaderAvatarTex;
	private UISprite leaderAvatarSpr;

	private UILabel labelTips;
	private int times;

	private Dictionary<GameObject,ModuleEnum> buttonInfo = new Dictionary<GameObject, ModuleEnum> ();
	public override void Init (UIConfigItem config, Dictionary<string, object> data = null) {
		base.Init (config, data);
		InitButton ();
		UpdateLeaderAvatar(null);
		AddListener();

		times = 2;
	}
	
	public override void DestoryUI () {
		RemoveListener ();
		base.DestoryUI ();
	}

    private void AddListener(){
//		Debug.LogError("main menu addlistener");
		MsgCenter.Instance.AddListener(CommandEnum.ModifiedParty, UpdateLeaderAvatar);
    }

    private void RemoveListener(){
//		Debug.LogError("main menu removeListener");
		MsgCenter.Instance.RemoveListener(CommandEnum.ModifiedParty, UpdateLeaderAvatar);
    }
	
	private void InitButton() {
		labelTips = FindChild< UILabel >("Label_Tips");

		GameObject go = FindChild ("Btn_Friends");
		FindChild ("Btn_Friends/Label").GetComponent<UILabel> ().text = TextCenter.GetText("MainMenu_Friends");
		buttonInfo.Add (go, ModuleEnum.FriendMainModule);

		go = FindChild ("Btn_Home");
		FindChild ("Btn_Home/Label").GetComponent<UILabel> ().text = TextCenter.GetText("MainMenu_Home");
		buttonInfo.Add (go, ModuleEnum.HomeModule);

		go = FindChild ("Btn_Scratch");
		FindChild ("Btn_Scratch/Label").GetComponent<UILabel> ().text = TextCenter.GetText("MainMenu_Scratch");
		buttonInfo.Add (go, ModuleEnum.ScratchModule);

		go = FindChild ("Btn_Shop");
		FindChild ("Btn_Shop/Label").GetComponent<UILabel> ().text = TextCenter.GetText("MainMenu_Shop");
		buttonInfo.Add (go, ModuleEnum.ShopModule);

		go = FindChild ("Btn_Others");
		FindChild ("Btn_Others/Label").GetComponent<UILabel> ().text = TextCenter.GetText("MainMenu_Others");
		buttonInfo.Add (go, ModuleEnum.OthersModule);

		go = FindChild ("Btn_Units");
		FindChild ("Btn_Units/Label").GetComponent<UILabel> ().text = TextCenter.GetText("MainMenu_Units");
		buttonInfo.Add (go, ModuleEnum.UnitsMainModule);

		foreach (var item in buttonInfo.Keys) {
			UIEventListenerCustom.Get(item).onClick = ClickMenuBtn;
		}

		leaderAvatarTex = transform.FindChild("Btn_Units/Texture_Avatar_Leader").GetComponent<UITexture>();
		leaderAvatarSpr = transform.FindChild("Btn_Units/Sprite_Border").GetComponent<UISprite>();
	}

	private void ClickMenuBtn( GameObject btn ) {
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		ModuleEnum targetScene = buttonInfo [ btn ];
		ModuleManager.Instance.ShowModule(targetScene);

		Umeng.GA.Event ("BottomMenu",targetScene.ToString ());

	}

	private void SetMenuValid(object args){
        bool valid = (bool)args;
        foreach (var item in buttonInfo.Keys) {
          UIButtonScale btnScale = item.GetComponent<UIButtonScale>() ;
			if(btnScale == null)
				continue;
            btnScale.enabled = valid;
            if(valid)  
				UIEventListenerCustom.Get(item).onClick += ClickMenuBtn; 
            else 
				UIEventListenerCustom.Get(item).onClick -= ClickMenuBtn;
        }
    }

	private UnitInfo leaderUnitInfo;
	private void UpdateLeaderAvatar(object msg){
		UserUnit newestLeaderUnit = DataCenter.Instance.UnitData.PartyInfo.CurrentParty.GetUserUnit()[ 0 ];
//		Debug.LogError ((newestLeaderUnit == null) + "  leaderUnitInfo == null : " + (leaderUnitInfo == null));
		if(newestLeaderUnit == null){
			leaderAvatarTex.mainTexture = null;
			leaderUnitInfo = null;
//			Debug.LogError("newestLeaderUnit is NULL, return...");
			return;
		}

		if(leaderUnitInfo == null){
			//first step in
//			Debug.Log("UpdateLeaderAvatar(), Leader data is FRIST assigned.");
			ResourceManager.Instance.GetAvatar(UnitAssetType.Profile,newestLeaderUnit.unitId, o=>{
				leaderAvatarTex.mainTexture = o as Texture2D;

				leaderUnitInfo = newestLeaderUnit.UnitInfo;
				SetUVByConfig(leaderUnitInfo.showPos);

				string sprName = "";
				switch ( leaderUnitInfo.type ){
				case EUnitType.UFIRE : 
					sprName = "border_main_fire";
					break;
				case EUnitType.ULIGHT : 
					sprName = "border_main_light";
					break;
				case EUnitType.UDARK : 
					sprName = "border_main_dark";
					break;
				case EUnitType.UWATER : 
					sprName = "border_main_water";
					break;
				case EUnitType.UNONE : 
					sprName = "border_main_none";
					break;
				case EUnitType.UWIND : 
					sprName = "border_main_wind";
					break;
				default:
					break;
				}
				
				leaderAvatarSpr.spriteName = sprName;
			} );

			return;
		}

	 	if(leaderUnitInfo.id != newestLeaderUnit.UnitInfo.id){
//			Debug.LogError("else if  leaderUnitInfo.ID  : " + leaderUnitInfo.ID + " newestLeaderUnit.UnitInfo.ID : " +newestLeaderUnit.UnitInfo.ID);
			//changed
//			Debug.Log("UpdateLeaderAvatar(), Leader data CHANGED." + newestLeaderUnit.UnitInfo.ID + " leaderUnitInfo: " + leaderUnitInfo.ID);
			ResourceManager.Instance.GetAvatar(UnitAssetType.Profile,newestLeaderUnit.unitId, o =>{
				leaderAvatarTex.mainTexture = o as Texture2D;

				leaderUnitInfo = newestLeaderUnit.UnitInfo;
				SetUVByConfig(leaderUnitInfo.showPos);
				string sprName = "";
				switch ( leaderUnitInfo.type ){
				case EUnitType.UFIRE : 
					sprName = "border_main_fire";
					break;
				case EUnitType.ULIGHT : 
					sprName = "border_main_light";
					break;
				case EUnitType.UDARK : 
					sprName = "border_main_dark";
					break;
				case EUnitType.UWATER : 
					sprName = "border_main_water";
					break;
				case EUnitType.UNONE : 
					sprName = "border_main_none";
					break;
				case EUnitType.UWIND : 
					sprName = "border_main_wind";
					break;
				default:
					break;
				}

				leaderAvatarSpr.spriteName = sprName;
			});
 

		}
		else{
			//not changed
//			Debug.LogError("else   leaderUnitInfo.ID  : " + leaderUnitInfo.ID + " newestLeaderUnit.UnitInfo.ID : " +newestLeaderUnit.UnitInfo.ID);
//			Debug.Log("UpdateLeaderAvatar(), Leader data NOT CHANGED.");
		}
	}

	private void SetUVByConfig(bbproto.UVPosition pos){
		//Debug.Log("SetUVByConfig()...");
		//TODO read from uv config file
		float x = pos.x;//0.21f;
		float y = pos.y;//0.35f;
		float w = pos.w;//0.52f;
		float h = pos.h;//0.55f;
		leaderAvatarTex.uvRect = new Rect(x, y, w, h);
	}

	private void EnableDisplay(object args){
		this.gameObject.SetActive((bool)args);
	}
	
	public void ShowTips(){
		times--;
		if (times <= 0) {
			if (DataCenter.Instance.UserData.LoginInfo != null && DataCenter.Instance.UserData.LoginInfo.rank != null) {
				if (DataCenter.Instance.UserData.LoginInfo.rank < 5) {
					labelTips.text = TextCenter.GetText ("Tips_A_" + Utility.MathHelper.RandomToInt (1, 13));
				} else if (DataCenter.Instance.UserData.LoginInfo.rank < 10) {
					labelTips.text = TextCenter.GetText ("Tips_B_" + Utility.MathHelper.RandomToInt (1, 10));
				} else if (DataCenter.Instance.UserData.LoginInfo.rank < 20) {
					labelTips.text = TextCenter.GetText ("Tips_C_" + Utility.MathHelper.RandomToInt (1, 18));
				} else if (DataCenter.Instance.UserData.LoginInfo.rank < 30) {
					labelTips.text = TextCenter.GetText ("Tips_D_" + Utility.MathHelper.RandomToInt (1, 18));
				} else {
					labelTips.text = TextCenter.GetText ("Tips_E_" + Utility.MathHelper.RandomToInt (1, 24));
				}	
			} else {
				labelTips.text = TextCenter.GetText ("Tips_A_" + Utility.MathHelper.RandomToInt (1, 13));
			}
			times = 2;
		}
		
		labelTips.GetComponent<TweenPosition> ().enabled = true;
		labelTips.GetComponent<TweenPosition> ().ResetToBeginning ();
	}
	
}
