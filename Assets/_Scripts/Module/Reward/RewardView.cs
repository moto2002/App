using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class RewardView : ViewBase {

	public  List<int> bonusIDs = new List<int>();

	private DragPanel dragPanel;

	private Dictionary<int,List<BonusInfo>> aList = new Dictionary<int, List<BonusInfo>>();

	private int currentContentIndex = 0;

	private GameObject content;

	private GameObject OKBtn;

	private Dictionary<int,GameObject> Nums;

	UILabel tabInfo;


	public override void Init(UIConfigItem config, Dictionary<string, object> data = null) {
		base.Init(config,data);
		InitUI();
	}
	
	public override void ShowUI() {
		base.ShowUI();

		bonusIDs.Clear ();

		RefreshView ();

//		ShowUIAnimation ();

//		GetBonusList.SendRequest (OnRequest);
	}

//	void OnRequest(object data){
//			//		bbproto.BonusInfo bsInfo = 
//		Debug.Log ("purchase success, change to reward. rsp data:"+data);
//		bbproto.RspBonusList rsp = data as bbproto.RspBonusList;
//		if (rsp != null && rsp.bonus != null ) {
//			DataCenter.Instance.UserData.LoginInfo.Bonus = rsp.bonus;
//
//
////			MsgCenter.Instance.Invoke(CommandEnum.GotoRewardMonthCardTab);
////			ModuleManger.Instance.ShowModule (ModuleEnum.Reward);
//		}
//	}

	public override void CallbackView (params object[] args)
	{
		switch (args[0].ToString()) {
		case "take_award":
			bonusIDs.Add ((args[1] as BonusInfo).id);
			aList [currentContentIndex].Remove (args[1] as BonusInfo);
			RefreshView ();
			break;
		default:
				break;
				}

	}

	public override void HideUI() {
		base.HideUI();


//		Debug.Log ("bonusIDs: " + bonusIDs.Count);
		if(bonusIDs.Count > 0)
			BonusController.Instance.AcceptBonus(OnAcceptBonus,bonusIDs);

//		int count = dragPanel.ScrollItem.Count;
//		for (int i = 0; i < count; i++) {
//			GameObject go = dragPanel.ScrollItem[i];
//			GameObject.Destroy(go);
//		}
//		dragPanel.ScrollItem.Clear();

		dragPanel.Clear ();
//		iTween.Stop (gameObject);

//		aList.Clear ();

	}

	private void OnAcceptBonus(object data){
		RspAcceptBonus rsp = data as RspAcceptBonus;

		if(rsp.header.code == ErrorCode.SUCCESS)
		{
			foreach (var num in bonusIDs) {
				for (int i = DataCenter.Instance.UserData.LoginInfo.Bonus.Count - 1; i >= 0; i--) {
					if(DataCenter.Instance.UserData.LoginInfo.Bonus[i].id == num){
						DataCenter.Instance.UserData.LoginInfo.Bonus.RemoveAt(i);
						continue;
					}
				}
			}
			DataCenter.Instance.UserData.AccountInfo.stone = rsp.stone;
			DataCenter.Instance.UserData.AccountInfo.money = rsp.money;
			DataCenter.Instance.UserData.AccountInfo.friendPoint = rsp.friendPoint;
			DataCenter.Instance.UnitData.UserUnitList.AddMyUnitList(rsp.newUnitList);

			MsgCenter.Instance.Invoke(CommandEnum.SyncChips);
			
//			MsgCenter.Instance.Invoke(CommandEnum.SyncStamina);
			MsgCenter.Instance.Invoke(CommandEnum.RefreshPlayerCoin);

			MsgCenter.Instance.Invoke (CommandEnum.RefreshRewardList);
		}
		bonusIDs.Clear ();
	} 

	public override void DestoryUI () {

		dragPanel.DestoryUI ();

		UIEventListenerCustom.Get (OKBtn).onClick -= OnClickOK;
		MsgCenter.Instance.RemoveListener (CommandEnum.GotoRewardMonthCardTab, OnGotoTab);
		aList.Clear ();
		base.DestoryUI ();
	}

	private void InitUI(){
		content = FindChild ("Content");
		OKBtn = FindChild ("OkBtn");
		
		FindChild<UILabel> ("OkBtn/Label").text = TextCenter.GetText("OK");
		FindChild<UILabel> ("1/Label").text = TextCenter.GetText ("Reward_Tab1");
		FindChild<UILabel> ("2/Label").text = TextCenter.GetText ("Reward_Tab2");
		FindChild<UILabel> ("3/Label").text = TextCenter.GetText ("Reward_Tab3");
		FindChild<UILabel> ("4/Label").text = TextCenter.GetText ("Reward_Tab4");
		FindChild<UILabel> ("5/Label").text = TextCenter.GetText ("Reward_Tab5");
		
		Nums = new Dictionary<int, GameObject> ();
		
		Nums.Add (1, FindChild ("1/Num"));
		Nums.Add (2, FindChild ("2/Num"));
		Nums.Add (3, FindChild ("3/Num"));
		Nums.Add (4, FindChild ("4/Num"));
		Nums.Add (5, FindChild ("5/Num"));
		
		FindChild<UILabel> ("Title").text = TextCenter.GetText ("Reward_Title");
		tabInfo = FindChild<UILabel> ("Info");

		//init data
		aList.Clear ();
		foreach (var item in DataCenter.Instance.UserData.LoginInfo.Bonus) {
			if(item.type <= 3){
				
				if(!aList.ContainsKey(item.type))
					aList[item.type] = new List<BonusInfo>();
				aList[item.type].Add(item);
			}else if(item.type == 4){
				if(!aList.ContainsKey(5))
					aList[5] = new List<BonusInfo>();
				aList[5].Add(item);
			}else if(item.type >4){
				if(!aList.ContainsKey(4))
					aList[4] = new List<BonusInfo>();
				aList[4].Add(item);
			}
		}

		dragPanel = new DragPanel("RewardDragPanel", "Prefabs/UI/Reward/RewardItem",typeof(RewardItemView), transform);


		UIEventListenerCustom.Get (OKBtn).onClick += OnClickOK;

		MsgCenter.Instance.AddListener (CommandEnum.GotoRewardMonthCardTab, OnGotoTab);
	}

	void OnClickOK(GameObject obj){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );

		ModuleManager.Instance.HideModule (ModuleEnum.RewardModule);
	}

	private void RefreshView(){
		Debug.Log ("reward view refresh");
		for (int i = 1; i < 6; i++) {
			int count = 0;
			if(aList.ContainsKey(i)){
				foreach (var item in aList[i]) {
					if(item.enabled == 1){
						count++;
					}
				}
			}
			if(count > 0){
				Nums[i].SetActive(true);
//				UIToggle.
				Nums[i].transform.Find("Label").gameObject.GetComponent<UILabel>().text = count.ToString();
			}else{
				Nums[i].SetActive(false);
			}
			if (!aList.ContainsKey (currentContentIndex)) {
				dragPanel.Clear();
			}else{
				dragPanel.SetData<BonusInfo> (aList[currentContentIndex],OnTakeAward as DataListener);
			}
		}

	}


	private void OnTakeAward(object data){


	}

	/// <summary>
	/// Shows the tab info. this function is used in RewardUI's prefab.
	/// </summary>
	/// <param name="data">Data.</param>
	public void ShowTabInfo(object data){
		UIToggle toggle = UIToggle.GetActiveToggle (3);
		if (toggle != null){
			int i;
			int.TryParse(toggle.name,out i);
			if(currentContentIndex != i) {
				tabInfo.text = TextCenter.GetText ("Reward_Tab_Info" + toggle.ToString().Substring(0,1));
				currentContentIndex = i;
				RefreshView();
			}
		}
	}

	
	private void OnGotoTab(object data){
		for(int i = 1; i < 6; i++){
			if(i == (int)data){
				transform.FindChild(i+"").GetComponent<UIToggle>().startsActive = true;
				transform.FindChild(i+"").SendMessage("OnClick");
			}else{
				transform.FindChild(i+"").GetComponent<UIToggle>().startsActive = false;
			}

		}
	}
}
