package user

import (
	"net/http"
	//"time"
)

import (
	"../bbproto"
	"../common/Error"
	"../common/log"
	"../const"
	"./party"

	proto "code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func ChangePartyHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqChangeParty
	rspMsg := &bbproto.RspChangeParty{}

	handler := &ChangeParty{}
	e := handler.ParseInput(req, &reqMsg)
	if e.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, e))
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
	log.Printf("sendrsp err:%v, rspMsg:\n%+v", e, rspMsg)
}

/////////////////////////////////////////////////////////////////////////////

type ChangeParty struct {
	bbproto.BaseProtoHandler
}

func (t ChangeParty) verifyParams(reqMsg *bbproto.ReqChangeParty) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.Party == nil || reqMsg.Header.UserId == nil {
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return Error.New(cs.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t ChangeParty) FillResponseMsg(reqMsg *bbproto.ReqChangeParty, rspMsg *bbproto.RspChangeParty, rspErr Error.Error) (outbuffer []byte) {
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

func (t ChangeParty) ProcessLogic(reqMsg *bbproto.ReqChangeParty, rspMsg *bbproto.RspChangeParty) (e Error.Error) {
	log.T("ChangeParty ...")

	e = party.ChangeParty(nil, *reqMsg.Header.UserId, reqMsg.Party)

	return e
}