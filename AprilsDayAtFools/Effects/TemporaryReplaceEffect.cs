using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Tools;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class TemporaryReplaceBoxer : UnboxUnitHandlerSO
    {
        public override bool CanBeUnboxed(CombatStats stats, BoxedUnit unit, object senderData)
        {
            return true;
        }
        public static Dictionary<IUnit, IUnit> Replacements;
        public override void ProcessUnbox(CombatStats stats, BoxedUnit unit, object senderData)
        {
            Setup();
            if (!Replacements.ContainsKey(unit.unit) && unit.unit is CharacterCombat chara && !stats.combatSlots.CharacterSlots[Replacements[chara].SlotID].HasUnit)
            {
                stats.combatSlots.RemoveCharacterFromSlot(chara);
                stats.combatSlots.AddCharacterToSlot(chara, Replacements[chara].SlotID);
            }
            
            base.ProcessUnbox(stats, unit, senderData);
        }
        public static void Setup()
        {
            if (Replacements == null) Replacements = new Dictionary<IUnit, IUnit>();
        }
        public static void SetReplacement(IUnit target, IUnit replacement)
        {
            Setup();
            Replacements[target] = replacement;
        }
        public static TemporaryReplaceBoxer Default;
        public static TemporaryReplaceBoxer GetDefault()
        {
            Setup();

            if (Default == null)
            {
                Default = ScriptableObject.CreateInstance<TemporaryReplaceBoxer>();
                Default._unboxConditions = [TriggerCalls.TimelineEndReached];
            }

            return Default;
        }
    }

    public class TemporaryReplacementEffect : EffectSO
    {
        public bool OnlyUseAbilities;
        public WearableStaticModifierSetterSO[] _extraModifiers = [];
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach (TargetSlotInfo target in targets)
            {
                if (target.IsTargetCharacterSlot && target.HasUnit) 
                {
                    if (!OnlyUseAbilities || (target.Unit as CharacterCombat).CanUseAbilitiesNoTrigger)
                    {
                        if (stats.TryBoxCharacter(target.Unit.ID, TemporaryReplaceBoxer.GetDefault(), CombatType_GameIDs.Exit_Fleeting.ToString()))
                        {
                            CharacterSO character = getRandom(entryVariable);
                            int currentHealth = character.GetMaxHealth(entryVariable);
                            int[] usedAbilities = character.GenerateAbilities();
                            WearableStaticModifiers modifiers = new WearableStaticModifiers();
                            WearableStaticModifierSetterSO[] extraModifiers = _extraModifiers;
                            for (int i = 0; i < extraModifiers.Length; i++)
                            {
                                extraModifiers[i].OnAttachedToCharacter(modifiers, character, entryVariable);
                            }

                            string nameAdditionData = "Depiction of {0}";
                            CombatManager.Instance.AddSubAction(new ReplacementSpawnCharacterAction(target.Unit, character, target.Unit.SlotID, trySpawnAnyways: true, nameAdditionData, false, entryVariable, usedAbilities, currentHealth, modifiers));

                            exitAmount++;
                        }
                    }
                }
            }

            return exitAmount > 0;
        }

        public static CharacterSO getRandom(int rank = -1)
        {
            List<CharacterSO> randoms = [..LoadedAssetsHandler.LoadedCharacters.Values];

            CharacterSO ret = randoms.GetRandom();
            for (int i = 0; i < 144; i++)
            {
                if (ret == null || ret.Equals(null)) ret = randoms.GetRandom();
                else if (!ret.HasRankedData || ret.rankedData.Count < rank) randoms.GetRandom();
                else break;
            }
            return ret;
        }

        public class ReplacementSpawnCharacterAction : SpawnCharacterAction
        {
            public IUnit Origin;
            public ReplacementSpawnCharacterAction(IUnit origin, CharacterSO character, int preferredSlot, bool trySpawnAnyways, string nameAddition, bool permanentSpawn, int rank, int[] usedAbilities, int currentHealth, WearableStaticModifiers modifiers = null) : base(character, preferredSlot, trySpawnAnyways, nameAddition, permanentSpawn, rank, usedAbilities, currentHealth, modifiers)
            {
                Origin = origin;
            }

            public override IEnumerator Execute(CombatStats stats)
            {
                IEnumerator ret = base.Execute(stats);
                TemporaryReplaceBoxer.SetReplacement(Origin, stats.Characters[stats.Characters.Count - 1]);
                return ret;
            }
        }
    }
}
