﻿using UnityEngine;
using System.Collections.Generic;

public class BattleSkill : UIBaseUnity {
	public override void Init (string name) {
		base.Init (name);
		InitUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.MeetEnemy, MeetEnemy);
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
	}

	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.MeetEnemy, MeetEnemy);
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	void OnEnable () {

	}

	void OnDisable () {

	}

	private string[] SKill = new string[5]{ "LeaderSkill","NormalSKill1","NormalSkill2","ActiveSkill","PassiveSKill"};
	private const string path = "/Label";
	private const string pathName = "/SkillName";
	private const string pathDescribe = "/DescribeLabel";
	private Callback boostAcitveSkill;
	private Callback CloseSkill;
	private UILabel roundLabel;
	private UIButton boostButton;
	
	private Dictionary<string, SkillItem> skillDic = new Dictionary<string, SkillItem> ();

	void MeetEnemy(object data) {
		boost = true;	
	}

	void BattleEnd(object data) {
		boost = false;	
	}

	void InitUI () {
		SkillItem si = null;
		string info = string.Empty;

		info = SKill[0];
		si = new SkillItem();
		si.skillTypeLabel = FindChild<UILabel> (info + path);
		si.skillName = FindChild<UILabel>(info + pathName);
		si.skillDescribeLabel = FindChild<UILabel>(info + pathDescribe);
		skillDic.Add(info, si);

		info = SKill[4];
		si = new SkillItem();
		si.skillTypeLabel = FindChild<UILabel> ("PassiveSkill/Label");
		si.skillName = FindChild<UILabel>("PassiveSkill/SkillName");
		si.skillDescribeLabel = FindChild<UILabel>("PassiveSkill/DescribeLabel");
		skillDic.Add(info, si);

		info = SKill[1];
		si = new SkillItem();
		si.skillTypeLabel = FindChild<UILabel> ("NormalSKill1/Label");
		si.skillName = FindChild<UILabel>("NormalSKill1/SkillName");
		si.skillDescribeLabel = FindChild<UILabel>("NormalSKill1/DescribeLabel");
		List<UISprite> temp = new List<UISprite> ();
		for (int j = 1; j < 6; j++) {
			temp.Add(FindChild<UISprite> ("NormalSKill1/" + j));
		}
		si.skillSprite = temp;
		skillDic.Add(info, si);

		info = SKill[2];
		si = new SkillItem();
		si.skillTypeLabel = FindChild<UILabel> ("NormalSkill2/Label");
		si.skillName = FindChild<UILabel>("NormalSkill2/SkillName");
		si.skillDescribeLabel = FindChild<UILabel>("NormalSkill2/DescribeLabel");
		temp = new List<UISprite> ();
		for (int j = 1; j < 6; j++) {
			temp.Add(FindChild<UISprite> ("NormalSkill2/" + j));
		}
		si.skillSprite = temp;
		skillDic.Add(info, si);
		info = SKill[3];
		si = new SkillItem();
		si.skillTypeLabel = FindChild<UILabel> ("ActiveSkill/Label");
		si.skillName = FindChild<UILabel>("ActiveSkill/SkillName");
		si.skillDescribeLabel = FindChild<UILabel>("ActiveSkill/DescribeLabel");
		skillDic.Add(info, si);
		roundLabel = FindChild<UILabel>("RoundLabel");
		boostButton = FindChild<UIButton>("BoostButton");
		UIEventListener.Get (boostButton.gameObject).onClick = Boost;
		Transform trans = FindChild<Transform>("Title/Button_Close");
		UIEventListener.Get (trans.gameObject).onClick = Close;
	}

	void Boost(GameObject go) {
		if (boost && boostAcitveSkill != null) {
			boostAcitveSkill();	
		}
	}

	void Close(GameObject go) {
		if (CloseSkill != null) {
			CloseSkill();
		}
	}

	bool boost = false;

	public void Refresh(TUserUnit userUnitInfo, Callback boostSKill,Callback close) {
		boostAcitveSkill = boostSKill;
		CloseSkill = close;
		TUnitInfo tui = userUnitInfo.UnitInfo;

		SkillBaseInfo sbi = DataCenter.Instance.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.LeaderSkill, SkillType.LeaderSkill); //GetSkill (tui.LeaderSkill);
