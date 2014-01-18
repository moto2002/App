﻿

public enum SceneEnum
{
	None,

	Start,

	Quest,

	Friends,

	Scratch,

	Shop,

	Others,

	Units,

	QuestSelect,

	FriendSelect,

	Party,

	LevelUp,

	Evolve,

	Sell,

	UnitList,

	UnitCatalog,

	Fight,
}

public enum CommandEnum
{
	None = 0,

	ChangeScene = 1,
	#region view to model
	DragCardToBattleArea 	= 1000,
	InquiryBattleBaseData 	= 1001,
	MoveToMapItem 			= 1002,
	StartAttack				= 1003,
	#endregion

	#region model to view
	Person 					= 3000,
	BattleBaseData 			= 3001,
	UnitBlood 				= 3002,
	EnergyPoint				= 3003,
	ShowEnemy 				= 3004,
	AttackEnemy				= 3005,
	AttackPlayer			= 3006,
	AttackRecoverRange		= 3007,
	BattleEnd				= 3008,
	#endregion
}

public enum UIParentEnum : byte {
	/// <summary>
	/// screen center anchor
	/// </summary>
	Center = 0,

	/// <summary>
	/// screen top anchor
	/// </summary>
	Top = 1,

	/// <summary>
	/// screen bottom anchor
	/// </summary>
	Bottom = 2,

	/// <summary>
	/// no UIPanel component
	/// </summary>
	BottomNoPanel = 3,

	/// <summary>
	/// The none.
	/// </summary>
	None = 254,
}

public enum UIState
{
	UIInit,
	UICreat,
	UIShow,
	UIHide,
	UIDestory,
}

public enum ResourceEuum : byte
{
	Prefab = 0,
	Image = 1,
}


public enum CardPoolEnum
{
	ActorCard,

	FightCard,
}

public enum CardColorType : byte {

	fire = 0,

	water = 1,
	
	wind = 2,

	light = 3,

	dark = 4,

	nothing = 5,

	heart = 6,
}
