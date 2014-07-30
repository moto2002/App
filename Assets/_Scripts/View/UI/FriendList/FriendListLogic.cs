using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendListLogic : ConcreteComponent{
	public FriendListLogic(string uiName) : base( uiName ) {

	}

	public override void HideUI () {
		base.HideUI ();

		base.DestoryUI ();
	}

}