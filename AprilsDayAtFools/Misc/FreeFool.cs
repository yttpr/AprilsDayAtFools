using BrutalAPI;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace AprilsDayAtFools//change this to your namespace
{
    public static class FreeFool
    {
        public static void Setup()
        {
            Add("Merced_CH", "Merced", 3, new Color32(238, 195, 154, 255), true, false, false);
            Add("Cora_CH", "Cora", 3, new Color32(178, 79, 216, 255), true, false, false);
            Add("Xet_CH", "Xet", 2, new Color32(234, 208, 62, 255), true, false, false);
            Add("Saline_CH", "Saline", 3, new Color32(164, 130, 97, 255), true, false, false);
            Add("Didion_CH", "Didion", 1, new Color32(63, 63, 116, 255), true, true, true);
            Add("Rose_CH", "Rose", 1, new Color32(138, 17, 17, 255), true, false, true);
            Add("Prayer_CH", "Prayer", 1, new Color32(160, 92, 186, 255), true, false, true);
            Add("Kafka_CH", "Kafka", 1, new Color32(160, 92, 186, 255), false, false, true);
            Add("Bodybag_CH", "Bodybag", 1, new Color32(121, 30, 30, 255), true, true, false);
            Add("Hangman_CH", "Hangman", 2, new Color32(255, 248, 102, 255), true, false, false);
            Add("Saturn_CH", "Saturn", 3, new Color32(221, 221, 221, 255), false, true, false);
            Add("Hare_CH", "Hare", 1, new Color32(196, 74, 74, 255), false, false, false);
            Add("Moon_CH", "Moon", 2, new Color32(63, 63, 116, 255), false, true, false);
            Add("Clerk_CH", "Clerk", 2, new Color32(138, 17, 17, 255), true, false, false);
            Add("Esther_CH", "Esther", 3, new Color32(160, 92, 186, 255), true, false, false);
            Add("Rhys_CH", "Rhys", 2, new Color32(147, 55, 121, 255), true, false, true);
            Add("Wtmiyr_CH", "Wtmiyr", 1, new Color32(172, 50, 50, 255), true, true, true);
            Add("Joy_CH", "Joy", 2, new Color32(0, 212, 255, 255), true, true, true);
            Add("Patch_CH", "Patch", 2, new Color32(203, 191, 165, 255), true, true, false);
            Add("Rotcore_CH", "Rotcore", 3, new Color32(203, 191, 165, 255), true, false, false);
            Add("Catten_CH", "Catten", 2, new Color32(255, 255, 255, 255), true, true, false);
            Add("Sunflower_CH", "Sunflower", 1, new Color32(242, 215, 62, 255), true, false, false);
            Add("Snail_CH", "Snail", 1, new Color32(91, 171, 65, 255), true, false, true);
            Add("Lich_CH", "Lich", 2, new Color32(253, 153, 163, 255), false, false, false);
            Add("Izide_CH", "Izide", 2, new Color32(147, 55, 121, 255), true, false, false);
            Add("Six_CH", "Six", 1, new Color32(138, 17, 17, 255), true, false, false);
            if (April.Me) Add("Secret_CH", "Secret", 2, new Color32(241, 235, 232, 255), true, false, false);
            Add("Alpha_CH", "Alpha", 1, new Color32(238, 195, 154, 255), false, false, false);
            AddQualia("Qualia_CH", "Qualia", 3, new Color32(118, 66, 138, 255), false, false, false);

            //ok so basically what this is is,
            //Add("character id. the _CH", "any sort of name identifier, doesnt really matter", [area: 1 shore, 2 orph, 3 garden], color32(for the text color. its just thr rgb plus a 255 at the end for the alpha), bool do they face left, bool do they face center, bool should the show up in easy mode aswell)
        }
        public static void Add_Extra()
        {
            AddSaea("Saea_CH", "Saea", 3, Color.white, true, false, false);
            AddSecondLich("Lich_CH", "Lich", 2, new Color32(253, 153, 163, 255), true, false, false);
        }

        public static void Add_Siren()
        {
            if (LoadedAssetsHandler.LoadedZoneDBs.ContainsKey("TheSiren"))
            {
                AddModdedArea("Xet_CH", "Xet", "TheSiren", "Siren");
                AddModdedArea("Hangman_CH", "Hangman", "TheSiren", "Siren");
                AddModdedArea("Moon_CH", "Moon", "TheSiren", "Siren");
                AddModdedArea("Clerk_CH", "Clerk", "TheSiren", "Siren");
                AddModdedArea("Rhys_CH", "Rhys", "TheSiren", "Siren");
                AddModdedArea("Joy_CH", "Joy", "TheSiren", "Siren");
                AddModdedArea("Patch_CH", "Patch", "TheSiren", "Siren");
                AddModdedArea("Joy_CH", "Joy", "TheSiren", "Siren");
                AddModdedArea("Lich_CH", "Lich", "TheSiren", "Siren");
                AddModdedArea("Izide_CH", "Izide", "TheSiren", "Siren");
            }
        }
        public static void Add_Abyss()
        {
            if (LoadedAssetsHandler.LoadedZoneDBs.ContainsKey("TheAbyss"))
            {
                AddModdedArea("Merced_CH", "Merced", "TheAbyss", "Abyss");
                AddModdedArea("Cora_CH", "Cora", "TheAbyss", "Abyss");
                AddModdedArea("Saline_CH", "Saline", "TheAbyss", "Abyss");
                AddModdedArea("Saturn_CH", "Saturn", "TheAbyss", "Abyss");
                AddModdedArea("Esther_CH", "Esther", "TheAbyss", "Abyss");
                AddModdedArea("Rotcore_CH", "Rotcore", "TheAbyss", "Abyss");
                AddModdedArea("Qualia_CH", "Qualia", "TheAbyss", "Abyss");
            }
        }

        //this is for setting up the YarnProgram. if you do this somewhere else you dont have to do it again, but do note that in the .Add method it references the yarn program ID so you do need to make sure that's consistent
        public static void GenerateYarn()
        {
            Joyce.Yarn = Joyce.Assets.LoadAsset<YarnProgram>("Assets/Rooms2/Untitled.yarn");
            Dialogues.AddCustom_DialogueProgram("Aprils.Untitled", Joyce.Yarn);
        }

        //in this method below here, replace every instance of "Aprils" with whatever you feel like putting tbh.
        public static void Add(string id, string name, int zone, Color text, bool left, bool center, bool easy = false)
        {
            CharacterSO chara = LoadedAssetsHandler.GetCharacter(id);

            SpeakerBundle speaker = new SpeakerBundle();
            speaker.bundleTextColor = text;
            speaker.dialogueSound = chara.dxSound;
            speaker.portrait = chara.characterSprite;

            Dialogues.CreateAndAddCustom_SpeakerData(name + "_SpeakerData", speaker, left, center, []);

            //replace Joyce.Yarn with whatever your YarnProgram is
            //you may need to adjust your yarn IDs so that they match up with the naming convention here.
            Dialogues.CreateAndAddCustom_DialogueSO("Aprils." + name + ".TryHire", Joyce.Yarn, "Aprils.Untitled", "Aprils." + name + ".TryHire");

            Portals.AddPortalSign(id + "_Sign", chara.characterOWSprite, Portals.NPCIDColor);

            FreeFoolEncounterSO room = ScriptableObject.CreateInstance<FreeFoolEncounterSO>();
            room.encounterEntityIDs = new string[1] { chara.name };
            room._freeFool = chara.name;
            room.signID = id + "_Sign";
            room._dialogue = "Aprils." + name + ".TryHire";
            room.encounterRoom = "Aprils." + name + ".FreeFool";

            ModdedNPCs.AddCustom_FreeFoolEncounter("Aprils." + name + ".FreeFool", room);

            //here, i have a bit of a systematic thing for how i name and organize the prefabs in the bundle so you may need to adjust this to your specifications. 
            OldPatch_Prepare_NPC_RoomPrefab("Assets/Rooms2/" + name + "Room.prefab", "Aprils." + name + ".FreeFool", Joyce.Assets);//also change the assetbundle

            switch (zone)
            {
                case 1:
                    ZoneBGDataBaseSO zone1 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_01") as ZoneBGDataBaseSO;
                    zone1._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                    break;
                case 2:
                    ZoneBGDataBaseSO zone2 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_02") as ZoneBGDataBaseSO;
                    zone2._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                    break;
                case 3:
                    ZoneBGDataBaseSO zone3 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_03") as ZoneBGDataBaseSO;
                    zone3._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                    break;
            }
            
            if (easy)
            {
                switch (zone)
                {
                    case 1:
                        ZoneBGDataBaseSO zone1 = LoadedAssetsHandler.GetZoneDB("ZoneDB_01") as ZoneBGDataBaseSO;
                        zone1._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                        break;
                    case 2:
                        ZoneBGDataBaseSO zone2 = LoadedAssetsHandler.GetZoneDB("ZoneDB_02") as ZoneBGDataBaseSO;
                        zone2._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                        break;
                }
            }
        }
        public static void AddSaea(string id, string name, int zone, Color text, bool left, bool center, bool easy = false)
        {
            CharacterSO chara = LoadedAssetsHandler.GetCharacter(id);

            //replace Joyce.Yarn with whatever your YarnProgram is
            //you may need to adjust your yarn IDs so that they match up with the naming convention here.
            Dialogues.CreateAndAddCustom_DialogueSO("Aprils." + name + ".TryHire", Joyce.Yarn, "Aprils.Untitled", "Aprils." + name + ".TryHire");

            FreeFoolEncounterSO room = ScriptableObject.CreateInstance<FreeFoolEncounterSO>();
            room.encounterEntityIDs = new string[1] { chara.name };
            room._freeFool = chara.name;
            room.signID = id + "_Sign";
            room._dialogue = "Aprils." + name + ".TryHire";
            room.encounterRoom = "Aprils." + name + ".Unlock";

            ModdedNPCs.AddCustom_FreeFoolEncounter("Aprils." + name + ".FreeFool", room);

            switch (zone)
            {
                case 1:
                    ZoneBGDataBaseSO zone1 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_01") as ZoneBGDataBaseSO;
                    zone1._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                    break;
                case 2:
                    ZoneBGDataBaseSO zone2 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_02") as ZoneBGDataBaseSO;
                    zone2._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                    break;
                case 3:
                    ZoneBGDataBaseSO zone3 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_03") as ZoneBGDataBaseSO;
                    zone3._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                    break;
            }

            if (easy)
            {
                switch (zone)
                {
                    case 1:
                        ZoneBGDataBaseSO zone1 = LoadedAssetsHandler.GetZoneDB("ZoneDB_01") as ZoneBGDataBaseSO;
                        zone1._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                        break;
                    case 2:
                        ZoneBGDataBaseSO zone2 = LoadedAssetsHandler.GetZoneDB("ZoneDB_02") as ZoneBGDataBaseSO;
                        zone2._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                        break;
                }
            }
        }
        public static void AddSecondLich(string id, string name, int zone, Color text, bool left, bool center, bool easy = false)
        {
            CharacterSO chara = LoadedAssetsHandler.GetCharacter(id);

            //replace Joyce.Yarn with whatever your YarnProgram is
            //you may need to adjust your yarn IDs so that they match up with the naming convention here.
            Dialogues.CreateAndAddCustom_DialogueSO("Aprils." + name + ".Second", Joyce.Yarn, "Aprils.Untitled", "Aprils." + name + ".Second");

            FreeFoolEncounterSO room = ScriptableObject.CreateInstance<FreeFoolEncounterSO>();
            room.encounterEntityIDs = new string[1] { chara.name };
            room._freeFool = chara.name;
            room.signID = id + "_Sign";
            room._dialogue = "Aprils." + name + ".Second";
            room.encounterRoom = "Aprils." + name + ".FreeFool";

            ModdedNPCs.AddCustom_FreeFoolEncounter(UndeadPassiveHandler.Room, room);
        }
        public static void AddQualia(string id, string name, int zone, Color text, bool left, bool center, bool easy = false)
        {
            CharacterSO chara = LoadedAssetsHandler.GetCharacter(id);

            SpeakerBundle speaker = new SpeakerBundle();
            speaker.bundleTextColor = text;
            speaker.dialogueSound = chara.dxSound;
            speaker.portrait = ResourceLoader.LoadSprite("QualiaTalk.png");

            Dialogues.CreateAndAddCustom_SpeakerData(name + "_SpeakerData", speaker, left, center, []);

            //replace Joyce.Yarn with whatever your YarnProgram is
            //you may need to adjust your yarn IDs so that they match up with the naming convention here.
            Dialogues.CreateAndAddCustom_DialogueSO("Aprils." + name + ".TryHire", Joyce.Yarn, "Aprils.Untitled", "Aprils." + name + ".TryHire");

            Portals.AddPortalSign(id + "_Sign", chara.characterOWSprite, Portals.NPCIDColor);

            FreeFoolEncounterSO room = ScriptableObject.CreateInstance<FreeFoolEncounterSO>();
            room.encounterEntityIDs = new string[1] { chara.name };
            room._freeFool = chara.name;
            room.signID = id + "_Sign";
            room._dialogue = "Aprils." + name + ".TryHire";
            room.encounterRoom = "Aprils." + name + ".FreeFool";

            ModdedNPCs.AddCustom_FreeFoolEncounter("Aprils." + name + ".FreeFool", room);

            //here, i have a bit of a systematic thing for how i name and organize the prefabs in the bundle so you may need to adjust this to your specifications. 
            OldPatch_Prepare_NPC_RoomPrefab("Assets/Rooms2/" + name + "Room.prefab", "Aprils." + name + ".FreeFool", Joyce.Assets);//also change the assetbundle

            switch (zone)
            {
                case 1:
                    ZoneBGDataBaseSO zone1 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_01") as ZoneBGDataBaseSO;
                    zone1._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                    break;
                case 2:
                    ZoneBGDataBaseSO zone2 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_02") as ZoneBGDataBaseSO;
                    zone2._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                    break;
                case 3:
                    ZoneBGDataBaseSO zone3 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_03") as ZoneBGDataBaseSO;
                    zone3._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                    break;
            }

            if (easy)
            {
                switch (zone)
                {
                    case 1:
                        ZoneBGDataBaseSO zone1 = LoadedAssetsHandler.GetZoneDB("ZoneDB_01") as ZoneBGDataBaseSO;
                        zone1._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                        break;
                    case 2:
                        ZoneBGDataBaseSO zone2 = LoadedAssetsHandler.GetZoneDB("ZoneDB_02") as ZoneBGDataBaseSO;
                        zone2._FreeFoolsPool.Add("Aprils." + name + ".FreeFool");
                        break;
                }
            }
        }

        //area

        public static void AddModdedArea(string id, string name, string area_id, string area)
        {
            CharacterSO chara = LoadedAssetsHandler.GetCharacter(id);

            FreeFoolEncounterSO room = ScriptableObject.CreateInstance<FreeFoolEncounterSO>();
            room.encounterEntityIDs = new string[1] { chara.name };
            room._freeFool = chara.name;
            room.signID = id + "_Sign";
            room._dialogue = "Aprils." + name + ".TryHire";
            room.encounterRoom = "Aprils." + area + "." + name + ".FreeFool";

            ModdedNPCs.AddCustom_FreeFoolEncounter("Aprils." + area + "." + name + ".FreeFool", room);

            //here, i have a bit of a systematic thing for how i name and organize the prefabs in the bundle so you may need to adjust this to your specifications. 
            OldPatch_Prepare_NPC_RoomPrefab("Assets/Rooms2/" + area + "/" + name + "Room.prefab", "Aprils." + area + "." + name + ".FreeFool", Joyce.Assets);//also change the assetbundle

            ZoneBGDataBaseSO zone2 = LoadedAssetsHandler.GetZoneDB(area_id) as ZoneBGDataBaseSO;
            zone2._FreeFoolsPool.Add("Aprils." + area + "." + name + ".FreeFool");
        }

        //you dont need to touch this method i dont think
        public static void OldPatch_Prepare_NPC_RoomPrefab(string prefabBundlePath, string roomID, AssetBundle fileBundle)
        {
            NPCRoomHandler room = fileBundle.LoadAsset<GameObject>(prefabBundlePath).AddComponent<NPCRoomHandler>();

            room._npcSelectable = room.transform.GetChild(0).gameObject.AddComponent<BasicRoomItem>();
            room._npcSelectable._renderers = new SpriteRenderer[]
            {
                room._npcSelectable.transform.GetChild(0).GetComponent<SpriteRenderer>()
            };
            room._npcSelectable._detector = room._npcSelectable.transform.GetComponent<BoxCollider2D>();

            room._npcSelectable.SetMaterials(LoadedDBsHandler.MiscDB.GetMaterial(Misc.MaterialIDs.Outline.ToString()));

            LoadedAssetsHandler.TryAddExternalOWRoom(roomID, room);
        }

        //call this method in your awake to generate 1 billion free fool events into the run. for testing purposes.
        public static void Idk()
        {
            CardInfo info = new CardInfo()
            {
                pilePosition = PilePositionType.First,
                cardType = CardType.EventFreeFool
            };
            CardTypeInfo card = new CardTypeInfo();
            card._cardInfo = info;
            card._minimumAmount = 25;
            card._maximumAmount = 25;
            (LoadedAssetsHandler.GetZoneDB("ZoneDB_01") as ZoneBGDataBaseSO)._deckInfo._possibleCards = new List<CardTypeInfo>((LoadedAssetsHandler.GetZoneDB("ZoneDB_01") as ZoneBGDataBaseSO)._deckInfo._possibleCards) { card }.ToArray();
            (LoadedAssetsHandler.GetZoneDB("ZoneDB_02") as ZoneBGDataBaseSO)._deckInfo._possibleCards = new List<CardTypeInfo>((LoadedAssetsHandler.GetZoneDB("ZoneDB_02") as ZoneBGDataBaseSO)._deckInfo._possibleCards) { card }.ToArray();
            (LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_01") as ZoneBGDataBaseSO)._deckInfo._possibleCards = new List<CardTypeInfo>((LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_01") as ZoneBGDataBaseSO)._deckInfo._possibleCards) { card }.ToArray();
            (LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_02") as ZoneBGDataBaseSO)._deckInfo._possibleCards = new List<CardTypeInfo>((LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_02") as ZoneBGDataBaseSO)._deckInfo._possibleCards) { card }.ToArray();
            (LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_03") as ZoneBGDataBaseSO)._deckInfo._possibleCards = new List<CardTypeInfo>((LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_03") as ZoneBGDataBaseSO)._deckInfo._possibleCards) { card }.ToArray();
        }
    }
}
