using BrutalAPI;
using System.Linq;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class RandomStatusEffect : StatusEffect_Apply_Effect
    {
        public bool _usePreviousExit;

        [SerializeField]
        public string[] CanApply = ["Cursed_ID", "Frail_ID", "Ruptured_ID", "DivineProtection_ID", "Focused_ID", "Gutted_ID", "Linked_ID", "OilSlicked_ID", "Scars_ID", "Spotlight_ID", "Stunned_ID", "Remorse_ID", "WildCard_ID", "Salted_ID", "Paranoia_ID", "Anesthetics_ID", "Inverted_ID", "Left_ID", "Pale_ID", "Power_ID", "Determined_ID", "DivineSacrifice_ID", "Favor_ID", "Muted_ID", "Photo_ID", "Dodge_ID", "Salt_Entropy_ID", "Haste_ID", "Acid_ID", "Terror_ID", "Drowning_ID", "Pimples_ID"];
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (CanApply == null || CanApply.Length <= 0) return false;

            if (_usePreviousExit) entryVariable *= PreviousExitValue;

            foreach (TargetSlotInfo target in targets)
            {
                for (int i = 0; i < entryVariable; i++)
                {
                    for (int j = 0; j < 99; j++)
                    {
                        string temp = CanApply.GetRandom();
                        _Status = StatusField.GetCustomStatusEffect(temp);
                        if (_Status != null && !_Status.Equals(null)) break;
                    }

                    int amount = 1;
                    if (_Status.StatusID == "Pale_ID") amount *= 10;

                    if (base.PerformEffect(stats, caster, target.SelfArray(), areTargetSlots, amount, out int exi)) exitAmount += exi;
                }
            }

            return exitAmount > 0;
        }
    }
}
