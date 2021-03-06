﻿using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TrapInjuredInfo {
	private static TrapInjuredInfo instance;
	public static TrapInjuredInfo  Instance{
		get{
			if(instance == null){
				instance = new TrapInjuredInfo();
			}
			return instance;
		}
	}

	//trap type
	public const int mineInfo		= 1;
	public const int trappingInfo	= 2;
	public const int hungryInfo		= 3;
	public const int lostMoney		= 4;
	public const int environment	= 5;
	public const int stateException	= 6;

	public const int movePrev	= 7;
	public const int moveRandom	= 8;
	public const int moveStart	= 9;

	
	public const int MAX_TRAP_LV	= 10;

	/// <summary>
	/// 1: mine. 2: trapping. 3: hungryinfo. 4: lostmoney. 5: environment. 6: state excpeiton.
	/// </summary>
	private Dictionary<int, List<TrapInjuredValue>> TrapInjured = new Dictionary<int, List<TrapInjuredValue>> ();
	private TrapInjuredInfo () {
		//-----------------------------move---------------------------------
		List<TrapInjuredValue> temp = new List<TrapInjuredValue> ();
		TrapInjuredValue tjvm = new TrapInjuredValue();
		tjvm.trapIndex = 0;
		tjvm.trapLevel = 1;
		temp.Add( tjvm );
		TrapInjured.Add (movePrev, temp);
		
		temp = new List<TrapInjuredValue> ();
		tjvm = new TrapInjuredValue();
		tjvm.trapIndex = 0;
		tjvm.trapLevel = 3;
		temp.Add( tjvm );
		TrapInjured.Add (moveRandom, temp);
		
		temp = new List<TrapInjuredValue> ();
		tjvm = new TrapInjuredValue();
		tjvm.trapIndex = 0;
		tjvm.trapLevel = 4;
		temp.Add( tjvm );
		TrapInjured.Add (moveStart, temp);

	//-----------------------------mine------------------------------------
		temp = new List<TrapInjuredValue> ();
		for (int i = 1; i <= MAX_TRAP_LV; i++) {
			TrapInjuredValue tjv = new TrapInjuredValue();
			tjv.trapIndex = i;
			tjv.trapLevel = Mathf.CeilToInt((float)i / 2f);
			tjv.trapValue = 50 * Mathf.Pow(2, i - 1);
			temp.Add(tjv);
		}
		TrapInjured.Add (mineInfo, temp);

	//-----------------------------trapping---------------------------------
		temp = new List<TrapInjuredValue> ();
		for (int i = 1; i <= MAX_TRAP_LV; i++) {
			TrapInjuredValue tjv = new TrapInjuredValue();
			tjv.trapIndex = i;
			tjv.trapLevel = Mathf.CeilToInt((float)i / 2f);
			tjv.trapValue = 25 * Mathf.Pow(2, i - 1);
			temp.Add(tjv);
		}
		TrapInjured.Add (trappingInfo, temp);
	
	//-----------------------------hungryInfo-------------------------------
		temp = new List<TrapInjuredValue> ();
		for (int i = 1; i <= MAX_TRAP_LV; i++) {
			TrapInjuredValue tjv = new TrapInjuredValue();
			tjv.trapIndex = i;
			tjv.trapLevel = i < 5 ? i : 5;
			tjv.trapValue = i;
			temp.Add(tjv);
		}
		TrapInjured.Add (hungryInfo, temp);

	//-----------------------------lostMoney---------------------------------
		temp = new List<TrapInjuredValue> ();
		for (int i = 1; i <= MAX_TRAP_LV; i++) {
			TrapInjuredValue tjv = new TrapInjuredValue();
			tjv.trapIndex = i;
			tjv.trapLevel = i < 5 ? i : 5;
			tjv.trapValue = 0.05f * i;
			temp.Add(tjv);
		}
		TrapInjured.Add (lostMoney, temp);

	//-----------------------------environment-------------------------------
		temp = new List<TrapInjuredValue> ();
		for (int i = 1; i <= MAX_TRAP_LV; i++) {
			TrapInjuredValue tjv = new TrapInjuredValue();
			tjv.trapIndex = i;
			tjv.trapLevel = i ;
			tjv.trapValue = i + 1;
			temp.Add(tjv);
		}
		TrapInjured.Add (environment, temp);

	//-----------------------------stateException----------------------------
		temp = new List<TrapInjuredValue> ();
		for (int i = 1; i <= MAX_TRAP_LV; i++) {
			TrapInjuredValue tjv = new TrapInjuredValue();
			tjv.trapIndex = i;
			tjv.trapLevel = i;
			if(i >= 4) {
				tjv.trapValue = 0.1f + 0.05f * (i - 4);
			}
			temp.Add(tjv);
		}
		temp [0].trapValue = 0.03f;
		temp [1].trapValue = 0.05f;
		temp [2].trapValue = 0.08f;
		TrapInjured.Add (stateException, temp);
	}

	TrapInjuredValue FindTrapInjured(int index,int valueIndex) {
		if (index == -1) {
			return null;	
		}
		TrapInjuredValue tiv = TrapInjured[index].Find (a => a.trapIndex == valueIndex);
		if (tiv == default(TrapInjuredValue)) {
			return null;
		} 
		else {
			return tiv;
		}
	}

	public TrapInjuredValue FindInfo(int type,int valueIndex) {
		return FindTrapInjured (type, valueIndex);
	}

	public TrapInjuredValue FindMineInfo (int valueIndex) {
		return FindTrapInjured (mineInfo, valueIndex);
	}

	public TrapInjuredValue FindTrappingInfo (int valueIndex){
		return FindTrapInjured (trappingInfo, valueIndex);
	}

	public TrapInjuredValue FindHungryInfo (int valueIndex){
		return FindTrapInjured (hungryInfo, valueIndex);
	}

	public TrapInjuredValue FindLostMoney (int valueIndex){
		return FindTrapInjured (lostMoney, valueIndex);
	}

	public TrapInjuredValue FindEnvironment (int valueIndex){
		return FindTrapInjured (environment, valueIndex);
	}

	public TrapInjuredValue FindState (int valueIndex){
		return FindTrapInjured (stateException, valueIndex);
	}
}
