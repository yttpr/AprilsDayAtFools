using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class BackupBodiesCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is CharacterCombat chara)
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    CombatManager.Instance.AddRootAction(new SpawnCharacterAction(chara.Character, -1, false, "", false, chara.Rank, chara.UsedAbilities, chara.Character.GetMaxHealth(chara.Rank)));
                    return true;
                }
            }
            return false;
        }
    }
}
