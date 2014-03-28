﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCounterView : UIComponentUnity{
	UILabel titleLabel;
	UILabel curLabel;
	UILabel maxLabel;

	public override void Init(UIInsConfig config,IUICallback origin) {
		MsgCenter.Instance.AddListener(CommandEnum.RefreshItemCount, UpdateView);
		base.Init(config,origin);
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	public override void HideUI(){
		base.HideUI();
	}
	
	void InitUIElement(){
		titleLabel = FindChild<UILabel>("Label_Title");
		curLabel = FindChild<UILabel>("Label_Current");
		maxLabel = FindChild<UILabel>("Label_Max");
	}

	public void UpdateView(object msg){
		Dictionary<string, object> viewInfo = msg as Dictionary<string, object>;
		titleLabel.text = viewInfo["title"] as string;
		int current = (int)viewInfo["current"];
		int max = (int)viewInfo["max"];
		curLabel.text = TextCenter.Instace.GetCurrentText("CounterCurrent" , current);

		if(max == 0){
			maxLabel.text = string.Empty;
		}
		else{
			maxLabel.text = TextCenter.Instace.GetCurrentText("CounterMax" , max);
			if(current > max){
				curLabel.color = Color.red;
			}
		}
	}
	
}
