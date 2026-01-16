using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Tools;
using UnityEngine;

namespace AprilsDayAtFools
{

    public class ForceFirstTimelineActionEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            for (int i = stats.timeline.CurrentTurn; i < stats.timeline.Round.Count; i++)
            {
                if (stats.timeline.Round[i].isPlayer) continue;

                if (stats.timeline.Round[i].turnUnit is EnemyCombat enemy)
                {
                    enemy.SimpleSetStoredValue(IDs.ForcedTurn, 1);
                    EnemyPerformAbility(enemy, stats.timeline.Round[i].abilitySlot);

                    stats.timeline.Round.RemoveAt(i);
                    enemy.TurnsInTimeline--;

                    CombatManager.Instance.AddUIAction(new RemoveSlotTimelineUIAction([i]));
                    CombatManager.Instance.AddUIAction(new UpdateTimelinePointerUIAction(stats.timeline.CurrentTurn));

                    exitAmount++;

                    break;
                }
            }

            return exitAmount > 0;
        }
        public void EnemyPerformAbility(EnemyCombat self, int abilitySlot)
        {
            if (abilitySlot < 0 || abilitySlot >= self.Abilities.Count)
            {
                Debug.LogError(self.Name + " cannot use ability in slot " + abilitySlot + ", it does not exist");
                CombatManager.Instance.AddRootAction(new EnemyTurnEndAction(self.ID));
                return;
            }

            AbilitySO ability = self.Abilities[abilitySlot].ability;
            if (!self.CanUseAbility)
            {
                Debug.LogError(self.Name + " cannot use " + ability.GetAbilityLocData().text + " probably due to stunned");
                CombatManager.Instance.AddRootAction(new EnemyTurnEndAction(self.ID));
                return;
            }

            if (self.ContainsStatusEffect("Muted_ID"))
            {
                Debug.Log("is muted");
                if (!ability._abilityName.ToLower().Contains("slap"))
                {
                    Debug.Log("used not slap");
                    ability = LoadedAssetsHandler.GetCharacterAbility("Slap_A");

                    Vector3 loc = default(Vector3);
                    CombatStats stats = CombatManager.Instance._stats;
                    try
                    {
                        if (!self.IsUnitCharacter)
                        {
                            loc = stats.combatUI._enemyZone._enemies[self.FieldID].FieldEntity.Position;
                        }
                    }
                    catch { }

                    CombatManager.Instance.AddRootAction(new UIActionAction(new PlaySoundUIAction("event:/Hawthorne/Boowomp", loc)));
                }
            }

            StringReference args = new StringReference(ability.name);
            CombatManager.Instance.PostNotification(TriggerCalls.OnAbilityWillBeUsed.ToString(), self, args);

            if (!DebugUtils.videoMode)
            {
                CombatManager.Instance.AddRootAction(new UIActionAction(new ShowAttackInformationUIAction(self.ID, self.IsUnitCharacter, ability.GetAbilityLocData().text)));
            }
            CombatManager.Instance.AddRootAction(new PlayAbilityAnimationAction(ability.visuals, ability.animationTarget, self));
            CombatManager.Instance.AddRootAction(new EffectAction(ability.effects, self));
            CombatManager.Instance.AddRootAction(new CustomEndAbilityAction(self.ID, self.IsUnitCharacter, ability));
            CombatManager.Instance.AddRootAction(new ForceTurnCleanupAction(self));
        }
    }
}
