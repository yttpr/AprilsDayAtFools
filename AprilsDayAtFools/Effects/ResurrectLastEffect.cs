using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ResurrectLastEffect : EffectSO
    {
        public string PassiveToBlock;
        public bool SelfSlot;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            bool useself = SelfSlot;

            if (SelfSlot && stats.combatSlots.CharacterSlots[caster.SlotID].HasUnit) useself = false;

            List<CharacterCombat> list = stats.GetPossibleResurrectionCharacters();
            if (list.Count <= 0)
            {
                return false;
            }

            List<CharacterCombat> newList = new List<CharacterCombat>();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (PassiveToBlock == "") break;
                if (!list[i].ContainsPassiveAbility(PassiveToBlock)) newList.Add(list[i]);
            }
            if (newList.Count > 0) list = newList;

            CharacterCombat character = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            if (stats.ResurrectDeadCharacter(character, useself ? caster.SlotID : EternalHandler.EmptyCharSlot(stats), entryVariable))
            {
                exitAmount++;
            }

            return exitAmount > 0;
        }

        public static ResurrectLastEffect Create(string passive)
        {
            ResurrectLastEffect ret = ScriptableObject.CreateInstance<ResurrectLastEffect>();
            ret.PassiveToBlock = passive;
            return ret;
        }
    }
}
