package main

import (
	"bytes"
	"code.google.com/p/goprotobuf/proto"
	//"fmt"
	_ "html"
	//"io"
	//"io/ioutil"
	//"math/rand"
	//"net/http"
	//"time"
)
import (
	bbproto "bbproto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	"model/friend"
	"model/user"
	//redis "github.com/garyburd/redigo/redis"
)

func LoginPack(uid uint32) error {
	msg := &bbproto.ReqLoginPack{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.GetFriend = proto.Bool(true)
	msg.GetHelper = proto.Bool(true)
	msg.GetLogin = proto.Bool(true)
	msg.GetPresent = proto.Bool(true)

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_LOGIN_PACK)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response----------------------")
	rspmsg := &bbproto.RspLoginPack{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	//print rsp msg
	log.Printf("Reponse : [%v] error: %v", *rspmsg.Header.Code, *rspmsg.Header.Error)
	if rspmsg.Friends != nil {
		for k, friend := range rspmsg.Friends {
			log.Printf("friend[%v]: %+v", k, friend)
		}

		//for k, friend := range rspmsg.Friends.Friend {
		//	log.Printf("friend[%v]: %+v", k, friend)
		//}

		//for k, friend := range rspmsg.Friends.Helper {
		//	log.Printf("Helper[%v]: %v", k, friend)
		//}

		//for k, friend := range rspmsg.Friends.FriendIn {
		//	log.Printf("FriendIn[%v]: %v", k, friend)
		//}

		//for k, friend := range rspmsg.Friends.FriendOut {
		//	log.Printf("FriendOut[%v]: %v", k, friend)
		//}
	} else {
		log.Printf("rspmsg.Friends is nil")
	}

	log.Printf("LoginInfo: %+v", rspmsg.Login)
	log.Printf("-----------------------Response end.----------------------\n")

	return err
}

func AuthUser(uuid string, uid uint32) {
	msg := &bbproto.ReqAuthUser{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("0.0.1")
	//log.Printf("msg.Header.UserId:%v", msg.Header.UserId)
	//uid := uint32(0)
	msg.Header.UserId = &uid
	//msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.PacketId = proto.Int32(18)
	msg.Terminal = &bbproto.TerminalInfo{}
	if uuid == "" {
		msg.Terminal.Uuid = proto.String("b2c4adfd-e6a9-4782-814d-67ce34220101")
	} else {
		msg.Terminal.Uuid = proto.String(uuid)
	}

	msg.Terminal.DeviceName = proto.String("kory's ipod")
	msg.Terminal.Os = proto.String("iOS 6.1")
	//msg.Terminal.Platform = proto.String("official")

	buffer, err := proto.Marshal(msg)
	log.Printf("Marshal ret err:%v buffer:%v", err, buffer)

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_AUTH_USER)

	if err == nil {
		rspmsg := &bbproto.RspAuthUser{}
		err = proto.Unmarshal(rspbuff, rspmsg)
		log.Printf("rsp Unarshal ret err:%v rspmsg:%v", err, rspmsg)
		for k, friend := range rspmsg.Friends {
			log.Printf("Friend[%v]: %+v", k, friend)
		}
	} else {
		log.Printf("SendHttpPost ret err:%v", err)
	}

}

//func AddUsers(num uint32) error {
//	db := &data.Data{}
//	err := db.Open(string(consts.TABLE_USER))
//	if err != nil {
//		return err
//	}
//	defer db.Close()

//	//add to user table
//	tNow := uint32(time.Now().Unix())
//	for uid := uint32(101); uid-100 < num; uid++ {
//		rank := int32(uid-35) % 100
//		tNow += 3
//		name := "name" + common.Utoa(uid)

//		user := &bbproto.UserInfo{}
//		user.UserId = &uid
//		user.Rank = &rank
//		user.UserName = &name
//		user.LoginTime = &tNow

//		zUserinfo, err := proto.Marshal(user)
//		if err != nil {
//			return err
//		}
//		err = db.Set(common.Utoa(uid), zUserinfo)
//	}
//	return err
//}

func AddUsers(startNum int, count int) {
	for i := startNum + 0; i < startNum+count; i++ {
		AuthUser("b2c4adfd-e6a9-4782-814d-67ce3422x"+common.Itoa(i), 0)
	}
}

type MyTp struct {
	x int32
}

type My struct {
	MyTp MyTp
}

func test() {
	//db := &data.Data{}
	//err := db.Open("0")
	//defer db.Close()
	//if err != nil {
	//	return
	//}

	//var value []byte
	//value, err = db.Gets("userinfo")
	//if err != nil {
	//	log.Printf("[ERROR] GetUserInfo ret err:%v", err)
	//}

	//unit := &bbproto.UnitInfo{}

	//err = proto.Unmarshal(value, unit)
	//if err != nil {
	//	log.Printf("[ERROR] GetUserInfo for ret err:%v", err)
	//}
	//log.Printf("unit:%+v", unit)

	return
}

func ResetStamina(uid uint32) (e Error.Error) {
	db := &data.Data{}
	err := db.Open(consts.TABLE_USER)
	defer db.Close()
	if err != nil || uid <= 0 {
		return
	}

	userDetail := &bbproto.UserInfoDetail{}

	var value []byte
	value, err = db.Gets(common.Utoa(uid))
	if err != nil {
		log.Error("[ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		return Error.New(err)
	}

	if len(value) != 0 {
		err = proto.Unmarshal(value, userDetail)
		if err != nil {
			log.Error("[ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		}
	}

	userDetail.User.StaminaNow = proto.Int(9000)
	userDetail.User.StaminaMax = proto.Int(9000)

	//save data
	zUserData, err := proto.Marshal(userDetail)
	if err != nil {
		return Error.New(EC.MARSHAL_ERROR, err)
	}

	if err = db.Set(common.Utoa(*userDetail.User.UserId), zUserData); err != nil {
		log.Error("SET_DB_ERROR for userDetail: %v", *userDetail.User.UserId)
		return Error.New(EC.READ_DB_ERROR, err)
	}

	return Error.OK()
}

func UpdateHelperUidRank(maxUserId uint32) {
	db := &data.Data{}
	err := db.Open(consts.TABLE_USER)
	defer db.Close()
	if err != nil {
		return
	}

	for uid := uint32(101); uid < maxUserId; uid++ {
		userDetail, _, err := user.GetUserInfo(db, uid)
		if err != nil {
			log.Error("GetUser(%v) ret err:%v", uid, err.Error())
			continue
		}
		if userDetail.User != nil {
			rank := *userDetail.User.Rank
			e := friend.SetHelperUidRank(db, uid, uint32(rank))
			if e.IsError() {
				log.Error("SetHelperUidRank(%v) ret err:%v", uid, e.Error())
			}
		}
	}
}

func main() {
	Init()
	//AddUsers(100, 1000)

	//UpdateHelperUidRank(1000)

	AuthUser("b2c4adfd-e6a9-4782-814d-67ce34220162", 0) //1101
	//ResetStamina(154)
	//LoginPack(101)

	log.Fatal("bbsvr test client finish.")
}
