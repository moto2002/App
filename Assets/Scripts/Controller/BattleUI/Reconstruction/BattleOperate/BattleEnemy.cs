using UnityEngine;
using System.Collections.Generic;

public class BattleEnemy : UIBaseUnity {
	private static Dictionary<uint, EnemyItem> monster = new Dictionary<uint, EnemyItem> ();
	public static Dictionary<uint, EnemyItem> Monster {
		get{
			return monster;
		}
	}
	private GameObject tempGameObject;
	[HideInInspector]
	public Battle battle;

	private UILabel attackInfoLabel;

	private string[] attackInfo = new string[4] {"Nice", "Good", "Great", "Excellent"};

	public override void Init (string name) {
		base.Init (name);
		tempGameObject = transform.Find ("EnemyItem").gameObject;
		tempGameObject.SetActive (false);
		transform.localPosition += new Vector3 (0f, battle.cardHeight * 5.5f, 0f);
		attackInfoLabel = FindChild<UILabel>("Label");
		attackInfoLabel.transform.localScale = new Vector3 (2f, 2f, 2f);
	}

	int count = 0;
	public override void HideUI () {
		base.HideUI ();
		Clear ();
		gameObject.SetActive (false);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, AttackEnemy);
		MsgCenter.Instance.RemoveListener (CommandEnum.DropItem, DropItem);
		count --;
		Debug.LogError ("battle enemy hideui " + count);
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
		count ++;
		Debug.LogError ("battle enemy ShowUI " + count);
		MsgCenter.Instance.AddListener (CommandEnum.DropItem, DropItem);
	}

	void AttackEnemyEnd(object data) {
		int index = DGTools.RandomToInt (0, 4);
		attackInfoLabel.text = attackInfo [index];
		iTween.ScaleTo (attackInfoLabel.gameObject, iTween.Hash ("scale", new Vector3 (1f, 1f, 1f), "time", 0.5f, "easetype", iTween.EaseType.easeInQuart, "oncomplete", "End", "oncompletetarget", gameObject));
	}

	void AttackEnemy(object data) {

	}

	void End() {
		attackInfoLabel.text = "";
		attackInfoLabel.transform.localScale = new Vector3 (2f, 2f, 2f);
	}

	public void Refresh(List<TEnemyInfo> enemy) {
		Clear();
		List<EnemyItem> temp = new List<EnemyItem> ();
		for (int i = 0; i < enemy.Count; i++) {
			GameObject go = NGUITools.AddChild(gameObject,tempGameObject);
			go.SetActive(true);

			EnemyItem ei = go.AddComponent<EnemyItem>();
			ei.Init(enemy[i]);
			temp.Add(ei);
			monster.Add(enemy[i].EnemySymbol,ei);
		}
		SortEnemyItem (temp);
	}

	void DropItem(object data) {
		int pos = (int)data;
		uint posSymbol = (uint)pos;

		if (monster.ContainsKey (posSymbol) && monster[posSymbol].enemyInfo.IsDead) {
			monster.Remove (posSymbol);	
		}
	}

	void Clear() {
		foreach (var item in monster) {
			if(item.Value != null) {
				item.Value.DestoryUI();
			}
		}
		monster.Clear();
	}
	float interv = 10f;
	void SortEnemyItem(List<EnemyItem> temp) {
		int count = temp.Count;
		if (count == 0) {	return;	}
		CompressTextureWidth (temp);
		if (count == 1) { 
			temp[0].transform.localPosition = Vector3.zero; 
			return; 
		}
		int centerIndex = 0;
		if (DGTools.IsOddNumber (count)) {
			centerIndex = ((count + 1) >> 1) - 1;		
			temp[centerIndex].transform.localPosition = Vector3.zero;
			DisposeCenterLeft(centerIndex, temp);
			DisposeCenterRight(centerIndex,temp);
		} else {
			centerIndex = (count >> 1) - 1;
			int centerRightIndex = centerIndex + 1;
			temp[centerIndex].transform.localPosition = new Vector3(0f - (temp[centerIndex].texture.width >> 2),0f,0f);
			temp[centerRightIndex].transform.localPosition = new Vector3(0f + (temp[centerRightIndex].texture.width >> 2),0f,0f);
			DisposeCenterLeft(centerIndex--, temp);
			centerRightIndex++;
			DisposeCenterRight(centerRightIndex , temp);
		}
	}

	void CompressTextureWidth (List<EnemyItem> temp) {
		int width = Screen.width;
		int allWidth = 0;
		for (int i = 0; i < temp.Count; i++) {
			allWidth += temp[i].texture.width;
		}
		float probability = (float)width / allWidth;
		if (probability * 2 < 1) {
			probability *= 2;
		}
//		Debug.LogError (" probability : " + probability + "  width :" + width + " allWidth : " + allWidth);
		if (probability < 1f) {
			interv *= probability;
			for (int i = 0; i < temp.Count; i++) {
				float tempWidth = temp [i].texture.width * probability;
				float tempHeight = temp [i].texture.height * probability;
				temp [i].texture.width = (int)tempWidth;
				temp [i].texture.height = (int)tempHeight;
			}	
		} else {
			interv = 10;
		}
	}

	void DisposeCenterLeft(int centerIndex,List<EnemyItem> temp) {
		int tempIndex = centerIndex - 1;
		while(tempIndex >= 0) {
			Vector3 localPosition = temp[tempIndex + 1].transform.localPosition;
			temp[tempIndex].transform.localPosition = new Vector3(localPosition.x - ((temp[tempIndex].texture.width >> 1) + interv) , 0f, 0f);
			tempIndex--;
		}
	}

	void DisposeCenterRight (int centerIndex, List<EnemyItem> temp) {
		int tempIndex = centerIndex;

		while(tempIndex < temp.Count) {
			Vector3 localPosition = temp[tempIndex - 1].transform.localPosition;
			temp[tempIndex].transform.localPosition = new Vector3(localPosition.x + ((temp[tempIndex].texture.width >> 1) + interv), 0f, 0f);
			tempIndex++;
		}
	}
}

public class ShowEnemyUtility {
	public uint enemyID;
	public int enemyBlood;
	public int attackRound;
}



