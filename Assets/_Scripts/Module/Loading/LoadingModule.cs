// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
//using System;
using bbproto;
using UnityEngine;
using System.Collections.Generic;


public class LoadingModule : ModuleBase {
    
	public int currentVersion = -1;

	public LoadingModule(UIConfigItem config):base(  config) {
		CreateUI<LoadingView> ();

//		GameDataPersistence.Instance.StoreData(GameDataPersistence.UUID, "");
//		GameDataPersistence.Instance.StoreData(GameDataPersistence.USER_ID, 0);
//		BattleConfigData.Instance.ClearData ();
    }

	public override void OnReceiveMessages (params object[] data)
	{
		if (data [0] == "StartLogin") {
			UserController.Instance.Login(LoginSuccess, QuestStarCallback);
//			netBase.OnRequest(null, LoginSuccess);
		}else if(data[0] == "FirstLogin"){
			UserController.Instance.Login( LoginSuccess, QuestStarCallback, (uint)data[1]);
		}
	}

	void QuestStarCallback(object data) {
		RspQuestStarList rsp = data as bbproto.RspQuestStarList;
		Debug.LogWarning("QuestStar:"+ rsp.copyInfo.normalCopyInfo.questStarList);
		foreach(QuestStarObj star in rsp.copyInfo.normalCopyInfo.questStarList) {
			Debug.LogWarning("    qId:"+star.questId +" => star:"+star.star);
		}

		//
		if ( rsp.copyInfo != null ) {
			DataCenter.Instance.NormalCopyInfo = rsp.copyInfo.normalCopyInfo;
			DataCenter.Instance.NormalCopyInfo.CopyType = ECopyType.CT_NORMAL;
			DataCenter.Instance.EliteCopyInfo = rsp.copyInfo.eliteCopyInfo;
			DataCenter.Instance.EliteCopyInfo.CopyType = ECopyType.CT_ELITE;
		}
	}

    void LoginSuccess(object data) {
		bbproto.RspAuthUser rspAuthUser;


		Debug.Log ("login success: " + data);
        if (data != null) {
            rspAuthUser = data as bbproto.RspAuthUser;
            if (rspAuthUser == null) {
//				Debug.LogError("authUser response rspAuthUser == null");
                return;
            }
            
            if (rspAuthUser.header.code != 0) {
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspAuthUser.header.code);
//				Debug.LogError("rspAuthUser return code: "+rspAuthUser.header.code+" error:" + rspAuthUser.header.error);
                return;
            }
            
			if(rspAuthUser.newAppVersion > 0){

				TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("HighVersionToLoadTitle"),TextCenter.GetText("HighVersionToLoad"),TextCenter.GetText("OK"),o=>{
					Debug.Log("app url: " + rspAuthUser.appUrl);
					Application.OpenURL (rspAuthUser.appUrl);
				});
				return;
			}

            uint userId = rspAuthUser.user.userId;
            
            if (rspAuthUser.isNewUser == 1) {
                LogHelper.Log("New user registeed, save userid:" + userId);
                GameDataPersistence.Instance.StoreData(GameDataPersistence.USER_ID, rspAuthUser.user.userId);
            }
            
            //TODO: update localtime with servertime
            //localTime = rspAuthUser.serverTime

            //save to GlobalData
			GameTimer.GetInstance().InitDateTime(rspAuthUser.serverTime);
			GameTimer.GetInstance().recovertime = rspAuthUser.user.staminaRecover - rspAuthUser.serverTime;

            if (rspAuthUser.account != null) {
				DataCenter.Instance.UserData.AccountInfo = rspAuthUser.account;
            }
            
            if (rspAuthUser.user != null) {
				DataCenter.Instance.UserData.UserInfo = rspAuthUser.user;
                if (rspAuthUser.evolveType != null) {
                    DataCenter.Instance.UserData.UserInfo.EvolveType = rspAuthUser.evolveType;
                }
            } else {
//				Debug.LogError("authUser response rspAuthUser.user == null");
            }
            
