﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyUnitDragUILogic : ConcreteComponent {

	public MyUnitDragUILogic(string uiName):base(uiName) {}

	public override void ShowUI(){
		base.ShowUI();

	}

	void GetCurPartyData(){
		//GlobalData
	}
}
