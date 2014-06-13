﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleMenu : UIBaseUnity {
	private UIButton CloseButton;

	//==================questinfo component=======
	private UILabel areaNameLabel;
	private UILabel questNameLabel;
	private UILabel floorLabel;
	private UIScrollView scrollView;
	private UIGrid grid;
	private GameObject itemObject;
	private List<GameObject> itemList = new List<GameObject> ();
	//============================================

	//==================options component=======
	private UIButton bgmOnButton;
	private UIButton bgmOffButton;
	private UIButton seOnButton;
	private UIButton seOffButton;
	private UIButton guideOnButton;
	private UIButton guideOffButton;
	//============================================

	//==================options component=======
	private UIButton exitButton;
	private UIButton cancelButton;
	//============================================

	private UIButton closeButton;
	private UIToggle defaultToggle;

	private BattleQuest _battleQuest;
	public BattleQuest battleQuest {
		get { return _battleQuest; }
		set { _battleQuest = value; }
	}

	private AudioManager audioManager;

	public override void Init (string name) {
		base.Init (name);
		audioManager = AudioManager.Instance;
	
		closeButton = FindChild<UIButton> ("Title/Button_Close");
		UIEventListener.Get (closeButton.gameObject).onClick = CancelButton;

		string Path = "Tabs/Content_QuestInfo";
		areaNameLabel = FindChild<UILabel> (Path + "/AreaNameLabel");
		areaNameLabel.text = ConfigBattleUseData.Instance.currentQuestInfo.Name;

		questNameLabel = FindChild<UILabel>(Path + "/QuestNameLabel");
		questNameLabel.text = ConfigBattleUseData.Instance.currentStageInfo.StageName;

		floorLabel = FindChild<UILabel>(Path + "/FloorLabel");
		TQuestDungeonData tdd = ConfigBattleUseData.Instance.questDungeonData;
		floorLabel.text = (tdd.currentFloor + 1) + "/" + tdd.Floors.Count;
		string path = Path + "/GetUnitsDragPanel/ScrollView";
		scrollView = FindChild<UIScrollView>(path);
		grid = FindChild<UIGrid>(path + "/UIGrid");
		itemObject = transform.Find (Path + "/GetUnitsDragPanel/ScrollView/UIGrid/Item").gameObject;

		defaultToggle = FindChild<UIToggle> ("Tabs/Tab_QuestInfo");

		Path = "Tabs/Content_Options/";
		bgmOnButton = FindChild<UIButton> (Path + "Button_BGM_ON");
		UIEventListener.Get (bgmOnButton.gameObject).onClick = BGMOn;

		bgmOffButton = FindChild<UIButton> (Path + "Button_BGM_OFF");
		UIEventListener.Get (bgmOffButton.gameObject).onClick = BGMOff;

		seOnButton = FindChild<UIButton> (Path + "Button_SE_ON");
		UIEventListener.Get (seOnButton.gameObject).onClick = SeOnButton;

		seOffButton = FindChild<UIButton> (Path + "Button_SE_OFF");
		UIEventListener.Get (seOffButton.gameObject).onClick = SeOffButton;

		guideOnButton = FindChild<UIButton> (Path + "Button_GUIDE_ON");
		UIEventListener.Get (guideOnButton.gameObject).onClick = GUIDEOnButton;

		guideOffButton = FindChild<UIButton> (Path + "Button_GUIDE_OFF");
		UIEventListener.Get (guideOffButton.gameObject).onClick = GUIDEOffButton;

		Path = "Tabs/Content_Retire/";
		exitButton = FindChild<UIButton> (Path + "Button_OK");
		UIEventListener.Get (exitButton.gameObject).onClick = ExitButton;

		cancelButton = FindChild<UIButton> (Path + "Button_Cancel");
		UIEventListener.Get (cancelButton.gameObject).onClick = CancelButton;
//		MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.MessageWindow, true));
//		RefreshDropItem ();
		ShowUI ();
	}

	void RefreshDropItem () {
		TClearQuestParam cqp = battleQuest.GetQuestData ();
		int getUnitCount = cqp.getUnit.Count;
		int itemCount = itemList.Count;
		int count = getUnitCount - itemCount;

		if (count > 0) {
			for (int i = 0; i < count; i++) {
				uint unitID = cqp.getUnit [itemCount + i];
				GameObject go = NGUITools.AddChild (grid.gameObject, itemObject);
				go.SetActive (true);
				UISprite sprite = go.transform.Find ("ItemSprite").GetComponent<UISprite> ();
				TUnitInfo tui = DataCenter.Instance.GetUnitInfo (unitID);
				sprite.spriteName = DGTools.GetUnitDropSpriteName (tui.Rare);
				go.name = itemList.Count.ToString();
				itemList.Add(go);
			}	
		} else {
			count = -count;
			for (int i = 0; i < count; i++) {
				GameObject go = itemList[getUnitCount + i];
				Destroy(go);
				itemList.Remove(go);
			}
		}
		scrollView.ResetPosition ();
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		RefreshDropItem ();
		defaultToggle.value = true;
		_battleQuest.battle.SwitchInput (true);
		BattleBottom.notClick = true;
		TouchEventBlocker.Instance.SetState (BlockerReason.MessageWindow, true);
//		MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.MessageWindow, true));
	}

	public override void HideUI () {
		base.HideUI ();
		gameObject.SetActive (false);
		Main.Instance.GInput.IsCheckInput = true;
		BattleBottom.notClick = false;
		TouchEventBlocker.Instance.SetState (BlockerReason.MessageWindow, false);
//		MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.MessageWindow, false));
	}

	void BGMOn(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
		audioManager.CloseBackground (false);
	}

	void BGMOff(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
		audioManager.CloseBackground (true);
	}

	void SeOnButton (GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
		audioManager.CloseSound (false);
	}

	void SeOffButton(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
		audioManager.CloseSound (true);
	}

	void GUIDEOnButton(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
	}

	void GUIDEOffButton(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
	}

	void ExitButton(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
		HideUI ();
		Battle.isShow = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd);

//		_battleQuest.NoFriendExit ();
		_battleQuest.Retire (false);
	}

	void CancelButton(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
		HideUI ();
	}
}
