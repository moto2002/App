﻿#region
// leiliang
// use to store enter battle need data. befoure enten battle. init data from disk or server. dont konw battle is contine or a new.
using System;

#endregion

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using bbproto;

public class BattleConfigData {

	public const byte startCardID = 0;
	public const byte endCardID = 4;
	public const byte cardPoolSingle = 5;
	public const byte cardCollectionCount = 5;
	public const byte cardSep = 13;
	public const byte cardDepth = 3;
	public static Vector3 cardPoolInitPosition = new Vector3(-255f,275f,0f);
	public int[] cardTypeID = new int[4] {1,2,3,7};

	private static BattleConfigData instance;

	public static BattleConfigData Instance {
		get {
			if(instance == null) {
				instance = new BattleConfigData ();
			}
			return instance;
		}
	}

	private BattleConfigData () { 
	
	}
	
	public Coordinate roleInitCoordinate;

	public TQuestDungeonData questDungeonData;
	
	public TQuestInfo currentQuestInfo;

	private TStageInfo _currentStageInfo;
	public TStageInfo currentStageInfo {
		set { _currentStageInfo = value; }// Debug.LogError("currentStageInfo : " + value + " id : " + value.ID) ; }
		get { return _currentStageInfo; }
	}

	public TFriendInfo BattleFriend;

	public int gotFriendPoint;  //After ClearQuest response from server, gotFriendPoint will be assigned.

	public bool NotDeadEnemy = false;

	private TUnitParty _party;
	public TUnitParty party {
		get { return _party; }
		set {
			_party = value;
			UnitParty up = _party == null ? null : _party.Object;
			WriteBuff<UnitParty>(unitPartyName, up);
		}
	}

	private AttackInfo _posionAttack = null;
	public AttackInfo posionAttack {
		get { return _posionAttack; }
		set { _posionAttack = value;
			AttackInfoProto aip = _posionAttack == null ? null : _posionAttack.Instance;
			WriteBuff<AttackInfoProto> (posionAttackName, aip);
		}
	}

	private AttackInfo _reduceHurtAttack = null;
	public AttackInfo reduceHurtAttack {
		get { return _reduceHurtAttack; }
		set { _reduceHurtAttack = value; 
			AttackInfoProto aip = _reduceHurtAttack == null ? null : _reduceHurtAttack.Instance;
			WriteBuff<AttackInfoProto> (reduceHurtName, aip); 
		}
	}

	private AttackInfo _reduceDefenseAttack = null;
	public AttackInfo reduceDefenseAttack {
		get { return _reduceDefenseAttack; }
		set { _reduceDefenseAttack = value;
			AttackInfoProto aip = _reduceDefenseAttack == null ? null : _reduceDefenseAttack.Instance;
//			Debug.LogError(aip.skillID);
			WriteBuff<AttackInfoProto>(reduceDefenseName, aip); 
		}
	}

	private AttackInfo _strengthenAttack = null;
	public AttackInfo strengthenAttack {
		get { return _strengthenAttack; }
		set { _strengthenAttack = value; 
			AttackInfoProto aip = _strengthenAttack == null ? null : _strengthenAttack.Instance;
//			Debug.LogError(aip.skillID);
			WriteBuff<AttackInfoProto>(strengthenAttackName, aip); 
		}
	}

	private TrapPosion _trapPoison = null;
	public TrapPosion trapPoison {
		get { return _trapPoison; }
		set { _trapPoison = value;
			TrapInfo ti = _trapPoison == null ? null :  _trapPoison.GetTrap;
			WriteBuff<TrapInfo>(trapPoisonName, ti);
		}
	}

	private EnvironmentTrap _trapEnvironment = null;
	public EnvironmentTrap trapEnvironment {
		get { return _trapEnvironment; }
		set { _trapEnvironment = value; 
			TrapInfo ti = _trapEnvironment == null ? null :  _trapEnvironment.GetTrap;
			WriteBuff<TrapInfo>(trapEnvironmentName, ti);
		}
	}

	private TStoreBattleData _storeBattleData;
	public TStoreBattleData storeBattleData {
		get { return _storeBattleData; }
	}

