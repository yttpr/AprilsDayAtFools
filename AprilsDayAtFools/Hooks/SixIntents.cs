using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{

    public static class SixIntents
    {
        public static string[] abilities => ["Six_Fingers_1_A", "Six_Fingers_2_A", "Six_Fingers_3_A", "Six_Fingers_4_A"];
        public static List<string> default_intents => [IntentColor.Intent, "Status_Cursed", "Status_Frail", "Status_Ruptured", "Status_Gutted", "Status_Linked", "Status_Scars", "Status_OilSlicked",
            Acid.Intent, Drowning.Intent, Entropy.Intent, Karma.Intent, Pale.Intent, Pimples.Intent, Terror.Intent];

        public static string[] potential_intents => ["Status_Remorse",
            "Status_Salted", "Status_Disappearing", "Status_CoralColony", "Status_Infirm", "Status_Downfall",
            "Status_Paranoia", "Status_Madness", "Status_DivineSacrifice", "Status_Muted", "Status_Left"];

        public static void Setup()
        {
            List<string> final_intents = default_intents;
            foreach (string intent in potential_intents)
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
    }
}
