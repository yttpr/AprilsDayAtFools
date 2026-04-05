using BrutalAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static UnityEngine.EventSystems.EventTrigger;

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
                    if (stats.AddNewEnemy(enemy, num, false, "Spawn_Basic", enemy.health))
                    {
                        exitAmount++;
                        CombatManager.Instance.AddUIAction(new ClearEnemyStatusUIAction(stats.Enemies[stats.Enemies.Count - 1]));
                    }
                }
            }

            return exitAmount > 0;
        }

        public class ClearEnemyStatusUIAction : CombatAction
        {
            public EnemyCombat Enemy;
            public ClearEnemyStatusUIAction(EnemyCombat enemy)
            {
                Enemy = enemy;
            }
            public override IEnumerator Execute(CombatStats stats)
            {
                if (stats.combatUI._enemiesInCombat.TryGetValue(Enemy.ID, out EnemyCombatUIInfo uiInfo))
                {
                    //dude.... idfk what im doing
                    stats.combatUI._enemyZone._enemies[uiInfo.FieldID].UpdateStatusListLayout([], []);
                }
                yield break;
            }
        }
    }
}
