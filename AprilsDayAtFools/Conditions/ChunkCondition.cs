using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class ChunkCondition : EffectorConditionSO
    {
        public EffectSO Show;
        public EffectSO Damage;
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is IntegerReference reference && effector is IUnit unit)
            {
                CombatManager.Instance.AddSubAction(new EffectAction([
                    Effects.GenerateEffect(Show),
                    Effects.GenerateEffect(Damage, reference.value, Slots.Sides),
                    ], unit));
            }
            return true;
        }
        public static ChunkCondition Create()
        {
            ChunkCondition ret = ScriptableObject.CreateInstance<ChunkCondition>();
            ret.Show = ScriptableObject.CreateInstance<ChunkEffect>();
            ret.Damage = BasicEffects.Indirect;
            return ret;
        }
    }
    public class ChunkEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(caster.ID, caster.IsUnitCharacter, "Chunk", ResourceLoader.LoadSprite("ChunkPassive.png")));
            exitAmount = 0;
            return true;
        }
    }
}
