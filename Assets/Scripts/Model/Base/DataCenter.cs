using UnityEngine;
using System.Collections.Generic;
using bbproto;

public enum ModelEnum {
    UserInfo = 100,
    AccountInfo,
    SupportFriends,
    FriendList,
    PartyInfo,
    MyUnitList,
    UserUnitList,
    UnitValue,
    Skill,
    UnitInfo,
    EnemyInfo,
    UnitBaseInfo,
    TrapInfo,
    FriendBaseInfo,

    User            = 1000,
    UnitPartyInfo   = 1001,
    
    UIInsConfig     = 2000,
    MapConfig       = 2001,

    /// temp
    TempEffect      = 10000, 
    HaveCard,
    ItemObject,

}

public enum Effect {
    DragCard = 8,
}

public class DataCenter {

    public static DataCenter Instance {
        get {
            if (instance == null) {
                instance = new DataCenter();
            }
            return instance;
        }
        
    }
    private static DataCenter instance;
    private DataCenter() {
    }

    public const int maxEnergyPoint = 20;
    public const int posStart = 0;
    public const int posEnd = 5;
    public const int minNeedCard = 2;
    public const int maxNeedCard = 5;

    public TUserInfo UserInfo { 
        get { return getData(ModelEnum.UserInfo) as TUserInfo; } 
        set { setData(ModelEnum.UserInfo, value); } 
    }
    public TAccountInfo AccountInfo {
        get { return getData(ModelEnum.AccountInfo) as TAccountInfo; }
        set { setData(ModelEnum.AccountInfo, value); }
    }
    public List<TFriendInfo> SupportFriends { 
        get { return getData(ModelEnum.SupportFriends) as List<TFriendInfo>; }
        set { setData(ModelEnum.SupportFriends, value); } 
    }
    public TFriendList FriendList { 
        get { return getData(ModelEnum.FriendList) as TFriendList; }
        set { setData(ModelEnum.FriendList, value); } 
    }

    public TPartyInfo PartyInfo { 
        get { return getData(ModelEnum.PartyInfo) as TPartyInfo; }
        set { setData(ModelEnum.PartyInfo, value); }
    }

