using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class ClownKiller
    {
        public static void Setup() => NotificationHook.AddAction(NotifCheck);
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnWillApplyDamage.ToString() && sender is CharacterCombat chara && args is DamageDealtValueChangeException value)
            {
                if (chara.Character.name == "RabidDog_CH")
                {
                    if (value.damagedUnit is EnemyCombat enemy && (enemy.Enemy.name == "Waltz_EN" || enemy.Enemy.name == "Clown_EN"))
                    {
                        value.AddModifier(new MultiplyIntValueModifier(true, 3));
                    }
                    else if (value.damagedUnit.Name.ToLower().Contains("clown"))
                    {
                        value.AddModifier(new MultiplyIntValueModifier(true, 3));
                    }
                }
            }
        }
    }
}
