using UnityEngine;
using System.Collections;

public class SellView : UIBase
{
	private SellUnity window;
	private SceneInfoBar sceneInfoBar;

	public SellView(string uiName):base(uiName){}

	public override void CreatUI ()
	{
		sceneInfoBar = ViewManager.Instance.GetViewObject( UIConfig.sharePath + "SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		window = ViewManager.Instance.GetViewObject(  UIConfig.unitPath + "SellWindow" ) as SellUnity;
		window.Init ("SellWindow");
		currentUIDic.Add( window.UIName, window );
	}
	
	public override void ShowUI ()
	{
		SetUIActive(true);
		sceneInfoBar.BackBtn.isEnabled = true;
		sceneInfoBar.UITitleLab.text = UIName;
		UIEventListener.Get(sceneInfoBar.BackBtn.gameObject).onClick += BackUI;
	}
	
	public override void HideUI ()
	{
		SetUIActive(false);
		UIEventListener.Get(sceneInfoBar.BackBtn.gameObject).onClick -= BackUI;
	}
	
	private void SetUIActive(bool b)
	{
		window.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
	}
	
	private void BackUI(GameObject btn)
	{
		controllerManger.BackToPrevScene();
	}
}