	private TEvolveStart _evolveInfo;
	public TEvolveStart evolveInfo {
		set {
			_evolveInfo = value;
		}
	}

//	private byte _gameState;
	public byte gameState {
		set { GameDataPersistence.Instance.StoreDataNoEncrypt(gameStateName, value); }
		get {
			byte _gameState = 0;
			if(GameDataPersistence.Instance.HasInfo(gameStateName)) {
				string info = GameDataPersistence.Instance.GetDataNoEncrypt(gameStateName);
				_gameState = byte.Parse(info);
			}
			return _gameState;
		}
	}

	public void ResetFromServer(TQuestDungeonData tdd) {
		InitStoreBattleData ();
		roleInitCoordinate = new Coordinate (MapConfig.characterInitCoorX, MapConfig.characterInitCoorY);//
		_storeBattleData.roleCoordinate = roleInitCoordinate;
		_storeBattleData.colorIndex = 0;
		questDungeonData = tdd;
		WriteFriend ();
		WriteQuestInfo ();
		WriteStageInfo ();
		WriteQuestDungeonData ();
	}

	void InitStoreBattleData() {
		StoreBattleData sbd = new StoreBattleData ();
		_storeBattleData = new TStoreBattleData (sbd);
		sbd.sp = DataCenter.maxEnergyPoint;
		sbd.hp = DataCenter.Instance.PartyInfo.CurrentParty.GetInitBlood ();
		sbd.xCoordinate = MapConfig.characterInitCoorX;
		sbd.yCoordinate = MapConfig.characterInitCoorY;
	}

	public void ResetFromDisk() {
		ReadFriend ();
		ReadQuestDungeonData ();
		ReadQuestInfo ();
		ReadStageInfo ();
		ReadAllBuff ();
		ReadRuntimeData ();
		roleInitCoordinate = _storeBattleData.roleCoordinate;
		if (_storeBattleData.colorIndex > 5) {
			_storeBattleData.colorIndex -= 5;	
		} else {
			_storeBattleData.colorIndex  = 0;
		}
	}

	public void StoreMapData () {
		WriteAllBuff ();
		StoreRuntimData ();
	}

	public void StoreQuestDungeonData(TQuestDungeonData tqdd) {
		questDungeonData = tqdd;

		WriteQuestDungeonData ();
	}
	
	void ReadRuntimeData () {
		byte[] runtimeData = ReadFile (storeBattleName);
		StoreBattleData qi = ProtobufSerializer.ParseFormBytes<StoreBattleData> (runtimeData);
//		Debug.LogError ("ReadRuntimeData : " + qi.sp + " hp : " + qi.hp); 
		_storeBattleData = new TStoreBattleData (qi);
	}

	void StoreRuntimData () {
//		Debug.LogError ("StoreRuntimData : " + _storeBattleData.instance.hp);
		byte[] battleData = ProtobufSerializer.SerializeToBytes<StoreBattleData> (_storeBattleData.instance);
		WriteToFile (battleData, storeBattleName);
	}

	public void StoreData (uint questID) {
		int id = (int)questID;
		GameDataPersistence.Instance.StoreIntDatNoEncypt (GameDataPersistence.battleStore, id);
//		Debug.LogError ("StoreData : " + id);
	}

	public void ClearData () {
		GameDataPersistence.Instance.StoreIntDatNoEncypt (GameDataPersistence.battleStore, 0);
//		Debug.LogError ("ClearData : " + GameDataStore.Instance.GetIntDataNoEncypt (GameDataStore.battleStore));
	}

	public int hasBattleData () {
		return GameDataPersistence.Instance.GetIntDataNoEncypt (GameDataPersistence.battleStore);
	}

	public void ClearActiveSkill() {
		posionAttack = null;
		reduceHurtAttack = null;
		reduceDefenseAttack = null;
		strengthenAttack = null;
	}

	private const string floderPath = "/Battle/";
	public const string isBattle = "/true";
	public const string friendFileName = "/Friend";
	public const string questDungeonDataName = "/DungeonData";
	public const string questInfoName = "/Quest";
	public const string stageInfoName = "/Stage";
	public const string storeBattleName = "/StoreBattle";
	public const string posionAttackName = "/Posion";
	public const string reduceHurtName = "/ReduceHurt";
	public const string reduceDefenseName = "/ReduceDefense";
	public const string strengthenAttackName = "/StrengthenAttack";
	public const string trapPoisonName = "/TrapPoison";
	public const string trapEnvironmentName = "/TrapEnvironment";
	public const string unitPartyName = "/UnitParty";

	public const string gameStateName = "GameState";

	public void Init(){
		questDungeonData.currentFloor = _storeBattleData.questData.Count > 0 ? _storeBattleData.questData.Count - 1 : 0;

	}

