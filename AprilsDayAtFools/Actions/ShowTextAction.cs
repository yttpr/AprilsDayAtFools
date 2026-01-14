using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class ShowTextAction : CombatAction
    {
        public int _id;

        public bool _isUnitCharacter;

        public string _passiveName;

        public Sprite _passiveSprite;

        public string _audio;

        public ShowTextAction(int id, bool isUnitCharacter, string passiveName, string audio = "", Sprite passiveSprite = null)
        {
            _id = id;
            _isUnitCharacter = isUnitCharacter;
            _passiveName = passiveName;
            _passiveSprite = passiveSprite;
            _audio = audio;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            yield return stats.combatUI.ShowPassiveInformation(_id, _isUnitCharacter, _passiveName, _audio, _passiveSprite);
        }
    }
}
