using BepInEx.Configuration;
using BepInEx;
using BrutalAPI;
using System;
using System.Reflection;
using UnityEngine;
using System.IO;

namespace AprilsDayAtFools
{
    public class Class1
    {
        public static bool UnlockedByDefault => Joyce.UnlockByDefault;
        public static void Test()
        {
            DamageByStoredValueEffect d;
            ResurrectEffect r;
            CopyCasterAndSpawnCharacterAnywhereEffect c;
            MainMenuController m;
        }
        public static void StringDump()
        {

        }

        public static void Config()
        {
            ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "AprilsDayAtFoolsConfig.cfg"), true);
            ConfigEntry<bool> unlock_all = config.Bind<bool>("AprilsDayAtFools", "UnlockAllByDefault", false, "Unlocks the 10 secret fools, skipping Saea's quest.");
            Joyce.UnlockByDefault = unlock_all.Value;
        }
        public static void SetAssets()
        {
            Joyce.Assets = AssetBundle.LoadFromMemory(ResourceLoader.ResourceBinary("codachan"));
            Joyce.Attacks = AssetBundle.LoadFromMemory(ResourceLoader.ResourceBinary("lunic"));
            Joyce.Other = AssetBundle.LoadFromMemory(ResourceLoader.ResourceBinary("painstar"));
            Joyce.Yarn = Joyce.Assets.LoadAsset<YarnProgram>("Assets/Rooms2/Untitled.yarn");
            Dialogues.AddCustom_DialogueProgram("Aprils.Untitled", Joyce.Yarn);
        }
        public static void PCall(Action call)
        {
            try { call(); }
            catch (Exception ex)
            {
                try
                {
                    Debug.LogError(call.GetMethodInfo().ReflectedType + " " + call.GetMethodInfo().Name + " FUCKING FAILED TO GET ADDED");
                }
                catch
                {
                    Debug.LogError("some fucking function failed to get added");
                }

                Debug.LogError(ex.ToString() + ex.Message + ex.StackTrace);
            }
        }
    }
}