	string GetPath (string path) {
		return Application.persistentDataPath + path;
	}

	void WriteAllBuff() {
		AttackInfoProto attack = null;
		if (posionAttack != null) {
			attack = posionAttack.Instance;
		}
		WriteBuff<AttackInfoProto> (posionAttackName, attack);
		attack = null;

		if (reduceHurtAttack != null) {
			attack = reduceHurtAttack.Instance;
		}
		WriteBuff<AttackInfoProto> (reduceHurtName, attack);
		attack = null;

		if (reduceDefenseAttack != null) {
			attack = reduceDefenseAttack.Instance;
		}
		WriteBuff<AttackInfoProto> (reduceDefenseName, attack);
		attack = null;

		if (strengthenAttack != null) {
			attack = strengthenAttack.Instance;
		}
		WriteBuff<AttackInfoProto> (strengthenAttackName, attack);
		attack = null;
	}

	void ReadAllBuff() {
		_posionAttack = ReadBuff<AttackInfo, AttackInfoProto> (posionAttackName);
		_reduceHurtAttack = ReadBuff<AttackInfo, AttackInfoProto> (reduceHurtName);
		_reduceDefenseAttack = ReadBuff<AttackInfo, AttackInfoProto> (reduceDefenseName);
		_strengthenAttack = ReadBuff<AttackInfo, AttackInfoProto> (strengthenAttackName);
//		_trapPoison = ReadBuff<TrapPosion, TrapInfo> (trapPoisonName);
//		_trapEnvironment = ReadBuff<EnvironmentTrap, TrapInfo> (trapEnvironmentName);
		_party = ReadBuff<TUnitParty, UnitParty> (unitPartyName);

		if (File.Exists (trapPoisonName)) {
			_trapPoison = Activator.CreateInstance(typeof(TrapPosion), ProtobufSerializer.ParseFormBytes<TrapInfo> (ReadFile (trapPoisonName))) as TrapPosion;
		}
		if (File.Exists (trapEnvironmentName)) {
			_trapEnvironment = Activator.CreateInstance(typeof(EnvironmentTrap), ProtobufSerializer.ParseFormBytes<TrapInfo> (ReadFile (trapEnvironmentName))) as EnvironmentTrap;
		}


	}

	void WriteBuff<T>(string name, T buff) where T : ProtoBuf.IExtensible {
		if (string.IsNullOrEmpty (name)) {
			return;	
		}
	
		if (buff == null) {
			string path = GetPath (name);
			try {
				File.Delete(path);
			} catch (System.Exception ex) {
				Debug.LogError("WriteBuff ex : " + ex.Message);
			}
			return;
		}
//		Debug.LogError (" WriteBuff<T> : " + name);
		byte[] attack = ProtobufSerializer.SerializeToBytes<T> (buff);
		WriteToFile (attack, name);
	}

	T ReadBuff<T,T1> (string name) where T : ProtobufDataBase where T1 : ProtoBuf.IExtensible {
		if (string.IsNullOrEmpty (name)) {
			return null;	
		}
		string path = GetPath (name);
		if (!File.Exists (path)) {
			return null;	
		}

		byte[] attackInfo = ReadFile (name);
		T1 aip = ProtobufSerializer.ParseFormBytes<T1> (attackInfo);
		T t = Activator.CreateInstance(typeof(T), aip) as T;
//		Debug.LogError ("t : " + t);
		return t;
	}


	
	//stage
	public void WriteStageInfo() {
//		Debug.LogError ("WriteStageInfo currentStageInfo");
		if (currentStageInfo == null)
			return;

//		Debug.LogError ("WriteStageInfo currentStageInfo : " + currentStageInfo);
		byte[] stage = ProtobufSerializer.SerializeToBytes<StageInfo> (currentStageInfo.stageInfo);
		WriteToFile (stage, stageInfoName);
	}

	void ReadStageInfo() {
		byte[] stageInfo = ReadFile (stageInfoName);
		if (stageInfo == null) {
			return;	
		}
		StageInfo qi = ProtobufSerializer.ParseFormBytes<StageInfo> (stageInfo);
		currentStageInfo = new TStageInfo (qi);
	}
	//end

	//quest info
	public void WriteQuestInfo() {
		if (currentQuestInfo == null)
			return;
		byte[] quest = ProtobufSerializer.SerializeToBytes<QuestInfo> (currentQuestInfo.questInfo);
		WriteToFile (quest, questInfoName);
	}

