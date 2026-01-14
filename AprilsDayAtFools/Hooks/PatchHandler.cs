using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class PatchHandler
    {
        public static void Setup()
        {
            NotificationHook.AddAction(NotifCheck);
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnDamaged.ToString() && sender is CharacterCombat chara)
            {
                if (chara.Character.name == "Patch_CH")
                {
                    CombatManager.Instance.AddUIAction(new ShowTextAction(chara.ID, chara.IsUnitCharacter, "..."));
                }
            }
        }
    }
}
