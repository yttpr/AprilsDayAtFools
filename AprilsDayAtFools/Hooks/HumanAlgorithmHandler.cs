using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AprilsDayAtFools
{
    public static class HumanAlgorithmHandler
    {
        public static TriggerCalls Trigger => (TriggerCalls)13490332;


        public static void Setup()
        {
            foreach (MethodInfo method in typeof(CharacterCombat).GetMethods())
            {
                if (method.Name == nameof(CharacterCombat.TryPerformRandomAbility))
                {
                    if (method.GetParameters().Length == 0)
                    {
                        IDetour hook1 = new Hook(method, typeof(HumanAlgorithmHandler).GetMethod(nameof(Character_UseAbil_Random), ~BindingFlags.Default));
                    }
                    else
                    {
                        IDetour hook1 = new Hook(method, typeof(HumanAlgorithmHandler).GetMethod(nameof(Character_UseAbil_Given), ~BindingFlags.Default));
                    }
                }
            }


            
        }

        public static bool Character_UseAbil_Random(Func<CharacterCombat, bool> orig, CharacterCombat self)
        {
            bool ret = orig(self);
            CombatManager.Instance.AddSubAction(new ForceUsedAbilityAction(self));
            return ret;
        }
        public static bool Character_UseAbil_Given(Func<CharacterCombat, AbilitySO, bool> orig, CharacterCombat self, AbilitySO abil)
        {
            bool ret = orig(self, abil);
            CombatManager.Instance.AddSubAction(new ForceUsedAbilityAction(self));
            return ret;
        }


    }

    public class ForceUsedAbilityAction : CombatAction
    {
        public IUnit unit;
        public ForceUsedAbilityAction(IUnit unit)
        {
            this.unit = unit;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            CombatManager.Instance.PostNotification(HumanAlgorithmHandler.Trigger.ToString(), unit, null);
            yield break;
        }
    }
}
