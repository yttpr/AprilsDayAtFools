using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Tools;

namespace AprilsDayAtFools
{
    public static class Research
    {
        public static void Add()
        {
            //Foundling();
        }
        public static void Foundling()
        {
            EnemySO foundling = LoadedAssetsHandler.GetEnemy("Foundling_Derelict_EN");
            if (foundling == null || foundling.Equals(null)) foundling = LoadedAssetsHandler.GetEnemy("DerelictFoundling_EN");

            foreach (EffectInfo info in foundling.enterEffects) Debug.Log(info.effect);

            CasterAddExtraAbilitySetFromPreviousEffect test = foundling.enterEffects[1].effect as CasterAddExtraAbilitySetFromPreviousEffect;
            Debug.Log("going through ABILITY SETS");
            foreach (IntListWrapper wrapper in test._AbilitySets)
            {
                string working = "";
                foreach (int num in wrapper.list)
                {
                    working += num.ToString();
                    working += ", ";
                }
                Debug.Log("int list wrapper: " + working);
            }
            Debug.Log("going through POOLS DATA");
            foreach (ExtraAbilityInfoListWrapper wrapper in test._PoolsData)
            {
                Debug.Log("NEW POOL DATA");
                foreach (ExtraAbilityInfo info in wrapper.list)
                {
                    Debug.Log(info.ability);
                }
            }
        }
    }
}
