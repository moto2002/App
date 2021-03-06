using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class UnitItemInfo : MonoBehaviour{
	private GameObject _scrollItem;
	public GameObject scrollItem {
		set { _scrollItem = value; Init(); }
		get { return _scrollItem; }
	}

	public UITexture mainTexture;

    public UILabel stateLabel;

	public UISprite mask;

	public UISprite star;

	private UserUnit _userUnitItem;

	public UserUnit userUnitItem {
		set { _userUnitItem = value; RefreshInfo();}
		get { return _userUnitItem; }
	}
	public UISprite hightLight;

    private bool _isCollect = false;
	public bool isCollect {
		get { return _isCollect; }
		set { _isCollect = value; }
	}

	private bool _isPartyItem = false;
	public bool isPartyItem {
		get { return _isPartyItem; }
		set { _isPartyItem = value; }
	}

    public bool isSelect = false;
    public bool isEnable = false;

	public void SetMask(bool b) {
		mask.enabled = b;
		if (star != null) {
			star.enabled = b;		
		}
	}

	public void SetLabelInfo (string info) {
		stateLabel.text = info;
	}

	public void IsFavorate (int value) {
		bool b = false;
		if (value == 1) {
			b = true;		
		}
		_isCollect = b;
		if (star != null) {
			star.enabled = b;
		}
	}

	public void IsPartyItem (bool b) {
		if (b && stateLabel != null) {
			stateLabel.text = "Party";	
		}
		isPartyItem = b;
	}

	void Init() {
//		Debug.LogError ("Init start ");
		Transform trans = scrollItem.transform;
		mainTexture = trans.Find ("Texture_Avatar").GetComponent<UITexture> ();
		stateLabel = trans.Find ("Label_Party").GetComponent<UILabel> ();
		mask = trans.Find ("Mask").GetComponent<UISprite> ();
		star = trans.Find ("StarMark").GetComponent<UISprite> ();
		hightLight = trans.Find ("HighLight").GetComponent<UISprite> ();
//		UIEventListenerCustom listener = UIEventListenerCustom.Get (scrollItem);
//		listener.onClick = ClickItem;
//		listener.LongPress = LongPress;
//		Debug.LogError ("Init end ");
	}

	void RefreshInfo() {
		ResourceManager.Instance.GetAvatar (UnitAssetType.Avatar,userUnitItem.unitId, o=>{
//			userUnitItem.UnitInfo
			mainTexture.mainTexture = o as Texture2D;
		});
		IsFavorate (userUnitItem.isFavorite);
		bool isParty = (DataCenter.Instance.UnitData.PartyInfo.UnitIsInParty (userUnitItem.uniqueId) > 0);
		IsPartyItem(isParty);
		bbproto.EvolveInfo ei = userUnitItem.UnitInfo.evolveInfo;
		if (ei == null || ei.materialUnitId.Count == 0) {
			SetMask (true);	
			UIEventListenerCustom listener = UIEventListenerCustom.Get (scrollItem);
			listener.onClick = null;
			listener.LongPress = null;
		} else {
			SetMask(false);
			UIEventListenerCustom listener = UIEventListenerCustom.Get (scrollItem);
			listener.onClick = ClickItem;
			listener.LongPress = LongPress;
		}
	}

	public UICallback callback;

	public void ClickItem(GameObject go) {
		if (callback != null) {
			callback(go);
		}
	}

	public void LongPress(GameObject go) {
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule,"user_unit",userUnitItem);
	}
}

public class UnitItemViewInfo {
	private GameObject viewItem;
	public GameObject ViewItem{
		get{
			return viewItem;
		}
		set{
			viewItem = value;
		}
	}

    private bool isEnable;
    public bool IsEnable {
        get{ return isEnable;}
		set{ isEnable = value;}
    }

    private bool isParty;
    public bool IsParty {
        get{ return isParty; }
		set{ isParty = value;
		}
    }

    private bool isCollected;
    public bool IsCollected {
        get{ return isCollected;}
		set{ isCollected = value;}
    }
        
	private Color typeColor;
	public Color TypeColor{
		get{
			return typeColor;
		}
		set{
			typeColor = value;
		}
	}

    private string crossShowTextBefore;
    public string CrossShowTextBefore {
        get {
            return crossShowTextBefore;
        }
        set {
            crossShowTextBefore = value;
        }
    }

    private string crossShowTextAfter;
    public string CrossShowTextAfter {
        get {
            return crossShowTextAfter;
        }
        set {
            crossShowTextAfter = value;
        }
    }

    private UITexture avatar;
    public UITexture Avatar {
        get {
            return avatar;
        }
    }

    private UserUnit dataItem;
    public UserUnit DataItem {
        get {
            return dataItem;
        }
    }

	private FriendInfo helperItem;
	public FriendInfo HelperItem {
		get {
			return helperItem;
		}
	}

    private UnitItemViewInfo(){}

