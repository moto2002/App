﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCounterView : UIComponentUnity{
	UILabel titleLabel;
	UILabel curLabel;
	UILabel maxLabel;

	public override void Init(UIInsConfig config,IUICallback origin) {
		base.Init(config,origin);
		MsgCenter.Instance.AddListener(CommandEnum.RefreshItemCount, UpdateView);
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
		ShowUIAnimation();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void DestoryUI () {
		MsgCenter.Instance.RemoveListener(CommandEnum.RefreshItemCount, UpdateView);
		base.DestoryUI ();
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
		curLabel.text = TextCenter.GetText("CounterCurrent" , current);

		Vector3 pos = this.gameObject.transform.localPosition;
		if (viewInfo.ContainsKey ("posy")) {
			pos.y = (int)viewInfo["posy"];
			pos.z = 0;
		}
//		pos.y = (int)viewInfo["posy"];
		this.gameObject.transform.localPosition = pos;
		
		if(max == 0){
			maxLabel.text = string.Empty;
		}
		else{
			maxLabel.text = TextCenter.GetText("CounterMax" , max);
			if(current > max){
				curLabel.color = Color.red;
			} else{
				curLabel.color = Color.white;
			}
		}
	}

	private void ShowUIAnimation(){
		transform.localPosition = new Vector3(1000, -792, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 210, "time", 0.4f, "islocal", true));
	}
}
