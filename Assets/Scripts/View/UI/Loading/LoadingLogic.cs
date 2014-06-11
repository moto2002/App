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


public class LoadingLogic : ConcreteComponent {
    
    public LoadingLogic(string uiName):base(uiName) {
        MsgCenter.Instance.AddListener(CommandEnum.StartFirstLogin, StartFirstLogin);
    }
    
    public override void CreatUI () {
        base.CreatUI ();
    }
    
    public override void ShowUI () {
//		GameDataStore.Instance.StoreData(GameDataStore.UUID, "");
//        GameDataStore.Instance.StoreData(GameDataStore.USER_ID, 0);

        base.ShowUI ();
    }
    
    public override void HideUI () {
        base.HideUI ();

		DestoryUI ();
    }

    public void StartLogin(){
        INetBase netBase = new AuthUser();
        netBase.OnRequest(null, LoginSuccess);
    }

    public void StartFirstLogin(object args){
        uint roleSelected = (uint)args;
        AuthUser.FirstLogin(roleSelected, LoginSuccess);
    }
	bbproto.RspAuthUser rspAuthUser;
    void LoginSuccess(object data) {
        if (data != null) {
            rspAuthUser = data as bbproto.RspAuthUser;
            if (rspAuthUser == null) {
				Debug.LogError("authUser response rspAuthUser == null");
                return;
            }
            
            if (rspAuthUser.header.code != 0) {
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspAuthUser.header.code);
				Debug.LogError("rspAuthUser return code: "+rspAuthUser.header.code+" error:" + rspAuthUser.header.error);
                return;
            }
            
            uint userId = rspAuthUser.user.userId;
            
            if (rspAuthUser.isNewUser == 1) {
                LogHelper.Log("New user registeed, save userid:" + userId);
                GameDataStore.Instance.StoreData(GameDataStore.USER_ID, rspAuthUser.user.userId);
            }
            
            //TODO: update localtime with servertime
            //localTime = rspAuthUser.serverTime

            //save to GlobalData
			GameTimer.GetInstance().InitDateTime(rspAuthUser.serverTime);

            if (rspAuthUser.account != null) {
                DataCenter.Instance.AccountInfo = new TAccountInfo(rspAuthUser.account);
            }
            
            if (rspAuthUser.user != null) {
				Debug.Log("authUser response userId:" + rspAuthUser.user.userId);

				DataCenter.Instance.UserInfo = new TUserInfo(rspAuthUser.user);
                if (rspAuthUser.evolveType != null) {
                    DataCenter.Instance.UserInfo.EvolveType = rspAuthUser.evolveType;
                }
            }
            else {
				Debug.LogError("authUser response rspAuthUser.user == null");
            }
            
            if (rspAuthUser.friends != null) {
                DataCenter.Instance.SupportFriends = new List<TFriendInfo>();
                foreach (FriendInfo fi in rspAuthUser.friends) {
                    TFriendInfo tfi = new TFriendInfo(fi);
                    DataCenter.Instance.SupportFriends.Add(tfi);
					DataCenter.Instance.UserUnitList.Add(tfi.UserId, tfi.UserUnit.ID, tfi.UserUnit);
                }
            }
            else {
                Debug.LogError("rsp.friends==null");
            }
            
			DataCenter.Instance.EventStageList = new List<TStageInfo>();
			if (rspAuthUser.eventList != null) {
				foreach (StageInfo stage in rspAuthUser.eventList) {
					TStageInfo tsi = new TStageInfo(stage);
					DataCenter.Instance.EventStageList.Add(tsi);
				}
			}
			
			if (rspAuthUser.unitList != null) {
                foreach (UserUnit unit in rspAuthUser.unitList) {
//					DataCenter.Instance.MyUnitList.Add(userId, unit.uniqueId, TUserUnit.GetUserUnit(userId,unit));
					DataCenter.Instance.UserUnitList.Add(userId, unit.uniqueId, TUserUnit.GetUserUnit(userId, unit));
                }
                LogHelper.Log("rspAuthUser add to myUserUnit.count: {0}", rspAuthUser.unitList.Count);
            }
            
            if (rspAuthUser.party != null && rspAuthUser.party.partyList != null) {
                DataCenter.Instance.PartyInfo = new TPartyInfo(rspAuthUser.party);
                //TODO: replace ModelManager.GetData(UnitPartyInfo) with DataCenter.Instance.PartyInfo.CurrentParty
                ModelManager.Instance.SetData(ModelEnum.UnitPartyInfo, DataCenter.Instance.PartyInfo.CurrentParty);
            }
            
            if (rspAuthUser.questClear != null) {
                DataCenter.Instance.QuestClearInfo = new TQuestClearInfo(rspAuthUser.questClear);
            }
            
			DataCenter.Instance.CatalogInfo = new TUnitCatalog(rspAuthUser.meetUnitFlag, rspAuthUser.haveUnitFlag);

			if( rspAuthUser.notice != null){
				DataCenter.Instance.NoticeInfo = new TNoticeInfo(rspAuthUser.notice);
			}

			if( rspAuthUser.login != null){
				DataCenter.Instance.LoginInfo = new TLoginInfo(rspAuthUser.login);
			}
			NoviceGuideStepEntityManager.CurrentNoviceGuideStage = (NoviceGuideStage)rspAuthUser.userGuideStep;

//            TestUtility.Test();
            //Debug.Log("UIManager.Instance.ChangeScene(SceneEnum.Start) before...");
            //      Debug.LogError("login end");
			recoverQuestID = (uint)ConfigBattleUseData.Instance.hasBattleData();
			if(recoverQuestID > 0) {
				MsgWindowParams mwp = new MsgWindowParams ();
				mwp.btnParams = new BtnParam[2];
				mwp.titleText = TextCenter.GetText("BattleContinueTitle");
				mwp.contentText = TextCenter.GetText("BattleContinueContent");
				
				BtnParam sure = new BtnParam ();
				sure.callback = SureRetry;
				sure.text = "OK";
				mwp.btnParams[0] = sure;
				
				sure = new BtnParam ();
				sure.callback = Cancel;
				sure.text = "Cancel";
				mwp.btnParams[1] = sure;
				
				MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,mwp);
			}
			else{
				EnterGame();
			}
        }
    }

	private void StartFight(){

		StartQuest sq = new StartQuest ();
		StartQuestParam sqp = new StartQuestParam ();
		sqp.currPartyId = DataCenter.Instance.PartyInfo.CurrentPartyId;
		sqp.helperUserUnit = null;//pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
//		QuestItemView questInfo = pickedInfoForFight[ "QuestInfo"] as QuestItemView;
		sqp.questId = 0;//questInfo.Data.ID;
		sqp.stageId = 0;//questInfo.StageID;
		sqp.startNew = 1;
		sqp.isUserGuide = 1;
		sq.OnRequest (sqp, RspStartQuest);

	}
	
	private void RspStartQuest(object data) {
		TQuestDungeonData tqdd = null;
		bbproto.RspStartQuest rspStartQuest = data as bbproto.RspStartQuest;
		if (rspStartQuest.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rspStartQuest.header.code+", error:"+rspStartQuest.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspStartQuest.header.code);
			return;
		}
		if (rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null) {
			LogHelper.Log("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);
			DataCenter.Instance.UserInfo.StaminaNow = rspStartQuest.staminaNow;
			DataCenter.Instance.UserInfo.StaminaRecover = rspStartQuest.staminaRecover;
			tqdd = new TQuestDungeonData(rspStartQuest.dungeonData);
			ModelManager.Instance.SetData(ModelEnum.MapConfig, tqdd);
		}
		
		if (data == null || tqdd == null) { return; }
		EnterBattle (tqdd);
		
	} 

	private void EnterBattle (TQuestDungeonData tqdd) {
		ConfigBattleUseData.Instance.BattleFriend = null;//pickedHelperInfo;//pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
		ConfigBattleUseData.Instance.ResetFromServer(tqdd);
		UIManager.Instance.EnterBattle();
	}

	uint recoverQuestID = 0;

	void EnterGame () {
//		

		//LogHelper.Log ("notice list: " + DataCenter.Instance.NoticeInfo.NoticeList);
//		if (DataCenter.Instance.NoticeInfo != null && DataCenter.Instance.NoticeInfo.NoticeList != null) {
//			UIManager.Instance.ChangeScene (SceneEnum.OperationNotice);	
//		}

//		UIManager.Instance.ChangeScene (SceneEnum.Reward);

		if (rspAuthUser.isNewUser == 1) {
			StartFight();
//			TurnToReName();
		} else {
			UIManager.Instance.ChangeScene(SceneEnum.Start);
			
			UIManager.Instance.ChangeScene(SceneEnum.Home);
		}
	}

	void SureRetry(object data) {
		ConfigBattleUseData.Instance.ResetFromDisk();
		RecoverParty ();
		UIManager.Instance.EnterBattle();
	}

	void RecoverParty() {
		GameState gs = (GameState)ConfigBattleUseData.Instance.gameState;
//		Debug.LogError ("gs : " + gs);
		if (gs == GameState.Evolve) {
			TPartyInfo tpi = DataCenter.Instance.PartyInfo;
			tpi.CurrentPartyId = tpi.AllParty.Count;
			tpi.AllParty.Add(ConfigBattleUseData.Instance.party);
		}
		DataCenter.gameState = gs;
	}

	void Cancel(object data) {
		RetireQuest.SendRequest (RetireQuestCallback, recoverQuestID);
	}

	void RetireQuestCallback(object data) {
		ConfigBattleUseData.Instance.ClearData ();
		ConfigBattleUseData.Instance.gameState = (byte)GameState.Normal;
		EnterGame();
	}

    void TurnToReName() {
        //      Debug.Log("PlayerInfoBar.TurnToReName() : Start");
        if (DataCenter.Instance.UserInfo == null) {
            Debug.LogError("DataCenter.Instance.UserInfo is null");
            return;
        }
        
        if (DataCenter.Instance.UserInfo.NickName == null) {
            Debug.LogError("DataCenter.Instance.UserInfo.NickName is null");
            return;
        }
        
        if (DataCenter.Instance.UserInfo.NickName.Length == 0) {
            UIManager.Instance.ChangeScene(SceneEnum.Others);
            Debug.Log("PlayerInfoBar.ChangeScene( Others ).");
        }
        
        Debug.Log("PlayerInfoBar.TurnToReName() : End. NickName is " + DataCenter.Instance.UserInfo.NickName);
    }
}