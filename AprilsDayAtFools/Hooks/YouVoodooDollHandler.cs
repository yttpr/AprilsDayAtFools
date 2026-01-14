using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class YouVoodooDollHandler
    {
        public static void Setup()
        {
            NotificationHook.AddAction(NotifCheck);
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnAbilityWillBeUsed.ToString() && sender is CharacterCombat chara && args is StringReference reference)
            {
                if (chara.HasUsableItem && chara.HeldItem.name == "Aprils_YouVoodooDoll_TW") return;

                foreach (CombatAbility ability in chara.CombatAbilities)
                {
                    if (ability.ability.name == reference.value)
                    {
                        CombatManager.Instance.AddSubAction(new RootActionAction(new YouVoodooDollAction(ability.ability)));
                        return;
                    }
                }
            }
        }
    }

    public class YouVoodooDollAction : CombatAction
    {
        public AbilitySO Ability;

        public YouVoodooDollAction (AbilitySO _ability)
        {
            Ability = _ability;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            foreach (CharacterCombat chara in stats.CharactersOnField.Values)
            {
                if (chara.HasUsableItem && chara.HeldItem.name == "Aprils_YouVoodooDoll_TW")
                {
                    chara.ShowItem();
                    TryPerformGivenAbility(chara, Ability);
                }
            }
            yield return null;
        }

        public static void TryPerformGivenAbility(IUnit unit, AbilitySO selectedAbility)
        {
            CombatManager.Instance.AddSubAction(new ShowAttackInformationUIAction(unit.ID, unit.IsUnitCharacter, selectedAbility.GetAbilityLocData().text));
            CombatManager.Instance.AddSubAction(new PlayAbilityAnimationAction(selectedAbility.visuals, selectedAbility.animationTarget, unit));
            CombatManager.Instance.AddSubAction(new SafetyEffectAction(selectedAbility.effects, unit));
            unit.SetVolatileUpdateUIAction();
        }
    }
}
