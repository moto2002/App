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
	List<UITexture> texureList = new List<UITexture>();
	//Dictionary< GameObject, > partyItemDic = new Dictionary<int, GameObject>();

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
		Debug.Log("PartyPagePanel.FindUIElement() : Start...");

		FindLabel();
		FindButton();
		FindTexture();

		Debug.Log("PartyPagePanel.FindUIElement() : End...");
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

	void FindItem(){
//		GameObject go;

	}

	void FindTexture() {
		UITexture tex;
		for( int i = 1; i < 5; i++) {
			tex = FindChild< UITexture >("Unit" + i.ToString() + "/role" );
			texureList.Add(tex);
		}
	}

	void InitUIElement(){
		InitIndexTextDic();
		partyCountLabel.text = partyTotalCount.ToString();
	}

	
	void UpdateLabel(int index){
		Debug.Log("PartyPagePanel.UpdateLabel(), index is " + index);
		curPartyPrefixLabel.text = index.ToString();
		curPartysuffixLabel.text = partyIndexDic[ index ].ToString();
		curPartyIndexLabel.text = index.ToString();
	}
	
	void UpdateTexture(List<Texture2D> tex2dList){
		Debug.Log("PartyPagePanel.UpdateTexture(), Start...");
		for (int i = 0; i < tex2dList.Count; i++) {
			if(tex2dList[ i ] == null){
				Debug.LogError(string.Format("PartyPagePanel.UpdateTexture(), Pos[{0}] source is null, do nothing!", i));
				continue;
			} else {
				texureList[ i ].mainTexture = tex2dList[ i ];
				Debug.Log(string.Format("PartyPagePanel.UpdateTexture(), Pos[{0}] texture is showing", i));
			}
		}
		Debug.Log("PartyPagePanel.UpdateTexture(), End...");
	}

	void SetUIElement(){
		Debug.Log("PartyPagePanel.SetUIElement() : Start...");
		UIEventListener.Get(leftButton.gameObject).onClick = PageBack;
		UIEventListener.Get(rightButton.gameObject).onClick = PageForward;
		Debug.Log("PartyPagePanel.SetUIElement() : End...");
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

		if(viewInfoDic.TryGetValue("index",out curPartyIndex)){
			UpdateLabel((int)curPartyIndex);
		}

		if(viewInfoDic.TryGetValue("texture",out tex2dList)){
			List<Texture2D> temp = tex2dList as List<Texture2D>;
			UpdateTexture(temp);
		}
	}
	
}
