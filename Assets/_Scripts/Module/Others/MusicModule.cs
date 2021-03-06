using UnityEngine;
using System.Collections;

public class MusicModule : ModuleBase {

	public MusicModule(UIConfigItem config):base(  config){
		CreateUI<MusicView> ();
	}
	
	public override void InitUI(){
		base.InitUI(); 
	}
	
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		//		UnityEngine.Debug.LogError("HideScene");
		base.HideUI();
		
	}
	
	public override void DestoryUI(){
		base.DestoryUI();
	}
}
