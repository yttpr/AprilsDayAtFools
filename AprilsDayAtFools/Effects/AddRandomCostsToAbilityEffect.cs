using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AprilsDayAtFools
{
    public class AddRandomCostsToAbilityEffect : EffectSO
    {
        public ManaColorSO[] Options;
        public string[] Ability_IDs;
        public bool _usePreviousExit;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (Options == null || Options.Length <= 0) return false;
            if (Ability_IDs == null || Ability_IDs.Length <= 0) return false;

            if (_usePreviousExit)
                entryVariable *= PreviousExitValue;

            if (entryVariable <= 0) return false;

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit && target.Unit is CharacterCombat chara)
                {
                    foreach (CombatAbility abil in chara.CombatAbilities)
                    {
                        if (Ability_IDs.Contains(abil.ability.name))
                        {
                            List<ManaColorSO> new_cost = [.. abil.cost];
                            for (int i = 0; i < entryVariable; i++)
                            {
                                new_cost.Add(Options.GetRandom());
                                exitAmount++;
                            }
                            abil.cost = new_cost.ToArray();
                        }
                    }

                    CombatManager.Instance.AddUIAction(new CharacterUpdateAllAttacksUIAction(chara.ID, chara.CombatAbilities.ToArray()));
                }
            }

            return exitAmount > 0;
        }
    }
}