    public static UnitItemViewInfo Create(UserUnit dataItem) {
        UnitItemViewInfo partyUnitItemView = new UnitItemViewInfo();
        partyUnitItemView.InitWithTUserUnit(dataItem);
        return partyUnitItemView;
    }
	public static UnitItemViewInfo Create(UserUnit dataItem, bool inAllParty) {
		UnitItemViewInfo partyUnitItemView = new UnitItemViewInfo();
		partyUnitItemView.InitWithTUserUnit(dataItem);
		if (inAllParty){
			partyUnitItemView.SetStateInAllParty();
		}
		return partyUnitItemView;
	}

	public void InitView(GameObject viewItem){
		this.ViewItem = viewItem;

		if(ViewItem == null) return;
		avatar = ViewItem.transform.FindChild("Texture_Avatar").GetComponent<UITexture>();
		ResourceManager.Instance.GetAvatar(UnitAssetType.Avatar, dataItem.unitId, o=>{
//			dataItem.UnitInfo
			avatar.mainTexture = o as Texture2D;
		});
	}



	public static UnitItemViewInfo Create(FriendInfo friendItem){
		UnitItemViewInfo partyUnitItemView = new UnitItemViewInfo();
		partyUnitItemView.InitWithTFriendInfo(friendItem);
		return partyUnitItemView;
	}

	public void SetStateInAllParty(){
		IsParty = (DataCenter.Instance.UnitData.PartyInfo.UnitIsInParty(dataItem.uniqueId) > 0);
	}

    public void RefreshStates(Dictionary <string, object> statesDic) {
        foreach (var key in statesDic.Keys) {
            switch (key) {
            case "collect":
                RefreshMarkState((bool)statesDic[key]);
                break;
            case "enable":
                RefreshEnableState((bool)statesDic[key]);
                break;
            case "party":
                RefreshPartyState((bool)statesDic[key]);
                break;
            case "cross":
                RefreshCrossTextState(statesDic[key] as List<string>);
                break;
            default:
                break;
            }
        }
    }
    private void RefreshCrossTextState(List<string> textList) {
        this.crossShowTextBefore = textList[ 0 ];
        this.crossShowTextAfter = textList[ 1 ];
    }

    private void InitWithTUserUnit(UserUnit dataItem) {
        InitDataItem(dataItem);
        InitWithArgs();
//        GetAvatar();
		GetTypeColor();
    }

	private void InitWithTFriendInfo(FriendInfo dataItem) {
		this.dataItem = dataItem.UserUnit;
		this.helperItem = dataItem;
		InitWithArgs();
//		GetAvatar();
		GetTypeColor();
	}
       
    private void InitDataItem(UserUnit dataItem) {
        this.dataItem = dataItem;
    }

    private void InitCrossShowText() {
        crossShowTextBefore = dataItem.level.ToString();
        crossShowTextAfter = (dataItem.addHp + dataItem.addAttack).ToString();
    }

    private void InitWithArgs() {
        Dictionary <string, object> initArgs = new Dictionary<string, object>();
        initArgs.Add("collect", false);
        initArgs.Add("enable", false);
        if (DataCenter.Instance.UnitData.PartyInfo == null || dataItem == null) {
            Debug.LogError("InitWithArgs(), GlobalData.PartyInfo == null, return");
            return;
        }
        initArgs.Add("party", DataCenter.Instance.UnitData.PartyInfo.UnitIsInCurrentParty(dataItem.uniqueId));

        List<string> textList = new List<string>();
        textList.Add(dataItem.level.ToString());
        textList.Add((dataItem.addHp + dataItem.addAttack).ToString());
        initArgs.Add("cross", textList);
        RefreshStates(initArgs);
    }
	
    private void GetAvatar() {
//        avatar = dataItem.UnitInfo.GetAsset(UnitAssetType.Avatar);
    }

	private void GetTypeColor(){
		switch (dataItem.UnitInfo.type){
			case bbproto.EUnitType.UFIRE : 
				typeColor = Color.red;
				break;
			case bbproto.EUnitType.ULIGHT : 
				typeColor = Color.yellow;
				break;
			case bbproto.EUnitType.UNONE :
				typeColor = Color.gray;
				break;
			case bbproto.EUnitType.UDARK :
				typeColor = Color.grey;
				break;
			case bbproto.EUnitType.UWATER : 
				typeColor = Color.cyan;
				break;
			case bbproto.EUnitType.UWIND : 
				typeColor = Color.green;
				break;
			default:
				typeColor = Color.white;
				break;
		}
		typeColor.a = 0.65f;
//		Debug.LogError("typeColor : " + typeColor.ToString());

	}
	
    private void RefreshPartyState(bool state) {
        this.isParty = state;
    }

    private void RefreshMarkState(bool state) {
        this.isCollected = state;
	}
		
    private void RefreshEnableState(bool state) {
        this.isEnable = state;
    }

}

