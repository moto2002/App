﻿using UnityEngine;
using System.Collections;
using System.Text;

public class GameDataStore {
	private static GameDataStore instance;
	public static GameDataStore Instance {
		get {
			if(instance == null) {
				instance = new GameDataStore();
			}
			return instance;
		}
	}

	public void StoreData(string key, object value) {
		string info = value.ToString ();
		info = AES.Encrypt (info);
		PlayerPrefs.SetString (key, info);
	}

	public string GetData(string key) {
		string info = string.Empty;
		if (PlayerPrefs.HasKey (key)) {
			info = PlayerPrefs.GetString (key);

			info = AES.Decrypt (info);

		} 
		return info;
	}

	public int GetInt(string key) {
		string data = GetData(key);
		if (data.Length == 0)
			return 0;
		return System.Convert.ToInt32(data);
	}

	public uint GetUInt(string key) {
		string data = GetData(key);
		if (data.Length == 0)
			return 0;
		return System.Convert.ToUInt32 (data);
	}

	/// <summary>
	/// data key list
	/// </summary>

	public const string USER_ID = "userid";
	public const string UUID = "uuid";
}