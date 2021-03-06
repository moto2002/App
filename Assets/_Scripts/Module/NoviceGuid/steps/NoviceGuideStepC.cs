using UnityEngine;
using System.Collections;
using bbproto;

public class NoviceGuideStepC_1:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepC_2);

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_6"), "coor", new Vector3(-10, 240));

		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("quest_new"), new Vector3 (0, 40, 3),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
			GoToNextState();
		});

	}

}

//select friend
public class NoviceGuideStepC_2:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepC_3);

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_7"));

		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("battle_helper"), new Vector3 (0, 0, 1),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
			GoToNextState();
		});
		
	}

}

//select friend
public class NoviceGuideStepC_3:NoviceGuidStep{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepC_4);
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("friend_one"), new Vector3 (0, 20, 3),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
			GoToNextState();
		});
	}
}

//fight_ready
public class NoviceGuideStepC_4:NoviceGuidStep
{	
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepC_5);
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("fight_btn"), new Vector3 (0, 0, 1), true, true, o1 => {
			NoviceGuideUtil.RemoveAllArrows ();
			GoToNextState();
		});
	}
}
//boost skill
public class NoviceGuideStepC_5:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = null;

		UserUnit uu = DataCenter.Instance.UnitData.PartyInfo.CurrentParty.UserUnit [0];//.ActiveSkill
		SkillBase sbi = DataCenter.Instance.BattleData.GetSkill (uu.MakeUserUnitKey (), uu.UnitInfo.activeSkill, SkillType.ActiveSkill);
		sbi.ResetCooling();
		sbi.Excute ();

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_8"),"coor", new Vector3(-80,-70,1), "rotate",true);

		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("battle_leader"), new Vector3 (0, 0, 1),true,true,o=>{
			ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_9"),"coor", new Vector3(20, 68, 1));
				NoviceGuideUtil.RemoveAllArrows();
				
				MsgCenter.Instance.AddListener(CommandEnum.AttackEnemyEnd,OnSkillRelease);
				MsgCenter.Instance.AddListener(CommandEnum.BattleSkillPanel,OnBattleSkillShow);
			});
	}

	void OnBattleSkillShow(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleSkillPanel,OnBattleSkillShow);
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("boost_skill"), new Vector3 (0, 20, 3),true,true,o1=>{
			NoviceGuideUtil.RemoveAllArrows();
		});
	}

	void OnSkillRelease(object data){
		NoviceGuideStepManager.Instance.CurrentGuideStep = NoviceGuideStage.NoviceGuideStepC_BLANK;
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd,OnSkillRelease);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips",TextCenter.GetText ("guide5_content"));
	}
}
