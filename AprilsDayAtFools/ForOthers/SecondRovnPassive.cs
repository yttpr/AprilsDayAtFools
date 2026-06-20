using System;
using System.Collections.Generic;
using System.Text;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UI.CanvasScaler;

namespace YourNamespace
{
    public class MimicExtraAbilityPassive : BasePassiveAbilitySO
    {
        public static string StoredValueID => "MimicExtraAbility_PA";

        public override bool DoesPassiveTrigger => true;
        public override bool IsPassiveImmediate => false;


        public override void TriggerPassive(object sender, object args)
        {
            if (args is AbilityUsageReference used)
            {
                if (!used.m_IsUnitCharacter) return;

                CharacterCombat chara = CombatManager.Instance._stats.TryGetCharacter(used.m_UnitID);
                ExtraAbilityInfo extra = null;

                foreach (CombatAbility ability in chara.CombatAbilities)
                {
                    if (ability.ability == used.m_AbilityUsed)
                    {
                        extra = new ExtraAbilityInfo() { ability = ability.ability, cost = ability.cost, rarity = ability.rarity };

                        break;
                    }
                }

                if (extra != null && sender is IUnit unit)
                {
                    unit.TryGetStoredData(StoredValueID, out UnitStoreDataHolder data);

                    if (data.m_ObjectData is ExtraAbilityInfo old)
                    {
                        if (unit is CharacterCombat self) self.TryRemoveSingleExtraAbility(old);
                        else unit.TryRemoveExtraAbility(old);
                    }

                    unit.AddExtraAbility(extra);

                    data.m_ObjectData = extra;
                }
            }
        }

        public override void OnPassiveConnected(IUnit unit)
        {
        }
        public override void OnPassiveDisconnected(IUnit unit)
        {
        }
    }
    public class OnlyTriggerOnPartyMemberAbilityUsageCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            return args is AbilityUsageReference reference && reference.m_IsUnitCharacter;
        }
    }
    public static class ExtraAbilityExtension
    {

        public static void TryRemoveSingleExtraAbility(this CharacterCombat self, ExtraAbilityInfo extra)
        {
            for (int num = self.ExtraAbilities.Count - 1; num >= 0; num--)
            {
                if (!(self.ExtraAbilities[num].ability != extra.ability))
                {
                    self.ExtraAbilities.RemoveAt(num);
                    break;
                }
            }

            for (int num2 = self.CombatAbilities.Count - 1; num2 >= 0; num2--)
            {
                if (!(self.CombatAbilities[num2].ability != extra.ability))
                {
                    self.CombatAbilities.RemoveAt(num2);
                    CombatManager.Instance.AddUIAction(new CharacterRemoveAttackUIAction(self.ID, num2));
                    break;
                }
            }
        }

    }


    public class PerformCurrentMimickedAbilityEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            caster.TryGetStoredData(MimicExtraAbilityPassive.StoredValueID, out UnitStoreDataHolder data, true);

            if (data.m_ObjectData is ExtraAbilityInfo info)
            {
                caster.TryPerformRandomAbility(info.ability);
                return true;
            }

            return false;
        }
    }
    public class AddMimickedAbilityPermenantlyEffect : EffectSO
    {
        public string _ifConsistentReplacement_ID = "";

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            ExtraAbilityInfo extra = null;

            caster.TryGetStoredData(MimicExtraAbilityPassive.StoredValueID, out UnitStoreDataHolder holder);
            if (holder.m_ObjectData is ExtraAbilityInfo) extra = holder.m_ObjectData as ExtraAbilityInfo;

            if (extra != null && _ifConsistentReplacement_ID != "")
            {
                caster.TryGetStoredData(_ifConsistentReplacement_ID, out UnitStoreDataHolder data);

                if (data.m_ObjectData is ExtraAbilityInfo old)
                {
                    if (caster is CharacterCombat self) self.TryRemoveSingleExtraAbility(old);
                    else caster.TryRemoveExtraAbility(old);
                }

                caster.AddExtraAbility(extra);

                data.m_ObjectData = extra;

                return true;
            }
            else if (extra != null)
            {
                caster.AddExtraAbility(extra);
                return true;
            }

            return false;
        }
    }
}
