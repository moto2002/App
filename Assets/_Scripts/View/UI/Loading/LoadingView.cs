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
	private UILabel tapLogin;

	private bool initComplete = false;

    public override void Init ( UIInsConfig config, IUICallback origin ) {
        base.Init (config, origin);
        InitUI();
    }
    
    public override void ShowUI () {
//		GameDataStore.Instance.StoreData (GameDataStore.UUID, "");
//		GameDataStore.Instance.StoreData (GameDataStore.USER_ID, "");

		base.ShowUI ();
		#if UNITY_ANDROID
//		Debug.Log ("Umeng.Start('android')...");
		string channelId = "android";
		Umeng.GA.StartWithAppKeyAndChannelId ("5374a17156240b3916013ee8", channelId);
		//		Umeng.GA.Bonus.
		#elif UNITY_IPHONE
		Debug.Log ("Umeng.Start('ios')...");
		string channelId = "ios";
		Umeng.GA.StartWithAppKeyAndChannelId ("539a56ce56240b8c1f074094", channelId);
		#endif
		
		#if !UNITY_EDITOR
		Debug.Log("device info: " + SystemInfo.deviceUniqueIdentifier);
		//		Debug.Log("GetDeviceInfo: " + Umeng.GA.GetDeviceInfo());
		#endif

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
		tapLogin = FindChild ("ClickLabel").GetComponent<UILabel>();

		tapLogin.enabled = false;
    }

	private void CouldLogin(){
		Debug.Log ("load complete, could login");

		ResourceManager.Instance.Init (o => {
			EffectManager em = EffectManager.Instance;
			ConfigDragPanel dragPanelConfig = new ConfigDragPanel();
			
			TextCenter.Instance.Init (o1=>{
				
				AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_home);
				ModelManager.Instance.Init();
				
				initComplete = true;
				//				Debug.Log("init complete: " + initComplete);

				UIEventListener.Get(this.gameObject).onClick = ClickToLogin;
				tapLogin.enabled = true;
			});
		});

//		if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage != NoviceGuideStage.NONE) {
//			NoviceMsgWindowLogic guideWindow = CreatComponent<NoviceMsgWindowLogic>(UIConfig.noviceGuideWindowName);
//			guideWindow.CreatUI();
//		}
		tapLogin.text = 
#if LANGUAGE_CN
	ServerConfig.touchToLogin;
#elif LANGUAGE_EN
	"TAP SCREEN TO START";
#else
	"TAP SCREEN TO START";
#endif
		//TextCenter.GetText("Text_TapToLogin");

	}

	protected T CreatComponent<T>(string name) where T : ConcreteComponent {
		T component = ViewManager.Instance.GetComponent (name) as T;
		if (component == null) {
			component = System.Activator.CreateInstance(typeof(T), name) as T;
		}
		LogHelper.Log ("component: " + component);
		return component;
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
		Debug.Log("click to login: " + initComplete);
		if(initComplete)
			Login();
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
		UIManager.Instance.ChangeScene (SceneEnum.Preface);
    }

//	private void checkResourceUpdate(){
//		ResourceUpdate rs = GetComponent<ResourceUpdate> ();
//
//
//	}
}
