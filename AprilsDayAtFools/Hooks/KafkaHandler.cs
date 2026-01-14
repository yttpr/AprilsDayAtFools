using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class KafkaHandler
    {
        public static void Setup()
        {
            NotificationHook.AddAction(NotifCheck);
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.CanDie.ToString() && sender is IUnit unit && unit.IsUnitCharacter)
            {
                if (args is BooleanReference reference && !reference.value)
                {
                    CombatManager.Instance.AddUIAction(new CharacterSetExtraSpriteUIAction(unit.ID, IDs.Kafka));
                }
            }
        }
    }
}
