﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyInfoView : UIComponentUnity {
	
	UIToggle focus;

	Dictionary<string, UILabel> viewLabel = new Dictionary<string, UILabel>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		FindUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
		SetUIElement();
		ShowTween();
	}

	public override void HideUI(){
		base.HideUI();
		ResetUIElement();
	}

	void FindUIElement(){
		Debug.Log("PartyInfoPanel.FindUIElement(), Start...");
		UILabel label;

		label = FindChild<UILabel>("content_status/VauleLabel/Label_HP_Value");
		viewLabel.Add("hp", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_CurCost_Value");
		viewLabel.Add("curCost", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_MaxCost_Value");
		viewLabel.Add("maxCost", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_Fire_Value");
		viewLabel.Add("fire", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_Water_Value");
		viewLabel.Add("water", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_Wind_Value");
		viewLabel.Add("wind", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_Light_Value");
		viewLabel.Add("light", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_Dark_Value");
		viewLabel.Add("dark", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_Wu_Value");
		viewLabel.Add("wu",label);

		label = FindChild<UILabel>("content_leader_skill/VauleLabel/Label_LeaderSkillName");
		viewLabel.Add("skillName", label);

		label = FindChild<UILabel>("content_leader_skill/VauleLabel/Label_LeaderSkillDscp");
		viewLabel.Add("skillDscp", label);

		focus = FindChild<UIToggle>("tab_status");

		Debug.Log("PartyInfoPanel.FindUIElement(), End...");
	}

	void SetUIElement(){
		Debug.Log("PartyInfoPanel.SetUIElement(), Start...");

		Debug.Log("PartyInfoPanel.SetUIElement(), End...");
	}

	void ResetUIElement(){
		Debug.Log("PartyInfoPanel.ResetUIElement(), Start...");
		ResetTabFocus();
		Debug.Log("PartyInfoPanel.ResetUIElement(), End...");
	}

	void ResetTabFocus(){
		focus.startsActive = true;
	}

	void UpdateLabel(Dictionary<string, string> text){
		Debug.Log("PartyInfoPanel.UpdateLabel(), Start...");

		foreach (var item in text.Keys){
			if( viewLabel.ContainsKey(item)){
				viewLabel[item].text = text[item];
			}
		}
	
		Debug.Log("PartyInfoPanel.UpdateLabel(), End...");
	}

	public override void Callback(object data){
		base.Callback(data);

		Dictionary<string,string> viewInfoDic = data as Dictionary<string,string>;
		if( viewInfoDic == null ){
			Debug.LogError("PartyInfoPanel.Callback(), ViewInfo is Null!");
			return;
		}	
		UpdateLabel(viewInfoDic);
	}

	void ShowTween()
	{
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list)
		{		
			if (tweenPos == null)
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

}