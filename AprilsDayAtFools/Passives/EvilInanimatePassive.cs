using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class EvilInanimatePassive : BasePassiveAbilitySO
    {
        [Header("Status")]
        public RupturedSE_SO _RupturedStatus;

        public GuttedSE_SO _GuttedStatus;

        public override bool IsPassiveImmediate => true;

        public override bool DoesPassiveTrigger => true;

        public override void TriggerPassive(object sender, object args)
        {
            IUnit unit = sender as IUnit;
            if (args is BooleanWithTriggerReference booleanWithTriggerReference)
            {
                if (booleanWithTriggerReference.shouldTrigger)
                {
                    CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(unit.ID, unit.IsUnitCharacter, GetPassiveLocData().text, passiveIcon));
                }

                booleanWithTriggerReference.value = false;
            }
            else if (args is CanHealReference canHealReference)
            {
                CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(unit.ID, unit.IsUnitCharacter, GetPassiveLocData().text, passiveIcon));
                canHealReference.value = false;
            }
            else if (args is StatusFieldApplication statusFieldApplication && (_GuttedStatus.IsStatus(statusFieldApplication.statusID) || _RupturedStatus.IsStatus(statusFieldApplication.statusID)))
            {
                CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(unit.ID, unit.IsUnitCharacter, GetPassiveLocData().text, passiveIcon));
                statusFieldApplication.canBeApplied = false;
            }
        }

        public override void OnPassiveConnected(IUnit unit)
        {
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
        }
    }
}
