using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class AdvancedDamageTrigger
    {
        public static TriggerCalls Dealt => (TriggerCalls)3947294;
        public static TriggerCalls Received => (TriggerCalls)2340251;

        public static void PostTrigger(AdvancedDamageTempInfo info, object sender, object args)
        {
            if (sender is IUnit target && args is IntegerReference reference)
            {
                AdvancedDamageInfo ret = new AdvancedDamageInfo(reference.value, target, info.Direct, info.Attacker, info.Type);

                CombatManager.Instance.PostNotification(Received.ToString(), target, ret);

                if (info.Attacker != null) CombatManager.Instance.PostNotification(Dealt.ToString(), info.Attacker, ret);
            }
        }
        public static void PostTrigger(bool direct, object sender, object args)
        {
            if (sender is IUnit target && args is IntegerReference reference)
            {
                AdvancedDamageInfo ret = new AdvancedDamageInfo(reference.value, target, direct, null, "");

                CombatManager.Instance.PostNotification(Received.ToString(), target, ret);
            }
        }
    }

    public class AdvancedDamageInfo : IntegerReference
    {
        public readonly string Type;
        public readonly bool Direct;

        public readonly IUnit Killer;
        public readonly IUnit Target;

        public AdvancedDamageInfo(int amount, IUnit target, bool direct, IUnit killer, string type) : base(amount)
        {
            Type = type;
            Direct = direct;
            Killer = killer;
            Target = target;
        }
    }
}
