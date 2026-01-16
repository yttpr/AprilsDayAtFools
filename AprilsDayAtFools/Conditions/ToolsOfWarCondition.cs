using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ToolsOfWarCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageDealtValueChangeException reference)
            {
                (effector as IUnit).ShowItem();
                reference.AddModifier(new ToolsOfWarModifier(effector as IUnit, true, ""));
            }
            return true;
        }
    }

    public class ToolsOfWarModifier : IntValueModifier
    {
        public IUnit caster;
        public string type;
        public bool direct;
        public ToolsOfWarModifier(IUnit unit, bool direct, string type) : base(10)
        {
            caster = unit;
            this.type = type;
            this.direct = direct;
        }
        public override int Modify(int value)
        {
            DamageReceivedValueChangeException ex = Help.GenerateDamageTakenException(value, type, DeathType_GameIDs.Basic.ToString(), direct, false, caster.SlotID, caster.SlotID + caster.Size - 1, caster, caster);
            CombatManager.Instance.PostNotification(TriggerCalls.OnBeingDamaged.ToString(), caster, ex);
            return ex.GetModifiedValue();
        }
    }
}
