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
    
	public int currentVersion = -1;

    public LoadingLogic(string uiName):base(uiName) {
        MsgCenter.Instance.AddListener(CommandEnum.StartFirstLogin, StartFirstLogin);
    }
    
    public override void CreatUI () {
        base.CreatUI ();
    }
    
    public override void ShowUI () {

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
//				Debug.LogError("authUser response rspAuthUser == null");
                return;
            }
            
            if (rspAuthUser.header.code != 0) {
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspAuthUser.header.code);
//				Debug.LogError("rspAuthUser return code: "+rspAuthUser.header.code+" error:" + rspAuthUser.header.error);
                return;
            }
            
			if(rspAuthUser.newAppVersion > 0){
				MsgWindowParams mwp = new MsgWindowParams ();
				mwp.btnParams = new BtnParam[1];
				mwp.titleText = TextCenter.GetText("HighVersionToLoadTitle");
				mwp.contentText = TextCenter.GetText("HighVersionToLoad");
				
				BtnParam sure = new BtnParam ();
				sure.callback = o=>{
					Debug.Log("app url: " + rspAuthUser.appUrl);
					Application.OpenURL (rspAuthUser.appUrl);
				};
				sure.text = TextCenter.GetText("OK");
				mwp.btnParam = sure;
				
				MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,mwp);
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
			GameTimer.GetInstance().recovertime = rspAuthUser.user.staminaRecover - rspAuthUser.serverTime;

            if (rspAuthUser.account != null) {
                DataCenter.Instance.AccountInfo = new TAccountInfo(rspAuthUser.account);
            }
            
            if (rspAuthUser.user != null) {
				DataCenter.Instance.UserInfo = new TUserInfo(rspAuthUser.user);
                if (rspAuthUser.evolveType != null) {
                    DataCenter.Instance.UserInfo.EvolveType = rspAuthUser.evolveType;
                }
            } else {
//				Debug.LogError("authUser response rspAuthUser.user == null");
            }
            
            if (rspAuthUser.friends != null) {
				List<TFriendInfo> supportFriends = new List<TFriendInfo>();
                foreach (FriendInfo fi in rspAuthUser.friends) {
                    TFriendInfo tfi = new TFriendInfo(fi);
					supportFriends.Add(tfi);
					DataCenter.Instance.UserUnitList.Add(tfi.UserId, tfi.UserUnit.ID, tfi.UserUnit);
                }
				DataCenter.Instance.SupportFriends = supportFriends;
            } else {
//                Debug.LogError("rsp.friends==null");
            }
            
			DataCenter.Instance.EventStageList = new List<TStageInfo>();
			if (rspAuthUser.eventList != null) {
				foreach (StageInfo stage in rspAuthUser.eventList) {
					if(stage.quests.Count >0){
						TStageInfo tsi = new TStageInfo(stage);
						DataCenter.Instance.EventStageList.Add(tsi);
					}
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

			if( rspAuthUser.notice != null) {
				DataCenter.Instance.NoticeInfo = new TNoticeInfo(rspAuthUser.notice);
				DataCenter.Instance.HelperCount = rspAuthUser.helpCountInfo;
			}

			if( rspAuthUser.login != null) {
				DataCenter.Instance.LoginInfo = new TLoginInfo(rspAuthUser.login);
			}

			NoviceGuideStepEntityManager.InitGuideStage(rspAuthUser.userGuideStep);
//			NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.NONE;
#if UNITY_EDITOR
//			NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.SCRATCH;
//			NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.NONE;
#endif

			recoverQuestID = (uint)ConfigBattleUseData.Instance.hasBattleData();
			if(recoverQuestID > 0) {
				if(NoviceGuideStepEntityManager.isInNoviceGuide()){
					SureRetry(null);
				} else {
					MsgWindowParams mwp = new MsgWindowParams ();
					mwp.btnParams = new BtnParam[2];
					mwp.titleText = TextCenter.GetText("BattleContinueTitle");
					mwp.contentText = TextCenter.GetText("BattleContinueContent");
					
					BtnParam sure = new BtnParam ();
					sure.callback = SureRetry;
					sure.text = TextCenter.GetText("Resume");
					mwp.btnParams[0] = sure;
					
					sure = new BtnParam ();
					sure.callback = Cancel;
					sure.text = TextCenter.GetText("Discard");
					mwp.btnParams[1] = sure;
					
					MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,mwp);
				}
			}
			else{
				EnterGame();
			}
        }
    }
	
	private void StartFight() {
		StartQuest sq = new StartQuest ();
		StartQuestParam sqp = new StartQuestParam ();
		sqp.currPartyId = DataCenter.Instance.PartyInfo.CurrentPartyId;
		sqp.helperUserUnit = null;	//pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
		sqp.questId = 0;			//questInfo.Data.ID;
		sqp.stageId = 0;			//questInfo.StageID;
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

		Umeng.GA.StartLevel ("Quest" + tqdd.QuestId);

		EnterBattle (tqdd);
	} 

	private void EnterBattle (TQuestDungeonData tqdd) {
		ConfigBattleUseData.Instance.BattleFriend = null;//pickedHelperInfo;//pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
//		Debug.LogError(tqdd.)
		ConfigBattleUseData.Instance.ResetFromServer(tqdd);
		UIManager.Instance.EnterBattle();
	}

	uint recoverQuestID = 0;

	void EnterGame () {
		if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.GOLD_BOX) {
			StartFight();
		} else {
			UIManager.Instance.ChangeScene(SceneEnum.Start);
			
			UIManager.Instance.ChangeScene(SceneEnum.Home);

			if (!NoviceGuideStepEntityManager.isInNoviceGuide()) {
				if (DataCenter.Instance.NoticeInfo != null && DataCenter.Instance.NoticeInfo.NoticeList != null
				    && DataCenter.Instance.NoticeInfo.NoticeList.Count > 0 ) {
					UIManager.Instance.ChangeScene (SceneEnum.OperationNotice);	
				}
				else { // no 
					if (DataCenter.Instance.LoginInfo.Bonus != null && DataCenter.Instance.LoginInfo.Bonus != null
					    && DataCenter.Instance.LoginInfo.Bonus.Count > 0 ) {
//						Debug.LogError("show Reward scene... ");
						foreach (var item in DataCenter.Instance.LoginInfo.Bonus) {
							if(item.enabled == 1){
								UIManager.Instance.ChangeScene (SceneEnum.Reward);
								return;
							}
						}
							
					}
				}	
			}
		}
	}

	void SureRetry(object data) {
		ConfigBattleUseData.Instance.ResetFromDisk();
		RecoverParty ();
		UIManager.Instance.EnterBattle();
	}

	void RecoverParty() {
		GameState gs = (GameState)ConfigBattleUseData.Instance.gameState;
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
        if (DataCenter.Instance.UserInfo == null) {
//            Debug.LogError("DataCenter.Instance.UserInfo is null");
            return;
        }
        
        if (DataCenter.Instance.UserInfo.NickName == null) {
            Debug.LogError("DataCenter.Instance.UserInfo.NickName is null");
            return;
        }
        
        if (DataCenter.Instance.UserInfo.NickName.Length == 0) {
            UIManager.Instance.ChangeScene(SceneEnum.Others);
//            Debug.Log("PlayerInfoBar.ChangeScene( Others ).");
        }
        
        Debug.Log("PlayerInfoBar.TurnToReName() : End. NickName is " + DataCenter.Instance.UserInfo.NickName);
    }
}

