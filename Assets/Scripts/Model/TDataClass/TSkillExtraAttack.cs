using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillExtraAttack : SkillBaseInfo {
	private SkillExtraAttack instance;
	public TSkillExtraAttack (object instance) : base (instance) {
		this.instance = instance as SkillExtraAttack;
		skillBase = this.instance.baseInfo;
	}
	
	public AttackInfo AttackValue (float attackValue, TUserUnit id) {
		AttackInfo ai = AttackInfo.GetInstance (); //new AttackInfo ();
		ai.AttackValue = attackValue * instance.attackValue;
		ai.AttackType = (int)instance.unitType;
		ai.AttackRange = 1;//attack all enemy
		ai.UserUnitID = id.MakeUserUnitKey();
		return ai;
	}
}