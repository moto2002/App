using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveSkill : SkillBaseInfo, IActiveSkillExcute {
	protected int initSkillCooling = 0;
	protected bool coolingDone = false;
	public ActiveSkill (object instance) : base (instance) { 
		configBattleUseData = ConfigBattleUseData.Instance;
	}

	~ActiveSkill () {

	}

	public void RefreashCooling () {
		DisposeCooling ();
	}

	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}

	private string skillStoreID;
	private ConfigBattleUseData configBattleUseData;
	public void StoreSkillCooling (string id) {
		skillStoreID = id;
		if (configBattleUseData.hasBattleData() > 0) {
				ReadSkillCooling ();
		} else {
			Store();
		}
	}

	void Store() {
		GameDataStore.Instance.StoreIntDatNoEncypt(skillStoreID, skillBase.skillCooling);
	}

	void ReadSkillCooling () {
		skillBase.skillCooling = GameDataStore.Instance.GetIntDataNoEncypt (skillStoreID);
	}

	protected void DisposeCooling () {
		bool temp = coolingDone;
		coolingDone = CheckCooling (skillBase);
		if (!temp && coolingDone) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_as_activate);	
		}
		Store ();
	}

	private bool CheckCooling(SkillBase sb) {
		if (sb.skillCooling == 0) {
			return true;
		}
		sb.skillCooling --;
		if (sb.skillCooling == 0) {
			return true;
		} 
		else {
			return false;
		}
	}

	protected void InitCooling() {
		skillBase.skillCooling = initSkillCooling;
		if (skillBase.skillCooling > 0) {
			coolingDone = false;
		}

		Store ();
	}

	public virtual AttackInfo ExcuteByDisk (AttackInfo ai) {
		return null;
	}

	public virtual object Excute (string userUnitID, int atk = -1) {
		return null;
	}
}
