using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class HeavenFix
    {
        public static void Add()
        {
            InanimatePassiveAbility newInan = ScriptableObject.Instantiate(Passives.Inanimate) as InanimatePassiveAbility;
            newInan._triggerOn = [TriggerCalls.CanHeal];
            newInan.name = "NewInanimate";
            newInan._enemyDescription = "This enemy is an inanimate object and thus cannot be healed.";

            LoadedAssetsHandler.GetEnemy("Heaven_BOSS").passiveAbilities[1] = newInan;
        }
    }
}
