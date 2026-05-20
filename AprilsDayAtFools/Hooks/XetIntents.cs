using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class XetIntents
    {
        public static string[] abilities => ["Xet_Rearrange_1_A", "Xet_Rearrange_2_A", "Xet_Rearrange_3_A", "Xet_Rearrange_4_A"];
        public static string[] b_abilities => ["Xet_Replace_1_A", "Xet_Replace_2_A", "Xet_Replace_3_A", "Xet_Replace_4_A"];
        public static List<string> default_intents => [IntentColor.Intent, "Status_Cursed", "Status_Frail", "Status_Ruptured", "Status_DivineProtection", "Status_Focused", "Status_Gutted", "Status_Linked", "Status_Scars", "Status_OilSlicked", 
            Acid.Intent, Anesthetics.Intent, Determined.Intent, Dodge.Intent, Drowning.Intent, Entropy.Intent, Haste.Intent, Inverted.Intent, Karma.Intent, Pale.Intent, Pimples.Intent, Power.Intent, Terror.Intent];

        public static string[] potential_intents => ["Status_WildCard", "Status_Growth", "Status_Remorse", 
            "Status_Salted", "Status_Disappearing", "Status_Partaking", "Status_CoralColony", "Status_Infirm", "Status_Downfall", 
            "Status_Paranoia", "Status_Madness", "Status_DivineSacrifice", "Status_Favor", "Status_Muted", "Status_Photo", "Status_Left"];
        public static string[] pos_potential_intents => ["Status_WildCard", "Status_Growth", 
            "Status_Partaking", 
            "Status_Favor", "Status_Photo"];
        public static List<string> pos_default_intents => [IntentColor.Intent, "Status_DivineProtection", "Status_Focused", "Status_Gutted", "Status_Linked", 
            Anesthetics.Intent, Determined.Intent, Dodge.Intent, Haste.Intent, Inverted.Intent, Pimples.Intent, Power.Intent];

        
        public static void Setup()
        {
            Setup_Rearrange();
            Setup_Replace();
        }

        public static void Setup_Rearrange()
        {
            List<string> final_intents = pos_default_intents;
            foreach (string intent in pos_potential_intents)
            {
                if (LoadedDBsHandler.IntentDB.m_IntentBasicPool.ContainsKey(intent))
                    final_intents.Add(intent);
            }

            string[] intents = final_intents.ToArray();
            IntentTargetInfo info = new IntentTargetInfo();
            info.targets = Slots.Front;
            info.intents = intents;

            foreach (string abil in abilities)
            {
                AbilitySO ability = LoadedAssetsHandler.GetCharacterAbility(abil);
                ability.intents.Add(info);
            }
        }
        public static void Setup_Replace()
        {
            List<string> final_intents = default_intents;
            foreach (string intent in potential_intents)
            {
                if (LoadedDBsHandler.IntentDB.m_IntentBasicPool.ContainsKey(intent))
                    final_intents.Add(intent);
            }

            string[] intents = final_intents.ToArray();
            IntentTargetInfo info = new IntentTargetInfo();
            info.targets = Slots.LeftRight;
            info.intents = intents;

            foreach (string abil in b_abilities)
            {
                AbilitySO ability = LoadedAssetsHandler.GetCharacterAbility(abil);
                ability.intents.Add(info);
            }
        }
    }
}
