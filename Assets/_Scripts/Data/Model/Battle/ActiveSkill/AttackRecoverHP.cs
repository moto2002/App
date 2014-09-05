﻿using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillAttackRecoverHP : ActiveSkill {
	private SkillAttackRecoverHP instance;
//	public bool CoolingDone {
//		get {
//			return coolingDone;
//		}
//	}

	public TSkillAttackRecoverHP(object instance) : base (instance) {
//		skillBase = DeserializeData<SkillSingleAtkRecoverHP> ().baseInfo;	
		this.instance = instance as SkillAttackRecoverHP;
		skillBase = this.instance.baseInfo;
//		initSkillCooling = skillBase.skillCooling;
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
	}

//	public void RefreashCooling () {
//		DisposeCooling ();
//	}

	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
//		SkillSingleAtkRecoverHP ssarh = DeserializeData<SkillSingleAtkRecoverHP> ();
		AttackInfo ai = AttackInfo.GetInstance (); //new AttackInfo ();
		ai.AttackType = (int)instance.unitType;
		ai.AttackRange = (int)instance.attackType;
		ai.UserUnitID = userUnitID;
		ai.SkillID = skillBase.id;
		if (instance.type == EValueType.MULTIPLE) {
			ai.AttackValue = atk * instance.value;		
		} else if(instance.type == EValueType.FIXED) {
			ai.AttackValue = instance.value;
		}
		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillDrawHP, null);
		return ai;
	}

	


}