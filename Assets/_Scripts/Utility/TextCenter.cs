// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class TextCenter {
    public static TextCenter Instance {
        get {
            if (instance == null){
                instance = new TextCenter();
//                instance.Init();
//                instance.InitSecond();
//                instance.InitThird();
            }
            return instance;
        }
    }

	public static string GetText(string key){
		return Instance.InnerGetText( key );
	}

	public static string GetText(string key, params object[] args){
		string result = Instance.InnerGetText( key );
		if (!string.IsNullOrEmpty (result)) {
//			Debug.LogError ("result : " + result);
			result = string.Format (result, args);
		} else {
			result = string.Format(" ",args);		
		}
		if(result == null) {
			result = "";
		}
        
        return result;
    }

    public void Test(){
        LogHelper.Log("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT TextHelper.Test() start");
		LogHelper.Log("test get string {0}, result {1}", "Error", TextCenter.GetText("Error"));
		LogHelper.Log("test get string {0}, result {1}", "error1", TextCenter.GetText("error1", "test error1"));
    }

	private static TextCenter instance = new TextCenter ();

    private Dictionary<string, string> textDict;
	public string InnerGetText(string key) {
		string result = ""; //default set to key
		if(textDict != null)
		textDict.TryGetValue(key, out result);
		if(result == null || result == "") {
			result = "";
		}

		return result;
	}

	private string langStr = 
	#if LANGUAGE_CN
	"Language/lang_cn";
	#elif LANGUAGE_EN
	"Language/lang_en";
	#else
	"Language/lang_en";
	#endif

    public void Init(ResourceCallback callback){
        textDict = new Dictionary<string, string>();

		Debug.Log (langStr+" load");
        //
//		string[] data = File.ReadAllLines (Application.dataPath + "/Resources/Language/lang_en.txt");
//		ResourceManager.Instance.LoadLocalAsset ("Language/lang_en", o => {
		ResourceManager.Instance.LoadLocalAsset (langStr, o => {
			string readData = (o as TextAsset).text;
			string[] data = readData.Split ('\n');

			foreach (string s in data) {
					//Debug.Log("config: " + s + "length: " + s.Length);
				if (s.Length > 0 && s [0] != '#') {
		
					int i = s.IndexOf ('=');
					if (i < 0) {
						Debug.LogError("lang_text: INVALID Line: "+s);
						continue;
					}
					string key = s.Substring (0, i);
					string value = s.Substring (i + 1);
							//Debug.Log("sub: " + s.Substring(0,i)+"   " +s.Substring(i));
					if(!textDict.ContainsKey(key))
						textDict.Add (key, value);
				}
			}

			if(callback != null){
				callback(o);
			}
		});
	}


