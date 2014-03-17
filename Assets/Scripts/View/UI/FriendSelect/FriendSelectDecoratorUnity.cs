using UnityEngine;
using System.Collections.Generic;

public class FriendSelectDecoratorUnity : UIComponentUnity/*,IUICallback */{
    private GameObject msgBox;
    private UIImageButton btnStart;
    private UIButton btnSure;
    private UIButton btnCancel;
    private UIButton btnSeeInfo;
    private UILabel labelCurrentPartyIndex;
    private UILabel labelPartyTotalCount;
    private GameObject leftArrowBtn;
    private GameObject rightArrowBtn;
//    private DragPanel friendsScroller;
    private GameObject friendItem;
    private int currentPartyIndex;
    private int partyTotalCount;
    private int initPartyPage = 1;
    private Dictionary<int, UITexture> partySprite = new Dictionary<int,UITexture>();
    private Dictionary<int, UnitBaseInfo> unitBaseInfo = new Dictionary<int, UnitBaseInfo>();
    private UITexture friendSprite;
    private UnitBaseInfo friendBaseInfo;
	
	GameObject dragPanelCell;
	DragPanel dragPanel;
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	
	public override void Callback(object data){
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "CreateDragList" : 
				CallBackDispatcherHelper.DispatchCallBack(CreateDragView, cbdArgs);
				break;
			default:
				break;
		}
	}

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans", 			transform);
		dragPanelArgs.Add("scrollerScale", 			Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos", 		-105 * Vector3.up);
		dragPanelArgs.Add("position", 					Vector3.zero);
		dragPanelArgs.Add("clipRange", 				new Vector4(0, 0, 640, 220));
		dragPanelArgs.Add("gridArrange", 			UIGrid.Arrangement.Horizontal);
		dragPanelArgs.Add("maxPerLine", 			0);
		dragPanelArgs.Add("scrollBarPosition",		new Vector3(-320, -120, 0));
        dragPanelArgs.Add("cellWidth", 				140);
        dragPanelArgs.Add("cellHeight", 				140);
    }
        
	DragPanel CreateDragPanel(string name, int count){
		DragPanel panel = new DragPanel(name, dragPanelCell);
		panel.CreatUI();
		panel.AddItem(count, dragPanelCell);
		return panel;
	}

	void CreateDragView(object args){
		LogHelper.Log("FriendSelectDecoratorUnity.CreateDragView(), receive call from logic, to create drag list...");
		List<TFriendInfo> friendInfoList = args as List<TFriendInfo>;
	
		dragPanel = CreateDragPanel("SupportFriendList", friendInfoList.Count);

		UpdateAvatarTexture(friendInfoList);
		UpdateCountLabel(friendInfoList.Count, 50);
		UpdateEventListener();

//		UpdateCrossShow();
		dragPanel.DragPanelView.SetScrollView(dragPanelArgs);
	}

	void ClickItem(GameObject item){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickItem", dragPanel.ScrollItem.IndexOf(item));
		ExcuteCallback(cbdArgs);
	}
	
	void PressItem(GameObject item){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("PressItem", dragPanel.ScrollItem.IndexOf(item));
                ExcuteCallback(cbdArgs);
    }
	
	void UpdateEventListener(){
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem [i];
			UIEventListenerCustom.Get(scrollItem).onClick = ClickItem;
                        UIEventListenerCustom.Get(scrollItem).LongPress = PressItem;
                }
        }

	void UpdateCountLabel(int cur, int max){
//		curCountLabel.text = cur.ToString();
//		maxCountLabel.text = max.ToString();
	}

	void UpdateAvatarTexture(List<TFriendInfo> friendInfoList){

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem [i];
			UITexture uiTexture = scrollItem.transform.FindChild("Texture_Avatar").GetComponent<UITexture>();
                        uiTexture.mainTexture = friendInfoList [i].UserUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
                }
        }

        public override void Init(UIInsConfig config, IUICallback origin) {
        base.Init(config, origin);
        InitUI();
    }
	
    public override void ShowUI() {
        base.ShowUI();

        ShowTween();
        btnStart.isEnabled = false;
//        friendsScroller.DragPanelView.gameObject.SetActive(true);
    }
	
    public override void HideUI() {
        base.HideUI();
    }
	
    public override void DestoryUI() {
        base.DestoryUI();
    }

    private void InitUI() {
        friendBaseInfo = DataCenter.Instance.FriendBaseInfo;
        InitPartyLabel();
        InitPartyArrow();
        InitPartyUnits();
        InitMsgBox();
        InitFriendList();

		dragPanelCell = Resources.Load("Prefabs/UI/Friend/AvailFriendItem") as GameObject;
		InitDragPanelArgs();
    }
	
    private void SendUnitPage(int pageIndex) {
        IUICallback call = origin as IUICallback;
        if (call == null) {
            return;		
        } 
        call.Callback(pageIndex);
    }

