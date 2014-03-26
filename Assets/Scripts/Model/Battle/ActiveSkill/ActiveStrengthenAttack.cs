using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveStrengthenAttack : ActiveSkill, IActiveSkillExcute {
	private SkillStrengthenAttack instance;
	public ActiveStrengthenAttack (object instance) : base (instance) {
		this.instance = instance as SkillStrengthenAttack;
		skillBase = this.instance.baseInfo;
		if (skillBase.skillCooling == 0) {
			coolingDone = true;	
		}
	}

	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}
	
	public void RefreashCooling () {
		DisposeCooling ();
	}
	AttackInfo ai = null;
	public object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
//		SkillStrengthenAttack ssa = DeserializeData<SkillStrengthenAttack> ();
		ai = new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.AttackType = (int)instance.targetType;
		ai.AttackRace = (int)instance.targetRace;
		ai.AttackValue = instance.value;
		ai.AttackRound = instance.periodValue;
		MsgCenter.Instance.Invoke(CommandEnum.StrengthenTargetType, ai);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
		ai.AttackRound --;
		return ai;
	}

	void EnemyAttackEnd(object data) {
		if (ai == null) {
			return;	
		}
		if (ai.AttackRound <= 0) {
			MsgCenter.Instance.Invoke(CommandEnum.StrengthenTargetType, ai);
			MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
		}
		else{
			MsgCenter.Instance.Invoke(CommandEnum.StrengthenTargetType, ai);
			ai.AttackRound--;
		}
	}
}
