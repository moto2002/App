using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using bbproto;

/// <summary>
/// main will always exist until the game close
/// </summary>

public class Main : MonoBehaviour {
    public GameObject uiRoot;
    private static Main mainScrpit;

    public static Main Instance {
        get {
            if (mainScrpit == null)
                mainScrpit = (Main)FindObjectOfType(typeof(Main));

            return mainScrpit;
        }
    }

	[HideInInspector]
	public bool[] DebugEnable;

	private Camera bottomCamera;

	[HideInInspector]
	public Camera effectCamera;

    private GameInput gInput;

    public GameInput GInput {
        get{ 
			return gInput;
		}
    }

    private GameTimer gTimer;

    private static byte globalDataSeed = 0;

    public static byte GlobalDataSeed {
        get {
            return globalDataSeed;
        }
    }

    private UICamera nguiCamera;
    public UICamera NguiCamera {
        get {
            if (nguiCamera == null) {
                nguiCamera = Camera.main.GetComponent<UICamera>();
            }
            return nguiCamera;
        }
    }

	public MessageAdapt messageAdapt { get; private set; }

	[HideInInspector]
	public UIRoot root;

    void Awake() {
		mainScrpit = this;
		DontDestroyOnLoad(gameObject);
        TrapInjuredInfo tii = TrapInjuredInfo.Instance;
        globalDataSeed = (byte)Random.Range(0, 255);
		root = uiRoot.GetComponent<UIRoot> ();
        gInput = gameObject.AddComponent<GameInput>();
        gTimer = gameObject.AddComponent<GameTimer>();
		messageAdapt = gameObject.AddComponent<MessageAdapt> ();

        // init manager class
        ViewManager.Instance.Init(uiRoot);

#if !UNITY_EDITOR
		Application.RegisterLogCallback(HandleException);
#endif
//		
    }

	void HandleException(string condition, string stackTrace, LogType type){
		if (type == LogType.Error || type == LogType.Exception || type==LogType.Assert ) {
#if INNER_TEST
			UserController.Instance.SendLog(condition,stackTrace);
			TipsManager.Instance.ShowMsgWindow("Application Error-"+condition,stackTrace,TextCenter.GetText("OK"));
#else
			UserController.Instance.SendLog(condition,stackTrace);
#endif
		}
	}

    /// <summary>
    /// start game
    /// </summary>
    void OnEnable() {
		DebugEnable = new bool[]{true,true,true,true,true,true};
		SetResolution();

		DataCenter.Instance.Init ();
		ModuleManager.Instance.ShowModule(ModuleEnum.LoadingModule);

		ModuleManager.Instance.GetOrCreateModule (ModuleEnum.MsgWindowModule);
		ModuleManager.Instance.GetOrCreateModule (ModuleEnum.MaskModule);

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

	public const float DefaultSize = 1.5f;
	public const int DefaultHeight = 960;

	void SetResolution() {
		float currentSize = Screen.height / (float)Screen.width;
		UIPanel rootPanel = uiRoot.GetComponent<UIPanel>();

		if (currentSize >= DefaultSize) {
			float sizePropotion = currentSize / DefaultSize;
			int height = System.Convert.ToInt32( DefaultHeight * sizePropotion );
			root.manualHeight = height;
			rootPanel.clipRange = new Vector4(0, 0, height / currentSize, height);
		} else {
			root.manualHeight = DefaultHeight;
			rootPanel.clipRange = new Vector4(0, 0, 640, root.manualHeight);
		}
	}

//	public UILabel test_screen_width;
//	public UILabel test_screen_height;
//	public UILabel test_screen_manual_height;
//	void TestScreenAdaption(int w, int h, int mh){
//		test_screen_width.text = "Screen_Width : " + w;
//		test_screen_height.text = "Screen_Height : " + h;
//		test_screen_manual_height.text = "Manual_Height : " + mh;
//	}
}
