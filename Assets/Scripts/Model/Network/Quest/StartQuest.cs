using UnityEngine;
using System.Collections;
using bbproto;

public class StartQuestParam {
	public uint stageId;
	public uint questId;
	public uint helperUserId;
	public uint helperUniqueId;
	public int currPartyId;
}

public class StartQuest: ProtoManager {
	private bbproto.ReqStartQuest reqStartQuest;
	private bbproto.RspStartQuest rspStartQuest;
	private StartQuestParam questParam;

	public StartQuest(){
		MsgCenter.Instance.AddListener (CommandEnum.ReqStartQuest, OnReceiveCommand);
	}

	~StartQuest() {
		MsgCenter.Instance.RemoveListener (CommandEnum.ReqStartQuest, OnReceiveCommand);
	}

	public override bool MakePacket () {
//		LogHelper.Log ("StartQuest.MakePacket()...");

		Proto = Protocol.START_QUEST;
		reqType = typeof(ReqStartQuest);
		rspType = typeof(RspStartQuest);

		reqStartQuest = new ReqStartQuest ();
		reqStartQuest.header = new ProtoHeader ();
		reqStartQuest.header.apiVer = Protocol.API_VERSION;

		if (  GlobalData.userInfo != null )
			reqStartQuest.header.userId = GlobalData.userInfo.UserId;


		reqStartQuest.stageId = questParam.stageId;
		reqStartQuest.questId = questParam.questId;
		reqStartQuest.helperUserId = questParam.helperUserId;
		reqStartQuest.currentParty = 0;//questParam.currPartyId;
		TUserUnit userunit = GlobalData.userUnitList.GetMyUnit (questParam.helperUniqueId);
		if ( userunit != null )
			reqStartQuest.helperUnit = userunit.Object;

		LogHelper.Log ("helperUserId:{0} currParty:{1} userunit:{2}", reqStartQuest.helperUserId, reqStartQuest.currentParty, userunit);

		ErrorMsg err = SerializeData (reqStartQuest); // save to Data for send out
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		if (!success) { return; }

		rspStartQuest = InstanceObj as bbproto.RspStartQuest;
//		LogHelper.Log("reponse userId:"+rspStartQuest.user.userId);

		GlobalData.userInfo.StaminaNow = rspStartQuest.staminaNow;
		GlobalData.userInfo.StaminaRecover = rspStartQuest.staminaRecover;

		LogHelper.Log ("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);
		if(rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null ){
			TQuestDungeonData dungeonData = new TQuestDungeonData (rspStartQuest.dungeonData);
			//send response to caller
			MsgCenter.Instance.Invoke (CommandEnum.RspStartQuest, dungeonData);

		} else{
			MsgCenter.Instance.Invoke (CommandEnum.RspStartQuest, null);
		}

	}

	void OnReceiveCommand(object data) {
		questParam = data as StartQuestParam;
		if (questParam == null) {
			LogHelper.Log ("StartQuest: Invalid param data.");
			return;
		}

		LogHelper.Log ("OnReceiveCommand(StartQuest): stageId:{0} questId:{1} helperUserId:{2} helperUniqueId:{3} currParty:{4}",
			questParam.stageId, questParam.questId,questParam.helperUserId,questParam.helperUniqueId,questParam.currPartyId);

		Send (); //send request to server
	}

}
