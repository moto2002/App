﻿using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveReduceDefense : ActiveSkill, IActiveSkillExcute {
	private SkillReduceDefence instance;
	public ActiveReduceDefense(object instance) : base (instance) {
		this.instance = instance as SkillReduceDefence;

		skillBase = this.instance.baseInfo;	
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
	}
	TClass<uint, int, float> tc;

	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}

	public void RefreashCooling () {
		DisposeCooling ();
	}

	public object Excute (uint userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
//		Debug.LogError("ActiveReduceDefense excute ");
//		SkillReduceDefence srd = DeserializeData<SkillReduceDefence> ();
		tc = new TClass<uint, int, float> ();
		tc.arg1 = userUnitID;
		tc.arg2 = (int)instance.period;
		tc.arg3 = instance.value;
		MsgCenter.Instance.Invoke (CommandEnum.ReduceDefense, tc);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
		return null;
	}

	void EnemyAttackEnd(object data) {
//		Debug.LogError ("excute EnemyAttackEnd");
		tc.arg2 --;
		MsgCenter.Instance.Invoke (CommandEnum.ReduceDefense, tc);
		if (tc.arg2 <= 0) {
//			Debug.LogWarning("remove EnemyAttackEnd");
			MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
			tc = null;
		}
	}

}
