﻿using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TCityInfo : ProtobufDataBase {
	private CityInfo instance;
	private List<TStageInfo> stageInfo;
	public TCityInfo (CityInfo ci) : base (ci) {
		instance = ci;

		InitStageInfo (ci);
	}

	void InitStageInfo (CityInfo ci) {
		stageInfo = new List<TStageInfo> ();
		for (int i = 0; i < ci.stages.Count; i++) {
			TStageInfo tsi = new TStageInfo(instance.stages[i]);
			stageInfo.Add(tsi);
		}
	}

	public CityInfo cityInfo {
		get { return instance; }
	}

	public uint ID {
		get {
			return instance.id;
		}
	}

	public int State {
		get {
			return instance.state;
		}
	}

	public string CityName {
		get {
			return instance.cityName;
		}
	}

	public string Description {
		get {
			return instance.description;
		}
	}

	public Position Position {
		get {
			return instance.pos;
		}
	}

	public int PositionX {
		get {
			return Position.x;
		}
	}

	public int PositionY {
		get {
			return Position.y;
		}
	}

	public List<TStageInfo> Stages {
		get {
			return stageInfo;
		}
	}
}