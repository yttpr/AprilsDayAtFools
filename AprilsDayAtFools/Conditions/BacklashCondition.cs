using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{

    public class BacklashCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is IntegerReference reference)
            {
                if (reference.value >= 7)
                {
                    SetCasterExtraSpritesEffect bodybagSprites = ScriptableObject.CreateInstance<SetCasterExtraSpritesEffect>();
                    bodybagSprites._ExtraSpriteID = IDs.Bodybag;
                    CombatManager.Instance.AddSubAction(new EffectAction(new EffectInfo[]
                    {
                        Effects.GenerateEffect(ScriptableObject.CreateInstance<ShowBacklashPassiveEffect>()),
                        Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyShieldSlotEffect>(), reference.value, Targeting.Slot_SelfAll),
                        Effects.GenerateEffect(bodybagSprites)
                    }, effector as IUnit));
                    return false;
                }

                CombatManager.Instance.AddSubAction(new EffectAction(new EffectInfo[]
                {
                    Effects.GenerateEffect(ScriptableObject.CreateInstance<ShowBacklashPassiveEffect>()),
                    Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyShieldSlotEffect>(), reference.value, Targeting.Slot_SelfAll),
                }, effector as IUnit));
            }
            return false;
        }
        public class ShowBacklashPassiveEffect : EffectSO
        {
            public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
            {
                exitAmount = 0;
                CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(caster.ID, caster.IsUnitCharacter, "Backlash", ResourceLoader.LoadSprite("BacklashPassive.png")));
                return true;
            }
        }
    }
}
