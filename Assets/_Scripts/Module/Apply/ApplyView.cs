using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ApplyView : ViewBase{
	private SortRule curSortRule;
	private FriendInfo curPickedFriend;
	private DragPanel dragPanel;
	private List<FriendInfo> friendOutDataList = new List<FriendInfo>();
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);

		curSortRule = SortUnitTool.GetSortRule (SortRuleByUI.ApplyView);//DEFAULT_SORT_RULE;
		CreateDragView();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();

		SortUnitByCurRule();
		RefreshCounter();
	}
	
	public override void HideUI(){
		base.HideUI();
//		dragPanel.DestoryUI();

		RmvCmdListener();
	}

	protected override void ToggleAnimation (bool isShow)
	{
		if (isShow) {
			//			Debug.Log("Show Module!: [[[---" + config.moduleName + "---]]]pos: " + config.localPosition.x + " " + config.localPosition.y);
			gameObject.SetActive(true);
			transform.localPosition = new Vector3(config.localPosition.x, config.localPosition.y, 0);

//			transform.localPosition = new Vector3(-1000, -478, 0);
//			iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
			//			iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
		}else{
			//			Debug.Log("Hide Module!: [[[---" + config.moduleName + "---]]]");
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
			//			iTween.MoveTo(gameObject, iTween.Hash("x", -1000, "time", 0.4f, "islocal", true,"oncomplete","AnimationComplete","oncompletetarget",gameObject));
		}

	}

	private void CreateDragView(){
		friendOutDataList = DataCenter.Instance.FriendData.FriendOut;
		dragPanel = new DragPanel("ApplyDragPanel","Prefabs/UI/UnitItem/FriendUnitPrefab",typeof(FriendUnitItem),transform);
		dragPanel.SetData<FriendInfo> (friendOutDataList, ClickItem as DataListener);
	}

	private void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		string title = TextCenter.GetText("ApplyCounterTitle");
		int current = DataCenter.Instance.FriendData.FriendOut.Count;
		int max = 0;
		countArgs.Add("title", title);
		countArgs.Add("current", current);
		countArgs.Add("max", max);
		countArgs.Add("posy", -772);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	private void ClickItem(object data){
		FriendUnitItem item = data as FriendUnitItem;
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		curPickedFriend = item.FriendInfo;
//		MsgCenter.Instance.Invoke(CommandEnum.ViewApplyInfo, curPickedFriend);
//		ModuleManager.Instance.ShowModule (ModuleEnum.ApplyMessageModule, "data", curPickedFriend,"title",TextCenter.GetText ("DeleteApply"),"content",TextCenter.GetText ("ConfirmDelete"));
	}

	private void SortUnitByCurRule(){
		SortUnitTool.SortByTargetRule(curSortRule, friendOutDataList);
		SortUnitTool.StoreSortRule (curSortRule, SortRuleByUI.ApplyView);

		dragPanel.SetData<FriendInfo> (friendOutDataList);
	}

	private void ReceiveSortInfo(object msg){
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}
	
	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}
	
}

