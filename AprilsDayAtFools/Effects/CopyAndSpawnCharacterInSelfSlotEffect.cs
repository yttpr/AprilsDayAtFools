using BrutalAPI;
using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CopyAndSpawnPermenantCharacterExhaustedInSelfSlotEffect : EffectSO
    {
        public WearableStaticModifierSetterSO[] _extraModifiers;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (caster is CharacterCombat chara)
            {
                WearableStaticModifiers modifiers = new WearableStaticModifiers();
                WearableStaticModifierSetterSO[] extraModifiers = _extraModifiers;
                if (extraModifiers == null) extraModifiers = [];
                for (int i = 0; i < extraModifiers.Length; i++)
                {
                    extraModifiers[i].OnAttachedToCharacter(modifiers, chara.Character, chara.Rank);
                }
                CombatManager.Instance.AddSubAction(new SpawnExhaustedCharacterAction(chara.Character, chara.SlotID, trySpawnAnyways: true, "", true, chara.Rank, chara.UsedAbilities, chara.Character.GetMaxHealth(chara.Rank), modifiers));
                exitAmount++;
            }
            return exitAmount > 0;
        }
    }
    public class SpawnExhaustedCharacterAction : CombatAction
    {
        public int _preferredSlot;

        public CharacterSO _character;

        public bool _trySpawnAnyways;

        public string _nameAddition;

        public bool _permanentSpawn;

        public int _rank;

        public int[] _usedAbilities;

        public int _currentHealth;

        public WearableStaticModifiers _modifiers;

        public SpawnExhaustedCharacterAction(CharacterSO character, int preferredSlot, bool trySpawnAnyways, string nameAddition, bool permanentSpawn, int rank, int[] usedAbilities, int currentHealth, WearableStaticModifiers modifiers = null)
        {
            _preferredSlot = preferredSlot;
            _character = character;
            _trySpawnAnyways = trySpawnAnyways;
            _nameAddition = nameAddition;
            _permanentSpawn = permanentSpawn;
            _rank = rank;
            _usedAbilities = usedAbilities;
            _currentHealth = currentHealth;
            _modifiers = modifiers;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            int num;
            if (_preferredSlot >= 0)
            {
                num = stats.combatSlots.GetCharacterFitSlot(_preferredSlot, 1);
                if (num == -1 && _trySpawnAnyways)
                {
                    num = stats.GetRandomCharacterSlot();
                }
            }
            else
            {
                num = stats.GetRandomCharacterSlot();
            }

            if (num != -1)
            {
                stats.AddNewExhaustedCharacter(_character, num, _nameAddition, _permanentSpawn, _rank, _usedAbilities, _currentHealth, _modifiers);
            }

            yield return null;
        }
    }
    public static class SpawnExhaustedCharacterHandler
    {
        public static bool AddNewExhaustedCharacter(this CombatStats self, CharacterSO character, int slot, string nameAddition, bool permanent, int rank, int[] usedAbilities, int currentHealth, WearableStaticModifiers modifiers)
        {
            int firstEmptyCharacterFieldID = self.GetFirstEmptyCharacterFieldID();
            if (firstEmptyCharacterFieldID == -1)
            {
                return false;
            }

            if (modifiers == null)
            {
                modifiers = new WearableStaticModifiers();
            }

            int count = self.Characters.Count;
            CharacterCombat characterCombat = new CharacterCombat(count, firstEmptyCharacterFieldID, character, isMainCharacter: false, rank, usedAbilities, currentHealth, null, modifiers, self.IsPlayerTurn, nameAddition, permanent);
            characterCombat._canUseAbilities = false;
            characterCombat._canSwap = false;
            self.Characters.Add(count, characterCombat);
            self.AddCharacterToField(count, firstEmptyCharacterFieldID);
            self.combatSlots.AddCharacterToSlot(characterCombat, slot);
            CombatManager.Instance.AddUIAction(new CharacterSpawnUIAction(characterCombat.ID));
            characterCombat.ConnectPassives();
            CombatManager.Instance.ProcessImmediateAction(new TriggerUnitGeneralEventAction(TriggerCalls.OnAllyHasSpawned.ToString(), self, ignoChars: false, ignoEnems: true, characterCombat));
            CombatManager.Instance.ProcessImmediateAction(new TriggerUnitGeneralEventAction(TriggerCalls.OnOpponentHasSpawned.ToString(), self, ignoChars: true));
            return true;
        }
    }
}
