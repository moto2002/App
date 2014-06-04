// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;


public class LoadingView : UIComponentUnity {
    public override void Init ( UIInsConfig config, IUICallback origin ) {
        base.Init (config, origin);
        InitUI();
    }
    
    public override void ShowUI () {
//		GameDataStore.Instance.StoreData (GameDataStore.UUID, "");
//		GameDataStore.Instance.StoreData (GameDataStore.USER_ID, "");
        base.ShowUI ();
		Umeng.GA.StartWithAppKeyAndChannelId ("5374a17156240b3916013ee8","android");
		LogHelper.Log("device info: " + SystemInfo.deviceUniqueIdentifier);
//		NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces ();
//		Debug.LogError ("nis.Length : " + nis.Length);
//		if (nis.Length > 0) {
//			Debug.LogError (nis [0].GetPhysicalAddress ().ToString());
//		}

//		Debug.Log ("enum toString(): " + NoviceGuideStepEntityID.Loading.ToString());

    }
    
    public override void HideUI () {
        base.HideUI ();

    }

    private void InitUI (){
        UIEventListener.Get(this.gameObject).onClick = ClickToLogin;
    }

    private bool CheckIfFirstLogin(){
        bool ret = false;
        uint userId = GameDataStore.Instance.GetUInt(GameDataStore.USER_ID);
        string uuid = GameDataStore.Instance.GetData(GameDataStore.UUID);
        if (userId == 0 && uuid.Length == 0) {
            return true;
        }
        return ret;
    }

    private void ClickToLogin(GameObject btn){
//		if (checkResourceUpdate ()) {
//		Debug.LogError("click to login");
			Login();		
//		}
//		UIEventListener.Get(this.gameObject).onClick = null;
//		GameObject.Find ("LoadProgress").GetComponent<ResourceUpdate>().StartDownload();
    }

	private void Login(){
		if (CheckIfFirstLogin()){
			LogHelper.Log("firstLogin");
			SelectRoleFirst();
		}
		else {
			LogHelper.Log("login directly");
			LoginDirectly();

		}
	}
	
	private void LoginDirectly(){
		Umeng.GA.Event ("Login");
		LoadingLogic loadingLogic = origin as LoadingLogic;
        loadingLogic.StartLogin();
    }

    private void SelectRoleFirst(){
//		UIManager.Instance.ChangeScene(SceneEnum.Start);
//		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.Preface;
//		NoviceGuideStepEntityManager.Instance ().StartStep ();
        UIManager.Instance.ChangeScene(SceneEnum.SelectRole);
    }

//	private void checkResourceUpdate(){
//		ResourceUpdate rs = GetComponent<ResourceUpdate> ();
//
//
//	}
}