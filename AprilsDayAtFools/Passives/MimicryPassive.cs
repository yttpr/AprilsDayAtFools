using BrutalAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class MimicryPassive : BasePassiveAbilitySO
    {
        public static TriggerCalls CustomTrigger => (TriggerCalls)19475610;
        public override bool DoesPassiveTrigger => true;
        public override bool IsPassiveImmediate => false;

        public override void TriggerPassive(object sender, object args)
        {
            if (args is StringReference reference)
            {
                AbilitySO ability = null;

                if (sender is CharacterCombat chara)
                {
                    foreach (CombatAbility abil in chara.CombatAbilities)
                    {
                        if (abil.ability.name == reference.value) ability = abil.ability;
                    }
                }
                else if (sender is EnemyCombat enemy)
                {
                    foreach (CombatAbility aby in enemy.Abilities)
                    {
                        if (aby.ability.name == reference.value) ability = aby.ability;
                    }
                }

                if (ability != null && !ability.Equals(null))
                {
                    CombatManager.Instance.AddRootAction(new PostMimicryCallAction(sender as IUnit, ability));
                }
            }
            if (args is AbilitySO ability2)
            {
                TryPerformGivenAbility(sender as IUnit, ability2);
            }
        }
        public static void TryPerformGivenAbility(IUnit unit, AbilitySO selectedAbility)
        {
            CombatManager.Instance.AddSubAction(new ShowAttackInformationUIAction(unit.ID, unit.IsUnitCharacter, selectedAbility.GetAbilityLocData().text));
            CombatManager.Instance.AddSubAction(new PlayAbilityAnimationAction(selectedAbility.visuals, selectedAbility.animationTarget, unit));
            CombatManager.Instance.AddSubAction(new SafetyEffectAction(selectedAbility.effects, unit));
            unit.SetVolatileUpdateUIAction();
        }

        public override void OnPassiveConnected(IUnit unit)
        {
        }
        public override void OnPassiveDisconnected(IUnit unit)
        {
        }
    }

    public class PostMimicryCallAction : CombatAction
    {
        public IUnit origin;
        public AbilitySO ability;
        public PostMimicryCallAction(IUnit origin, AbilitySO ability)
        {
            this.origin = origin;
            this.ability = ability;
        }
        public override IEnumerator Execute(CombatStats stats)
        {
            CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(origin.ID, origin.IsUnitCharacter, "Mimicry", ResourceLoader.LoadSprite("MimicryPassive.png")));
            foreach (CharacterCombat character in CombatManager.Instance._stats.CharactersOnField.Values)
            {
                if (character == origin) continue;
                CombatManager.Instance.PostNotification(MimicryPassive.CustomTrigger.ToString(), character, ability);
            }
            foreach (EnemyCombat enemy in CombatManager.Instance._stats.EnemiesOnField.Values)
            {
                if (enemy == origin) continue;
                CombatManager.Instance.PostNotification(MimicryPassive.CustomTrigger.ToString(), enemy, ability);
            }
            yield return null;
        }
    }
}
