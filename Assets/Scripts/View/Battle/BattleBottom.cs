using UnityEngine;
using System.Collections.Generic;

public class BattleBottom : MonoBehaviour {
	private Camera bottomCamera;
	private RaycastHit rch;
	private TUnitParty upi;
	private Dictionary<int,GameObject> actorObject = new Dictionary<int,GameObject>();
	private GameObject battleSkillObject;
	private BattleSkill battleSkill;
//	public BattleSkill BattleSkillIns{
//		get { return battleSkill; }
//	}

	public static bool notClick = false;

//	[HideInInspector]
	private BattleQuest _battleQuest;
	public BattleQuest battleQuest {
		set {
			_battleQuest = value;
			battleSkill.battleQuest = battleQuest;
		}
		get{
			return _battleQuest;
		}
	}

	public void Init(Camera bottomCamera) {
		this.bottomCamera = bottomCamera;
		battleSkillObject = Resources.Load("Prefabs/BattleSkill") as GameObject;
		battleSkillObject = NGUITools.AddChild (ViewManager.Instance.CenterPanel, battleSkillObject);
		battleSkill = battleSkillObject.GetComponent<BattleSkill> ();
		battleSkill.Init ("BattleSkill");
//		battleSkill.battleQuest = battleQuest;
		battleSkillObject.SetActive (false);

		if (upi == null) {
			upi = DataCenter.Instance.PartyInfo.CurrentParty; 
		}
		Dictionary<int,TUserUnit> userUnitInfo = upi.UserUnit;
		for (int i = 0; i < 5; i++) {

			GameObject temp = transform.Find("Actor/" + i).gameObject;	
			if(userUnitInfo[i] == null) {
				temp.gameObject.SetActive(false);
				continue;
			}

			TUnitInfo tui = userUnitInfo[i].UnitInfo;
			temp.renderer.material.SetTexture("_MainTex",tui.GetAsset(UnitAssetType.Profile));
			UITexture tex =  transform.Find("ActorP/" + i).GetComponent<UITexture>();
			tex.color =  DGTools.TypeToColor(tui.Type);
		}

		List<int> haveInfo = new List<int> (userUnitInfo.Keys);
		for (int i = 0; i < 5; i++) {
			if(!haveInfo.Contains(i)) {
				actorObject[i].SetActive(false);
			}
		}
	}

	void OnDestroy() {
		if(battleSkill != null) 
			Destroy (battleSkill.gameObject);
		battleSkill = null;
	} 

	void OnDisable () {
		GameInput.OnUpdate -= OnRealease;
	}

	void OnEnable () {
		GameInput.OnUpdate += OnRealease;
	}

	void OnRealease () {
		if (notClick) {
			return;	
		}

		if (Input.GetMouseButtonDown (0)) {
			Ray ray = bottomCamera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out rch,100f,GameLayer.Bottom << 31)) {
				string name = rch.collider.name;
				CheckCollider(name);
			}	
		}
	}

	TUserUnit tuu;
	void CheckCollider (string name) {
		if (upi == null || battleQuest.role.isMove) {
			Debug.LogError("upi is null");
			return;	
		}
//		try{
			int id = System.Int32.Parse (name);
			if (upi.UserUnit.ContainsKey (id)) {
				foreach (var item in actorObject.Values) {
					if(item.name == name) {
						item.renderer.material.color = Color.white;
						continue;
					}
					item.renderer.material.color = Color.gray;
				}

				tuu = upi.UserUnit [id];
				battleSkillObject.SetActive(true);
				battleSkill.Refresh(tuu, Boost, Close);
				BattleMap.waitMove = true;
				battleQuest.battle.ShieldGameInput(false);
			}
//		}
//		catch(System.Exception ex) {
//			Debug.LogError("exception : " + ex.Message + " name : " + name);
//		}
	}

	public void Boost() {
		CloseSkillWindow ();
		MsgCenter.Instance.Invoke(CommandEnum.LaunchActiveSkill, tuu);
	}

	public void Close () {
		CloseSkillWindow ();
	}

	void CloseSkillWindow () {
		foreach (var item in actorObject.Values) {
			item.renderer.material.color = Color.white;
		}

		BattleMap.waitMove = false;
		if (battleQuest.battle.isShowEnemy) {
			battleQuest.battle.ShieldGameInput(true);
		}
		battleSkillObject.SetActive(false);
	}
}
