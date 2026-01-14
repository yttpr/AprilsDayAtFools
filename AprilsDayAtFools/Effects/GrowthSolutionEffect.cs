using BrutalAPI;
using FMOD.Studio;
using System;
using System.Collections.Generic;
using System.Text;
using Tools;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class GrowthSolutionEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            CharacterCombat check = null;

            foreach (TargetSlotInfo target in Targeting.Slot_AllyRight.GetTargets(stats.combatSlots, caster.SlotID, caster.IsUnitCharacter))
            {
                if (target.HasUnit) return false;
            }
            foreach (TargetSlotInfo target in Targeting.Slot_AllyLeft.GetTargets(stats.combatSlots, caster.SlotID, caster.IsUnitCharacter))
            {
                if (target.HasUnit && target.Unit is CharacterCombat chara)
                    check = chara;
            }

            if (check == null) return false;

            CombatManager.Instance.AddUIAction(new PlayAbilityAnimationAction(Visuals.Excommunicate, Slots.Sides, caster));

            CharacterSO character = check.Character;
            int rank = 0;
            WearableStaticModifiers modifiers = new WearableStaticModifiers();
            WearableStaticModifierSetterSO[] extraModifiers = [];
            for (int i = 0; i < extraModifiers.Length; i++)
            {
                extraModifiers[i].OnAttachedToCharacter(modifiers, character, rank);
            }

            string nameAdditionData = LocUtils.GameLoc.GetNameAdditionData(NameAdditionLocID.NameAdditionNone);

            int currentHealth = 1;
            CombatManager.Instance.AddSubAction(new SpawnCharacterAction(character, caster.SlotID + 1, trySpawnAnyways: false, nameAdditionData, true, rank, check.UsedAbilities, currentHealth, modifiers));

            exitAmount = entryVariable;
            return true;
        }
    }
}
