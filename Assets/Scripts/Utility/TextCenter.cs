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

public partial class TextCenter {

    public static TextCenter Instace {
        get {
            if (instance == null){
                instance = new TextCenter();
                instance.Init();
                instance.InitSecond();
                instance.InitThird();
            }
            return instance;
        }
    }

    public string GetCurrentText(string key){
        string result = "";
        textDict.TryGetValue(key, out result);
        return result;
    }

    public string GetCurrentText(string key, params object[] args){
        string result = "";
        textDict.TryGetValue(key, out result);
        result = string.Format(result, args);
        return result;
    }

    public void Test(){
        LogHelper.Log("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT TextHelper.Test() start");
        LogHelper.Log("test get string {0}, result {1}", "error", GetCurrentText("error"));
        LogHelper.Log("test get string {0}, result {1}", "error1", GetCurrentText("error1", "test error1"));
    }

    private static TextCenter instance;

    private Dictionary<string, string> textDict;

    private void Init(){
        textDict = new Dictionary<string, string>();

        //

        textDict.Add("OK", "OK");
        textDict.Add("Cancel", "Cancel");
        textDict.Add("Back", "Back");
        textDict.Add("Open", "Open");

        textDict.Add("SearchError", "Search Error");
        textDict.Add("UserNotExist", "The Friend {0} you search not exist");

        textDict.Add("InputError", "Input Error");
        textDict.Add("InputEmpty", "Could not Input empty ID!");

//        textDict.Add("SearchError", "Input Error");
        textDict.Add("UserAlreadyFriend", "The ID {0} you searched is already your friend!");

        textDict.Add("RefuseAll", "The ID {0} you searched is already your friend!");
        textDict.Add("ConfirmRefuseAll", "Are you sure to refuse all friend apply?");

        textDict.Add("RefreshFriend", "Friend Update");
        textDict.Add("ConfirmRefreshFriend", "Are you sure to update friend list?");

        //////// shop

        textDict.Add("FriendExpand", "Friend Max Expansion");
        textDict.Add("ConfirmFriendExpansion", "Are you sure to use 1 coin to expand your friend limit?");
        textDict.Add("FriendExpansionInfo", "Now friends {0}/{1}");
        textDict.Add("DoFriendExpand", "Expand");

        textDict.Add("FriendExpansionFailed", "Friend Max Expansion Failed");
        textDict.Add("FriendCountLimitReachedMax", "Now your friend limit has reached max.");
        textDict.Add("FriendExpandStonesNotEnough", "Now your stone not enough to expand friend limit.");
        
        textDict.Add("FriendExpansionFinish", "Friend Max Expansion Completed");
        textDict.Add("FriendExpansionResult", "Friend Limited Max reached {0}!");

        textDict.Add("StaminaRecover", "Recover your stamina");
        textDict.Add("ConfirmStaminaRecover", "Are you sure to use 1 coin to make your stamina full?");
        textDict.Add("DoStaminaRecover", "Recover");

        textDict.Add("StaminaRecoverFailed", "Stamina Recovering Failed");
        textDict.Add("StaminaStillFull", "Now your stamina still full, not need to recover it.");
        textDict.Add("StaminaRecoverStonesNotEnough", "Now your stone not enough to recover stamina.");

        textDict.Add("StaminaRecoverFinish", "Stamina Recover Completed");
        textDict.Add("StaminaRecoverResult", "All Stamina Recovered!");

        textDict.Add("UnitExpand", "Units count limit expand");
        textDict.Add("ConfirmUnitExpansion", "Are you sure to use 1 coin to expand your unit limit?");
        textDict.Add("UnitExpansionInfo", "Now Units {0}/{1}");
        textDict.Add("DoUnitExpansion", "Expand");

        textDict.Add("UnitExpansionFailed", "Unit Max Expansion Failed");
        textDict.Add("UnitCountLimitReachedMax", "Now your unit limit has reached max.");
        textDict.Add("UnitExpandStonesNotEnough", "Now your stone not enough to expand unit limit.");

        textDict.Add("UnitExpansionFinish", "Unit Max Expansion Completed");
        textDict.Add("UnitExpansionResult", "Unit Limited Max Reached {0}!");
        
        /////// gacha
        textDict.Add("FriendGacha", "Start Friend Point Scratch");
        textDict.Add("FriendGachaTitle", "Friend Point Scratch");
        textDict.Add("FriendGachaDescription", "Use friend Scratch to get units");
        textDict.Add("FriendGachaStatus", "1 gacha: {0} points  {1} gachas: {2} points\nnow friend points {3}");

        textDict.Add("ConfirmOneFriendGacha", "Gacha 1 time");
        textDict.Add("ConfirmMaxFriendGacha", "Gacha {0} times");

        textDict.Add("FriendGachaFailed", "Friend Gacha Failed");
        textDict.Add("UnitCountReachedMax", "Now your units owned overed unit limit:{0}/{1}, can't do any gacha.");
        textDict.Add("GachaFriendPointNotEnough", "1 friend gacha need {0} friend points\nYour friend points not enough.");

        textDict.Add("RareGacha", "Start Rare Scratch");
        textDict.Add("RareGachaTitle", "Rare Scratch");
        textDict.Add("RareGachaDescription", "Use stones to get units");
        textDict.Add("RareGachaStatus", "1 gacha: {0} stones {1} gachas: {2} stones\nnow stones {3}");

        textDict.Add("ConfirmOneRareGacha", "Gacha 1 time");
        textDict.Add("ConfirmMaxRareGacha", "Gacha {0} times");

        textDict.Add("RareGachaFailed", "Rare Gacha Failed");
        textDict.Add("RareGachaStoneNotEnough", "1 rare gacha need {0} stones\nYour stones not enough.");

        textDict.Add("EventGacha", "Start Event Scratch");
        textDict.Add("EventGachaTitle", "Event Scratch");
        textDict.Add("EventGachaDescription", "Use stones to get units");
        textDict.Add("EventGachaStatus", "1 gacha: {0} stones {1} gachas: {2} stones\nnow stones {3}");

        textDict.Add("ConfirmOneEventGacha", "Gacha 1 time");
        textDict.Add("ConfirmMaxEventGacha", "Gacha {0} times");

        textDict.Add("EventGachaFailed", "Event Gacha Failed");
        textDict.Add("EventGachaNotOpen", "Our next event gacha will coming soon, please wait it!");
        textDict.Add("EventGachaStoneNotEnough", "1 event gacha need {0} stones\nYour stones not enough.");

        textDict.Add("FriendScratch", "Friend Scratch");
        textDict.Add("RareScratch", "Rare Scratch");
        textDict.Add("EventScratch", "Event Scratch");

        textDict.Add("GachaChances", "chances {0}/{1}");
        textDict.Add("Lv", "Lv {0}");

        ///
	    textDict.Add ("StartQuestFail", "Start quest request failed");

    }
}