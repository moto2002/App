using UnityEngine;
using System.Collections;

public class MainMenuController : ConcreteComponent, IUICallback {
	public MainMenuController (string uiName) : base(uiName) {}

	public override void ShowUI () {
		base.ShowUI ();
	}

	public override void HideUI () {
		base.HideUI ();
	}

	public void CallbackView (object data){
		try {
			SceneEnum se = (SceneEnum)data;
			if(se == SceneEnum.QuestSelect){
				if(CheckUnitCountLimit()){
					//msg box show 
					MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetUnitExpansionMsgParams());
					return;
				}
			}
			UIManager.Instance.ChangeScene(se);
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}

	bool CheckUnitCountLimit(){
//		Debug.LogError("Current MyUnitList count is " + DataCenter.Instance.MyUnitList.Count);
//		Debug.LogError("Current MyUnit Max is " + DataCenter.Instance.UserInfo.UnitMax);
		if(DataCenter.Instance.UserUnitList.GetAllMyUnit().Count > DataCenter.Instance.UserInfo.UnitMax){
			Debug.LogError("MyUnitList's count > MyMax!!! Refuse to scene of Quest...");
			return true;
		}
		else
			return false;
	}

	MsgWindowParams GetUnitExpansionMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.GetText("UnitOverflow");
		msgParams.contentText = TextCenter.GetText("UnitOverflowText",
		                                                          DataCenter.Instance.UserUnitList.GetAllMyUnit().Count,
		                                                          DataCenter.Instance.UserInfo.UnitMax);
		msgParams.btnParams = new BtnParam[2]{ new BtnParam(), new BtnParam()};
		msgParams.btnParams[ 0 ].text = TextCenter.GetText("DoUnitExpansion");
		msgParams.btnParams[ 0 ].callback = CallBackScratchScene;
		return msgParams;
	}

	void CallBackScratchScene(object args){
		UIManager.Instance.ChangeScene(SceneEnum.Shop);
	}
}
