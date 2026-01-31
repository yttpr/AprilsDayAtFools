using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ImmediateFleeEffect : FleeTargetEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    exitAmount++;
                    targetSlotInfo.Unit.UnitWillFlee();
                    IUnit unit = targetSlotInfo.Unit;
                    if (unit.IsUnitCharacter)
                    {
                        CharacterCombat characterCombat = stats.TryGetCharacterOnField(unit.ID);
                        if (characterCombat != null)
                        {
                            characterCombat.FleeCharacter();
                            characterCombat.DisconnectPassives();
                            characterCombat.RemoveAllStatusEffects(showInfo: false);
                            characterCombat.RemoveAndDisconnectAllPassiveAbilities(disconnectPassives: false);
                            characterCombat.DettachWearable();
                            CombatManager.Instance.AddUIAction(new CharacterFleetingUIAction(characterCombat.ID));
                            stats.RemoveCharacter(unit.ID);
                            characterCombat.FinalizeFleeting();
                        }
                    }
                    else
                    {
                        EnemyCombat enemyCombat = stats.TryGetEnemyOnField(unit.ID);
                        if (enemyCombat != null)
                        {
                            enemyCombat.FleeEnemy();
                            enemyCombat.DisconnectPassives();
                            enemyCombat.RemoveAllStatusEffects(showInfo: false);
                            enemyCombat.FinalizationEnd(disconnectPassives: false);
                            CombatManager.Instance.AddUIAction(new EnemyFleetingUIAction(enemyCombat.ID));
                            stats.RemoveEnemy(unit.ID);
                            enemyCombat.FinalizeFleeting();
                        }
                    }
                }
            }

            return exitAmount > 0;
        }
    }
}
