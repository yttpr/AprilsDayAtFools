using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

                            //cost math
                            /*if (new_cost.Count > stats.combatUI._characterCost.SlotCount)
                            {
                                List<ManaSlotLayout> new_slots = [.. stats.combatUI._characterCost._costSlots];
                                List<ManaColorSO> cost_object = [.. stats.combatUI._characterCost.CurrentCost];
                                List<CostSlotUIInfo> ui_slots = [.. stats.combatUI._costBarInfo];

                                for (int k = 0; k < new_cost.Count - stats.combatUI._characterCost.SlotCount; k++)
                                {
                                    GameObject original = new_slots[new_slots.Count - 1].gameObject;
                                    GameObject clone = GameObject.Instantiate(original, original.transform.parent);
                                    new_slots.Add(clone.GetComponent<ManaSlotLayout>());

                                    cost_object.Add(null);

                                    ui_slots.Add(new CostSlotUIInfo());
                                }

                                stats.combatUI._characterCost._costSlots = new_slots.ToArray();
                                stats.combatUI._characterCost.CurrentCost = cost_object.ToArray();
                                stats.combatUI._costBarInfo = ui_slots.ToArray();

                                Debug.Log("update cost layout to meet: " + new_slots.Count.ToString());
                            }*/
                        }
                    }

                    CombatManager.Instance.AddUIAction(new CharacterUpdateAllAttacksUIAction(chara.ID, chara.CombatAbilities.ToArray()));
                }
            }

            return exitAmount > 0;
        }
    }
}
