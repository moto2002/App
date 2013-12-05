﻿using UnityEngine;
using System.Collections;

public class BattleShow : UIBase 
{
	#region IUIInterface implementation

	private DragUI topUI;

	private DragUI downUI;

	public BattleShow(string uiName):base(uiName)
	{

	}

	public override void CreatUI ()
	{
		BattleShowUnity bsu = ViewManager.Instance.GetViewObject("BattleShow") as BattleShowUnity; 

		currentUIDic.Add(bsu.UIName,bsu);


		topUI = new DragUI(bsu.TopLeft,bsu.TopRight,bsu.DragItem);

		topUI.ShowData(3);

		downUI = new DragUI(bsu.BottomLeft,bsu.BottomRight,bsu.DragItem);
		downUI.ShowData(4);

		UIEventListener listen = UIEventListener.Get(topUI.DragList[0]);

		listen.onClick = ClickQuest;
	}

	public override void ShowUI ()
	{
		SetActive(true);
	}

	public override void HideUI ()
	{
		SetActive(false);
	}

	public override void DestoryUI ()
	{

	}

	void SetActive(bool b)
	{
		insUIObject.SetActive(b);
		topUI.insUIObject.SetActive(b);
		downUI.insUIObject.SetActive(b);
	}

	#endregion

	void ClickQuest(GameObject go)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.QuestSelect);
	}

}
