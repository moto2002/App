﻿using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillSingleAtkRecoverHP : ActiveSkill ,IActiveSkillExcute{
	private SkillSingleAtkRecoverHP instance;
	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}

	public TSkillSingleAtkRecoverHP(object instance) : base (instance) {
		this.instance = instance as SkillSingleAtkRecoverHP;
		skillBase = this.instance.baseInfo;
		initSkillCooling = skillBase.skillCooling;
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
	}

	public void RefreashCooling () {
		DisposeCooling ();
	}

	public object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
//		SkillSingleAtkRecoverHP ssarh = DeserializeData<SkillSingleAtkRecoverHP> ();
		AttackInfo ai = new AttackInfo ();
		ai.AttackType = (int)instance.unitType;

//		ai.AttackRange = 1; //(int)instance.attackType;

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
