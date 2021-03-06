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

}