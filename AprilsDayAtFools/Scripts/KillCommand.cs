using BrutalAPI;
using FMODUnity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{

    public static class KillCommand
    {
        public static DebugCommand KILL;
        public static void Add()
        {
            KILL = new DebugCommand("die", "Instantly die.", new List<DebugCommandArgument>(), delegate
            {
                CombatManager.Instance.AddPriorityRootAction(new PerformDelegateAction(delegate (CombatStats x)
                {
                    foreach (CharacterCombat chara in CombatManager.Instance._stats.CharactersOnField.Values)
                    {
                        chara.Damage(999, null, "Basic");
                    }
                }));
                DebugController.Instance.WriteLine("Killing self.");
            });


            DebugController.Commands.children.Add(KILL);
        }

    }
}
