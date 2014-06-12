using UnityEngine;
using System.Collections;

public class PartyUnitItem : MyUnitItem {
	public static PartyUnitItem Inject(GameObject item){
		PartyUnitItem view = item.GetComponent<PartyUnitItem>();
		if (view == null) view = item.AddComponent<PartyUnitItem>();
		return view;
	}
	public delegate void UnitItemCallback(PartyUnitItem puv);
	public UnitItemCallback callback;

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



public class LevelUpUnitItem : MyUnitItem {
	public static LevelUpUnitItem Inject(GameObject item){
		LevelUpUnitItem view = item.GetComponent<LevelUpUnitItem>();
		if (view == null) view = item.AddComponent<LevelUpUnitItem>();
		return view;
	}
	public delegate void UnitItemCallback(LevelUpUnitItem puv);
	public UnitItemCallback callback;
	
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
//		IsEnable = !IsParty;
	}
	
	protected override void UpdateFocus(){
		lightSpr.enabled = IsFocus;
	}
	
	protected override void RefreshState(){
		base.RefreshState();
		if(userUnit != null){
			IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(userUnit.ID);
		}
	}
}
