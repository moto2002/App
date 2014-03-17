package friend

import (
	"fmt"
	"math/rand"
	"strconv"
)

import (
	bbproto "bbproto"
	proto "code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	redis "github.com/garyburd/redigo/redis"
)


func GetFriendsData(db *data.Data, sUid string, isGetOnlyFriends bool, friendsInfo map[string]bbproto.FriendInfo) (err error) {
	if db == nil {
		return fmt.Errorf("[ERROR] db pointer is nil.")
	}

	log.T("begin friendsInfo[%v] is: %v", sUid, len(friendsInfo))

	zFriendDatas, err := db.HGetAll(sUid)
	if err != nil {
		log.Fatal(" HGetAll('%v') ret err:%v", sUid, err)
		return err
	}

	log.T("TABLE_FRIEND :: HGetAll('%v') ret err:%v, friendsInfo: %v",
		sUid, err, friendsInfo)

	friendNum := len(zFriendDatas) / 2

	log.T("GetFiendsData:: friendNum=%v friendsInfo len:%v", friendNum, len(friendsInfo))
	for i := 0; len(zFriendDatas) > 0; i++ {
		var sFid, sFridata []byte
		zFriendDatas, err = redis.Scan(zFriendDatas, &sFid, &sFridata)
		if err != nil {
			continue
		}

		friendData := &bbproto.FriendData{}
		err = proto.Unmarshal(sFridata, friendData) //unSerialize to friend
		if err != nil {
			log.Error(" unSerialize FriendData '%v' ret err:%v. sFridata:%v", sFid, err, sFridata)
			return err
		}

		if isGetOnlyFriends && *friendData.FriendState != bbproto.EFriendState_ISFRIEND &&
			*friendData.FriendState != bbproto.EFriendState_FRIENDHELPER {
			log.T("isGetOnlyFriends:  skip -> (fid:%v, friendState:%v)", sFid, *friendData.FriendState)
			continue
		}

		//assign friend data fields
		friInfo := bbproto.FriendInfo{}
		friInfo.UserId = friendData.UserId
		friInfo.FriendState = friendData.FriendState
		friInfo.FriendStateUpdate = friendData.FriendStateUpdate
		friendsInfo[string(sFid)] = friInfo

		log.T("got friendData[%v]: %v", sFid, friInfo)
	}

	log.T("now friendsInfo[%v] is: %v", sUid, len(friendsInfo))

	return err
}

func GetHelperData(db *data.Data, uid uint32, rank uint32, friendsInfo map[string]bbproto.FriendInfo) (err error) {
	sUserSpace := consts.X_USER_RANK + strconv.Itoa(int(uid%consts.N_USER_SPACE_PARTS))

	offset := 0 //rand.Intn(2)
	count := 3 + rand.Intn(3)

	minRank := 1
	if rank > consts.N_HELPER_RANK_RANGE {
		minRank = int(rank - consts.N_HELPER_RANK_RANGE)
	}

	zHelperIds, err := db.ZRangeByScore(sUserSpace,
		int(minRank), int(rank+consts.N_HELPER_RANK_RANGE),
		offset, count)

	if err != nil {
		return err
	}

	log.T("ZRangeByScore(%v,[%v,%v],[%v,%v]) ret err:%v, got helper count:%v",
		sUserSpace, minRank, rank+consts.N_HELPER_RANK_RANGE, offset, count, err, len(zHelperIds))

	//helperCount := len(zHelperIds)
	//helperUids = make([]string, helperCount)

	for i := 0; len(zHelperIds) > 0; i++ {
		var sFid string
		zHelperIds, err = redis.Scan(zHelperIds, &sFid)
		if err != nil {
			log.Error(" redis.Scan(zHelperIds, &sFid) ret err:%v", err)
			continue
		}

		log.T("helper %v: fid=%v", i, sFid)

		//assign friend data fields
		uid := common.Atou(sFid)
		state := bbproto.EFriendState_FRIENDHELPER
		tNow := common.Now()
		friInfo := bbproto.FriendInfo{}
		friInfo.UserId = &uid
		friInfo.FriendState = &state
		friInfo.FriendStateUpdate = &tNow

		friendsInfo[string(sFid)] = friInfo
	}

	return
}

