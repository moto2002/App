package quest

import (
	"fmt"
	"net/http"
)

import (
	"bbproto"
	"common"
	"common/EC"
	"common/Error"
	"common/log"
	"data"
	"model/quest"
	"model/user"
	"model/friend"

	"code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func StartQuestHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqStartQuest
	rspMsg := &bbproto.RspStartQuest{}

	handler := &StartQuest{}
	e := handler.ParseInput(req, &reqMsg)
	if e.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, Error.New(EC.INVALID_PARAMS, e.Error())))
		return
	}

	e = handler.verifyParams(&reqMsg)
	if e.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, e))
		return
	}

	// game logic

	e = handler.ProcessLogic(&reqMsg, rspMsg)

	e = handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, e))
	log.T("sendrsp err:%v, rspMsg:%+v.\n===========================================", e, rspMsg.Header)
}

/////////////////////////////////////////////////////////////////////////////

type StartQuest struct {
	bbproto.BaseProtoHandler
}

func (t StartQuest) FillResponseMsg(reqMsg *bbproto.ReqStartQuest, rspMsg *bbproto.RspStartQuest, rspErr Error.Error) (outbuffer []byte) {
	// fill protocol header
	{
		rspMsg.Header = reqMsg.Header //including the sessionId
		rspMsg.Header.Code = proto.Int(rspErr.Code())
		rspMsg.Header.Error = proto.String(rspErr.Error())
	}

	// serialize to bytes
	outbuffer, err := proto.Marshal(rspMsg)
	if err != nil {
		return nil
	}
	return outbuffer
}

func (t StartQuest) verifyParams(reqMsg *bbproto.ReqStartQuest) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.StageId == nil || reqMsg.QuestId == nil ||
		reqMsg.HelperUserId == nil || reqMsg.HelperUnit == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	//!!IMPORTANT!!: client protobuf.net cannot serialize when value='0', so convert nil to 0.
	if reqMsg.CurrentParty == nil {
		reqMsg.CurrentParty = proto.Int32(0)
	}

	if *reqMsg.Header.UserId == 0 || *reqMsg.StageId == 0 || *reqMsg.QuestId == 0 ||
		*reqMsg.HelperUserId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	return Error.OK()
}

