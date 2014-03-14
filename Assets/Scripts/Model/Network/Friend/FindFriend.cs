// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using bbproto;


public class FindFriend: ProtoManager {
    // req && rsp
    private bbproto.ReqFindFriend reqFindFriend;
    private bbproto.RspFindFriend rspFindFriend;
    // state for req
    private uint friendUid;
    // data
    private TFriendList friendList;
    
    public FindFriend() {
    }
    
    ~FindFriend () {
    }
    
    public static void SendRequest(DataListener callBack, uint friendUid) {
        FindFriend findFriend = new FindFriend();
        findFriend.friendUid = friendUid;
        findFriend.OnRequest(null, callBack);
    }
    
    //Property: request server parameters
    public uint FriendUid { get { return friendUid; } set { friendUid = value; } }
    
    
    //make request packet==>TODO rename to request
    public override bool MakePacket() {
        Proto = Protocol.FIND_FRIEND;
        reqType = typeof(ReqFindFriend);
        rspType = typeof(RspFindFriend);
        
        reqFindFriend = new ReqFindFriend();
        reqFindFriend.header = new ProtoHeader();
        reqFindFriend.header.apiVer = Protocol.API_VERSION;
        reqFindFriend.header.userId = DataCenter.Instance.UserInfo.UserId;
        
        //request params
        reqFindFriend.friendUid = friendUid;
        
        ErrorMsg err = SerializeData(reqFindFriend); // save to Data for send out
        
        return (err.Code == (int)ErrorCode.SUCCESS);
    }
    
}