	void ReadQuestInfo() {
		byte[] friend = ReadFile (questInfoName);
		if (friend == null) {
			return;	
		}
		QuestInfo qi = ProtobufSerializer.ParseFormBytes<QuestInfo> (friend);
		currentQuestInfo = new TQuestInfo (qi);
	}
	//end

	//dungeonData
	public void WriteQuestDungeonData () {
//		Debug.LogError("WriteQuestDungeonData  : " + questDungeonData.Instance.hp)
		
		byte[] tdd = ProtobufSerializer.SerializeToBytes<QuestDungeonData> (questDungeonData.Instance);
		WriteToFile (tdd, questDungeonDataName);
	}

	void ReadQuestDungeonData() {
		byte[] questData = ReadFile (questDungeonDataName);
		QuestDungeonData qdd = ProtobufSerializer.ParseFormBytes<QuestDungeonData> (questData);
		questDungeonData = new TQuestDungeonData (qdd);
	}
	//end

	//friend
	public void WriteFriend() {
		if (BattleFriend == null)
			return;
		byte[] friend = ProtobufSerializer.SerializeToBytes<FriendInfo>(BattleFriend.Instance);
		WriteToFile (friend, friendFileName);
	}

	void ReadFriend() {
		byte[] friend = ReadFile (friendFileName);
		if (friend == null) {
			return;	
		}
		FriendInfo fi = ProtobufSerializer.ParseFormBytes<FriendInfo> (friend);
		BattleFriend = new TFriendInfo (fi);
	}
	//end 

	void DeleteAndWrite(string fileName){
		if (File.Exists (fileName)) {
			File.Delete(fileName);
		}
	}

	void WriteToFile(byte[] data, string fileName){
		string path = GetPath (fileName);
		DeleteAndWrite (path);
		try {
			FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
			fs.Write(data, 0, data.Length);
			fs.Close();
			fs.Dispose();
//			Debug.LogError("write to file success : " + fileName);
		} catch (System.Exception ex) {
			Debug.LogError("WriteToFile exception : " + ex.Message);
		}
	}

	byte[] ReadFile(string fileName) {
		string path = GetPath (fileName);
		byte[] data = null;
		try {
			FileStream fs = new FileStream (path, FileMode.Open, FileAccess.Read);
			byte[] readData = new byte[fs.Length];
			fs.Read (readData, 0, (int)fs.Length);
			fs.Close ();
			fs.Dispose ();
			data = readData;
		} catch (System.Exception ex) {
			Debug.LogError ("ReadFile exception : " + ex.Message);
		}

		return data;
	}

	public int GetMapID () {
//		Debug.LogError ("currentStageInfo == null :  " + (currentStageInfo == null));
		if (currentStageInfo == null || NoviceGuideStepEntityManager.isInNoviceGuide()) {
			return 3; //2 is default stage id.
		} else {
			int stageID = ((int)currentStageInfo.ID) % 10;
			if (BattleConfigData.Instance.currentStageInfo.CityId == 1) {	
				return stageID == 1 ? 7 : -- stageID;
			} else {
				return stageID;
			}
		}
	}

	public void ResetRoleCoordinate(){
		_storeBattleData.roleCoordinate = roleInitCoordinate;
	}

	public void InitRoleCoordinate(Coordinate coor){
		roleInitCoordinate = coor;
		_storeBattleData.roleCoordinate = coor;

	}

	public void RefreshCurrentFloor(RspRedoQuest rrq){
		storeBattleData.questData.RemoveAt (storeBattleData.questData.Count - 1);
		ClearQuestParam cq = new ClearQuestParam ();
		TClearQuestParam cqp = new TClearQuestParam (cq);
		storeBattleData.questData.Add (cqp);
		TQuestDungeonData tqdd = new TQuestDungeonData (rrq.dungeonData);
		int floor = questDungeonData.currentFloor;
		List<TQuestGrid> reQuestGrid = tqdd.Floors[floor];
		questDungeonData.Floors [floor] = reQuestGrid;
		questDungeonData.Boss = tqdd.Boss;
	}

	
	public int ResumeColorIndex(){
		int i = questDungeonData.Colors [storeBattleData.colorIndex];
		storeBattleData.colorIndex++;
//		Debug.Log ("index: " + storeBattleData.colorIndex);
		return i;
	}
}
