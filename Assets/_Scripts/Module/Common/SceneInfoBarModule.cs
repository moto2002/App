using UnityEngine;
using System.Collections;

public class SceneInfoBarModule : ModuleBase {
	
	public SceneInfoBarModule(UIConfigItem config):base(  config) {
		CreateUI<SceneInfoBarView> ();
    }
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void OnReceiveMessages (params object[] data)
	{
		SceneInfoBarView v = view as SceneInfoBarView;
		switch (data.Length) {
		case 1:
			if(data[0] is ModuleEnum){
				ModuleEnum name = (ModuleEnum)data[0];
				switch (name) {
				case ModuleEnum.HomeModule:
					v.gameObject.SetActive(false);
					break;
				case ModuleEnum.RewardModule:
				case ModuleEnum.MusicModule:
				case ModuleEnum.OperationNoticeModule:
				case ModuleEnum.NicknameModule:
				case ModuleEnum.UnitDetailModule:
				case ModuleEnum.MsgWindowModule:
				case ModuleEnum.NoviceGuideTipsModule:
				case ModuleEnum.NoviceMsgWindowModule:
				case ModuleEnum.MaskModule:
				case ModuleEnum.UnitSortModule:
				case ModuleEnum.ItemCounterModule:
				case ModuleEnum.ApplyMessageModule:
					break;
				case ModuleEnum.FriendMainModule:
				case ModuleEnum.ScratchModule:
				case ModuleEnum.ShopModule:
				case ModuleEnum.OthersModule:
				case ModuleEnum.UnitsMainModule:
					v.gameObject.SetActive(true);
					v.SetBackBtnActive(false);
					v.SetSceneName((ModuleEnum)data[0] );
					break;
				default:
					view.gameObject.SetActive(true);
					v.SetBackBtnActive(true,GetBackModule(name));
					v.SetSceneName((ModuleEnum)data[0]);
					break;
				}
			}else if(data[0] is string){
				if(data[0].ToString() == "level_up"){
					v.SetBackBtnActive(true,ModuleEnum.UnitLevelupAndEvolveModule);
				}else if(data[0].ToString() == "evolve"){
					v.SetBackBtnActive(true,ModuleEnum.EvolveModule);
				}else if(data[0].ToString() == "quest"){
					v.SetBackBtnActive(true,ModuleEnum.QuestSelectModule);
				}
			}
			break;
		case 2:
		 	if(data[0].ToString() == "stage"){
				v.SetSceneName((ModuleEnum)data[1]);
			}
			break;
		default:
			Debug.LogError("Scene Info Args Length Err: ");
			break;
		}

	}

	private ModuleEnum GetBackModule(ModuleEnum name){
		ModuleEnum backName = ModuleEnum.None;
		switch (name) {
			case ModuleEnum.FriendListModule:
			case ModuleEnum.ApplyModule:
			case ModuleEnum.ReceptionModule:
			case ModuleEnum.SearchFriendModule:
				backName = ModuleEnum.FriendMainModule;
				break;
			case ModuleEnum.GameRaiderModule:
				backName = ModuleEnum.OthersModule;
				break;
			case ModuleEnum.PartyModule:
			case ModuleEnum.LevelUpModule:
			case ModuleEnum.SellUnitModule:
			case ModuleEnum.CatalogModule:
			case ModuleEnum.UnitsListModule: 
			case ModuleEnum.EvolveModule:
				backName = ModuleEnum.UnitsMainModule;
				break;
			case ModuleEnum.StageSelectModule:
				backName = ModuleEnum.HomeModule;
				break;
			case ModuleEnum.GachaModule:
				backName = ModuleEnum.ScratchModule;
				break;
			case ModuleEnum.QuestSelectModule:
				backName = ModuleEnum.StageSelectModule;
				break;
			case ModuleEnum.FightReadyModule:
				backName = ModuleEnum.FriendSelectModule;
				break;
			case ModuleEnum.UnitSelectModule:
			case ModuleEnum.UnitSourceModule:
				backName = ModuleEnum.UnitLevelupAndEvolveModule;
				break;
			default:
				backName = ModuleEnum.None;
				break;

		}
		return backName;
	}
}
