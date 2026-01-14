using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class DamageTargetEffectsCondition : EffectorConditionSO
    {
        public EffectInfo[] Effects;
        public bool ShowItem;

        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is AdvancedDamageInfo info)
            {
                CombatManager.Instance.AddSubAction(new DamageTargetEffectsConditionAction(info.Target, effector as IUnit, Effects, ShowItem));
            }
            return true;
        }

        public static DamageTargetEffectsCondition Create(EffectInfo[] effects, bool showitem)
        {
            DamageTargetEffectsCondition ret = ScriptableObject.CreateInstance<DamageTargetEffectsCondition>();
            ret.Effects = effects;
            ret.ShowItem = showitem;
            return ret;
        }
    }

    public class DamageTargetEffectsConditionAction : CombatAction
    {
        public IUnit Caster;
        public IUnit Target;
        public bool ShowItem;
        public EffectInfo[] Effects;
        public DamageTargetEffectsConditionAction(IUnit target, IUnit caster, EffectInfo[] effects, bool showItem = false)
        {
            Caster = caster;
            Target = target;
            Effects = effects != null ? effects : [];
            ShowItem = showItem;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            if (ShowItem && Caster != null) Caster.ShowItem();
            return new EffectAction(Effects, Target).Execute(stats);
        }
    }
}
