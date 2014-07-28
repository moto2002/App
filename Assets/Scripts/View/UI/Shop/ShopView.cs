using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;

public class ShopView : UIComponentUnity {
	private Dictionary<string,UIButton> buttonDic = new Dictionary<string, UIButton>();
    private UIButton btnFriendsExpansion;
    private UIButton btnStaminaRecover;
    private UIButton btnUnitExpansion;
	private GameObject infoPanelRoot;
	private GameObject windowRoot;

	private GameCurrencyEventHandler handler;

	private DragPanel dragPanel;

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI();

		handler = new GameCurrencyEventHandler ();

		try{
			StoreController.Initialize (new GameCurrencyAssets ());
		}catch(System.Exception e){
			Debug.LogException(e);
		}

	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowUIAnimation();

		#if UNITY_ANDROID
		StoreController.StartIabServiceInBg();
		#endif
	}
	
	public override void HideUI () {
		base.HideUI ();

		int count = dragPanel.ScrollItem.Count;
		for (int i = 0; i < count; i++) {
			GameObject go = dragPanel.ScrollItem[i];
			GameObject.Destroy(go);
		}
		dragPanel.ScrollItem.Clear();

		#if UNITY_ANDROID
		StoreController.StopIabServiceInBg();
		#endif
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
		dragPanel.DestoryUI ();
	}

	private void InitUI() {
		UIButton[] buttons = FindChild("btns").GetComponentsInChildren< UIButton >();
		for (int i = 0; i < buttons.Length; i++){
			buttonDic.Add( string.Format("Chip{0}", i), buttons[ i ] );
			UIEventListener.Get( buttons[ i ].gameObject ).onClick = ClickButton;
		}

        btnFriendsExpansion = FindChild<UIButton>("top/FriendsExpansion");
		UILabel friendExpandLabel = btnFriendsExpansion.transform.GetComponentInChildren<UILabel>();
		friendExpandLabel.text = TextCenter.GetText("Btn_Friend_Expand");

        btnStaminaRecover = FindChild<UIButton>("top/StaminaRecover");
		UILabel recoverLabel = btnStaminaRecover.transform.GetComponentInChildren<UILabel>();
		recoverLabel.text = TextCenter.GetText("Btn_Stamina_Recover");

        btnUnitExpansion = FindChild<UIButton>("top/UnitExpansion");
		UILabel unitExpandLabel = btnUnitExpansion.transform.GetComponentInChildren<UILabel>();
		unitExpandLabel.text = TextCenter.GetText("Btn_Unit_Expand");

        UIEventListener.Get(btnFriendsExpansion.gameObject).onClick = OnClickFriendExpansion;
        UIEventListener.Get(btnStaminaRecover.gameObject).onClick = OnClickStaminaRecover;
        UIEventListener.Get(btnUnitExpansion.gameObject).onClick = OnClickUnitExpansion;

		infoPanelRoot = transform.FindChild("top").gameObject;
		windowRoot = transform.FindChild("btns").gameObject;

		CreateDragView ();
	}

	private void CreateDragView(){

		List<ShopItemData> friendOutDataList = new List<ShopItemData> ();
		friendOutDataList.Add (new ShopItemData (0,ShopItemEnum.MonthCard,1,"ms.chip.pack1"));
		friendOutDataList.Add (new ShopItemData (100,ShopItemEnum.MonthCard,150,"ms.chip.pack2"));
		friendOutDataList.Add (new ShopItemData (100,ShopItemEnum.MonthCard,150,"ms.chip.pack3"));
		friendOutDataList.Add (new ShopItemData (100,ShopItemEnum.MonthCard,150,"ms.chip.pack4"));
		friendOutDataList.Add (new ShopItemData (100,ShopItemEnum.MonthCard,150,"ms.chip.pack5"));
		friendOutDataList.Add (new ShopItemData (100,ShopItemEnum.MonthCard,150,"ms.chip.pack6"));

		dragPanel = new DragPanel("RewardDragPanel", ShopItem.Prefab);
		dragPanel.CreatUI();
		dragPanel.AddItem (0);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.ShopListDragPanelArgs,transform);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			ShopItem fuv = ShopItem.Inject(dragPanel.ScrollItem[ i ]);
			fuv.Data = friendOutDataList[ i ];
//			fuv.callback = ClickItem;
		}
	}

	private void ClickButton( GameObject button) {
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
	}

    private void OnClickFriendExpansion( GameObject button ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DoFriendExpansion", null);
        ExcuteCallback(cbdArgs);
    }

    private void OnClickStaminaRecover( GameObject button ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DoStaminaRecover", null);
        ExcuteCallback(cbdArgs);
    }

    private void OnClickUnitExpansion( GameObject button ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DoUnitExpansion", null);
        ExcuteCallback(cbdArgs);
    }

	private void ShowUIAnimation(){
		infoPanelRoot.transform.localPosition = new Vector3(-1000, -310, 0);
		windowRoot.transform.localPosition = new Vector3(1000, -620, 0);
		iTween.MoveTo(infoPanelRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
		iTween.MoveTo(windowRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
	}

	public void Buy1(){
		StoreInventory.BuyItem (GameCurrencyAssets.CHIP_PACK1.ItemId);
	}
	
	public void Buy2(){
		StoreInventory.BuyItem (GameCurrencyAssets.CHIP_PACK2.ItemId);
	}

	public void Buy3(){
		StoreInventory.BuyItem (GameCurrencyAssets.CHIP_PACK3.ItemId);
	}

	public void Buy4(){
		StoreInventory.BuyItem (GameCurrencyAssets.CHIP_PACK4.ItemId);
	}

	public void Buy5(){
		StoreInventory.BuyItem (GameCurrencyAssets.CHIP_PACK5.ItemId);
	}

	public void Buy6(){
		StoreInventory.BuyItem (GameCurrencyAssets.CHIP_PACK6.ItemId);
	}
}
