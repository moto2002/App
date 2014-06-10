﻿using UnityEngine;
using System.Collections;

public class EvolveDragItem : MyUnitItem {
	public static EvolveDragItem Inject(GameObject go) {
		EvolveDragItem edi = go.GetComponent<EvolveDragItem> ();
		if (edi == null) {
			edi = go.AddComponent<EvolveDragItem>();	
		}
		return edi;
	}

	public delegate void EvolveItemCallback(EvolveDragItem puv);
	public EvolveItemCallback callback;

	[HideInInspector]
	public bool CanEvolve = false;
	
	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}
	
	protected override void InitUI(){
		base.InitUI();
	}
	
	protected override void InitState(){
		base.InitState();
		IsFocus = false;
		
		if(userUnit != null){
			IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(userUnit.ID);
			IsEnable = !IsParty;
		}
	}
	
	protected override void UpdatePartyState(){
		partyLabel.enabled = IsParty;
		IsEnable = !IsParty;
	}
	
	protected override void UpdateFocus(){
		lightSpr.enabled = IsFocus;
	}
	
	protected override void RefreshState(){
		base.RefreshState();
		if(userUnit != null){
			IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(userUnit.ID);
			//IsEnable is FALSE as long as IsParty is TRUE
			IsEnable = !IsParty;
		}
	}

}