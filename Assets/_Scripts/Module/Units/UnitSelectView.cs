﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class UnitSelectView : ViewBase {

	DragPanel dragPanel;

	private List<UserUnit> avalibleList;

	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);
		avalibleList = new List<UserUnit>();
		dragPanel = new DragPanel ("UnitSelectDragPanel","Prefabs/UI/Units/UnitSelectItem",typeof(UnitSelectItemView),transform);
	}

//	public override void CallbackView (params object[] args)
//	{
//		switch (args [0].ToString ()) {
//			case "level_up":
//				break;
//
//		}
//	}

	public override void ShowUI ()
	{

		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
		if (viewData != null) {
			if(viewData.ContainsKey("type") && viewData["type"] == "level_up"){
				ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"level_up");
				ModuleManager.Instance.ShowModule (ModuleEnum.UnitSortModule,"from","level_up");
				GetAvalibleList();
				if(avalibleList.Count <= 0){
					TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("LevelUp_NeedTitle"),TextCenter.GetText("LevelUp_NoMaterial"),TextCenter.GetText("OK"));
					ModuleManager.SendMessage (ModuleEnum.SceneInfoBarModule, "no_back", ModuleEnum.UnitSelectModule);
					ModuleManager.Instance.HideModule(ModuleEnum.UnitSelectModule);
					ModuleManager.Instance.ShowModule(ModuleEnum.UnitLevelupAndEvolveModule);
					return;
				}else{
					SortRule sui = SortUnitTool.GetSortRule(SortRuleByUI.UnitLevelupAndEvolveView);
					SortUnitByCurRule(sui);
					dragPanel.SetData<UserUnit>(avalibleList, SelectItem as DataListener);
				}

			}
		}
		base.ShowUI ();
	}

	private void GetAvalibleList(){
		avalibleList.Clear ();
		List<UserUnit> list = viewData ["list"] as List<UserUnit>;
		foreach (var item in DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ()) {
			if(!DataCenter.Instance.UnitData.PartyInfo.UnitIsInCurrentParty(item) && item.isFavorite == 0 && ! list.Contains(item) && item != viewData["base_info"]){
				avalibleList.Add(item);
			}
		}
	}

	private void SelectItem(object data){
		if (DataCenter.Instance.UnitData.PartyInfo.UnitIsInParty(data as UserUnit) > 0) {
			TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("LevelUp_PartyItem_Title"), TextCenter.GetText ("LevelUp_PartyItem_Content"), TextCenter.GetText ("OK"), TextCenter.GetText ("Cancel"),o=>{
				ModuleManager.SendMessage (ModuleEnum.SceneInfoBarModule, "no_back", ModuleEnum.UnitSelectModule);
				ModuleManager.Instance.HideModule (ModuleEnum.UnitSelectModule);
				ModuleManager.Instance.ShowModule (ModuleEnum.UnitLevelupAndEvolveModule, "unit_info", data as UserUnit, "unit_index" ,viewData["index"]);
			});	
		}else{
			ModuleManager.SendMessage (ModuleEnum.SceneInfoBarModule, "no_back", ModuleEnum.UnitSelectModule);
			ModuleManager.Instance.HideModule (ModuleEnum.UnitSelectModule);
			ModuleManager.Instance.ShowModule (ModuleEnum.UnitLevelupAndEvolveModule, "unit_info", data as UserUnit, "unit_index" ,viewData["index"]);
		}


	}

	public override void HideUI ()
	{
		base.HideUI ();
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
	}

	private void ReceiveSortInfo(object msg){
		SortRule curSortRule = (SortRule)msg;
		SortUnitByCurRule (curSortRule);
		dragPanel.SetData<UserUnit> (avalibleList);
	}

	private void SortUnitByCurRule(SortRule curSortRule){
		SortUnitTool.SortByTargetRule(curSortRule, avalibleList);
		SortUnitTool.StoreSortRule (curSortRule, SortRuleByUI.UnitLevelupAndEvolveView);
	}
}
