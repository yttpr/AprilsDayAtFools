using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CascadeBySortedHealthEffect : EffectSO
    {
        public bool _direct;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    List<IUnit> units = [];
                    if (target.Unit.IsUnitCharacter)
                    {
                        foreach (CharacterCombat chara in stats.CharactersOnField.Values)
                            if (chara.CurrentHealth > target.Unit.CurrentHealth) units.Add(chara);
                    }
                    else
                    {
                        foreach (EnemyCombat enemy in stats.EnemiesOnField.Values)
                            if (enemy.CurrentHealth > target.Unit.CurrentHealth) units.Add(enemy);
                    }

                    units = Sorter.SortByHealth(units, true);

                    int amount = PreviousExitValue;
                    for (int i = 0; i < units.Count; i++)
                    {
                        amount += entryVariable;

                        int num = amount;
                        if (_direct) num = caster.WillApplyDamage(num, units[i]);
                        DamageInfo info = units[i].Damage(num, _direct ? caster : null, "Basic", -1, _direct, _direct, _direct);
                        exitAmount += info.damageAmount;
                    }
                }
            }

            if (_direct) caster.DidApplyDamage(exitAmount);

            return exitAmount > 0;
        }
    }
}