//		if (sbi == null) {
//			bbproto.SkillBase sb = new bbproto.SkillBase();
//			sb.id = 0;
//			sb.description = "-";
//			sb.name = "-";
//			sbi = new SkillBaseInfo(sb);
//		} else if(string.IsNullOrEmpty (sbi.SkillDescribe)) {
//			sbi .SkillDescribe = "-";
//		}

		Refresh (0, sbi);

		sbi = DataCenter.Instance.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.NormalSkill1, SkillType.NormalSkill); 				//.GetSkill (tui.NormalSkill1);
		Refresh (1, sbi);

		sbi = DataCenter.Instance.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.NormalSkill2, SkillType.NormalSkill); 				//.GetSkill (tui.NormalSkill2);
		Refresh (2, sbi);

		sbi = DataCenter.Instance.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.ActiveSkill, SkillType.ActiveSkill); 
		Refresh (3, sbi);

		if (sbi != null && sbi.BaseInfo.skillCooling == 0) {

			boostButton.isEnabled = true;
		}
		else {
			boostButton.isEnabled = false;
		}

		if (sbi == null) {
			roundLabel.text = "";		
		} 
		else {
			roundLabel.text =  sbi.BaseInfo.skillCooling + "  round";
		}

		sbi = DataCenter.Instance.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.PassiveSkill, SkillType.PassiveSkill);				//.GetSkill (tui.PassiveSkill);

		Refresh (4, sbi);
	}

	void Refresh(int index, SkillBaseInfo sbi) {
		if (index == 0 && sbi == null) {
			skillDic [SKill [index]].ShowSkillInfo (sbi, true);
		} else {
			skillDic [SKill [index]].ShowSkillInfo (sbi);
		}
	}
}

public class SkillItem {
	/// <summary>
	/// The skill type label. don't change this label content;
	/// </summary>
	public UILabel skillTypeLabel;
	public UILabel skillName;
	public UILabel skillDescribeLabel;
	public List<UISprite> skillSprite;

	public void ShowSkillInfo (SkillBaseInfo sbi, bool isLeaderSkill = false) {
		if (sbi == null) {
//			if(isLeaderSkill) {
//				ClearLeaderSkill();
//			}
//			else{
//				Clear();
//			}
			Clear();
			return;
		}
		skillTypeLabel.enabled = true;
		skillName.text = sbi.SkillName;
		skillDescribeLabel.text = sbi.SkillDescribe;

		TNormalSkill tns = sbi as TNormalSkill;
		if (tns != null) {
			ShowSprite (tns.Blocks);
		} else {
			ShowSprite(null);
		}
	}

	void Clear() {
//		skillTypeLabel.enabled = false;
		skillName.text = "-";
		skillDescribeLabel.text = "-";
		ShowSprite (null);
	}

	void ClearLeaderSkill () {
		skillName.text = "-";
		skillDescribeLabel.text = "-";
		ShowSprite (null);
	}

	void ShowSprite (List<uint> blocks) {
		if (skillSprite == null) {
			return;	
		}

		if (blocks == null) {
			for (int i = 0; i < skillSprite.Count; i++) {
				skillSprite[i].spriteName = string.Empty;	
			}
			return;
		}

		for (int i = 0; i < blocks.Count; i++) {
			skillSprite[i].spriteName = blocks[i].ToString();
//			Debug.LogError(" i : " + i + "  skillSprite[i] : " + skillSprite[i] + " name : " + skillSprite[i].spriteName);
		}
		for (int i = blocks.Count; i < skillSprite.Count; i++) {
			skillSprite[i].spriteName = string.Empty;	
		}
	}

}