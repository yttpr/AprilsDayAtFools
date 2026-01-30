using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ImmediatePerformEffectPassive : PerformEffectPassiveAbility
    {
        public override bool IsPassiveImmediate => true;

        public override void TriggerPassive(object sender, object args)
        {
            IUnit caster = sender as IUnit;
            CombatManager.Instance.ProcessImmediateAction(new ImmediateEffectAction(effects, caster));
        }
    }
}
