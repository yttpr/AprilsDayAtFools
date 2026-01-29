using Tools;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class MulticolorStoreData_ModIntSO : UnitStoreData_ModIntSO
    {
        public Color m_AltColor = Color.black;
        public string m_AltText = "";
        public string m_AltLocID = "";
        public override bool TryGetUnitStoreDataToolTip(UnitStoreDataHolder holder, out string result)
        {
            if (base.TryGetUnitStoreDataToolTip(holder, out result)) return true;

            result = GenerateAltString(holder.m_MainData);
            return true;
        }
        public string GenerateAltString(int value)
        {
            string text = m_AltText;
            if (m_AltLocID != "")
            {
                text = LocUtils.GameLoc.GetUIDataFromString(m_AltLocID, text);
            }

            string text2 = string.Format(text, value);
            string text3 = ColorUtility.ToHtmlStringRGB(m_AltColor);
            string text4 = "<color=#" + text3 + ">";
            string text5 = "</color>";
            return text4 + text2 + text5;
        }

        public static UnitStoreData_ModIntSO CreateAndAdd(string id, string formattedText, Color color, string altText, Color altCol, bool showDefaultIfOverThreshold = true, int threshold = 0)
        {
            MulticolorStoreData_ModIntSO unitStoreData_ModIntSO = ScriptableObject.CreateInstance<MulticolorStoreData_ModIntSO>();
            unitStoreData_ModIntSO._UnitStoreDataID = id;
            unitStoreData_ModIntSO.m_Text = formattedText;
            unitStoreData_ModIntSO.m_AltText = altText;
            unitStoreData_ModIntSO.m_TextColor = color;
            unitStoreData_ModIntSO.m_AltColor = altCol;
            unitStoreData_ModIntSO.m_ShowIfDataIsOver = showDefaultIfOverThreshold;
            unitStoreData_ModIntSO.m_CompareDataToThis = threshold;
            LoadedDBsHandler.MiscDB.AddNewUnitStoreData(id, unitStoreData_ModIntSO);
            return unitStoreData_ModIntSO;
        }
    }
}

