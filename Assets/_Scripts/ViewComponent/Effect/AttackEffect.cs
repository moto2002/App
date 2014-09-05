﻿using UnityEngine;
using System.Collections.Generic;
using bbproto;
using System.Collections;

public class AttackEffect : MonoBehaviour {
	private GameObject effect;
	private Queue<AttackEffectItem> inactiveEffect = new Queue<AttackEffectItem>();
	private Queue<AttackEffectItem> attackEffectQueue = new Queue<AttackEffectItem>();

	private GameObject activeEffect;
	private UISprite avatarTexture;

	private Vector3 activeEnd = Vector3.zero;

	public const float activeSkillEffectTime = 1.5f;
	public const float activeSkillShowNameTime = 0.5f;

	void Awake() {
		effect = transform.Find ("AE").gameObject;
		effect.SetActive (false);
		activeEffect = transform.Find ("ActiveSkill").gameObject;
		avatarTexture = activeEffect.transform.Find ("Avatar").GetComponent<UISprite> ();
		activeEffect.SetActive (false);
	}

	public void RefreshItem (string userUnitID, int skillID, float atk = 0, bool recoverHP = false) {
		AttackEffectItem aei = GetAttackEffectItem ();
		int atkShow = (int)atk;
		aei.RefreshInfo(userUnitID, skillID, End, atkShow, recoverHP);
	}

	void End() {
		AttackEffectItem aei = attackEffectQueue.Dequeue ();
		if (inactiveEffect.Count < 3) {
			aei.gameObject.SetActive (false);
			inactiveEffect.Enqueue (aei);
		} else {
			Destroy(aei.gameObject);
		}
	}

	string skillName = "";

	public void PlayActiveSkill(AttackInfo ai) {
		activeEffect.SetActive (true);
		activeEffect.transform.localPosition = BattleCardArea.activeSkillStartPosition;
		TUserUnit tuu = DataCenter.Instance.UserUnitList.GetMyUnit(ai.UserUnitID);
		ResourceManager.Instance.GetAvatarAtlas (tuu.UnitInfo.ID, avatarTexture);
		SkillBaseInfo sbi = DataCenter.Instance.GetSkill (ai.UserUnitID, ai.SkillID, SkillType.ActiveSkill);
		skillName = sbi == null ? "" : TextCenter.GetText (SkillBaseInfo.SkillNamePrefix + sbi.skillBase.id);//sbi.SkillName;
		iTween.MoveTo (activeEffect, iTween.Hash ("position", BattleCardArea.startPosition, "time", activeSkillEffectTime - 0.5f, "oncompletetarget", gameObject, "oncomplete", "ActiveSkillEnd", "islocal", true,"easetype", iTween.EaseType.easeInOutQuad));  
//		Debug.LogError ("PlayActiveSkill MoveTo");
		AudioManager.Instance.PlayAudio (AudioEnum.sound_as_fly);
	}

	void ActiveSkillEnd() {
		AttackEffectItem aei = GetAttackEffectItem ();
//		Debug.LogError ("ActiveSkillEnd");
//		aei.ShowActiveSkill (avatarTexture, skillName, End);
//		avatarTexture.mainTexture = null;
		activeEffect.SetActive (false);
	}

	AttackEffectItem GetAttackEffectItem () {
		AttackEffectItem aei;
		if (inactiveEffect.Count == 0) {
			GameObject go = NGUITools.AddChild (gameObject, effect);
			aei = go.GetComponent<AttackEffectItem> ();
		} else {
			aei = inactiveEffect.Dequeue();
		}
		
		aei.gameObject.SetActive (true);
		attackEffectQueue.Enqueue(aei);

		return aei;
	}
}