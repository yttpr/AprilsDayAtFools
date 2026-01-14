using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class OnPauseEffectPassive : PerformEffectPassiveAbility
    {
        public override void CustomOnTriggerAttached(IPassiveEffector caller)
        {
            base.CustomOnTriggerAttached(caller);
        }
        public override void CustomOnTriggerDettached(IPassiveEffector caller)
        {
            base.CustomOnTriggerDettached(caller);
        }
    }
}
