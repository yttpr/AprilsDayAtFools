using System;
using System.Collections.Generic;
using System.Text;
using static UnityEngine.GraphicsBuffer;

namespace AprilsDayAtFools
{
    public class SpawnAssistantByPigmentUsedEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            int rank = 0;
            if (caster is CharacterCombat chara) rank = chara.Rank;

            exitAmount = 0;

            foreach (ManaColorSO mana in PigmentUsedCollector.lastUsed)
            {
                EnemySO enemy = LoadedAssetsHandler.GetEnemy(PatchSetup.GetAssistant(mana, rank));

                CombatManager.Instance.AddSubAction(new SpawnEnemyAction(enemy, caster.SlotID, false, true, "Spawn_Basic"));

                exitAmount++;
            }

            return exitAmount > 0;
        }
    }
}
