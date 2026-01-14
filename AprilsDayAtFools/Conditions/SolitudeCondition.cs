using BrutalAPI;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class SolitudeCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageReceivedValueChangeException hitBy)
            {
                if (hitBy.damageTypeID == CombatType_GameIDs.Dmg_Fire.ToString())
                {
                    hitBy.AddModifier(new ImmZeroMod());
                    CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(effector.ID, effector.IsUnitCharacter, "Solitude", ResourceLoader.LoadSprite("SolitudePassive.png")));
                    if (Random.Range(0, 100) < 30) CombatManager.Instance.AddUIAction(new CharacterSetExtraSpriteUIAction(effector.ID, IDs.Moon));
                }
            }
            return false;
        }
    }
}
