﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerManager
{
	#region singleton
	private static ControllerManager instance;

	/// <summary>
	/// singleton 
	/// </summary>
	/// <value>The instance.</value>
	public static ControllerManager Instance
	{
		get
		{
			if(instance  == null)
				instance = new ControllerManager();

			return instance;
		}
	}
	#endregion

	private IUIInterface currentScene;

	#region UI object manager
	private Dictionary<string,IUIInterface> uiDic = new Dictionary<string, IUIInterface>();

	/// <summary>
	/// add ui to uimanager
	/// </summary>
	/// <param name="uiName">ui name.</param>
	/// <param name="ui">ui object.</param>
	public void AddUIObject(string uiName,IUIInterface ui)
	{
		if(!uiDic.ContainsKey(uiName))
			uiDic.Add(uiName,ui);
		else
			uiDic[uiName] = ui;
	}

	/// <summary>
	/// have this ui object?
	/// </summary>
	/// <returns><c>true</c> if this instance has user interface object the specified uiName; otherwise, <c>false</c>.</returns>
	/// <param name="uiName">ui name.</param>
	public bool HasUIObject(string uiName)
	{
		if(uiDic.ContainsKey(uiName))
			return true;

		return false;
	}

	/// <summary>
	/// get this ui object
	/// </summary>
	/// <returns>ui object.</returns>
	/// <param name="uiName">ui name.</param>
	public IUIInterface GetUI(string uiName)
	{
		if(uiDic.ContainsKey(uiName))
			return uiDic[uiName];
		else
			return null;
	}

	/// <summary>
	/// Remove UI
	/// </summary>
	/// <param name="uiName">User interface name.</param>
	public void RemoveUI(string uiName)
	{
		if(uiDic.ContainsKey(uiName))
		{
			if(currentScene == uiDic[uiName])
				currentScene = null;

			uiDic[uiName].DestoryUI();
			uiDic.Remove(uiName);
		}
	}
	
	#endregion

	public void ChangeScene(SceneEnum sEnum)
	{
		string uiName = sEnum.ToString();

		if(currentScene != null)
		{
			if(currentScene.UIName == uiName)
				return;
			else
				currentScene.HideUI();
		}

		if(HasUIObject(uiName))
			currentScene = GetUI(uiName);
		else
			currentScene = CreatScene(sEnum,uiName);

		currentScene.ShowUI();
	}

	IUIInterface CreatScene(SceneEnum sEnum,string uiName)
	{
		IUIInterface temp;
		switch (sEnum)
		{	
		case SceneEnum.Quest:
			temp = new BattleShow(uiName);
			break;
//		case SceneEnum.Scratch:
//
//			break;
//		case SceneEnum.Shop:
//
//			break;
//		case SceneEnum.Others:
//
//			break;
//		case SceneEnum.Units:
//
//			break;
		case SceneEnum.QuestSelect:
			temp = new QuestSelect(uiName);
			break;
		default:
			temp = new UIBase("Null");
			break;
		}

		temp.CreatUI();

		AddUIObject(uiName,temp);

		return temp;
	}
}