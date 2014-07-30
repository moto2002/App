﻿using UnityEngine;
using System.Collections;

public class QuestFullScreenTips : UIBaseUnity {
	public override void Init (string name) {
		base.Init (name);
		initLocalPosition = transform.localPosition;
		initLocalScale = transform.localScale;
		sprite = FindChild<UISprite>("Sprite");
		tweenAlpha = FindChild<TweenAlpha>("Sprite");
		HideUI ();
	}
	
	public override void ShowUI (){
		base.ShowUI ();
		HideUI (false);
	}

	public override void HideUI () {
		base.HideUI ();
		HideUI (true);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	private UISprite sprite;
	private TweenAlpha tweenAlpha;
	private Vector3 initLocalPosition = Vector3.zero;
	private Vector3 initLocalScale = Vector3.zero;
	private Callback callBack;

	void HideUI(bool b) {
		if (b) {
			sprite.spriteName = string.Empty;	
			transform.localPosition = initLocalPosition;
			transform.localScale = initLocalScale;
		} 
	}
	float tempTime = 0f;
	public void ShowTexture(string name,Callback cb,float time = 0f) {
		ShowUI ();
		tempTime = time;
		sprite.spriteName = name;
		callBack = cb;
		PlayAnimation (name);
	}

	void PlayAnimation (string name) {
		if (name == BossAppears) {
				PlayAppear ();
		} else if (name == ReadyMove) {
			PlayReadyMove ();
		} else {
			if(name == BackAttack || name == FirstAttack) {
				tempTime = 0.2f;
				transform.localPosition += new Vector3(0f, 100f, 0f);
			}
			else{
				transform.localPosition = initLocalPosition;
			}
			PlayAll ();
		}
	}

	void PlayReadyMove() {
		transform.localPosition = initLocalPosition;
		tweenAlpha.enabled = true;
		tweenAlpha.ResetToBeginning ();

		AudioManager.Instance.PlayAudio (AudioEnum.sound_quest_ready);

		iTween.ScaleFrom (gameObject, iTween.Hash ("scale", new Vector3 (3f, 3f, 3f), "time", tempTime, "easetype", iTween.EaseType.easeOutCubic, "oncomplete", "PlayEnd", "oncompletetarget", gameObject));
	}

	void PlayAll () {
		tweenAlpha.enabled = true;
		tweenAlpha.ResetToBeginning ();
		iTween.ScaleFrom (gameObject, iTween.Hash ("scale", new Vector3(3f,3f,3f), "time", tempTime == 0f ? 0.4f : tempTime, "easetype", iTween.EaseType.easeOutCubic, "oncomplete", "PlayEnd", "oncompletetarget", gameObject));

	}

	void PlayEnd () {
		GameTimer.GetInstance ().AddCountDown (0.8f, End);
	}

	void End() {
		HideUI ();
		if (callBack != null) {
			callBack();
		}
	}

	//---------------------------------------------appear-----------------------------------------------------
	private Vector3 position = Vector3.zero;
	private Vector3 startPosition = Vector3.zero;

	void PlayAppear () {
		float xOffset = -Screen.width;
		startPosition = new Vector3 (initLocalPosition.x + xOffset, initLocalPosition.y, initLocalPosition.z);
		transform.localPosition = startPosition;
		transform.localScale = new Vector3 (1f, 0.1f, 1f);

		StartMoveUI ("BossAppearAnim");
	}

	void StartMoveUI (string func) {
		iTween.MoveTo(gameObject,iTween.Hash("position",initLocalPosition,"time",0.2f,"islocal",true,"easetype",iTween.EaseType.easeInCubic,"oncomplete", func,"oncompletetarget", gameObject));
	}

	void BossAppearAnim() {
		iTween.ScaleTo (gameObject, iTween.Hash ("y", 1f, "time", 0.3f, "easetype", iTween.EaseType.easeInCubic, "oncomplete", "PlayEnd", "oncompletetarget", gameObject));
	}

	//---------------------------------------------appear-----------------------------------------------------
	public const string GameOver = "GAME-OVER-";
	public const string BossAppears = "boss-APPEARS";
	public const string OpenGate = "go-to-the-OPENED-GATE";
	public const string BossBattle = "tap-to-boss-battle!";
	public const string CheckOut = "tap-to-Check-Out-!";
	public const string SPLimit = "SP-LIMIT-OVER!-";
	public const string RankUp = "rank-up";
	public const string ReadyMove = "Ready-to-move-on";
	public const string QuestClear = "Quest--Clear!";
	public const string FirstAttack = "FIRST-ATTACK-";
	public const string BackAttack = "BACK-ATTACK-";
	public const string standReady = "stand-ready";
}