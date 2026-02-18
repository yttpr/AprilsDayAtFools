using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class HasConstrictingEffectCondition : EffectConditionSO
    {
        public bool Has;
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            //return true;//test
            return caster.ContainsPassiveAbility(PassiveType_GameIDs.Constricting.ToString()) == Has;
        }
        public static HasConstrictingEffectCondition Create(bool shouldHave)
        {
            HasConstrictingEffectCondition ret = ScriptableObject.CreateInstance<HasConstrictingEffectCondition>();
            ret.Has = shouldHave;
            return ret;
        }
    }
}
