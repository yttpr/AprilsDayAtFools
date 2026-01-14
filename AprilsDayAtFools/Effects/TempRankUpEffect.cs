using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace AprilsDayAtFools
{
    public class TempRankUpEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit && target.Unit is CharacterCombat chara)
                {
                    if (!chara.Character.HasRankedData) continue;
                    if (chara.Rank + 1 >= chara.Character.rankedData.Count) continue;
                    if (chara.Rank < 0)
                    {
                        Debug.LogWarning("character rank below 0? " + chara._currentName);
                        continue;
                    }

                    CharacterRankedData prev = chara.Character.rankedData[chara.Rank];

                    chara._rank++;

                    CharacterRankedData rank = chara.Character.rankedData[chara.Rank];
                    int difference = rank.health - prev.health;

                    foreach (CombatAbility ability in chara._combatAbilities)
                    {
                        for (int i = 0; i < prev.rankAbilities.Length; i++)
                        {
                            if (ability.ability == prev.rankAbilities[i].ability)
                            {
                                ability.ability = rank.rankAbilities[i].ability;
                                ability.cost = new ManaColorSO[rank.rankAbilities[i].cost.Length];
                                for (int g= 0; g < ability.cost.Length; g++)
                                {
                                    ability.cost[g] = rank.rankAbilities[i].cost[g];
                                }
                                break;
                            }
                        }
                    }

                    CombatManager.Instance.AddUIAction(new CharacterUpdateAllAttacksUIAction(chara.ID, chara.CombatAbilities.ToArray()));

                    CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(chara.ID, chara.IsUnitCharacter, "Leveled Up", chara.Character.characterSprite));

                    if (difference != 0) chara.MaximizeHealth(chara.MaximumHealth + difference);

                    chara.SimpleSetStoredValue(IDs.Level, chara.SimpleGetStoredValue(IDs.Level) + 1);
                    exitAmount++;
                }
            }
            return exitAmount > 0;
        }
    }
}
