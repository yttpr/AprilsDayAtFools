using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class AddPassiveWithDisplayEffect : EffectSO
    {
        public BasePassiveAbilitySO passive;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            List<int> ids = [];
            List<bool> ischars = [];
            List<string> passives = [];
            List<Sprite> sprites = [];
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit && target.Unit.AddPassiveAbility(passive))
                {
                    exitAmount++;
                    ids.Add(target.Unit.ID);
                    ischars.Add(target.Unit.IsUnitCharacter);
                    passives.Add(passive.GetPassiveLocData().text + " Added");
                    sprites.Add(passive.passiveIcon);
                }
            }

            CombatManager.Instance.AddUIAction(new ShowMultiplePassiveInformationUIAction(ids.ToArray(), ischars.ToArray(), passives.ToArray(), sprites.ToArray()));

            return exitAmount > 0;
        }
    }
}
