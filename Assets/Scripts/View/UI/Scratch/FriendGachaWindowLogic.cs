// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class FriendGachaWindowLogic: GachaWindowLogic{
    public FriendGachaWindowLogic(string uiName):base(uiName) {}

    public override void HideUI () {
        LogHelper.Log("HideUI(), hide");
        base.HideUI ();
    }
    protected override void BeforeSetTitleView(){
        titleText = TextCenter.Instace.GetCurrentText("FriendScratch");
    }

}