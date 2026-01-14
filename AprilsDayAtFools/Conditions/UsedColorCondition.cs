using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class UsedColorCondition : EffectConditionSO
    {
        public ManaColorSO mana;
        public bool Should;
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            if (mana == null) return false;
            return Should == PigmentUsedCollector.lastUsed.Contains(mana);
        }
        public static UsedColorCondition Create(ManaColorSO Mana, bool ShouldHave)
        {
            UsedColorCondition ret = ScriptableObject.CreateInstance<UsedColorCondition>();
            ret.mana = Mana;
            ret.Should = ShouldHave;
            return ret;
        }
    }
}
