using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class RemoveTargetFirstAbilitiesEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    if (target.Unit is CharacterCombat chara)
                    {
                        for (int i = 0; i < entryVariable; i++)
                        {
                            if (chara.CombatAbilities.Count <= 0) continue;

                            AbilitySO abil = chara.CombatAbilities[0].ability;
                            chara.CombatAbilities.RemoveAt(0);

                            CombatManager.Instance.AddUIAction(new DestroyAbilityUIAction(chara.ID, chara.IsUnitCharacter, abil._abilityName, abil.abilitySprite));

                            exitAmount++;
                        }

                        CombatManager.Instance.AddUIAction(new CharacterUpdateAllAttacksUIAction(chara.ID, chara.CombatAbilities.ToArray()));
                    }//chara
                    else if (target.Unit is EnemyCombat enemy)
                    {
                        //remove abilities
                        for (int i = 0; i < entryVariable; i++)
                        {
                            if (enemy.Abilities.Count <= 0) continue;

                            AbilitySO abil = enemy.Abilities[0].ability;
                            enemy.Abilities.RemoveAt(0);

                            CombatManager.Instance.AddUIAction(new DestroyAbilityUIAction(enemy.ID, enemy.IsUnitCharacter, abil._abilityName, abil.abilitySprite));

                            exitAmount++;
                        }

                        //update timeline ids
                        List<int> turns_removed = [];

                        for (int i = stats.timeline.Round.Count - 1; i >= stats.timeline.CurrentTurn; i--)
                        {
                            if (stats.timeline.Round[i].isPlayer) continue;

                            if (stats.timeline.Round[i].turnUnit == enemy)
                            {
                                if (stats.timeline.Round[i].abilitySlot < entryVariable)
                                {
                                    stats.timeline.Round.RemoveAt(i);
                                    enemy.TurnsInTimeline--;
                                    turns_removed.Add(i);
                                }
                                else
                                {
                                    TurnInfo turn = stats.timeline.Round[i];
                                    turn.abilitySlot -= entryVariable;
                                    stats.timeline.Round[i] = turn;

                                    TimelineInfo timeline = stats.combatUI._TimelineHandler.TimelineSlotInfo[i];
                                    timeline.abilitySlotID = turn.abilitySlot;
                                    stats.combatUI._TimelineHandler.TimelineSlotInfo[i] = timeline;
                                }
                            }
                        }

                        CombatManager.Instance.AddUIAction(new RemoveSlotTimelineUIAction(turns_removed.ToArray()));
                        CombatManager.Instance.AddUIAction(new UpdateTimelinePointerUIAction(stats.timeline.CurrentTurn));

                        CombatManager.Instance.AddUIAction(new EnemyUpdateAllAttacksUIAction(enemy.ID, enemy.Abilities.ToArray()));
                    }//enemy
                }
            }

            return exitAmount > 0;
        }
    }


    public class DestroyAbilityUIAction : CombatAction
    {
        public int _id;

        public string _itemName;

        public bool _isChara;

        public Sprite _itemSprite;

        public DestroyAbilityUIAction(int id, bool isChara, string name, Sprite sprite = null)
        {
            _id = id;
            _isChara = isChara;
            _itemName = name;
            _itemSprite = sprite;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            yield return stats.combatUI.ShowPassiveInformation(_id, _isChara, _itemName + " Removed", stats.audioController.itemConsumed, _itemSprite);
        }
    }
}
