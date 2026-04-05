using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class InstantlySpawnEnemyInSlot : EffectSO
    {
        public string enemyName;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            EnemySO enemy = LoadedAssetsHandler.GetEnemy(enemyName);
            if (enemy == null || enemy.Equals(null)) return false;

            foreach (TargetSlotInfo target in targets)
            {
                int num = stats.combatSlots.GetEnemyFitSlot(target.SlotID, enemy.size);

                if (num != -1)
                {
                    if (stats.AddNewEnemy(enemy, num, false, "Spawn_Basic", enemy.health)) exitAmount++;
                }
            }

            return exitAmount > 0;
        }
    }
}
