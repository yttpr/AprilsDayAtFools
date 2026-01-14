using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class SetCasterExtraSpritesRandomUpToEntryEffect : EffectSO
    {
        [SerializeField]
        public string _spriteType;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (caster.IsUnitCharacter)
            {
                int first = UnityEngine.Random.Range(1, entryVariable + 1);
                for (int i = 0; i < first; i++) CombatManager.Instance.AddUIAction(new CharacterSetExtraSpriteUIAction(caster.ID, _spriteType));
                return true;
            }

            return false;
        }
    }
}
