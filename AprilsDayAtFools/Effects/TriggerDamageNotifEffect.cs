using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace AprilsDayAtFools
{
    public class TriggerDamageNotifEffect : EffectSO
    {
        public bool _indirect;
        public string _damageType;
        public string _deathType;
        public bool _ignoreShield;
        public bool _useCaster;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            string type = _damageType;
            string death = _deathType;
            if (type == "") type = Utils.GetBasicDamageIDFromAmount(entryVariable);
            if (death == "") death = DeathType_GameIDs.Basic.ToString();

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    IntegerReference_Damage damaged = new IntegerReference_Damage(entryVariable, type, death, !_indirect, _indirect || _ignoreShield, areTargetSlots ? target.SlotID : target.Unit.SlotID, areTargetSlots ? target.SlotID : target.Unit.SlotID + target.Unit.Size - 1, _useCaster && !_indirect ? caster : null, target.Unit);

                    if (_indirect)
                    {
                        CombatManager.Instance.PostNotification(TriggerCalls.OnIndirectDamaged.ToString(), target.Unit, damaged);
                    }
                    else
                    {
                        CombatManager.Instance.PostNotification(TriggerCalls.OnDirectDamaged.ToString(), target.Unit, damaged);
                    }
                    CombatManager.Instance.PostNotification(TriggerCalls.OnDamaged.ToString(), target.Unit, damaged);

                    exitAmount++;
                }
            }

            return exitAmount > 0;
        }
    }
}
