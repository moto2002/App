package bbproto

import (
	_ "fmt"
	"io/ioutil"
	"net/http"
	//"strconv"
)
import (
	"../common/Error"
	"../common/log"
	"../const"
	proto "code.google.com/p/goprotobuf/proto"
)

type ProtoHandler interface {
	//parse input into reqMsg
	ParseInput(req *http.Request, reqMsg proto.Message) (e Error.Error)
	SendResponse(rsp http.ResponseWriter, data []byte) (e Error.Error)
}

type BaseProtoHandler struct {
}

func (t BaseProtoHandler) ParseInput(req *http.Request, reqMsg proto.Message) (e Error.Error) {
	reqBuffer, err := ioutil.ReadAll(req.Body)
	if err != nil {
		log.Error("ERR: ioutil.ReadAll failed: %v ", err)
		return Error.New(cs.IOREAD_ERROR, err.Error())
	}

	//log.T("recv reqBuffer: %+v", reqBuffer)

	err = proto.Unmarshal(reqBuffer, reqMsg) //unSerialize into reqMsg
	if err != nil {
		log.Error("Unmarshal proto err: %v", err)
		return Error.New(cs.UNMARSHAL_ERROR, err.Error())
	}
	log.T("==================================================")
	log.T("recv reqMsg: %+v", reqMsg)

	return Error.OK()
}

func (t BaseProtoHandler) SendResponse(rsp http.ResponseWriter, data []byte) (e Error.Error) {

	if data == nil {
		log.Fatal("Cannot SendResponse empty data bytes")
		return Error.New(cs.INVALID_PARAMS, "Cannot SendResponse empty data bytes")
	}

	size, err := rsp.Write(data)
	if err != nil {
		return Error.New(cs.IOWRITE_ERROR, err.Error())
	}

	log.T("reponse data size:%v", size)

	return Error.OK()
}
