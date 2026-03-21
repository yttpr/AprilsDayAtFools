using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class FixCasterTimelineIntentsEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (caster.IsUnitCharacter || !caster.IsAlive) return false;
            if (stats.timeline.IsConfused) return false;
            CombatManager.Instance.AddUIAction(new FixCasterTImelineIntentsUIAction(caster));
            return true;
        }
    }
    public class FixCasterTImelineIntentsUIAction : CombatAction
    {
        public IUnit caster;
        public FixCasterTImelineIntentsUIAction(IUnit _caster)
        {
            caster = _caster;
        }
        public override IEnumerator Execute(CombatStats stats)
        {
            for (int i = 0; i < stats.combatUI._TimelineHandler.TimelineSlotInfo.Count; i++)
            {
                TimelineInfo timeline = stats.combatUI._TimelineHandler.TimelineSlotInfo[i];
                if (timeline.isSecret) continue;
                if (timeline.enemyID == caster.ID)
                {
                    timeline.timelineIcon = (caster as EnemyCombat).Enemy.enemySprite;
                    foreach (TimelineSlotGroup slotgroup in stats.combatUI._timeline._slotsInUse)
                    {
                        if (slotgroup.slot.TimelineSlotID == i)
                        {
                            if (slotgroup.slot.TimelineSlotID <= stats.timeline.CurrentTurn) break;
                            AbilitySO newAbil = timeline.ability;
                            Debug.Log(i);
                            Debug.Log(timeline.ability.name);
                            if ((caster as EnemyCombat).AbilityCount > timeline.abilitySlotID) newAbil = (caster as EnemyCombat).Abilities[timeline.abilitySlotID].ability;
                            timeline.ability = newAbil;
                            Debug.Log(timeline.ability.name);
                            Sprite[] intents = null;
                            Color[] spriteColors = null;
                            bool cansee = timeline.timelineIcon != null && !timeline.timelineIcon.Equals(null);
                            if (cansee) intents = stats.combatUI._timeline.IntentHandler.GenerateSpritesFromAbility(timeline.ability, casterIsCharacter: false, out spriteColors);
                            slotgroup.SetInformation(slotgroup.slot.TimelineSlotID, cansee ? timeline.timelineIcon : stats.combatUI._timeline._blindTimelineIcon, true, intents, spriteColors);
                        }
                    }
                }
            }
            yield return null;
        }

    }
}
