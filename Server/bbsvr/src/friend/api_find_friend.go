package friend

import (
	"fmt"
	"net/http"
)

import (
	"bbproto"
	"common/EC"
	"common/Error"
	"common/log"
	"model/user"
	//"model/friend"

	proto "code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func FindFriendHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqFindFriend
	rspMsg := &bbproto.RspFindFriend{}

	handler := &FindFriend{}
	e := handler.ParseInput(req, &reqMsg)
	if e.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, Error.New(EC.INVALID_PARAMS, e.Error())))
		return
	}

	e = handler.verifyParams(&reqMsg)
	if e.IsError() {
		log.T("sendrsp err:%v, rspMsg:\n%+v", e, rspMsg)
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, e))
		return
	}

	// game logic

	e = handler.ProcessLogic(&reqMsg, rspMsg)

	e = handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, e))
	log.Printf("sendrsp err:%v, rspMsg:\n%+v", e, rspMsg)
}

/////////////////////////////////////////////////////////////////////////////

type FindFriend struct {
	bbproto.BaseProtoHandler
}

func (t FindFriend) FillResponseMsg(reqMsg *bbproto.ReqFindFriend, rspMsg *bbproto.RspFindFriend, rspErr Error.Error) (outbuffer []byte) {
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

func (t FindFriend) verifyParams(reqMsg *bbproto.ReqFindFriend) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t FindFriend) ProcessLogic(reqMsg *bbproto.ReqFindFriend, rspMsg *bbproto.RspFindFriend) (Err Error.Error) {

	friendUid := *reqMsg.FriendUid

	//get user's rank from user table
	userDetail, isUserExists, err := user.GetUserInfo(nil, friendUid)
	if err != nil {
		return Error.New(EC.EU_GET_USERINFO_FAIL, fmt.Sprintf("GetUserInfo failed for userId %v. err:%v", friendUid, err.Error()))
	}

	// get FriendInfo
	if isUserExists {
		rspMsg.Friend = &bbproto.FriendInfo{}
		if userDetail != nil {
			log.T("getUser(%v) ret userinfo: %v", friendUid, userDetail.User)

			rspMsg.Friend.UserId = userDetail.User.UserId
			rspMsg.Friend.NickName = userDetail.User.NickName
			rspMsg.Friend.Rank = userDetail.User.Rank
			rspMsg.Friend.LastPlayTime = userDetail.Login.LastPlayTime
			rspMsg.Friend.Unit = userDetail.User.Unit

		}
	} else {
		return Error.New(EC.EF_FRIEND_NOT_EXISTS, fmt.Sprintf("userId: %v not exists", friendUid))
	}

	return Error.OK()
}
