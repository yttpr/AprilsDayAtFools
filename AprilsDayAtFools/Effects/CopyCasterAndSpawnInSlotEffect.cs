using System;
using System.Collections.Generic;
using System.Text;
using Tools;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class CopyCasterAndSpawnInSlotEffect : EffectSO
    {
        [Header("Rank Data")]
        public int _rank;

        public NameAdditionLocID _nameAddition;

        public bool _permanentSpawn;

        public bool _maximizeHealth;

        public bool _canGetDeadCharacter;

        public bool _rankIsAdditive;

        public WearableStaticModifierSetterSO[] _extraModifiers;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (!caster.IsUnitCharacter)
            {
                return false;
            }

            CharacterCombat characterCombat = stats.TryGetCharacterOnField(caster.ID);
            if (characterCombat == null)
            {
                if (_canGetDeadCharacter)
                {
                    characterCombat = stats.TryGetCharacter(caster.ID);
                }

                if (characterCombat == null)
                {
                    return false;
                }
            }

            CharacterSO character = characterCombat.Character;
            int rank = (_rankIsAdditive ? Mathf.Max(0, _rank + characterCombat.Rank) : _rank);
            WearableStaticModifiers modifiers = new WearableStaticModifiers();
            WearableStaticModifierSetterSO[] extraModifiers = _extraModifiers;
            for (int i = 0; i < extraModifiers.Length; i++)
            {
                extraModifiers[i].OnAttachedToCharacter(modifiers, character, rank);
            }

            string nameAdditionData = LocUtils.GameLoc.GetNameAdditionData(_nameAddition);
            for (int j = 0; j < entryVariable; j++)
            {
                int currentHealth = (_maximizeHealth ? character.GetMaxHealth(rank) : characterCombat.CurrentHealth);
                CombatManager.Instance.AddSubAction(new SpawnCharacterAction(character, caster.SlotID, trySpawnAnyways: true, nameAdditionData, _permanentSpawn, rank, characterCombat.UsedAbilities, currentHealth, modifiers));
            }

            exitAmount = entryVariable;
            return true;
        }
    }
}
