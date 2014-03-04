using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class BattleQuest : UIBase {
//	public int MapWidth {
//		get{ return mapConfig.mapXLength; }
//	}

//
//	public int MapWidth {
//		get{ return questDungeonData.Floors.Count; }
//	}
//
//	public int MapHeight {
//		get{ return questDungeonData.Floors[0]; }
//	}

	private Coordinate roleInitPosition = new Coordinate();
	public Coordinate RoleInitPosition {
		get { 
			if(roleInitPosition.x != MapConfig.characterInitCoorX) {
				roleInitPosition.x = MapConfig.characterInitCoorX;
				roleInitPosition.y = MapConfig.characterInitCoorY;
			}
			return  roleInitPosition;
		}
	}

	private GameObject rootObject;
//	public static MapConfig mapConfig;
//	private SingleMapData currentMapData;

	private TQuestGrid currentMapData;
	public static TQuestDungeonData questDungeonData;

	private BattleMap battleMap;
	private Role role;
	private Battle battle;
	private BattleBackground background;
	public static BattleUseData bud;
	private Camera mainCamera;
	private BossAppear bossAppear;
	private int questFloor = 0;

	string backgroundName = "BattleBackground";

	public BattleQuest (string name) : base(name) {

		InitData ();
		rootObject = NGUITools.AddChild(viewManager.ParentPanel);
		string tempName = "Map";
		battleMap = viewManager.GetBattleMap(tempName) as BattleMap;
		battleMap.transform.localPosition = new Vector3 (-1100f, 0f, 0f);
		battleMap.BQuest = this;
		Init(battleMap,tempName);
		tempName = "Role";
		role = viewManager.GetBattleMap(tempName) as Role;
		role.BQuest = this;
		Init(role,tempName);
		background = viewManager.GetViewObject(backgroundName) as BattleBackground;
		background.transform.parent = viewManager.CenterPanel.transform.parent;
		background.transform.localPosition = Vector3.zero;
		background.Init (backgroundName);
		AddSelfObject (battleMap);
		AddSelfObject (role);
		AddSelfObject (background);
	}

	void InitData() {
//		mapConfig = ModelManager.Instance.GetData (ModelEnum.MapConfig,new ErrorMsg()) as MapConfig; //new MapConfig (); //

		questDungeonData = ModelManager.Instance.GetData (ModelEnum.MapConfig,new ErrorMsg()) as TQuestDungeonData;
	}

	void Init(UIBaseUnity ui,string name) {
		ui.Init(name);
	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void ShowUI () {
		Resources.UnloadUnusedAssets ();
		bud = new BattleUseData ();
		mainCamera = Camera.main;
		mainCamera.clearFlags = CameraClearFlags.Depth;
		mainCamera.enabled = false;
		GameTimer.GetInstance ().AddCountDown (0.5f, ShowScene);
		InitData ();
		base.ShowUI ();
		AddListener ();
		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData);
		if (bossAppear == null) {
			CreatBoosAppear();
		}

		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
	}

//	void ResetScene()

	public override void HideUI () {
		battleEnemy = false;
		bud.RemoveListen ();
		bud = null;
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		RemoveListener ();
		base.HideUI ();
		
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
	}

	void Reset () {
		battleEnemy = false;
		bud.RemoveListen ();
		bud = new BattleUseData ();
//		mainCamera = Camera.main;
//		mainCamera.clearFlags = CameraClearFlags.Depth;
		battleMap.HideUI ();
		role.HideUI ();
		background.HideUI ();
		mainCamera.enabled = false;
		battleMap.ShowUI ();
		role.ShowUI ();
		background.ShowUI ();
		GameTimer.GetInstance ().AddCountDown (1f, ShowScene);
		InitData ();
		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData);
		if (bossAppear == null) {
			CreatBoosAppear();
		}
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		bossAppear.DestoryUI ();
	}

	void CreatBoosAppear () {
		GameObject obj = Resources.Load("Prefabs/BossAppear") as GameObject;
		Vector3 pos = obj.transform.localPosition;
		GameObject go = NGUITools.AddChild (viewManager.BottomPanel, obj);
		go.transform.localPosition = pos;
		bossAppear = go.GetComponent<BossAppear> ();
		bossAppear.Init("BossAppear");
	}

	void ShowScene () {
		mainCamera.enabled = true;
	}
	
	public Vector3 GetPosition(Coordinate coor) {
		return battleMap.GetPosition(coor.x, coor.y);
	}

	public void TargetItem(Coordinate coor) {
		role.StartMove(coor);
	}
	  
	void Exit() {
		controllerManger.ExitBattle();
		UIManager.Instance.ExitBattle();
	}

	bool battleEnemy = false;
	public void ClickDoor () {
//		Debug.LogError ("ClickDoor : " + questFloor + " mapConfig.floor : " + mapConfig.floor);
//		if (questFloor == mapConfig.floor) {
		if(questFloor == questDungeonData.Floors.Count - 1){
			QuestStop ();
		} else {
			EnterNextFloor();
		}
	}

	void EnterNextFloor () {
		questFloor ++;
		Reset ();

	}

	void QuestStop () {
		bossAppear.PlayBossAppera (MeetBoss);
		role.Stop();
		MsgCenter.Instance.Invoke(CommandEnum.MeetEnemy, true);
		battleEnemy = true;
	}

	void QuestEnd () { }

	public void RoleCoordinate(Coordinate coor) {
		if(!battleMap.ReachMapItem (coor)) {
			if(coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
				battleMap.RotateAnim(null);
				return;
			}

			int index = coor.x - 1 + coor.y * 5;
			if(coor.y == 0 && coor.x < 2) {
				index ++;
			}
			currentMapData =  questDungeonData.Floors[questFloor][index];  //mapConfig.mapData[coor.x,coor.y];
			role.Stop();
//			Debug.LogError("ContentType : " + currentMapData.ContentType);
			MsgCenter.Instance.Invoke(CommandEnum.MeetEnemy, true);
			switch (currentMapData.Type) {
			case EQuestGridType.Q_NONE:
				battleMap.waitMove = true;
				battleMap.RotateAnim(MapItemNone);
				break;
			case EQuestGridType.Q_ENEMY:
				battleMap.waitMove = true;
				battleMap.RotateAnim(MapItemEnemy);
				break;
//			case MapItemEnum.key:
//				battleMap.waitMove = true;
//				battleMap.RotateAnim(MapItemKey);
//				break;
			case EQuestGridType.Q_TREATURE:				
				battleMap.waitMove = true;
				battleMap.ShowBox();
				battleMap.RotateAnim(MapItemCoin);
				break;
			case EQuestGridType.Q_TRAP:
				battleMap.waitMove = true;
				battleMap.RotateAnim(MapItemTrap);
				break;
//			case MapItemEnum.Exclamation : 
//				battleMap.waitMove = true;
//				battleMap.RotateAnim(MapItemExclamation);
//				break;
			default:
					break;
			}
		}
	}

	void MeetBoss () {
		battleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = questDungeonData.Boss; //bud.GetEnemyInfo(mapConfig.BossID);
		battle.ShowEnemy(temp);
	}

	void MapItemExclamation() {
		battleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}
	
	void MapItemTrap() {
		battleMap.waitMove = false;
//		TrapBase tb = GlobalData.trapInfo[currentMapData.TypeValue];
		TrapBase tb = currentMapData.TrapInfo;
		MsgCenter.Instance.Invoke(CommandEnum.MeetTrap, tb);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemCoin() {
		battleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.MeetCoin, currentMapData);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemKey() {
		battleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.OpenDoor, null);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemNone () {
		battleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemEnemy() {
		battleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = currentMapData.Enemy; //bud.GetEnemyInfo(currentMapData.MonsterID);
		battle.ShowEnemy(temp);
	}

	void ShowBattle() {
		if(battle == null) {	
			battle = new Battle("Battle"); 
			battle.CreatUI();
		}

		if(battle.GetState == UIState.UIShow)
			return;

		battle.ShowUI();
	}

	void BattleEnd(object data) {
		if (battleEnemy) {
			GameObject obj = Resources.Load("Prefabs/Victory") as GameObject;
			Vector3 tempScale = obj.transform.localScale;
			obj = NGUITools.AddChild(viewManager.CenterPanel,obj);
			obj.transform.localScale = tempScale;
			VictoryEffect ve = obj.GetComponent<VictoryEffect>();
			ve.Init("Victory");
			ve.PlayAnimation(QuestEnd,new VictoryInfo(100,0,0,100));
		}
	}

	void AddListener () {
		MsgCenter.Instance.AddListener (CommandEnum.BattleBaseData, BattleBase);
	}

	void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleBaseData, BattleBase);
	}

	void BattleBase (object data) {
		BattleBaseData bbd = (BattleBaseData)data;
		background.InitData (bbd.Blood, bbd.EnergyPoint);
	}
}
