using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendListView : UIComponentUnity{
	private SortRule curSortRule;
	private DragPanel dragPanel;
	private TFriendInfo curPickedFriend;
	private UIButton updateBtn;
	private List<TFriendInfo> friendDataList = new List<TFriendInfo>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
		CreateDragView();
		SortUnitByCurRule();
		RefreshCounter();
		AddCmdListener();
		ShowUIAnimation();
	}

	public override void HideUI(){
		base.HideUI();
		dragPanel.DestoryUI();
		RmvCmdListener();
	}
		
	void EnableUpdateButton(object args){
		updateBtn.gameObject.SetActive(true);
		UIEventListener.Get(updateBtn.gameObject).onClick += ClickUpdateBtn;
	}

	void ClickRefuseButton(GameObject args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefuseApplyButtonClick", null);
		ExcuteCallback(cbdArgs);
	}

	void InitUIElement(){
		updateBtn = FindChild<UIButton>("Button_Update");
		UIEventListener.Get(updateBtn.gameObject).onClick = ClickUpdateBtn;
		curSortRule = SortUnitTool.GetSortRule(SortRuleByUI.FriendListView);
	}

	void CreateDragView(){
		LogHelper.Log("FriendListView.CreateDragView(), receive call from logic, to create ui...");
		friendDataList = DataCenter.Instance.FriendList.Friend;
		dragPanel = new DragPanel("FriendDragPanel", FriendUnitItem.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(friendDataList.Count);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.FriendListDragPanelArgs, transform);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			FriendUnitItem fuv = FriendUnitItem.Inject(dragPanel.ScrollItem[ i ]);
			fuv.Init(friendDataList[ i ]);
			fuv.callback = ClickItem;
		}
	}

	void ShowUIAnimation(){
		transform.localPosition = new Vector3(-1000, -480, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
	}

	void ClickItem(FriendUnitItem item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		curPickedFriend = item.FriendInfo;
		MsgCenter.Instance.Invoke(CommandEnum.FriendBriefInfoShow, curPickedFriend);
	}


	void ClickUpdateBtn(GameObject button){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetMsgWindowParams());
	}

	MsgWindowParams GetMsgWindowParams(){
		MsgWindowParams msgWindowParam = new MsgWindowParams();
		msgWindowParam.titleText = TextCenter.GetText("RefreshFriend");
		msgWindowParam.contentText = TextCenter.GetText("ConfirmRefreshFriend");
		msgWindowParam.btnParams = new BtnParam[ 2 ]{new BtnParam(), new BtnParam()};
		msgWindowParam.btnParams[ 0 ].callback = CallBackRefreshFriend;
		return msgWindowParam;
	}

	void CallBackRefreshFriend(object args){
		MsgCenter.Instance.Invoke(CommandEnum.EnsureUpdateFriend);
	}

	void UpdateFriendList(object args){
		GetFriendList.SendRequest(OnGetFriendList);
	}

	void OnGetFriendList(object data){
		if (data == null)
			return;
		bbproto.RspGetFriend rsp = data as bbproto.RspGetFriend;
		
		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			LogHelper.Log("RspGetFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}
		
		bbproto.FriendList inst = rsp.friends;
		DataCenter.Instance.SetFriendList(inst);
		HideUI();
		ShowUI();
	}

	void DeleteFriendPicked(object msg){
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetDeleteMsgParams());
	}

	MsgWindowParams GetDeleteMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.GetText("DeleteNoteTitle");
		msgParams.contentText = TextCenter.GetText("DeleteNoteContent");
		msgParams.btnParams = new BtnParam[2]{ new BtnParam(), new BtnParam()};
		msgParams.btnParams[ 0 ].callback = CallBackDeleteFriend;
		return msgParams;
	}

	void CallBackDeleteFriend(object args){
		DelFriend.SendRequest(OnDelFriend, curPickedFriend.UserId);
	}

	void OnDelFriend(object data){
		if (data == null)
			return;
		
		Debug.Log("TFriendList.OnDelFriend() begin");
		LogHelper.Log(data);
		bbproto.RspDelFriend rsp = data as bbproto.RspDelFriend;
		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rsp.header.code+", error:"+rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}
		
		bbproto.FriendList inst = rsp.friends;
		LogHelper.LogError("OnRspDelFriend friends {0}", rsp.friends);
		
		DataCenter.Instance.SetFriendList(inst);
		
		HideUI();
		ShowUI();
	}

	void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		string title = TextCenter.GetText("FriendCounterTitle");
		int current = DataCenter.Instance.FriendCount;
		int max = DataCenter.Instance.UserInfo.FriendMax;
		countArgs.Add("title", title);
		countArgs.Add("current", current);
		countArgs.Add("max", max);
		countArgs.Add("posy", -774);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	private void ReceiveSortInfo(object msg){
		//curSortRule = SortUnitTool.GetNextRule(curSortRule);
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private void SortUnitByCurRule(){

		SortUnitTool.SortByTargetRule(curSortRule, friendDataList);
		SortUnitTool.StoreSortRule (curSortRule, SortRuleByUI.FriendListView);
		
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			FriendUnitItem fuv = dragPanel.ScrollItem[ i ].GetComponent<FriendUnitItem>();
			fuv.UserUnit = friendDataList[ i ].UserUnit;
			fuv.CurrentSortRule = curSortRule;
		}
	}

	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.EnsureDeleteFriend, DeleteFriendPicked);     
		MsgCenter.Instance.AddListener(CommandEnum.EnsureUpdateFriend, UpdateFriendList);
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}
	
	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureDeleteFriend, DeleteFriendPicked);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureUpdateFriend, UpdateFriendList);
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}
}

