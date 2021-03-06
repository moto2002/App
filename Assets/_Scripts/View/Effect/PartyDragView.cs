﻿using UnityEngine;
using System.Collections;
using bbproto;

public class PartyDragView : DragSliderBase {

	protected override void InitTrans () {
		moveParent = transform.Find ("DragParent");
		cacheLeftParent = transform.Find("LeftCache");
		cacheRightParent = transform.Find("RightCache");
	}

	public override void RefreshData () {
		UnitParty current = DataCenter.Instance.UnitData.PartyInfo.CurrentParty;
		UnitParty prev = DataCenter.Instance.UnitData.PartyInfo.GetPrePartyData;
		UnitParty next = DataCenter.Instance.UnitData.PartyInfo.GetNextPartyData;
		
		RefreshPartyInfo rpi = moveParent.GetComponent<RefreshPartyInfo> ();
		rpi.RefreshView (current);
		
		cacheLeftParent.GetComponent<RefreshPartyInfo> ().RefreshView (prev);
		cacheRightParent.GetComponent<RefreshPartyInfo> ().RefreshView (next);
		
		dragChangeViewData.RefreshView (rpi.partyView);
	}
}