            if (rspAuthUser.friends != null) {
				List<FriendInfo> supportFriends = new List<FriendInfo>();
                foreach (FriendInfo fi in rspAuthUser.friends) {
					supportFriends.Add(fi);
					DataCenter.Instance.UnitData.UserUnitList.Add(fi.userId, fi.UserUnit.uniqueId, fi.UserUnit);
                }
				DataCenter.Instance.FriendData.AddSupportFriend(supportFriends);
            } else {
//                Debug.LogError("rsp.friends==null");
            }
            
			DataCenter.Instance.QuestData.EventStageList = new List<StageInfo>();
			if (rspAuthUser.eventList != null) {
				foreach (StageInfo stage in rspAuthUser.eventList) {
					if(stage.quests.Count >0){
						DataCenter.Instance.QuestData.EventStageList.Add(stage);
					}
				}
			}
			
			if (rspAuthUser.unitList != null) {
                foreach (UserUnit unit in rspAuthUser.unitList) {
//					DataCenter.Instance.MyUnitList.Add(userId, unit.uniqueId, TUserUnit.GetUserUnit(userId,unit));
					unit.userID = userId;
					DataCenter.Instance.UnitData.UserUnitList.Add(userId, unit.uniqueId, unit);
                }
                LogHelper.Log("rspAuthUser add to myUserUnit.count: {0}", rspAuthUser.unitList.Count);
            }
            
            if (rspAuthUser.party != null && rspAuthUser.party.partyList != null) {
				DataCenter.Instance.UnitData.PartyInfo = rspAuthUser.party;
                //TODO: replace ModelManager.GetData(UnitPartyInfo) with DataCenter.Instance.UnitData.PartyInfo.CurrentParty
				DataCenter.Instance.SetData(ModelEnum.UnitPartyInfo, DataCenter.Instance.UnitData.PartyInfo.CurrentParty);
            }
            
            if (rspAuthUser.questClear != null) {
				DataCenter.Instance.QuestData.QuestClearInfo = rspAuthUser.questClear;
            }
            
			DataCenter.Instance.UnitData.CatalogInfo = new UnitCatalogInfo(rspAuthUser.meetUnitFlag, rspAuthUser.haveUnitFlag);

			if( rspAuthUser.notice != null) {
				DataCenter.Instance.CommonData.NoticeInfo = rspAuthUser.notice;
				DataCenter.Instance.FriendData.HelperInfo = rspAuthUser.helpCountInfo;
			}

			if( rspAuthUser.login != null) {
				DataCenter.Instance.UserData.LoginInfo = rspAuthUser.login;
			}


			NoviceGuideStepManager.Instance.InitGuideStage(rspAuthUser.userGuideStep);

//#endif
#if UNITY_EDITOR 
//			NoviceGuideStepManager.Instance.CurrentGuideStep = NoviceGuideStage.NONE;
#endif
//			NoviceGuideStepManager.Instance.CurrentGuideStep = NoviceGuideStage.NoviceGuideStepB_1;