func GetOnlyFriends(db *data.Data, uid uint32, rank uint32) (friendsInfo map[string]bbproto.FriendInfo, e Error.Error) {
	//get all friends & helper, but NOT include friendIn & friendOut
	return GetFriendInfo(db, uid, rank, true, true, true)
}

func GetFriendInfo(db *data.Data, uid uint32, rank uint32, isGetOnlyFriends bool, isGetFriend bool, isGetHelper bool) (friendsInfo map[string]bbproto.FriendInfo, e Error.Error) {
	if db == nil {
		return friendsInfo, Error.New(EC.INVALID_PARAMS, "[ERROR] db pointer is nil.")
	}
	if err := db.Select(consts.TABLE_FRIEND); err != nil {
		return friendsInfo, Error.New(EC.READ_DB_ERROR, err.Error())
	}

	friendsInfo = make(map[string]bbproto.FriendInfo)

	//get friends data
	if isGetFriend {
		sUid := common.Utoa(uid)
		err := GetFriendsData(db, sUid, isGetOnlyFriends, friendsInfo)
		if err != nil {
			log.Fatal(" GetFriendsData('%v') ret err:%v", sUid, err)
			return friendsInfo, Error.New(err)
		}

		log.T("GetFriendsData ret total %v friends", len(friendsInfo))
	}

	//get helper data
	if isGetHelper {
		err := GetHelperData(db, uid, rank, friendsInfo)
		if err != nil {
			log.Fatal(" GetHelperData(%v,%v) ret err:%v", uid, rank, err)
			return friendsInfo, Error.New(err)
		}
	}

	log.T("GetHelperData ret total %v helpers", len(friendsInfo))
	if len(friendsInfo) <= 0 {
		//log.T(err.Error())
		return friendsInfo, Error.New(EC.EF_FRIEND_NOT_EXISTS, fmt.Sprintf("[ERROR] Cannot find any friends/helpers for uid:%v rank:%v", uid, rank))
	}

	// retrieve userinfo by uids from TABLE_USER
	fids := redis.Args{}
	for _, friInfo := range friendsInfo {
		fids = fids.Add(common.Utoa(*friInfo.UserId))
	}

	if err := db.Select(consts.TABLE_USER); err != nil {
		return friendsInfo, Error.New(EC.READ_DB_ERROR)
	}

	userinfos, err := db.MGet(fids)
	if err != nil {
		return friendsInfo, Error.New(EC.READ_DB_ERROR)
	}
	//log.T("TABLE_USER.MGet(fids:%v) ret %v", fids, userinfos)

	for k, uinfo := range userinfos {
		if uinfo == nil {
			continue
		}
		userDetail := bbproto.UserInfoDetail{}
		zUser, err := redis.Bytes(uinfo, err)
		if err == nil && len(zUser) > 0 {
			if err = proto.Unmarshal(zUser, &userDetail); err != nil {
				log.Error(" Cannot Unmarshal userinfo(err:%v) userinfo: %v", err, uinfo)
				return friendsInfo, Error.New(err)
			}
		} else {
			return friendsInfo, Error.New("redis.Bytes(uinfo, err) fail.")
		}

		user := userDetail.User

		if user == nil || user.UserId == nil {
			log.Fatal("unexcepted error: user.UserId is nil. user:%v", user)
			continue
		}
		log.T("friend[%v] userId: %v -> name:%v rank:%v ",
			k, *user.UserId, *user.NickName, *user.Rank)

		uid = *user.UserId
		friInfo, ok := friendsInfo[common.Utoa(uid)]
		if ok {
			friInfo.Rank = user.Rank
			friInfo.NickName = user.NickName
			friInfo.LastPlayTime = userDetail.Login.LastPlayTime
			friInfo.Unit = userDetail.User.Unit

			friendsInfo[common.Utoa(uid)] = friInfo

			//log.T("new friend uid:%v rank:%v username:%v lastPlay:%v",
			//	uid, *newfriInfo.Rank, *newfriInfo.UserName, *newfriInfo.LastPlayTime)

		} else {
			log.Error(" cannot find friInfo for: %v.", uid)
		}
	}
	log.T("\nfriends's fids:%v friendsInfo:%v", fids, friendsInfo)
	log.T("===========GetFriends finished.==========\n")

	return friendsInfo, Error.OK()
}