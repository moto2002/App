﻿//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class ConfigDragPanel{
//	public int manualHeight;
//
//	public static Dictionary<string, object> OthersDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> UnitListDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> CatalogDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> LevelUpDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> PartyListDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> FriendListDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> HelperListDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> StoryStageDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> OnSaleUnitDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> QuestSelectDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> LevelUpBaseDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> LevelUpMaterialDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> RewardListDragPanelArgs = new Dictionary<string, object>();
//	public static Dictionary<string, object> ShopListDragPanelArgs = new Dictionary<string, object>();
//
//	public ConfigDragPanel(){
//		UIRoot uiRoot = Main.Instance.uiRoot.GetComponent<UIRoot> ();
//		manualHeight = uiRoot.manualHeight;
//		Config();
//	}
//
//	private void Config(){
//		ConfigOthersDragPanel();
//		ConfigUnitListDragPanel();
//		ConfigCatalogDragPanel();
//		ConfigLevelUpDragPanel();
//		ConfigPartyListDragPanel();
//		ConfigFriendListDragPanel();
//		ConfigHelperListDragPanel();
//		ConfigStoryStageDragPanel();
//		ConfigOnSaleUnitDragPanel();
//		ConfigQuestSelectDragPanel();
//		ConfigLevelUpBaseDragPanel();
//		ConfigLevelUpMaterialDragPanel();
//		ConfigRewardListDragPanel ();
//		ConfigShopListDragPanelArgs ();
//	}
//
//	private void ConfigUnitListDragPanel(){
//		//Debug.Log("ConfigDragPanel.Config(), UnitListDragPanelArgs...");
//		UnitListDragPanelArgs.Add("scrollerLocalPos",				220 * Vector3.up								);
//		UnitListDragPanelArgs.Add("position",							Vector3.zero									);
//		UnitListDragPanelArgs.Add("clipRange",						new Vector4(0, -210, 640, 600)			);
//		UnitListDragPanelArgs.Add("gridArrange",						UIGrid.Arrangement.Vertical			);
//		UnitListDragPanelArgs.Add("scrollBarPosition",				new Vector3(-320, -464, 0)				);
//		UnitListDragPanelArgs.Add("cellWidth",							100												);
//		UnitListDragPanelArgs.Add("cellHeight",						100												);
//		UnitListDragPanelArgs.Add("maxPerLine",						 5													);
//		UnitListDragPanelArgs.Add("depth",						 	  	1													);
//        
//	}
//
//	private void ConfigLevelUpDragPanel() {
//		//Debug.Log("ConfigDragPanel.Config(), LevelUpDragPanelArgs...");
//		LevelUpDragPanelArgs.Add("scrollerLocalPos",				-240 * Vector3.up							);
//		LevelUpDragPanelArgs.Add("position",							Vector3.zero									);
//		LevelUpDragPanelArgs.Add("clipRange",						new Vector4(0, 0, 640, 200)				);
//		LevelUpDragPanelArgs.Add("gridArrange",					UIGrid.Arrangement.Horizontal		);
//		LevelUpDragPanelArgs.Add("scrollBarPosition",				new Vector3(-320, -120, 0)				);
//		LevelUpDragPanelArgs.Add("cellWidth",						130												);
//		LevelUpDragPanelArgs.Add("cellHeight",						130												);
//		LevelUpDragPanelArgs.Add("maxPerLine",						 0													);
//	}
//
//	private void ConfigFriendListDragPanel(){
//		//Debug.Log("ConfigDragPanel.Config(), FriendListDragPanelArgs...");
//		FriendListDragPanelArgs.Add("scrollerLocalPos",			 220 * Vector3.up							);
//		FriendListDragPanelArgs.Add("position",						 Vector3.zero									);
//		FriendListDragPanelArgs.Add("clipRange",						 new Vector4(0, -210, 640, 600)		);
//		FriendListDragPanelArgs.Add("gridArrange",					 UIGrid.Arrangement.Vertical			);
//		FriendListDragPanelArgs.Add("scrollBarPosition",			 new Vector3(-320, -540, 0)				);
//		FriendListDragPanelArgs.Add("cellWidth",						 140												);
//		FriendListDragPanelArgs.Add("cellHeight",						 140												);
//		FriendListDragPanelArgs.Add("maxPerLine",					  4													);
//		FriendListDragPanelArgs.Add("depth",						 	  1													);
//	}
//
//	
//	private void ConfigHelperListDragPanel(){
//		//Debug.Log("ConfigDragPanel.Config(), HelperListDragPanelArgs...");
//		HelperListDragPanelArgs.Add("scrollerLocalPos",			 Vector3.zero									);
//		HelperListDragPanelArgs.Add("position", 						 Vector3.zero									);
//		HelperListDragPanelArgs.Add("gridArrange", 				 UIGrid.Arrangement.Horizontal		);
//		HelperListDragPanelArgs.Add("scrollMovement", 			 UIScrollView.Movement.Vertical		);
//		HelperListDragPanelArgs.Add("maxPerLine", 					 1													);
//		HelperListDragPanelArgs.Add("clipRange", 					 new Vector4(0, -15, 640, 630/*manualHeight - 290*/));
//		HelperListDragPanelArgs.Add("scrollBarPosition",			 new Vector3(1280, 1350, 0)				);
//		HelperListDragPanelArgs.Add("cellWidth", 					 0													);
//		HelperListDragPanelArgs.Add("cellHeight", 					 120												);
//		HelperListDragPanelArgs.Add("depth",						 	  2													);
//	}
//
//	
//	private void ConfigLevelUpBaseDragPanel(){
//		//Debug.Log("ConfigDragPanel.Config(), LevelUpBaseDragPanelArgs...");
//		LevelUpBaseDragPanelArgs.Add("scrollerLocalPos",		 -28 * Vector3.up							);
//		LevelUpBaseDragPanelArgs.Add("position", 					 Vector3.zero									);
//		LevelUpBaseDragPanelArgs.Add("clipRange", 				 new Vector4(0, -120, 640, 400)		);
//		LevelUpBaseDragPanelArgs.Add("gridArrange",				 UIGrid.Arrangement.Vertical			);
//		LevelUpBaseDragPanelArgs.Add("scrollBarPosition",		 new Vector3(-320, -315, 0)				);
//		LevelUpBaseDragPanelArgs.Add("cellWidth", 				 120												);
//		LevelUpBaseDragPanelArgs.Add("cellHeight",				 110												);
//		LevelUpBaseDragPanelArgs.Add("maxPerLine",				  3													);
//	}
//
//	private void ConfigLevelUpMaterialDragPanel(){
//		//Debug.Log("ConfigDragPanel.Config(), LevelUpMaterialDragPanelArgs...");
//		LevelUpMaterialDragPanelArgs.Add("scrollerLocalPos",	  -28 * Vector3.up							);
//		LevelUpMaterialDragPanelArgs.Add("position", 				  Vector3.zero									);
//		LevelUpMaterialDragPanelArgs.Add("clipRange", 			  new Vector4(0, -120, 640, 400)		);
//		LevelUpMaterialDragPanelArgs.Add("gridArrange", 		  UIGrid.Arrangement.Vertical			);
//		LevelUpMaterialDragPanelArgs.Add("scrollBarPosition",	  new Vector3(-320, -315, 0)			);
//		LevelUpMaterialDragPanelArgs.Add("cellWidth", 			  110												);
//		LevelUpMaterialDragPanelArgs.Add("cellHeight",			  110												);
//		LevelUpMaterialDragPanelArgs.Add("maxPerLine",			   3												);
//	}
//
//	private void ConfigOthersDragPanel(){
//		//Debug.Log("ConfigDragPanel.Config(), OthersDragPanelArgs...");
//		OthersDragPanelArgs.Add("scrollerLocalPos" ,				 -190*Vector3.up							);
//		OthersDragPanelArgs.Add("position", 							 Vector3.zero									);
//		OthersDragPanelArgs.Add("clipRange", 						 	 new Vector4(0, 0, 640, 200)				);
//		OthersDragPanelArgs.Add("gridArrange", 					 	 UIGrid.Arrangement.Horizontal		);
//		OthersDragPanelArgs.Add("scrollBarPosition", 				 new Vector3(-320,-120,0)				);
//		OthersDragPanelArgs.Add("cellWidth", 						 	 150												);
//		OthersDragPanelArgs.Add("cellHeight",						 	 130												);
//		OthersDragPanelArgs.Add("maxPerLine", 						  0													);
//	}
//
//	private void ConfigPartyListDragPanel(){
//		//Debug.Log("ConfigDragPanel.Config(), PartyListDragPanelArgs...");
//		PartyListDragPanelArgs.Add("scrollerLocalPos",				  100 * Vector3.up									);
//		PartyListDragPanelArgs.Add("position", 							 Vector3.zero								);
//		PartyListDragPanelArgs.Add("clipRange", 						 new Vector4(0, -100, 640, 315)				);
//		PartyListDragPanelArgs.Add("gridArrange", 					 UIGrid.Arrangement.Vertical			);
//		PartyListDragPanelArgs.Add("scrollBarPosition",				 new Vector3(-320, -251, 0)				);
//		PartyListDragPanelArgs.Add("cellWidth", 						 100												);
//		PartyListDragPanelArgs.Add("cellHeight",						 100												);
//		PartyListDragPanelArgs.Add("maxPerLine",					 3													);
//		PartyListDragPanelArgs.Add("depth",						 	  	1													);
//        
//	}
//        
//        private void ConfigStoryStageDragPanel(){
//		//Debug.Log("ConfigDragPanel.Config(), StoryStageDragPanelArgs...");
//		StoryStageDragPanelArgs.Add("scrollerLocalPos", 				215 * Vector3.up							);
//		StoryStageDragPanelArgs.Add("position", 							Vector3.zero								);
//		StoryStageDragPanelArgs.Add("clipRange", 						new Vector4(0, 0, 640, 200)			);
//		StoryStageDragPanelArgs.Add("gridArrange",					UIGrid.Arrangement.Horizontal	);
//		StoryStageDragPanelArgs.Add("scrollBarPosition", 				new Vector3(-320, -120, 0)			);
//		StoryStageDragPanelArgs.Add("cellWidth", 						230											);
//		StoryStageDragPanelArgs.Add("cellHeight", 						150											);
//		StoryStageDragPanelArgs.Add("maxPerLine", 					0												);
//	}
//
//	
//	private void ConfigQuestSelectDragPanel(){
//		//Debug.Log("ConfigDragPanel.Config(), QuestSelectDragPanelArgs...");
//		QuestSelectDragPanelArgs.Add("scrollerLocalPos",			 	Vector3.zero								);
//		QuestSelectDragPanelArgs.Add("position", 						Vector3.zero								);
//		QuestSelectDragPanelArgs.Add("gridArrange", 				 	UIGrid.Arrangement.Horizontal	);
//		QuestSelectDragPanelArgs.Add("scrollMovement", 			UIScrollView.Movement.Vertical	);
//		QuestSelectDragPanelArgs.Add("maxPerLine", 					1												);
//		QuestSelectDragPanelArgs.Add("clipRange", 					 	new Vector4(0, -30, 640, manualHeight - 290));
//		QuestSelectDragPanelArgs.Add("scrollBarPosition",			 	new Vector3(1280, 1350, 0)			);
//		QuestSelectDragPanelArgs.Add("cellWidth", 					 	0												);
//		QuestSelectDragPanelArgs.Add("cellHeight", 					 	122											);
//		QuestSelectDragPanelArgs.Add("depth",							2												);
//	}
//
//	
//	private void ConfigOnSaleUnitDragPanel(){
//		//Debug.Log("ConfigDragPanel.Config(), OnSaleUnitDragPanelArgs...");
//		OnSaleUnitDragPanelArgs.Add("scrollerLocalPos",				-158 * Vector3.up						);
//		OnSaleUnitDragPanelArgs.Add("position", 						Vector3.zero								);
//		OnSaleUnitDragPanelArgs.Add("clipRange", 						new Vector4(0, -100, 640, 350)		);
//		OnSaleUnitDragPanelArgs.Add("gridArrange", 					UIGrid.Arrangement.Vertical		);
//		OnSaleUnitDragPanelArgs.Add("scrollBarPosition",				new Vector3(-320, -248, 0)			);
//		OnSaleUnitDragPanelArgs.Add("cellWidth", 						100											);
//		OnSaleUnitDragPanelArgs.Add("cellHeight",						100											);
//		OnSaleUnitDragPanelArgs.Add("maxPerLine",					3												);
//		OnSaleUnitDragPanelArgs.Add("depth", 							1												);
//	}
//
//	private void ConfigCatalogDragPanel(){
//		//Debug.Log("ConfigDragPanel.Config(), CatalogDragPanelArgs...");
//		CatalogDragPanelArgs.Add("scrollerLocalPos",					280 * Vector3.up							);
//		CatalogDragPanelArgs.Add("position", 								Vector3.zero								);
//		CatalogDragPanelArgs.Add("clipRange", 							new Vector4(0, -235, 640, 640)		);
//		CatalogDragPanelArgs.Add("gridArrange", 						UIGrid.Arrangement.Vertical		);
//		CatalogDragPanelArgs.Add("scrollBarPosition",					new Vector3(-320, -565, 0)			);
//		CatalogDragPanelArgs.Add("cellWidth", 							120											);
//		CatalogDragPanelArgs.Add("cellHeight",							120											);
//		CatalogDragPanelArgs.Add("maxPerLine",							5												);
//	}
//
//	private void ConfigRewardListDragPanel(){
//		RewardListDragPanelArgs.Add("scrollerLocalPos",					-100 * Vector3.up					);
//		RewardListDragPanelArgs.Add("position", 								Vector3.zero								);
//		RewardListDragPanelArgs.Add("clipRange", 							new Vector4(0, -235, 640, 660)		);
//		RewardListDragPanelArgs.Add("gridArrange", 						UIGrid.Arrangement.Horizontal		);
//		RewardListDragPanelArgs.Add("scrollBarPosition",					new Vector3(307, 97, 0)			);
//		RewardListDragPanelArgs.Add("cellWidth", 							600											);
//		RewardListDragPanelArgs.Add("cellHeight",							120											);
//		RewardListDragPanelArgs.Add("maxPerLine",							1												);
//		RewardListDragPanelArgs.Add ("scrollMovement", 					UIScrollView.Movement.Vertical);
//		RewardListDragPanelArgs.Add ("scrollBarDir", 						UIProgressBar.FillDirection.TopToBottom);
//		RewardListDragPanelArgs.Add ("depth",								13												);
//	}
//
//	private void ConfigShopListDragPanelArgs(){
//		ShopListDragPanelArgs.Add("scrollerLocalPos",					-100 * Vector3.up					);
//		ShopListDragPanelArgs.Add("position", 								Vector3.zero								);
//		ShopListDragPanelArgs.Add("clipRange", 							new Vector4(0, 120, 640, 344)		);
//		ShopListDragPanelArgs.Add("gridArrange", 						UIGrid.Arrangement.Horizontal		);
//		ShopListDragPanelArgs.Add("scrollBarPosition",					new Vector3(306, 292, 0)			);
//		ShopListDragPanelArgs.Add("cellWidth", 							600											);
//		ShopListDragPanelArgs.Add("cellHeight",							89											);
//		ShopListDragPanelArgs.Add("maxPerLine",							1												);
//		ShopListDragPanelArgs.Add ("scrollMovement", 					UIScrollView.Movement.Vertical);
//		ShopListDragPanelArgs.Add ("scrollBarDir", 						UIProgressBar.FillDirection.TopToBottom);
//		ShopListDragPanelArgs.Add ("depth",								6												);
//	}
//
//}
