using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class HandHeldDioramaCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is StringReference reference && effector is IUnit)
            {
                CombatManager.Instance.AddRootAction(new RootActionAction(new SubActionAction(new HandHeldDioramaAction(effector as IUnit, reference.value, PigmentUsedCollector.lastUsed.ToArray()))));
            }
            return true;
        }
    }

    public class HandHeldDioramaAction : CombatAction
    {
        public IUnit caster;
        public string ability;
        public ManaColorSO[] usedCost;
        public HandHeldDioramaAction(IUnit unit, string name, ManaColorSO[] cost)
        {
            caster = unit;
            ability = name;
            usedCost = cost;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            if (caster is CharacterCombat chara)
            {
                foreach (CombatAbility abil in chara.CombatAbilities)
                {
                    if (abil.ability.name == ability)
                    {
                        caster.ShowItem();
                        abil.cost = usedCost;
                    }
                }
                foreach (CharacterCombatUIInfo value in stats.combatUI._charactersInCombat.Values)
                {
                    if (value.SlotID == chara.SlotID) value.UpdateAttacks(chara.CombatAbilities.ToArray());
                }
                CombatManager.Instance.AddUIAction(new CharacterUpdateAllAttacksUIAction(chara.ID, chara.CombatAbilities.ToArray()));
            }
            yield return null;
        }
    }
}
