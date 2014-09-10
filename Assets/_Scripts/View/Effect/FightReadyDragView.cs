﻿using UnityEngine;
using System.Collections;

public class FightReadyDragView : DragSliderBase {

	protected override void InitTrans () {
		moveParent = transform.Find("MoveParent");
		cacheRightParent = transform.Find("RightParent");
		cacheLeftParent = transform.Find ("LeftParent");
//		Debug.LogError ("cacheRightParent : " + cacheRightParent + " cacheLeftParent : " + cacheLeftParent);
	}

	public override void RefreshData () {
//		Debug.LogError("FightReadyDragView RefreshData () ");
		TUnitParty current = DataCenter.Instance.PartyInfo.CurrentParty;
		TUnitParty prev = DataCenter.Instance.PartyInfo.GetPrePartyData;
		TUnitParty next = DataCenter.Instance.PartyInfo.GetNextPartyData;
		
		FightReadyPage rpi = moveParent.GetComponent<FightReadyPage> ();
		rpi.RefreshParty (current);
		
		cacheLeftParent.GetComponent<FightReadyPage> ().RefreshParty (prev);
		cacheRightParent.GetComponent<FightReadyPage> ().RefreshParty (next);
		
		dragChangeViewData.RefreshView (rpi.partyViewList);
	}

	public override void RefreshData (TUnitParty tup) {
//		Debug.LogError("FightReadyDragView RefreshData (TUnitParty tup) ");
		FightReadyPage rpi = moveParent.GetComponent<FightReadyPage> ();
		rpi.RefreshParty (tup);
	}
}
