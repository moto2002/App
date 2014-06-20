﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour{

	public const string RelyOnSource = "RelyOnResource";

	public const string ResourceInit = "Resource_Init";

	public static Dictionary<ResourceAssetBundle,AssetBundleObj> assetBundles = new Dictionary<ResourceAssetBundle,AssetBundleObj> ();

	private static ResourceManager instance;

	public static ResourceManager Instance
	{
		get {
			if(instance == null)
				instance = FindObjectOfType<ResourceManager>();
			return instance;
		}
	}



	public void Init(DataListener callback){
		int num = 1;
		assetBundles[ResourceAssetBundle.PROTOBUF] = new AssetBundleObj(ResourceAssetBundle.PROTOBUF,ResourceManager.ResourceInit,o=>{num--; if(num <= 0)callback(null);},GetBundleTypeByKey(ResourceAssetBundle.PROTOBUF));
		StartCoroutine(DownloadResource(ResourceAssetBundle.PROTOBUF));
	}

	private Dictionary<string,object> objectDic = new Dictionary<string, object>();

	public Object LoadLocalAsset( string path, ResourceCallback callback ) {
		//the following resource will not be dynamiclly download.
		if (string.IsNullOrEmpty (path)) {
			return null;	
		}

		if (path.IndexOf ("Loading") >= 0 || path.IndexOf ("UIInsConfig") >= 0 || path.IndexOf ("ScreenMask") >= 0 || path.IndexOf ("CommonNoteWindow") >= 0) {
			Debug.Log("path: " + path);
			if (callback != null){
				callback (Resources.Load (path));
				return null;
			}else{
				return Resources.Load (path);
			}
		}

		if (path.IndexOf ("Config") == 0 || path.IndexOf ("Language") == 0 || path.IndexOf ("Protobuf") == 0 || path.IndexOf ("Avatar") == 0 || path.IndexOf ("Profile") == 0 || path.IndexOf ("Atlas") == 0) {
#if UNITY_EDITOR

//			ResourceAssetBundle key = GetBundleKeyByPath(path);
//			
//			if(!assetBundles.ContainsKey(key)){
//				assetBundles[key] = new AssetBundleObj(key,path,callback,GetBundleTypeByKey(key));
//				StartCoroutine(DownloadResource(key));
//			}else{
//				if(assetBundles[key].isLoading){
//					Debug.Log("======path: " + path);
//					if(!assetBundles[key].callbackList.ContainsKey(path)){
//						assetBundles[key].callbackList.Add(path,callback);
//					}
//				}else{
//					if(callback != null){
//						Debug.Log("resource load: " + path + " key: " + assetBundles[key].assetBundle);
//						callback(assetBundles[key].assetBundle.Load(path.Substring(path.LastIndexOf('/')+1), GetBundleTypeByKey(key)));
//						return null;
//					}else{
//						return assetBundles[key].assetBundle.Load(path.Substring(path.LastIndexOf('/')+1),  GetBundleTypeByKey(key));
//					}
//				}
//				
//			}
//			return null;
			
			
			string ext = null;
			if(path.IndexOf ("Prefabs") == 0){
				ext = ".prefab";

//				return uiAssets.Load(path);
			}else if(path.IndexOf ("Atlas") == 0){
				ext = ".prefab";
			}else if(path.IndexOf ("Language") == 0){
				ext = ".txt";
			}else if(path.IndexOf ("Protobuf") == 0){
				if(path.IndexOf ("skills") >= 0){
					ext = ".json";
				}else{
					ext = ".bytes";
				}
			}else if(path.IndexOf ("Avatar") == 0){
				ext = ".png";
			}else if(path.IndexOf ("Profile") == 0){
				ext = ".png";

				int num = 0;
				int.TryParse(path.Substring(path.LastIndexOf('/')),out num);
//				(int)(num/20)
			}

//			Debug.Log ("assets load: " + "Assets/ResourceDownload/" + path + ext + "  "  + Resources.LoadAssetAtPath <Object>("Assets/ResourceDownload/" + path+ ext));
			if(callback != null){
				callback(Resources.LoadAssetAtPath<Object> ("Assets/ResourceDownload/" + path + ext));
				return null;
			}else{
				return Resources.LoadAssetAtPath<Object> ("Assets/ResourceDownload/" + path + ext);
			}
#else 
//			if(path.IndexOf ("Config") == 0){
//
//			}else if(path.IndexOf ("Prefabs") == 0){
//				ext = ".prefab";
//			}else if(path.IndexOf ("Language") == 0){
//				ext = ".txt";
//			}else if(path.IndexOf ("Protobuf") == 0){
//				if(path.IndexOf ("skills") >= 0){
//					ext = ".json";
//				}else{
//					ext = ".bytes";
//				}
//			}else if(path.IndexOf ("Avatar") == 0 || path.IndexOf ("Profile") == 0){
//				ext = ".png";
//			}
			Debug.Log ("resource load no editor");
			if(callback != null){
				callback(Resources.Load (path));
				return null;
			}else{
				return Resources.Load (path);
			}
			return null;

#endif
		} else {
			Debug.Log ("resource load from resource: " + path);
			if(callback != null){
				callback(Resources.Load (path));
				return null;
			}else{
				return Resources.Load (path);
			}
			return null;
		}
		return null;

	}

	IEnumerator DownloadResource(ResourceAssetBundle key){

		string url = 
//	#if UNITY_IPHONE
		"file://" + Application.dataPath + "/ResourceDownload/" + GetBundleUrlByKey(key);
//	#elif UNITY_ANDROID
//		"file://" + "jar:file://" + Application.dataPath + "/ResourceDownload/" + GetBundleUrlByKey(key);
//	#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
//		"file://" + Application.dataPath + "/ResourceDownload/" + GetBundleUrlByKey(key);
//	#else
//		string.Empty;
//	#endif
		assetBundles [key].isLoading = true;
		Debug.Log ("download start url: " + url);

		WWW www = new WWW (url);
		if (!string.IsNullOrEmpty (www.error)) {
			Debug.Log(www.error);		
		}
		yield return www;
		Debug.Log ("download complete url: " + url);
		assetBundles [key].assetBundle = www.assetBundle;
		assetBundles [key].isLoading = false;

		if(checkRelies(assetBundles [key])){
			assetBundles [key].ExeCallback ();
		}

	}

	private ResourceAssetBundle GetBundleKeyByPath(string path){
//		Debug.Log ("download async: " + path);

		if(path.IndexOf ("Prefabs") == 0){

			return ResourceAssetBundle.UI;

		}else if(path.IndexOf ("Atlas") == 0){

		}else if(path.IndexOf ("Language") == 0){
			return ResourceAssetBundle.LANGUAGE;
		}else if(path.IndexOf ("Protobuf") == 0){
//			if(path.IndexOf ("skills") >= 0){
//
//			}else{
//
//			}
			return ResourceAssetBundle.PROTOBUF;
		}else if(path.IndexOf ("Avatar") == 0){
			
		}else if(path.IndexOf ("Profile") == 0){
			int num = 0;
			int.TryParse(path.Substring(path.LastIndexOf('/')),out num);
			switch((int)(num/20)){
				case 0:
					return ResourceAssetBundle.PROFILE_0;
				case 1:
					return ResourceAssetBundle.PROFILE_1;
				case 2:
					return ResourceAssetBundle.PROFILE_2;
				case 3:
					return ResourceAssetBundle.PROFILE_3;
				case 4:
					return ResourceAssetBundle.PROFILE_4;
				case 5:
					return ResourceAssetBundle.PROFILE_5;
				case 6:
					return ResourceAssetBundle.PROFILE_6;
				case 7:
					return ResourceAssetBundle.PROFILE_7;
				case 8:
					return ResourceAssetBundle.PROFILE_8;
				case 9:
					return ResourceAssetBundle.PROFILE_9;
				case 10:
					return ResourceAssetBundle.PROFILE_10;
				default:
					return ResourceAssetBundle.NONE;
			}
		}
		if (path == RelyOnSource) {
			return ResourceAssetBundle.UI;
		}
		return ResourceAssetBundle.NONE;
	}

	private string GetBundleUrlByKey(ResourceAssetBundle key){
		switch (key) {
			case ResourceAssetBundle.UI:
				return "Output/AllUI.unity3d";
			case ResourceAssetBundle.UI_ATLAS:
				return "Output/UI_Atlas.unity3d";
			case ResourceAssetBundle.LANGUAGE:
				return "Output/Language_en.unity3d";
			case ResourceAssetBundle.PROTOBUF:
				return "Output/Protobuf.unity3d";
			case ResourceAssetBundle.PROFILE_0:
				return "Output/Profile_0.unity3d";
			case ResourceAssetBundle.PROFILE_1:
				return "Output/Profile_1.unity3d";
			case ResourceAssetBundle.PROFILE_2:
				return "Output/Profile_2.unity3d";
			case ResourceAssetBundle.PROFILE_3:
				return "Output/Profile_3.unity3d";
			case ResourceAssetBundle.PROFILE_4:
				return "Output/Profile_4.unity3d";
			case ResourceAssetBundle.PROFILE_5:
				return "Output/Profile_5.unity3d";
			case ResourceAssetBundle.PROFILE_6:
				return "Output/Profile_6.unity3d";
			case ResourceAssetBundle.PROFILE_7:
				return "Output/Profile_7.unity3d";
			case ResourceAssetBundle.PROFILE_8:
				return "Output/Profile_8.unity3d";
			case ResourceAssetBundle.PROFILE_9:
				return "Output/Profile_9.unity3d";
			case ResourceAssetBundle.PROFILE_10:
				return "Output/Profile_10.unity3d";
			case ResourceAssetBundle.AVATAR:
				break;
			default:
				break;
		}
		return null;
	}

	private System.Type GetBundleTypeByKey(ResourceAssetBundle key){
		switch (key) {
			case ResourceAssetBundle.UI:
				return typeof(GameObject);
			case ResourceAssetBundle.UI_ATLAS:
				return typeof(GameObject);
			case ResourceAssetBundle.LANGUAGE:
				return typeof(TextAsset);
			case ResourceAssetBundle.PROTOBUF:
			case ResourceAssetBundle.PROFILE_0:
			case ResourceAssetBundle.PROFILE_1:
			case ResourceAssetBundle.PROFILE_2:
			case ResourceAssetBundle.PROFILE_3:
			case ResourceAssetBundle.PROFILE_4:
			case ResourceAssetBundle.PROFILE_5:
			case ResourceAssetBundle.PROFILE_6:
			case ResourceAssetBundle.PROFILE_7:
			case ResourceAssetBundle.PROFILE_8:
			case ResourceAssetBundle.PROFILE_9:
			case ResourceAssetBundle.PROFILE_10:
				return typeof(Object);
			case ResourceAssetBundle.AVATAR:
				break;
			default:
				break;
		}
		return typeof(Object);
	}
	
	Object LoadLocalFromCache(string assetName,string path) {
		object obj = null;
		if (objectDic.TryGetValue (assetName, out obj)) {
			return (Object)obj;
		} 
		else {
			if (path.IndexOf ("Config") == 0 || path.IndexOf ("Prefabs") == 0) {
				#if UNITY_EDITOR
				Debug.Log ("resource load no cache");
				Debug.Log ("assets load: " + "Assets/Resources/" + path + Resources.LoadAssetAtPath <Object>("Assets/ResourceDownload/" + path));
				obj = Resources.LoadAssetAtPath("Assets/ResourceDownload/" + path + assetName,typeof(GameObject));
				#else 
				Debug.Log ("resource load no editor no cache");
				obj = Resources.Load(path + assetName);
				#endif
			} else {
				obj = Resources.Load(path + assetName);
			}

			objectDic.Add(assetName,obj);
			return (Object)obj;
		}
	}

	private bool checkRelies(AssetBundleObj aObj){
		bool allComplete = true;
		foreach (var item in aObj.relies) {
			if(!ResourceManager.assetBundles.ContainsKey(item)){
				allComplete = false;
				assetBundles[item] = new AssetBundleObj(item,RelyOnSource,o=>{
					if(checkRelies(aObj)){
						Debug.Log("rely resource complete");
						aObj.ExeCallback();
					}
				});
				Debug.Log(aObj.name + " rely on: " + item);
				StartCoroutine(DownloadResource(item));
			}else if(ResourceManager.assetBundles[item].isLoading){
				allComplete = false;
			}

		}
		return allComplete;
	}
}