func (t StartQuest) ProcessLogic(reqMsg *bbproto.ReqStartQuest, rspMsg *bbproto.RspStartQuest) (e Error.Error) {
	cost := &common.Cost{}
	cost.Begin()

	stageId := *reqMsg.StageId
	questId := *reqMsg.QuestId
	uid := *reqMsg.Header.UserId

	db := &data.Data{}
	err := db.Open("")
	defer db.Close()
	if err != nil {
		return Error.New(EC.CONNECT_DB_ERROR, err.Error())
	}

	//get userinfo from user table
	userDetail, isUserExists, err := user.GetUserInfo(db, uid)
	if err != nil {
		return Error.New(EC.EU_GET_USERINFO_FAIL, fmt.Sprintf("GetUserInfo failed for userId %v. err:%v", uid, err.Error()))
	}
	if !isUserExists {
		return Error.New(EC.EU_USER_NOT_EXISTS, fmt.Sprintf("userId: %v not exists", uid))
	}
	log.T(" getUser(%v) ret userinfo: %v", uid, userDetail.User)

	// check user is already playing
	isRestartNewQuest := (reqMsg.RestartNew != nil && *reqMsg.RestartNew != 0)
	if isRestartNewQuest == false {
		if userDetail.Quest != nil && userDetail.Quest.State != nil {
			e = Error.New(EC.EQ_QUEST_IS_PLAYING, fmt.Sprintf("user(%v) is playing quest:%v", *userDetail.User.UserId, *userDetail.Quest.QuestId))
			log.T(e.Error())
			return e
		}
	}else { // start a new quest (discard old playing quest)
		if userDetail.Quest != nil {
			userDetail.Quest = nil
		}
	}

	//check Quest record for QuestState
	questState, e := quest.CheckQuestRecord(db, stageId, questId, uid)
	if e.IsError() {
		return e
	}

	//get questInfo
	stageInfo, e := quest.GetStageInfo(db, stageId)
	if e.IsError() {
		return e
	}

	questInfo, e := quest.GetQuestInfo(db, stageInfo, questId)
	if e.IsError() {
		return e
	}
	if questInfo == nil {
		return Error.New(EC.EQ_GET_QUESTINFO_ERROR, "GetQuestInfo ret ok, but result is nil.")
	}
	log.T("questInfo:%+v", questInfo)

	//update stamina
	log.T("--Old Stamina:%v staminaRecover:%v", *userDetail.User.StaminaNow, *userDetail.User.StaminaRecover)
	e = user.RefreshStamina(userDetail.User.StaminaRecover, userDetail.User.StaminaNow, *userDetail.User.StaminaMax)
	if e.IsError() {
		return e
	}
	log.T("--New StaminaNow: %v -> %v staminaRecover:%v",
		*userDetail.User.StaminaNow, *userDetail.User.StaminaNow-*questInfo.Stamina, *userDetail.User.StaminaRecover)

	if *userDetail.User.StaminaNow < *questInfo.Stamina {
		return Error.New(EC.EQ_STAMINA_NOT_ENOUGH, "stamina is not enough")
	}
	*userDetail.User.StaminaNow -= *questInfo.Stamina

	//get quest config
	questConf, e := quest.GetQuestConfig(db, questId)
	log.T(" questConf:%+v", questConf)
	if e.IsError() {
		return e
	}

	questDataMaker := &quest.QuestDataMaker{}
	//make quest data
	questData, e := questDataMaker.MakeData(&questConf)
	if e.IsError() {
		return e
	}

	//TODO:try getFriendState(helperUid) -> getFriendPoint

	//update latest quest record of userDetail
	if e = quest.FillUserQuest(userDetail, *reqMsg.CurrentParty, *reqMsg.HelperUserId, reqMsg.HelperUnit,
		questData.Drop, stageInfo, questInfo, questState); e.IsError() {
		return e
	}
	friendPoint, e := friend.GetFriendPoint(db, uid, *reqMsg.HelperUserId)
	if e.IsError() {
		return e
	}
	log.T("record fid:%v => friendPoint: %v", *reqMsg.HelperUserId, friendPoint)
	userDetail.Quest.GetFriendPoint = proto.Int32( friendPoint )


	//update currParty
	userDetail.Party.CurrentParty = reqMsg.CurrentParty

	//save updated userinfo
	if e = user.UpdateUserInfo(db, userDetail); e.IsError() {
		return e
	}

	//fill response
	rspMsg.StaminaNow = userDetail.User.StaminaNow
	rspMsg.StaminaRecover = userDetail.User.StaminaRecover
	rspMsg.DungeonData = &questData

	log.T("=========== StartQuest total cost %v ms. ============\n\n", cost.Cost())

	log.T(">>>>>>>>>>>>rspMsg begin<<<<<<<<<<<<<")
	log.T("--StaminaNow:%v", *rspMsg.StaminaNow)
	log.T("--StaminaRecover:%v", *rspMsg.StaminaRecover)

	log.T("--Boss:%+v", rspMsg.DungeonData.Boss)

	log.T("--Enemys: count=%v", len(rspMsg.DungeonData.Enemys))
	for k, enemy := range rspMsg.DungeonData.Enemys {
		log.T("\t enemy[%v]: %v", k, enemy)
	}

	log.T("--Drop: count=%v", len(rspMsg.DungeonData.Drop))
	for k, drop := range rspMsg.DungeonData.Drop {
		log.T("\t drop[%v]: %v", k, drop)
	}

	for k, floor := range rspMsg.DungeonData.Floors {
		log.T("--floor[%v]:", k)
		for _, grid := range floor.GridInfo {
			log.T("\t--[%+v] \n", grid)
		}
	}
	log.T(">>>>>>>>>>>>rspMsg end.<<<<<<<<<<<<<")

	return Error.OK()
}
