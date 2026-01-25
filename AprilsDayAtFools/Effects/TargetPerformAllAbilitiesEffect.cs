using BrutalAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class TargetPerformAllAbilitiesEffect : EffectSO
    {
        public static void CharaUseAbility(CharacterCombat self, int abilityID)
        {
            CombatManager.Instance.AddSubAction(new CharaUseAbilityByIndexAction(self, abilityID));
        }
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            List<EnemyCombat> enems = [];
            List<int> enemABs = [];

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    if (target.Unit is CharacterCombat chara)
                    {
                        for (int i = 0; i < chara.AbilityCount; i++)
                        {
                            CharaUseAbility(chara, i);
                        }
                    }
                    else if (target.Unit is EnemyCombat enemy)
                    {
                        for (int i = 0; i < enemy.AbilityCount; i++)
                        {
                            enems.Add(enemy);
                            enemABs.Add(i);
                            exitAmount++;
                        }
                    }
                }
            }

            stats.timeline.AddExtraEnemyTurns(enems, enemABs);

            return exitAmount > 0;
        }
    }

    public class CharaUseAbilityByIndexAction : CombatAction
    {
        public CharacterCombat self;
        public int index;

        public CharaUseAbilityByIndexAction(CharacterCombat chara, int index)
        {
            this.self = chara;
            this.index = index;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            AbilitySO ability = self.CombatAbilities[index].ability;
            StringReference args = new StringReference(ability.name);
            CombatManager.Instance.PostNotification(TriggerCalls.OnAbilityWillBeUsed.ToString(), self, args);

            CombatManager.Instance.AddUIAction(new ShowAttackInformationUIAction(self.ID, self.IsUnitCharacter, ability.GetAbilityLocData().text));
            CombatManager.Instance.AddUIAction(new PlayAbilityAnimBySlotsAction(ability.visuals, ability.animationTarget.GetTargets(stats.combatSlots, self.SlotID, self.IsUnitCharacter), ability.animationTarget.AreTargetSlots, self));
            CombatManager.Instance.ProcessImmediateAction(new ImmediateEffectAction(ability.effects, self));

            CombatManager.Instance.AddUIAction(new EndAbilityAction(new AbilityUsageReference(self.ID, self.IsUnitCharacter, ability)));

            self.SetVolatileUpdateUIAction();
            yield return null;
        }
    }
    public class PlayAbilityAnimBySlotsAction : CombatAction
    {
        public TargetSlotInfo[] _targetting;
        public bool _targetSlots;

        public AttackVisualsSO _visuals;

        public IUnit _caster;

        public PlayAbilityAnimBySlotsAction(AttackVisualsSO visuals, TargetSlotInfo[] targetting, bool targetSlots, IUnit caster)
        {
            _visuals = visuals;
            _targetting = targetting;
            _caster = caster;
            _targetSlots = targetSlots;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            yield return stats.combatUI.PlayAbilityAnimation(_visuals, _targetting, _targetSlots);
        }
    }
}
