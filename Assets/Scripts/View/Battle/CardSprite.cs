﻿using UnityEngine;
using System.Collections;

public class CardSprite : UIBaseUnity 
{
	public event UICallback<CardSprite> tweenCallback;
	
	private UISprite actorSprite;
	
	public UISprite ActorSprite {
		get{return actorSprite;}
	}
	
	private Vector3 initActorPosition;
	private Vector3 hideActorPosition = new Vector3 (10000f, 10000f, 10000f);
	
	private TweenPosition tweenPosition;
	public TweenPosition TweenP {
		get{return tweenPosition;}
	}
	
	private UIButtonScale anim;
	
	private TweenScaleExtend tse;
	public TweenScaleExtend TweenSE {
		get{return tse;}
	}
	
	private Vector3 initPosition;
	public Vector3 InitPosition {
		set{initPosition = value;}
	}
	
	private int initDepth;
	
	private Transform parentObject;
	
	private float xOffset = 0f;
	
	private float defaultMoveTime = 0.1f;
	
	[HideInInspector]
	public int itemID = -1;	
	[HideInInspector]
	public int location = -1;

	public override void Init (string name) {
		base.Init (name); 
		parentObject = transform.parent;
		actorSprite = GetComponent<UISprite>();
		initActorPosition = actorSprite.transform.localPosition;
		tweenPosition = GetComponent<TweenPosition>();
		tweenPosition.enabled = false;
		tse = GetComponent<TweenScaleExtend> ();
		tse.enabled = false;
		tweenPosition.eventReceiver = gameObject;
		tweenPosition.callWhenFinished = "TweenPositionCallback";
		initPosition = actorSprite.transform.localPosition;
		anim = GetComponent<UIButtonScale>();
		initDepth = actorSprite.depth;
	}
	
	public override void ShowUI () {
		if (!actorSprite.enabled)
			actorSprite.enabled = true;
		
		base.ShowUI ();
	}
	
	public override void HideUI () {
		actorSprite.spriteName = "";
		if(actorSprite.enabled)
			actorSprite.enabled = false;
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	Texture texure ;

	public void SetTexture(int color,int itemID) {
		this.itemID = itemID;
		actorSprite.spriteName = color.ToString ();
		xOffset = (float)actorSprite.width / 4;
	}
	
	public void Reset() {
		actorSprite.transform.localPosition = initPosition;
		actorSprite.spriteName = "";
	}
	
	public void SetTweenPosition(Vector3 start,Vector3 end) {
		tweenPosition.enabled = true;
		tweenPosition.from = start;
		tweenPosition.to = end;
		tweenPosition.duration = defaultMoveTime;
	}
	
	public void Move(Vector3 to,float time) {
		Move(transform.localPosition,to,time);
	}
	
	public void Move(Vector3 to) {
		Move(transform.localPosition,to,defaultMoveTime);
	}
	
	public void Move(Vector3 from,Vector3 to) {
		Move(from,to,defaultMoveTime);
	}
	
	public void Move(Vector3 from,Vector3 to, float time) {
		if(!tweenPosition.enabled )
			tweenPosition.enabled = true;
		tweenPosition.duration = time;
		tweenPosition.from = from;
		tweenPosition.to = to;
		tweenPosition.Reset ();
		initPosition = to;
	}
	
	public void Scale(Vector3 to, float time) {
		Scale(transform.localScale,to,time);
	}
	
	public void Scale(Vector3 from, Vector3 to, float time) {
		iTween.ScaleTo (gameObject, iTween.Hash("x", to.x,"y",to.y,"time", 0.3f,"easetype","easeoutquad"));
	}
	
	void TweenPositionCallback() {
		if(tweenCallback != null) {
			tweenCallback(this);
		}
	}
	
	void SetPosition(int sortID) {
		gameObject.layer = GameLayer.IgnoreCard;
		actorSprite.depth = initDepth + sortID + 1;
		Vector3 pos = Battle.ChangeCameraPosition() - vManager.ParentPanel.transform.localPosition;
		Vector3 offset = new Vector3(sortID * (float)actorSprite.width / 2f , - sortID * (float)actorSprite.height / 2, 0f) - transform.parent.localPosition;
		transform.localPosition  = new Vector3(pos.x,pos.y,transform.localPosition.z) + offset ;
	}
	
	public void SetPos (Vector3 to) {
		transform.localPosition = to;
		
		initPosition = to;
	}
}