			recoverQuestID = (uint)BattleConfigData.Instance.hasBattleData();
			if(recoverQuestID > 0) {
				if(NoviceGuideStepManager.Instance.isInNoviceGuide()){
					SureRetry(null);
				} else {
					TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("BattleContinueTitle"),TextCenter.GetText("BattleContinueContent"),TextCenter.GetText("Resume"),TextCenter.GetText("Discard"),SureRetry,Cancel);
				}
			}
			else{
				EnterGame();
			}
        }
    }

	void EnterGame () {
		ModuleManager.Instance.HideModule (ModuleEnum.LoadingModule);
		if (NoviceGuideStepManager.Instance.CurrentGuideStep == NoviceGuideStage.NoviceGuideStepA_1) {
			StartQuestParam sqp = new StartQuestParam ();
			sqp.currPartyId = DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId;
			sqp.helperUserUnit = null;	//pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
			sqp.questId = 0;			//questInfo.Data.ID;
			sqp.stageId = 0;			//questInfo.StageID;
			sqp.startNew = 1;
			sqp.isUserGuide = 1;
			QuestController.Instance.StartQuest(sqp, RspStartQuest);
		} else {
			ModuleManager.Instance.EnterMainScene();
			
			if (!NoviceGuideStepManager.Instance.isInNoviceGuide()) {
				if (DataCenter.Instance.CommonData.NoticeInfo != null && DataCenter.Instance.CommonData.NoticeInfo.NoticeList != null
				    && DataCenter.Instance.CommonData.NoticeInfo.NoticeList.Count > 0 ) {
					ModuleManager.Instance.ShowModule (ModuleEnum.OperationNoticeModule);	
				}
				else { // no 
					if (DataCenter.Instance.UserData.LoginInfo.Bonus != null && DataCenter.Instance.UserData.LoginInfo.Bonus != null
					    && DataCenter.Instance.UserData.LoginInfo.Bonus.Count > 0 ) {
						//						Debug.LogError("show Reward scene... ");
						foreach (var item in DataCenter.Instance.UserData.LoginInfo.Bonus) {
							if(item.enabled == 1){
								ModuleManager.Instance.ShowModule (ModuleEnum.RewardModule);
								return;
							}
						}
						
					}
				}	
			}
		}
	}

	private void RspStartQuest(object data) {
		QuestDungeonData tqdd = null;
		bbproto.RspStartQuest rspStartQuest = data as bbproto.RspStartQuest;
		if (rspStartQuest.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rspStartQuest.header.code+", error:"+rspStartQuest.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspStartQuest.header.code);
			return;
		}

		if (rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null) {
			LogHelper.Log("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);
			DataCenter.Instance.UserData.UserInfo.staminaNow = rspStartQuest.staminaNow;
			DataCenter.Instance.UserData.UserInfo.staminaRecover = rspStartQuest.staminaRecover;
			tqdd = rspStartQuest.dungeonData;
			tqdd.assignData();
//			DataCenter.Instance.SetData(ModelEnum.MapConfig, tqdd);
		}
		
		if (data == null || tqdd == null) { return; }

		Umeng.GA.StartLevel ("Quest" + tqdd.questId);

		EnterBattle (tqdd);
	} 

	private void EnterBattle (QuestDungeonData tqdd) {
		BattleConfigData.Instance.BattleFriend = null;//pickedHelperInfo;//pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
//		Debug.LogError(tqdd.)
		BattleConfigData.Instance.ResetFromServer(tqdd);
		BattleConfigData.Instance.StoreData (tqdd.questId);
		ModuleManager.Instance.EnterBattle();
	}

	uint recoverQuestID = 0;



	void SureRetry(object data) {
		BattleConfigData.Instance.ResetFromDisk();
//		RecoverParty ();
		ModuleManager.Instance.EnterBattle();
	}

//	void RecoverParty() {
//		GameState gs = (GameState)BattleConfigData.Instance.gameState;
//		if (gs == GameState.Evolve) {
//			PartyInfo tpi = DataCenter.Instance.UnitData.PartyInfo;
//			tpi.CurrentPartyId = tpi.AllParty.Count;
//			tpi.AllParty.Add(BattleConfigData.Instance.party);
//		}
//		DataCenter.gameState = gs;
//	}

	void Cancel(object data) {

		QuestController.Instance.RetireQuest (o=>{
			BattleConfigData.Instance.ClearData ();
			EnterGame();
		}, recoverQuestID);
	}

    void TurnToReName() {
        if (DataCenter.Instance.UserData.UserInfo == null) {
//            Debug.LogError("DataCenter.Instance.UserData.UserInfo is null");
            return;
        }
        
        if (DataCenter.Instance.UserData.UserInfo.nickName == null) {
            Debug.LogError("DataCenter.Instance.UserData.UserInfo.NickName is null");
            return;
        }
        
        if (DataCenter.Instance.UserData.UserInfo.nickName.Length == 0) {
			ModuleManager.Instance.ShowModule(ModuleEnum.OthersModule);
//            Debug.Log("PlayerInfoBar.ChangeScene( Others ).");
        }
        
        Debug.Log("PlayerInfoBar.TurnToReName() : End. NickName is " + DataCenter.Instance.UserData.UserInfo.nickName);
    }
}