public enum ResourceAssetBundle{
	NONE,
	UI,
	UI_ATLAS,
	AVATAR,
	LANGUAGE,
	PROTOBUF,
	PROFILE_0,
	PROFILE_1,
	PROFILE_2,
	PROFILE_3,
	PROFILE_4,
	PROFILE_5,
	PROFILE_6,
	PROFILE_7,
	PROFILE_8,
	PROFILE_9,
	PROFILE_10

}

public class AssetBundleObj{

	//the assetbundle pointer
	public AssetBundle assetBundle;

	//whether the resource is loading
	public bool isLoading;

	//all the callback attached to the resource(first param is path,seceond is callback).
	public Dictionary<string, ResourceCallback> callbackList ;

	//the resource relies on the other assetbundle.
	public List<ResourceAssetBundle> relies;

	//the resource name
	public ResourceAssetBundle name;

	public System.Type type;

	public AssetBundleObj(ResourceAssetBundle rName,string path = null,ResourceCallback callback = null,System.Type rType = null){
		callbackList = new Dictionary<string,ResourceCallback >();
		if (path != null) {
			callbackList.Add (path,callback);
			Debug.Log("======path: " + path);
		}
			
		name = rName;
		if (rType == null) {
			type = typeof(GameObject);
		} else {
			type = rType;
		}
		relies = GetResourceRelyResource(name);
	}

	public void ExeCallback(){
		foreach (var item in callbackList) {
			Debug.Log("asset bundle: " + item.Key.Substring(item.Key.LastIndexOf('/')+1));
			if(item.Key == ResourceManager.RelyOnSource || item.Key == ResourceManager.ResourceInit){
				item.Value(null);
			}else{
				if(item.Value == null){
					Debug.Log("no callback: " + item.Key);
				}else{
					item.Value(assetBundle.Load(item.Key.Substring(item.Key.LastIndexOf('/')+1),type));
				}

			}
		}
		callbackList.Clear ();
	}


	public static List<ResourceAssetBundle> GetResourceRelyResource(ResourceAssetBundle resource){
		List<ResourceAssetBundle> relies = new List<ResourceAssetBundle> ();
		if (resource == ResourceAssetBundle.UI) {
			relies.Add(ResourceAssetBundle.UI_ATLAS);	
		}

		return relies;
	}

}
