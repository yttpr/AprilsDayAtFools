using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

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
        }
    }
}
