using BrutalAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static UnityEngine.UI.CanvasScaler;

namespace AprilsDayAtFools
{
    public class GenerateSelfHealthDamageEffect : DamageEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (_usePreviousExitValue)
            {
                entryVariable *= base.PreviousExitValue;
            }

            exitAmount = 0;
            bool flag = false;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    int targetSlotOffset = (areTargetSlots ? (targetSlotInfo.SlotID - targetSlotInfo.Unit.SlotID) : (-1));
                    int amount = entryVariable;
                    DamageInfo damageInfo;
                    if (_indirect)
                    {
                        damageInfo = targetSlotInfo.Unit.Damage(amount, null, _DeathTypeID, targetSlotOffset, addHealthMana: false, directDamage: false, ignoresShield: true);
                    }
                    else
                    {
                        amount = caster.WillApplyDamage(amount, targetSlotInfo.Unit);

                        ManaColorSO startcolor = targetSlotInfo.Unit.HealthColor;
                        if (targetSlotInfo.Unit is CharacterCombat chara) chara._healthColor = caster.HealthColor;
                        else if (targetSlotInfo.Unit is EnemyCombat enemy) enemy._healthColor = caster.HealthColor;

                        damageInfo = targetSlotInfo.Unit.Damage(amount, caster, _DeathTypeID, targetSlotOffset, addHealthMana: true, directDamage: true, _ignoreShield);

                        if (targetSlotInfo.Unit is CharacterCombat chara2) chara2._healthColor = startcolor;
                        else if (targetSlotInfo.Unit is EnemyCombat enemy2) enemy2._healthColor = startcolor;
                    }

                    flag |= damageInfo.beenKilled;
                    exitAmount += damageInfo.damageAmount;
                }
            }

            if (!_indirect && exitAmount > 0)
            {
                caster.DidApplyDamage(exitAmount);
            }

            if (!_returnKillAsSuccess)
            {
                return exitAmount > 0;
            }

            return flag;
        }
    }
    public class SecretChangeTargetColorAction : CombatAction
    {
        public IUnit unit;
        public ManaColorSO pigment;
        public SecretChangeTargetColorAction(IUnit unit, ManaColorSO pigment)
        {
            this.unit = unit;
            this.pigment = pigment;
        }
        public override IEnumerator Execute(CombatStats stats)
        {
            if (unit is CharacterCombat chara2) chara2._healthColor = pigment;
            else if (unit is EnemyCombat enemy2) enemy2._healthColor = pigment;
            yield break;
        }
    }
}
