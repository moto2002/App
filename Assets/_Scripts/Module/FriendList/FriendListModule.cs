using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendListModule : ModuleBase{
	public FriendListModule(UIConfigItem config) : base(   config ) {
		CreateUI<FriendListView> ();
	}

}