    //TODO: reconstruct myUnitList
    public UserUnitList MyUnitList { 
        get { 
            UserUnitList ret = getData(ModelEnum.MyUnitList) as UserUnitList;
            if (ret == null) {
                ret = new UserUnitList();
                setData(ModelEnum.MyUnitList, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.MyUnitList, value); } 
    }
    public UserUnitList UserUnitList {
        get { 
            UserUnitList ret = getData(ModelEnum.UserUnitList) as UserUnitList;
            if (ret == null) {
                ret = new UserUnitList();
                setData(ModelEnum.UserUnitList, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.UserUnitList, value); } 
    }

    // unit configs table(come from config file: ) e.g.<hp, hpLevelConfigList>
    public Dictionary<int,TPowerTableInfo> UnitValue {
        get { 
            Dictionary<int,TPowerTableInfo> ret = getData(ModelEnum.UnitValue) as Dictionary<int, TPowerTableInfo>;
            if (ret == null) {
                ret = new Dictionary<int,TPowerTableInfo>();
                setData(ModelEnum.UnitValue, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.UnitValue, value); } 
    }

    public Dictionary<int, SkillBaseInfo> Skill {
        get { 
            Dictionary<int, SkillBaseInfo> ret = getData(ModelEnum.Skill) as Dictionary<int, SkillBaseInfo>;
            if (ret == null) {
                ret = new Dictionary<int, SkillBaseInfo>();
                setData(ModelEnum.Skill, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.Skill, value); } 
    }

    private Dictionary<uint, TUnitInfo>  UnitInfo {
        get { 
            Dictionary<uint, TUnitInfo> ret = getData(ModelEnum.UnitInfo) as Dictionary<uint, TUnitInfo>;
            if (ret == null) {
                ret = new Dictionary<uint, TUnitInfo>();
                setData(ModelEnum.UnitInfo, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.UnitInfo, value); } 
    }

    public Dictionary<uint, TEnemyInfo> EnemyInfo {
        get { 
            Dictionary<uint, TEnemyInfo> ret = getData(ModelEnum.EnemyInfo) as Dictionary<uint, TEnemyInfo>;
            if (ret == null) {
                ret = new Dictionary<uint, TEnemyInfo>();
                setData(ModelEnum.EnemyInfo, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.EnemyInfo, value); } 
    }

    public Dictionary<int, UnitBaseInfo> UnitBaseInfo {
        get { 
            Dictionary<int, UnitBaseInfo> ret = getData(ModelEnum.UnitBaseInfo) as Dictionary<int, UnitBaseInfo>;
            if (ret == null) {
                ret = new Dictionary<int, UnitBaseInfo>();
                setData(ModelEnum.UnitBaseInfo, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.UnitBaseInfo, value); } 
    }

    public Dictionary<uint, TrapBase> TrapInfo {
        get { 
            Dictionary<uint, TrapBase> ret = getData(ModelEnum.TrapInfo) as Dictionary<uint, TrapBase>;
            if (ret == null) {
                ret = new Dictionary<uint, TrapBase>();
                setData(ModelEnum.TrapInfo, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.TrapInfo, value); } 
    }

    public UnitBaseInfo FriendBaseInfo {
        get {
            UnitBaseInfo ret = getData(ModelEnum.FriendBaseInfo) as UnitBaseInfo;

            if (ret == null) {
                ret = UnitBaseInfo[195];
                setData(ModelEnum.FriendBaseInfo, ret);
            }
            return ret;
        }
        set { setData(ModelEnum.FriendBaseInfo, value); } 
    }

    public Dictionary<int, Object> TempEffect {
        get { 
            Dictionary<int, Object> ret = getData(ModelEnum.TempEffect) as Dictionary<int, Object>;
            if (ret == null) {
                ret = new Dictionary<int, Object>();
                setData(ModelEnum.TempEffect, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.TempEffect, value); } 
    } 

    public List<int> HaveCard {
        get { 
            List<int> ret = getData(ModelEnum.HaveCard) as List<int>;
            if (ret == null) {
                ret = new List<int>() {111,185,161,101,122,195};
                setData(ModelEnum.HaveCard, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.HaveCard, value); } 
    }

    public GameObject ItemObject {
        get {
            GameObject ret = getData(ModelEnum.ItemObject) as GameObject; 
            if (ret == null) {
                ret = Resources.Load("Prefabs/UI/Friend/FriendScrollerItem") as GameObject;
                setData(ModelEnum.ItemObject, ret);
            }
            return ret;
        }
        set { setData(ModelEnum.ItemObject, value); } 
    }

    public Object GetEffect(int type) {
        Object obj = null;
        if (!TempEffect.TryGetValue(type, out obj)) {
            string path = GetEffectPath(type);
            obj = Resources.Load(path);
            TempEffect.Add(type, obj);
        }
        return obj;
    }
    
    public string GetEffectPath(int type) {
        string path = string.Empty;
        switch (type) {
        case 1:
            path = "Effect/fire";
            break;
        case 2:
            path = "Effect/water";
            break;
        case 3:
            path = "Effect/wind";
            break;
        case 8:
            path = "Effect/card_effect";
            break;
        default:
            break;
        }
        return path;
    }

    public void RefreshUserInfo(TRspClearQuest clearQuest) {
        if (clearQuest == null) {
            return; 
        }
        UserInfo.RefreshUserInfo(clearQuest);
        AccountInfo.RefreshAcountInfo(clearQuest);
    }
    
    // return UserCost of curr Rank.
    public int UserCost {
        get {
            const int TYPE_MAXCOST_OF_RANK = 4; //type = 4: userRank -> cost
            return GetUnitValue(TYPE_MAXCOST_OF_RANK, UserInfo.Rank); 
        }
    }
    
    /// <summary>
    /// Gets the unit value.  1 =  exp. 2 = attack. 3 = hp. 4 = rankCost
    /// </summary>
    /// <returns>The unit value.</returns>
    /// <param name="type">Type.</param>
    /// <param name="level">Level.</param>
    
    public int GetUnitValue(int type, int level) {
        TPowerTableInfo pti = UnitValue[type];
        return pti.GetValue(level);
    }

    public int GetUnitValueTotal(int type, int level) {
        TPowerTableInfo pti = UnitValue[type];
        int totalValue = 0;
        for (int i=1; i<=level; i++)
            totalValue += pti.GetValue(level);
        return totalValue;
    }

    public TUnitInfo GetUnitInfo(uint unitID) {
        if (UnitInfo.ContainsKey(unitID)) {
            TUnitInfo tui = UnitInfo[unitID];
            return tui;
        }
        else {

			TUnitInfo tui = DGTools.LoadUnitInfoProtobuf(unitID);
//			Debug.LogError(unitID + " " + tui + "    " + tui.Type);
			if(tui == null) {
				Debug.LogError("uintid : " + unitID + " is invalid");
				return null;
			}
			UnitInfo.Add(tui.ID,tui);
			return tui;
        }
    }


    
    private void setData(ModelEnum modelType, object modelData) {
        ModelManager.Instance.SetData(modelType, modelData);
    }
    
    private object getData(ModelEnum modelType) {
        ErrorMsg errMsg = new ErrorMsg();
        return ModelManager.Instance.GetData(modelType, errMsg);
    }

}