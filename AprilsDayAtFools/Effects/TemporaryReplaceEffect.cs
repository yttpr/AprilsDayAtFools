using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class TemporaryReplaceBoxer : UnboxUnitHandlerSO
    {
        public static string Temporary => "ADAF_TemporaryAlly";
        public static string Late => "ADAF_Unboxing_SecondWave";
        public static string AllowedFlee => "ADAF_Temp_AllowedFled";
        public override bool CanBeUnboxed(CombatStats stats, BoxedUnit unit, object senderData)
        {
            if (unit.unit.SimpleGetStoredValue(Late) > 0)
            {
                unit.unit.SimpleSetStoredValue(Late, 0);
                return unit.unit.SimpleGetStoredValue(Temporary) <= 0;
            }
            //Debug.Log("canbeunboxed:" + unit.unit.Name + " has depiction: " + unit.unit.ContainsPassiveAbility(IDs.Depiction).ToString());
            if (!Replacements.ContainsKey(unit.unit))
            {
                unit.unit.SimpleSetStoredValue(Late, 1);
                return false;
            }

            IUnit replace = Replacements[unit.unit]; 
            
            for (int i = 0; i < Replacements.Count && replace != null && Replacements.ContainsKey(replace); i++)
            {
                IUnit oldkey = replace;
                replace = Replacements[oldkey];
            }

            if (replace == null || !replace.IsAlive || (replace.HasFled && replace.SimpleGetStoredValue(AllowedFlee) <= 0))
            {
                //Debug.Log(unit.unit.Name + " has null/fled/dead replacement");
                if (stats.combatSlots.CharacterSlots[replace.SlotID].HasUnit)
                {
                    unit.unit.SimpleSetStoredValue(Late, 1);
                    return false;
                }
            }

            //Debug.Log(unit.unit.Name + " can return");
            return unit.unit.SimpleGetStoredValue(Temporary) <= 0;
        }
        public static Dictionary<IUnit, IUnit> Replacements;
        public override void ProcessUnbox(CombatStats stats, BoxedUnit unit, object senderData)
        {
            Setup();
            //Debug.Log("begin unbox for: " + unit.unit.Name);
            if (Replacements.ContainsKey(unit.unit) && unit.unit is CharacterCombat chara)
            {
                //Debug.Log("was replaced yes. this should always trigger.");
                IUnit replace = Replacements[chara];

                for (int i = 0; i < Replacements.Count && Replacements.ContainsKey(replace); i++)
                {
                    IUnit oldkey = replace;
                    replace = Replacements[oldkey];
                    Replacements.Remove(oldkey);
                    //Debug.Log("went through one replaced replacement cycle.");
                }

                int slot = replace.SlotID;
                if (!replace.IsAlive || (replace.HasFled && replace.SimpleGetStoredValue(AllowedFlee) <= 0)) slot = GetRandomCharacterSlot();

                if (slot >= 0 && !stats.combatSlots.CharacterSlots[slot].HasUnit)
                {
                    //Debug.Log("is setting new slot id. should also always trigger.");
                    stats.combatSlots.RemoveCharacterFromSlot(chara);
                    stats.combatSlots.AddCharacterToSlot(chara, slot);
                }

                Replacements[unit.unit] = null;
            }
            //Debug.Log("slot position unboxing into: " + unit.unit.SlotID.ToString());
            
            base.ProcessUnbox(stats, unit, senderData);
        }
        public static int GetRandomCharacterSlot()
        {
            CombatStats stats = CombatManager.Instance._stats;
            int num = -1;
            List<int> list = new List<int>();
            for (int i = 0; i < stats.combatSlots.CharacterSlots.Length; i++)
            {
                list.Add(i);
            }

            while (list.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, list.Count);
                int preferredSlot = list[index];
                list.RemoveAt(index);
                num = stats.combatSlots.GetCharacterFitSlot(preferredSlot, 1);
                if (num != -1)
                {
                    bool allow = true;
                    foreach (IUnit replacer in Replacements.Values)
                    {
                        if (replacer.IsAlive && !(replacer.HasFled && replacer.SimpleGetStoredValue(AllowedFlee) <= 0) && replacer.SlotID == preferredSlot)
                        {
                            allow = false;
                            break;
                        }
                    }
                    if (!allow) continue;

                    break;
                }
            }

            return num;
        }
        public static void Setup()
        {
            if (Replacements == null) Replacements = new Dictionary<IUnit, IUnit>();
            Clean();
        }
        public static void Clean()
        {
            foreach (IUnit key in new List<IUnit>(Replacements.Keys))
            {
                if (Replacements[key] == null)
                    Replacements.Remove(key);
            }
        }
        public static void SetReplacement(IUnit target, IUnit replacement)
        {
            Setup();

            //if (replacement.ContainsPassiveAbility(IDs.Depiction) && target.ContainsPassiveAbility(IDs.Depiction) && Replacements.Values.Contains(target))

            Replacements[target] = replacement;
            replacement.SimpleSetStoredValue(Temporary, 1);
        }
        public static TemporaryReplaceBoxer Default;
        public static CannotUnboxHandler Empty;
        public static TemporaryReplaceBoxer GetDefault()
        {
            Setup();

            if (Default == null)
            {
                Default = ScriptableObject.CreateInstance<TemporaryReplaceBoxer>();
                Default._unboxConditions = [TriggerCalls.TimelineEndReached, TimelineEndHandler.Second];
            }

            return Default;
        }
        public static CannotUnboxHandler GetEmpty()
        {
            if (Empty == null)
            {
                Empty = ScriptableObject.CreateInstance<CannotUnboxHandler>();
                Empty._unboxConditions = [TriggerCalls.Count];
            }

            return Empty;
        }
    }
    public class CannotUnboxHandler : UnboxUnitHandlerSO
    {
        public override bool CanBeUnboxed(CombatStats stats, BoxedUnit unit, object senderData)
        {
            return false;
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
                        IUnit unit = target.Unit;

                        UnboxUnitHandlerSO handler = TemporaryReplaceBoxer.GetDefault();
                        //if (unit.ContainsPassiveAbility(IDs.Depiction)) handler = TemporaryReplaceBoxer.GetEmpty();

                        if (stats.TryBoxCharacter(unit.ID, handler, CombatType_GameIDs.Exit_Fleeting.ToString()))
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
                            CombatManager.Instance.AddSubAction(new ReplacementSpawnCharacterAction(unit, character, unit.SlotID, trySpawnAnyways: true, nameAdditionData, false, entryVariable, usedAbilities, currentHealth, modifiers));

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
                //randoms.Remove(ret);
                if (ret == null || ret.Equals(null)) ret = randoms.GetRandom();
                else if (!ret.HasRankedData || ret.rankedData.Count < rank) ret = randoms.GetRandom();
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
                    if (stats.AddNewCharacter(_character, num, _nameAddition, _permanentSpawn, _rank, _usedAbilities, _currentHealth, _modifiers))
                    {
                        TemporaryReplaceBoxer.SetReplacement(Origin, stats.Characters[stats.Characters.Count - 1]);
                    }
                }

                yield return null;
            }
        }
    }
}
