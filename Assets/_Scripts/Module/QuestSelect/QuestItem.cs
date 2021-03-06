using UnityEngine;
using System.Collections;
using bbproto;

public class QuestItem : MonoBehaviour {
	private UISprite bossAvatarSpr;
	private UISprite bossTypeSpr;
	private UISprite erotemeSpr;
	private UISprite maskSpr;
	private UISprite focusSpr;
	private UILabel clearFlagLabel;
	private UILabel questPosLabel;

	public static QuestItem Inject(GameObject prefab){
		QuestItem questItem = prefab.GetComponent<QuestItem>();
		if(questItem == null) questItem = prefab.AddComponent<QuestItem>();
		return questItem;
	}

	private void Awake(){
		bossAvatarSpr = transform.FindChild("Sprite_Avatar").GetComponent<UISprite>();
		bossTypeSpr = transform.FindChild("Sprite_Type").GetComponent<UISprite>();
		erotemeSpr = transform.FindChild("Sprite_Eroteme").GetComponent<UISprite>();
		maskSpr = transform.FindChild("Sprite_Mask").GetComponent<UISprite>();
		focusSpr = transform.FindChild("Sprite_Focus").GetComponent<UISprite>();
		clearFlagLabel = transform.FindChild("Label_Clear").GetComponent<UILabel>();
		questPosLabel = transform.FindChild("Label_Pos").GetComponent<UILabel>();
	}

	private StageInfo stageInfo;
	public StageInfo StageInfo{
		get{
			return stageInfo;
		}
		set{
			stageInfo = value;
		}
	}

	private QuestInfo questInfo;
	public QuestInfo QuestInfo{
		get{
			return questInfo;
		}
		set{
			questInfo = value;
			//binding data with view
			if(questInfo == null || stageInfo == null){
				//do some work about clear
			}
			else{
				//do some view show by QuestClear state
				uint bossID = questInfo.bossId[ 0 ];
				ResourceManager.Instance.GetAvatarAtlas(bossID, bossAvatarSpr);

				EUnitType unitType = DataCenter.Instance.UnitData.GetUnitInfo(bossID).type;
				bossTypeSpr.color = DGTools.TypeToColor(unitType);

			}
		}
	}

	private bool isClear;
	public bool IsClear{
		get{
			return isClear;
		}
		set{
			isClear = value;
			if(isClear){
				clearFlagLabel.text = "Clear";
				clearFlagLabel.color = Color.yellow;
				erotemeSpr.enabled = false;
				maskSpr.enabled = false;
				//UIEventListenerCustom.Get(gameObject).onClick = null;
			}
			else{
				clearFlagLabel.text = "New";
				clearFlagLabel.color = Color.green;
				erotemeSpr.enabled = true;
				maskSpr.enabled = true;
				//UIEventListenerCustom.Get(gameObject).onClick = ClickItem;
			}
			UIEventListenerCustom.Get(gameObject).onClick = ClickItem;
		}
	}

	private bool isFocus;
	public bool IsFocus{
		get{
			return isFocus;
		}
		set{
			isFocus = value;
			if(isFocus){
				focusSpr.enabled = true;
			}
			else{
				focusSpr.enabled = false;
			}
		}
	}

	private int position;
	public int Position{
		get{
			return position;
		}
		set{
			position = value;
			if(questInfo == null) {
				questPosLabel.text = string.Empty;
			}
			questPosLabel.text = string.Format("Quest : " + position);
		}
	}

	private  static GameObject itemPrefab;
	public static GameObject ItemPrefab{
		get{
			if(itemPrefab == null) {
				string sourcePath = "Prefabs/UI/Quest/QuestPrefab";
				itemPrefab = ResourceManager.Instance.LoadLocalAsset(sourcePath, null) as GameObject ;
			}
			return itemPrefab;
		}
	}

	public delegate void QuestItemCallback(QuestItem questItem);
	public QuestItemCallback callback;
	private void ClickItem(GameObject item){
		if(callback != null) callback(this);
	}
	
}
