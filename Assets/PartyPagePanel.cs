﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPagePanel : UIComponentUnity {

	int pageIndexOrigin = 1;
	int currentPartyIndex = 1;
	int partyTotalCount = 5;
	UILabel curPartyIndexLabel;
	UILabel partyCountLabel;
	UILabel curPartyPrefixLabel;
	UILabel curPartysuffixLabel;
	UIButton leftButton;
	UIButton rightButton;
	Dictionary< int, string > partyIndexDic = new Dictionary< int, string >();
	Dictionary< int, UITexture > unitTexureDic = new Dictionary< int, UITexture>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		FindUIElement();
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
		SetUIElement();
	}

	public override void HideUI(){
     
		base.HideUI();
		ResetUIElement();
	}

	void FindUIElement(){
		Debug.Log("PartyPagePanel.FindUIElement() : Start");

		FindLabel();
		FindButton();
		FindTexture();

		Debug.Log("PartyPagePanel.FindUIElement() : End");
	}

	void FindLabel(){
		curPartyIndexLabel = FindChild<UILabel>("Label_Cur_Party");
		partyCountLabel = FindChild<UILabel>("Label_Party_Count");
		curPartyPrefixLabel = FindChild<UILabel>("Label_Party_Index_Prefix");
		curPartysuffixLabel = FindChild<UILabel>("Label_Party_Index_Suffix");
	}

	void FindButton(){
		leftButton = FindChild<UIButton>("Button_Left");
		rightButton = FindChild<UIButton>("Button_Right");
	}

	void FindTexture() {
		UITexture temp;
		for( int i = 1; i < 5; i++) {
			temp = FindChild< UITexture >("Unit" + i.ToString() + "/role" );
			temp.enabled = false;
			unitTexureDic.Add(i, temp);
		}
	}

	void InitUIElement(){
		InitIndexTextDic();
		partyCountLabel.text = partyTotalCount.ToString();
	}

	
	void UpdateLabel(int index){
		//LeftCenter Label
		curPartyPrefixLabel.text = index.ToString();
		curPartysuffixLabel.text = partyIndexDic[ index ].ToString();
		//TopRight Label
		curPartyIndexLabel.text = index.ToString();
	}
	
	void UpdateTexture(List<Texture2D> tex2dList){
		for (int i = 0; i < unitTexureDic.Count; i++){
			if(tex2dList[ i ] == null)	continue;
			unitTexureDic[ i ].mainTexture = tex2dList[ i ] ;
		}
	}

	void SetUIElement(){
		Debug.Log("PartyPagePanel.SetUIElement() : Start");
		UIEventListener.Get(leftButton.gameObject).onClick = PageBack;
		UIEventListener.Get(rightButton.gameObject).onClick = PageForward;
		Debug.Log("PartyPagePanel.SetUIElement() : End");
	}

	void InitIndexTextDic() {
		partyIndexDic.Add( 1, "st");
		partyIndexDic.Add( 2, "nd");
		partyIndexDic.Add( 3, "rd");
		partyIndexDic.Add( 4, "th");
		partyIndexDic.Add( 5, "th");
	}

	void PageBack(GameObject button){
		Debug.Log("PartyPagePanel.PageBack() : Start");

		ExcuteCallback("PageBack");

		Debug.Log("PartyPagePanel.ExcuteCallback() : End");
	} 

	void PageForward(GameObject go){
		Debug.Log("PartyPagePanel.PageForward() : Start");

		ExcuteCallback("PageForward");

		Debug.Log("PartyPagePanel.PageForward() : End");
	}
	
	void ResetUIElement(){
		Debug.Log("PartyPagePanel.ResetUIElement() : Start");
		Debug.Log("PartyPagePanel.ResetUIElement() : End");
	}

	public override void Callback(object data){
		base.Callback(data);

		Dictionary<string,object> viewInfoDic = data as Dictionary<string,object>;
		if( viewInfoDic == null ){
			Debug.LogError("PartyPagePanel.Callback(), ViewInfo is Null!");
			return;
		}	

		object tex2dList;
		object curPartyIndex;

		if(viewInfoDic.TryGetValue("texture",out tex2dList)){
			UpdateTexture(tex2dList as List<Texture2D>);
		}

		if(viewInfoDic.TryGetValue("index",out curPartyIndex)){
			UpdateLabel((int)curPartyIndex);
		}
	}






}