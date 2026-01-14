using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace AprilsDayAtFools
{
    public class SafetyEffectAction : EffectAction
    {
        public SafetyEffectAction(EffectInfo[] effects, IUnit caster, int startResult = 0) : base(effects, caster, startResult)
        {
        }
        public override IEnumerator Execute(CombatStats stats)
        {
            IEnumerator ret = null;
            try
            {
                ret = base.Execute(stats);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Safety Effect Action failure");
                Debug.LogWarning(ex.ToString());
                CombatManager.Instance.AddUIAction(new ShowTextAction(_caster.ID, _caster.IsUnitCharacter, "Ability failed somehow. We tried our best :("));
                yield break;
            }
            yield return ret;
        }
    }
}