//        textDict.Add("OK", "OK");
//        textDict.Add("Cancel", "Cancel");
//        textDict.Add("Back", "Back");
//        textDict.Add("Open", "Open");
//
//        textDict.Add("retry", "Retry");
//        textDict.Add("error", "error");
//
//        textDict.Add("SearchError", "Search Error");
//        textDict.Add("UserNotExist", "The Friend {0} you search not exist");
//
//        textDict.Add("InputError", "Input Error");
//        textDict.Add("InputEmpty", "Could not Input empty ID!");
//
////        textDict.Add("SearchError", "Input Error");
//        textDict.Add("UserAlreadyFriend", "The ID {0} you searched is already your friend!");
//
//        textDict.Add("RefuseAll", "The ID {0} you searched is already your friend!");
//        textDict.Add("ConfirmRefuseAll", "Are you sure to refuse all friend apply?");
//
//        textDict.Add("RefreshFriend", "Friend Update");
//        textDict.Add("ConfirmRefreshFriend", "Are you sure to update friend list?");
//
//        //////// shop
//
//        textDict.Add("FriendExpand", "Friend Max Expansion");
//        textDict.Add("ConfirmFriendExpansion", "Are you sure to use 1 coin to expand your friend limit?");
//        textDict.Add("FriendExpansionInfo", "Now friends {0}/{1}");
//        textDict.Add("DoFriendExpand", "Expand");
//
//        textDict.Add("FriendExpansionFailed", "Friend Max Expansion Failed");
//        textDict.Add("FriendCountLimitReachedMax", "Now your friend limit has reached max.");
//        textDict.Add("FriendExpandStonesNotEnough", "Now your stone not enough to expand friend limit.");
//        
//        textDict.Add("FriendExpansionFinish", "Friend Max Expansion Completed");
//        textDict.Add("FriendExpansionResult", "Friend Limited Max reached {0}!");
//
//        textDict.Add("StaminaRecover", "Recover your stamina");
//        textDict.Add("ConfirmStaminaRecover", "Are you sure to use 1 coin to make your stamina full?");
//        textDict.Add("DoStaminaRecover", "Recover");
//
//        textDict.Add("StaminaRecoverFailed", "Stamina Recovering Failed");
//        textDict.Add("StaminaStillFull", "Now your stamina still full, not need to recover it.");
//        textDict.Add("StaminaRecoverStonesNotEnough", "Now your stone not enough to recover stamina.");
//
//        textDict.Add("StaminaRecoverFinish", "Stamina Recover Completed");
//        textDict.Add("StaminaRecoverResult", "All Stamina Recovered!");
//
//        textDict.Add("UnitExpand", "Units count limit expand");
//        textDict.Add("ConfirmUnitExpansion", "Are you sure to use 1 coin to expand your unit limit?");
//        textDict.Add("UnitExpansionInfo", "Now Units {0}/{1}");
//        textDict.Add("DoUnitExpansion", "Expand");
//
//        textDict.Add("UnitExpansionFailed", "Unit Max Expansion Failed");
//        textDict.Add("UnitCountLimitReachedMax", "Now your unit limit has reached max.");
//        textDict.Add("UnitExpandStonesNotEnough", "Now your stone not enough to expand unit limit.");
//
//        textDict.Add("UnitExpansionFinish", "Unit Max Expansion Completed");
//        textDict.Add("UnitExpansionResult", "Unit Limited Max Reached {0}!");
//        
//        /////// gacha
//        textDict.Add("FriendGacha", "Start Friend Point Scratch");
//        textDict.Add("FriendGachaTitle", "Friend Point Scratch");
//        textDict.Add("FriendGachaDescription", "Use friend Scratch to get units");
//        textDict.Add("FriendGachaStatus", "1 gacha: {0} points  {1} gachas: {2} points\nnow friend points {3}");
//
//        textDict.Add("ConfirmOneFriendGacha", "Gacha 1 time");
//        textDict.Add("ConfirmMaxFriendGacha", "Gacha {0} times");
//
//        textDict.Add("FriendGachaFailed", "[FF0000]Friend Gacha Failed[-]");
//        textDict.Add("UnitCountReachedMax", "Now your units owned overed unit limit:[FF0000]{0}/[-]{1}, can't do any gacha.");
//        textDict.Add("GachaFriendPointNotEnough", "1 friend gacha need {0} friend points\nYour friend points not enough.");
//
//        textDict.Add("RareGacha", "Start Rare Scratch");
//        textDict.Add("RareGachaTitle", "Rare Scratch");
//        textDict.Add("RareGachaDescription", "Use stones to get units");
//        textDict.Add("RareGachaStatus", "1 gacha: {0} stones {1} gachas: {2} stones\nnow stones {3}");
//
//        textDict.Add("ConfirmOneRareGacha", "Gacha 1 time");
//        textDict.Add("ConfirmMaxRareGacha", "Gacha {0} times");
//
//        textDict.Add("RareGachaFailed", "[FF0000]Rare Gacha Failed[-]");
//        textDict.Add("RareGachaStoneNotEnough", "1 rare gacha need {0} stones\nYour stones not enough.");
//
//        textDict.Add("EventGacha", "Start Event Scratch");
//        textDict.Add("EventGachaTitle", "Event Scratch");
//        textDict.Add("EventGachaDescription", "Use stones to get units");
//        textDict.Add("EventGachaStatus", "1 gacha: {0} stones {1} gachas: {2} stones\nnow stones {3}");
//
//        textDict.Add("ConfirmOneEventGacha", "Gacha 1 time");
//        textDict.Add("ConfirmMaxEventGacha", "Gacha {0} times");
//
//        textDict.Add("EventGachaFailed", "[FF0000]Event Gacha Failed[-]");
//        textDict.Add("EventGachaNotOpen", "Our next event gacha will coming soon, please wait it!");
//        textDict.Add("EventGachaStoneNotEnough", "1 event gacha need {0} stones\nYour stones not enough.");
//
//        textDict.Add("FriendScratch", "Friend Scratch");
//        textDict.Add("RareScratch", "Rare Scratch");
//        textDict.Add("EventScratch", "Event Scratch");
//
//        textDict.Add("GachaChances", "chances {0}/{1}");
//        textDict.Add("Lv", "Lv {0}");
//
//
//        textDict.Add("LevelUpMoneyNotEnough", "Level up need money {0}, your money is not enough.");
//        textDict.Add("MoneyNotEnoughTitle", "Money Not Enough");
//
//        ///
//	    textDict.Add ("StartQuestFail", "Start quest request failed");
//
//
//        // net error
//        textDict.Add("success", "success");
//        textDict.Add("ERROR_BASE", "ERROR_BASE");
//        textDict.Add("FAILED", "FAILED");
//        textDict.Add("INVALID_PARAMS", "INVALID_PARAMS");
//        textDict.Add("MARSHAL_ERROR", "MARSHAL_ERROR");
//        textDict.Add("UNMARSHAL_ERROR", "UNMARSHAL_ERROR");
//        textDict.Add("IOREAD_ERROR", "IOREAD_ERROR");
//        textDict.Add("IOWRITE_ERROR", "IOWRITE_ERROR");
//        textDict.Add("CONNECT_DB_ERROR", "CONNECT_DB_ERROR");
//        textDict.Add("READ_DB_ERROR", "READ_DB_ERROR");
//        textDict.Add("SET_DB_ERROR", "SET_DB_ERROR");
//        textDict.Add("DATA_NOT_EXISTS", "DATA_NOT_EXISTS");
//        
//        textDict.Add("EU_USER_BASE", "EU_USER_BASE");
//        textDict.Add("EU_INVALID_USERID", "EU_INVALID_USERID");
//        textDict.Add("EU_GET_USERINFO_FAIL", "EU_GET_USERINFO_FAIL");
//        textDict.Add("EU_USER_NOT_EXISTS", "EU_USER_NOT_EXISTS");
//        textDict.Add("EU_GET_NEWUSERID_FAIL", "EU_GET_NEWUSERID_FAIL");
//        textDict.Add("EU_UPDATE_USERINFO_ERROR", "EU_UPDATE_USERINFO_ERROR");
//        textDict.Add("EF_FRIEND_BASE", "EF_FRIEND_BASE");
//        textDict.Add("EF_FRIEND_NOT_EXISTS", "EF_FRIEND_NOT_EXISTS");
//        textDict.Add("EF_ADD_FRIEND_FAIL", "EF_ADD_FRIEND_FAIL");
//        textDict.Add("EF_DEL_FRIEND_FAIL", "EF_DEL_FRIEND_FAIL");
//        textDict.Add("EF_IS_ALREADY_FRIEND", "EF_IS_ALREADY_FRIEND");
//        textDict.Add("EF_INVALID_FRIEND_STATE", "EF_INVALID_FRIEND_STATE");
//        
//        textDict.Add("EQ_QUEST_BASE", "EQ_QUEST_BASE");
//        textDict.Add("EQ_QUEST_ID_INVALID", "EQ_QUEST_ID_INVALID");
//        textDict.Add("EQ_GET_QUESTINFO_ERROR", "EQ_GET_QUESTINFO_ERROR");
//        textDict.Add("EQ_STAMINA_NOT_ENOUGH", "EQ_STAMINA_NOT_ENOUGH");
//        textDict.Add("EQ_GET_QUEST_CONFIG_ERROR", "EQ_GET_QUEST_CONFIG_ERROR");
//        textDict.Add("EQ_GET_QUEST_LOG_ERROR", "EQ_GET_QUEST_LOG_ERROR");
//        textDict.Add("EQ_UPDATE_QUEST_RECORD_ERROR", "EQ_UPDATE_QUEST_RECORD_ERROR");
//        textDict.Add("EQ_INVALID_DROP_UNIT", "EQ_INVALID_DROP_UNIT");
//        textDict.Add("EQ_QUEST_IS_PLAYING", "EQ_QUEST_IS_PLAYING");
//        textDict.Add("E_UNIT_BASE", "E_UNIT_BASE");
//        textDict.Add("E_UNIT_ID_ERROR", "E_UNIT_ID_ERROR");
//        textDict.Add("E_LEVELUP_NO_ENOUGH_MONEY", "E_LEVELUP_NO_ENOUGH_MONEY");
//        textDict.Add("E_GET_UNIT_INFO_ERROR", "E_GET_UNIT_INFO_ERROR");
//
//        textDict.Add("CONNECT_ERROR", "Connect network error. Please confirm your networking and try again.");
//        textDict.Add("SERVER_500", "Now server is busy. Please wait and try again.");


}