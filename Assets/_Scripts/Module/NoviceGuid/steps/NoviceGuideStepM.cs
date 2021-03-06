using UnityEngine;
using System.Collections;

//city select
public class NoviceGuideStepM_1:NoviceGuidStep{
		
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepM_2);
		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide61_title"),TextCenter.GetText ("guide61_content"),TextCenter.GetText ("NEXT"),ClickOK);
		
	}
	
	private void ClickOK(object data){
		GameObject first = GameObject.FindWithTag ("city_one");
		if (first == null)
						return;
		NoviceGuideUtil.ForceOneBtnClick (first, OnClickItem as UICallback);
		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,1)});
	}
	
	private void OnClickItem(GameObject gm){
			UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
			NoviceGuideUtil.RemoveAllArrows ();

	}

	
}

//stage select
public class NoviceGuideStepM_2:NoviceGuidStep{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepM_3);
		
		GameObject first = GameObject.Find ("StageSelectWindow(Clone)").GetComponent<StageSelectView>().GetStageEvolveItem();
//		if(first == null)
//			stepEntity.GetStateMachine ().ChangeState (null);
		NoviceGuideUtil.ForceOneBtnClick (first,OnClickItem as UICallback);
//		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,1)});
	}

	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
//		NoviceGuideUtil.RemoveAllArrows ();
		
	}
	
	
}

//quest select
public class NoviceGuideStepM_3:NoviceGuidStep{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepM_5);
		
//		GameObject first = GameObject.Find ("QuestSelectWindow(Clone)").GetComponent<QuestSelectView>().GetDragItem(0);
//		NoviceGuideUtil.ForceOneBtnClick (first);
//		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,3)});
//		UIEventListenerCustom.Get (first).onClick += OnClickItem;
	}
	
	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		NoviceGuideUtil.RemoveAllArrows ();
		
	}

	
}


//quest select
public class NoviceGuideStepM_4:NoviceGuidStep{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepM_5);
//		GameObject first = GameObject.Find ("FriendSelectWindow(Clone)").GetComponent<FriendSelectView>().GetFriendItem(0);
//		NoviceGuideUtil.ForceOneBtnClick (first,OnClickItem as UICallback);
//		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,3)});
	}
	
	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		NoviceGuideUtil.RemoveAllArrows ();
		
	}

	
}

//fight ready
public class NoviceGuideStepM_5:NoviceGuidStep{
	
	public override void Enter()
	{
		nextState = null;
		GameObject first = GameObject.FindWithTag ("fight_btn");
		NoviceGuideUtil.ForceOneBtnClick (first,OnClickItem as UICallback);
		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,1)});
	}
	
	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		NoviceGuideUtil.RemoveAllArrows ();

		NoviceGuideStepManager.Instance.CurrentGuideStep = NoviceGuideStage.NONE;
	}
	
}