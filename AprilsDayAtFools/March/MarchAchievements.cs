using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class MarchAchievements
    {
        public static void Add()
        {
            bool isMarch = MarchCheck.IsMarch;

            AddFor(isMarch, "Merced", "Aprils_ThePeopleOfPaper_TW");
            AddFor(isMarch, "Cora", "Aprils_WitchsHeart_TW");
            AddFor(isMarch, "Xet", "Aprils_MachineEgg_TW");
            AddFor(isMarch, "Saline", "Aprils_Overdose_SW");
            AddFor(isMarch, "Didion", "Aprils_LoversPact_SW");
            AddFor(isMarch, "Rose", "Aprils_SundayGallows_SW");
            AddFor(isMarch, "Prayer", "Aprils_TheHumanAlgorithm_TW");
            AddFor(isMarch, "Kafka", "Aprils_ShrunkenDoor_TW");
            AddFor(isMarch, "Bodybag", "Aprils_Headstone_TW");
            AddFor(isMarch, "Hangman", "Aprils_Euthanasia_SW");
            AddFor(isMarch, "Saturn", "Aprils_BoundaryLines_SW");
            AddFor(isMarch, "Hare", "Aprils_Clay_SW");
            AddFor(isMarch, "Moon", "Aprils_PaleSun_TW");
            AddFor(isMarch, "Clerk", "Aprils_DoubleBladedKnife_SW");
            AddFor(isMarch, "Esther", "Aprils_6AMAlarm_SW");
            AddFor(isMarch, "Rhys", "Aprils_FriedHumanHand_SW");
            AddFor(isMarch, "Wtmiyr", "Aprils_CollateralDamage1999_SW");
            AddFor(isMarch, "Joy", "Aprils_AluminumCube_TW");
            AddFor(isMarch, "Patch", "Aprils_BoneNeedle_TW");
            AddFor(isMarch, "Six", "Aprils_PlasticFish_EW");
            AddFor(isMarch, "Rotcore", "Aprils_BloodDiamond_TW");
            AddFor(isMarch, "Catten", "Aprils_NullReferenceException_TW");
            AddFor(isMarch, "Sunflower", "Aprils_Broom_SW");
            AddFor(isMarch, "Saea", "Aprils_Damocles_TW");
            AddFor(isMarch, "Alpha", "Aprils_YouVoodooDoll_TW");
            if (April.Me) AddFor(isMarch, "Secret", "Aprils_Boneworms_SW");
        }

        public static void AddFor(bool isMarch, string name, string itemID)
        {
            if (isMarch)
            {
                Unlocking.GenerateSingleAchievementByID("inevitable_", name, itemID, "Aprils_" + name + "_March_ACH", "InevitableTitleLabel", "The Inevitable");
                Unlocking.AddSinglePearl(name + "_CH", "March_BOSS", "Aprils_" + name + "_March_ACH");
            }
            Unlocking.SetUpSingleUnlock("March_BOSS", name + "_CH", itemID, "Aprils_" + name + "_March_ACH", "Aprils_" + name + "_March_Unlock");
        }
    }
}
