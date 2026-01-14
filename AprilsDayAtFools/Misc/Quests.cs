using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Quests
    {
        public static void Add()
        {
            AddUnlock("Saea", "Saea_CH");
            AddUnlock("Alpha", "Alpha_CH");

            AddQuest("Saea_CH", "Saea", 3, Color.white, true, false, false);
        }
        public static void AddUnlock(string name, string id)
        {
            UnlockableModData data = new UnlockableModData(name);
            data.hasCharacterUnlock = true;
            data.character = id;
            data.hasQuestCompletion = true;
            data.questID = name;
            data.hasModdedAchievementUnlock = false;
            LoadedDBsHandler.UnlockablesDB.TryAddIDUnlock(data);
            SingleUnlockCheck unlock = ScriptableObject.CreateInstance<SingleUnlockCheck>();
            unlock.unlockData = data;
            unlock.unlockID = name;
            unlock.name = name;
            LoadedDBsHandler.UnlockablesDB.TryAddMiscUnlockCheck(unlock);
        }
        public static void AddQuest(string id, string name, int zone, Color text, bool left, bool center, bool easy = false)
        {
            CharacterSO chara = LoadedAssetsHandler.GetCharacter(id);

            SpeakerBundle speaker = new SpeakerBundle();
            speaker.bundleTextColor = text;
            speaker.dialogueSound = chara.dxSound;
            speaker.portrait = chara.characterSprite;

            Dialogues.CreateAndAddCustom_SpeakerData(name + "_SpeakerData", speaker, left, center, []);

            Dialogues.CreateAndAddCustom_DialogueSO("Aprils." + name + ".Start", Joyce.Yarn, "Aprils.Untitled", "Aprils." + name + ".Start");

            Portals.AddPortalSign(id + "_Sign", chara.characterOWSprite, Portals.NPCIDColor);

            ConditionEncounterSO room = ScriptableObject.CreateInstance<ConditionEncounterSO>();
            room.encounterEntityIDs = [chara.name];
            room.m_QuestName = name;
            room.m_QuestsCompletedNeeded = [];
            room.signID = id + "_Sign";
            room._dialogue = "Aprils." + name + ".Start";
            room.encounterRoom = "Aprils." + name + ".Unlock";

            ModdedNPCs.AddCustom_ConditionEncounter("Aprils." + name + ".Unlock", room);

            FreeFool.OldPatch_Prepare_NPC_RoomPrefab("Assets/Rooms2/" + name + "Room.prefab", "Aprils." + name + ".Unlock", Joyce.Assets);

            switch (zone)
            {
                case 1:
                    ZoneBGDataBaseSO zone1 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_01") as ZoneBGDataBaseSO;
                    zone1._QuestPool.Add("Aprils." + name + ".Unlock");
                    break;
                case 2:
                    ZoneBGDataBaseSO zone2 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_02") as ZoneBGDataBaseSO;
                    zone2._QuestPool.Add("Aprils." + name + ".Unlock");
                    break;
                case 3:
                    ZoneBGDataBaseSO zone3 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_03") as ZoneBGDataBaseSO;
                    zone3._QuestPool.Add("Aprils." + name + ".Unlock");
                    break;
            }

            if (easy)
            {
                switch (zone)
                {
                    case 1:
                        ZoneBGDataBaseSO zone1 = LoadedAssetsHandler.GetZoneDB("ZoneDB_01") as ZoneBGDataBaseSO;
                        zone1._FreeFoolsPool.Add("Aprils." + name + ".Unlock");
                        break;
                    case 2:
                        ZoneBGDataBaseSO zone2 = LoadedAssetsHandler.GetZoneDB("ZoneDB_02") as ZoneBGDataBaseSO;
                        zone2._FreeFoolsPool.Add("Aprils." + name + ".Unlock");
                        break;
                }
            }
        }
    }
}