//	
//    public void Callback(object data) {
//        if (data == null) {
//            ShowPartyInfo(null);
//        }
//        else {
//            Dictionary<int,UnitBaseInfo> upi = data as Dictionary<int,UnitBaseInfo>;
//            if (upi == null) {
//                return;
//            }
//            ShowPartyInfo(upi);
//        }
//    }

    private void ShowPartyInfo(Dictionary<int,UnitBaseInfo> name) {
        unitBaseInfo = name;
        if (name == null) {
            foreach (var item in partySprite.Values) {
                item.enabled = false;
            }
        }
        else {
            foreach (var item in partySprite) {
                if (name.ContainsKey(item.Key)) {
                    partySprite[item.Key].enabled = true;
                    string path = name[item.Key].GetHeadPath; 
                    partySprite[item.Key].mainTexture = Resources.Load(path) as Texture2D;
                }
                else {
                    partySprite[item.Key].enabled = false;
                }
            }
        }
    }

    private void InitPartyUnits() {
        UITexture temp;
        for (int i = 1; i < 5; i++) {
            temp = FindChild< UITexture >("Window/window_party/Unit" + i.ToString());
            UIEventListenerCustom.Get(temp.gameObject).LongPress = LongPressCallback;
            temp.enabled = false;
            partySprite.Add(i, temp);

        }
        friendSprite = FindChild< UITexture >("Window/window_party/Friend");
        friendSprite.enabled = false;
        SendUnitPage(1);
    }

    void LongPressCallback(GameObject target) {
        int posID = -1;
        foreach (var item in partySprite) {
            if (target == item.Value.gameObject) {
                posID = item.Key;
            }
        }
        MsgCenter.Instance.Invoke(CommandEnum.EnterUnitInfo, unitBaseInfo[posID]);
    }

    private void InitPartyArrow() {
        leftArrowBtn = FindChild("Window/window_party/left_arrow");
        rightArrowBtn = FindChild("Window/window_party/right_arrow");
		
        UIEventListener.Get(leftArrowBtn).onClick = BackParty;
        UIEventListener.Get(rightArrowBtn).onClick = ForwardParty;
    }
	
    private void InitPartyLabel() {
        labelCurrentPartyIndex = FindChild< UILabel >("Window/window_party/Label_party_current");
        labelPartyTotalCount = FindChild< UILabel >("Window/window_party/Label_party_total");
        currentPartyIndex = 1;
        partyTotalCount = UIConfig.partyTotalCount;	
        labelCurrentPartyIndex.text = currentPartyIndex.ToString();
        labelPartyTotalCount.text = partyTotalCount.ToString();
    }

    private void InitPartyInfoPanel() {
    }

    private void InitMsgBox() {
        msgBox = FindChild("Window/msg_box");
        btnSure = FindChild< UIButton >("Window/msg_box/btn_choose");
        btnCancel = FindChild< UIButton >("Window/msg_box/btn_exit");
        btnSeeInfo = FindChild< UIButton >("Window/msg_box/btn_see_info");
        btnStart = FindChild< UIImageButton >("ScrollView/btn_quest_start");	
        UIEventListener.Get(btnStart.gameObject).onClick = ClickStartBtn;
        UIEventListener.Get(btnCancel.gameObject).onClick = ClickCancelBtn;
        UIEventListener.Get(btnSure.gameObject).onClick = ClickChooseBtn;
        UIEventListener.Get(btnSeeInfo.gameObject).onClick = ClickSeeInfoBtn;
        msgBox.SetActive(false);
    }

    //Friend List
    private void InitFriendList() {
//        friendItem = Resources.Load("Prefabs/UI/Friend/AvailFriendItem") as GameObject;
//        friendsScroller = new DragPanel("FriendSelectScroller", friendItem);
//        friendsScroller.CreatUI();
//
//        friendsScroller.AddItem(1);
//        friendsScroller.DragPanelView.SetItemWidth(140);
//        friendsScroller.DragPanelView.gameObject.transform.parent = gameObject.transform.FindChild("ScrollView");
//        friendsScroller.DragPanelView.gameObject.transform.localScale = Vector3.one;
//        friendsScroller.DragPanelView.gameObject.transform.localPosition = -115 * Vector3.up;
//        for (int i = 0; i < friendsScroller.ScrollItem.Count; i++) {
//            friendsScroller.ScrollItem[i].GetComponentInChildren<UITexture>().mainTexture 
//				= Resources.Load(friendBaseInfo.GetHeadPath) as Texture2D;
//            UIEventListenerCustom ulc = UIEventListenerCustom.Get(friendsScroller.ScrollItem[i].gameObject);
//            ulc.LongPress = PickFriendLongpress;
//            ulc.onClick = PickFriend;
//        }

        //Get Friend List Data Interface
//		for (int i = 0; i < ConfigFriendList.selectableFriendList.Count; i++){
//			friendsScroller.AddItem(1);
//			friendsScroller.ScrollItem[ i ].GetComponentInChildren< UITexture >().mainTexture
//				= Resources.Load( ConfigFriendList.selectableFriendList[i].unitId) as Texture2D;
//		}

    }

    void PickFriendLongpress(GameObject go) {
        MsgCenter.Instance.Invoke(CommandEnum.EnterUnitInfo, friendBaseInfo);
    }

    private void ShowPartyFriend() {
    }
	
    void BackParty(GameObject go) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        currentPartyIndex = Mathf.Abs((currentPartyIndex - 1) % partyTotalCount);
        if (currentPartyIndex == 0)
            currentPartyIndex = partyTotalCount;
        labelCurrentPartyIndex.text = currentPartyIndex.ToString();
        SendUnitPage(currentPartyIndex);
    }

    void ForwardParty(GameObject go) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        currentPartyIndex++;
        if (currentPartyIndex > partyTotalCount) {
            currentPartyIndex = initPartyPage;
        } 
        labelCurrentPartyIndex.text = currentPartyIndex.ToString();
        SendUnitPage(currentPartyIndex);
    }

    void ClickCancelBtn(GameObject btn) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        msgBox.SetActive(false);
    }

    void ClickChooseBtn(GameObject btn) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        msgBox.SetActive(false);
        friendSprite.enabled = true;
        friendSprite.mainTexture = Resources.Load(friendBaseInfo.GetHeadPath) as Texture2D;
        btnStart.isEnabled = true;
    }

    void ClickSeeInfoBtn(GameObject btn) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        msgBox.SetActive(false);
    }

    void ClickStartBtn(GameObject btn) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        btnStart.isEnabled = false;

        RequestStartQuest();
    }

    INetBase startQuestNetBase; 
    void RequestStartQuest() {
        if (startQuestNetBase == null) {
            startQuestNetBase = new StartQuest();
        }

        StartQuestParam p = new StartQuestParam();
        p.currPartyId = 0;
        p.questId = 1101;
        p.stageId = 11;
		TFriendInfo tfi = DataCenter.Instance.SupportFriends [0];
		p.helperUserId = tfi.UserId;
		p.helperUniqueId = tfi.UserUnit.ID;
		startQuestNetBase.OnRequest(p, RspStartQuest);
    }

    void RspStartQuest(object data) {
        TQuestDungeonData tqdd = null;
        bbproto.RspStartQuest rspStartQuest = data as bbproto.RspStartQuest;
        if (rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null) {

            DataCenter.Instance.UserInfo.StaminaNow = rspStartQuest.staminaNow;
            DataCenter.Instance.UserInfo.StaminaRecover = rspStartQuest.staminaRecover;

            LogHelper.Log("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);

            tqdd = new TQuestDungeonData(rspStartQuest.dungeonData);
				
            ModelManager.Instance.SetData(ModelEnum.MapConfig, tqdd);
        }

        if (data == null || tqdd == null) {
            Debug.LogError("Request quest info fail : data " + data + "  TQuestDungeonData : " + tqdd);
            //TODO: show failed window for user to retry
            return;
        }

        UIManager.Instance.EnterBattle();
    } 

	
    void PickFriend(GameObject btn) {
//        Debug.LogError("PickFriend");
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        msgBox.SetActive(true);
    }

    private void ShowTween() {
        TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
        if (list == null)
            return;
        foreach (var tweenPos in list) {		
            if (tweenPos == null)
                continue;
            tweenPos.Reset();
            tweenPos.PlayForward();
        }
    }
	

}
