using BepInEx;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    [BepInPlugin("AprilsFoolsDay.Joyce", "Aprils' Day at Fools", "1.3.4.4")]//next tweak would be 1.3.5
    [BepInDependency("BrutalOrchestra.BrutalAPI", BepInDependency.DependencyFlags.HardDependency)]
    public class Joyce : BaseUnityPlugin
    {
        public static AssetBundle Assets;
        public static AssetBundle Attacks;
        public static AssetBundle Other;
        public static YarnProgram Yarn;
        public void Awake()
        {
            Logger.LogInfo("Aprils);");

            Class1.PCall(Class1.SetAssets);

            Class1.PCall(CustomVisuals.Setup);

            Class1.PCall(NotificationHook.Setup);
            Class1.PCall(SolitudeHandler.Setup);
            Class1.PCall(ConscientiousHandler.Setup);
            Class1.PCall(FlowersUnboxer.Setup);
            Class1.PCall(EwlauHandler.Setup);
            Class1.PCall(WarmupHandler.Setup);
            Class1.PCall(LuckOfDrawHandler.Setup);
            Class1.PCall(AmbushManager.Setup);
            Class1.PCall(AmbushHook.Setup);
            Class1.PCall(PigmentUsedCollector.Setup);
            Class1.PCall(EternalHandler.Setup);
            Class1.PCall(MoonHandler.Setup);
            Class1.PCall(RhysHandler.Setup);
            Class1.PCall(NailingHandler.Setup);
            Class1.PCall(HangmanHandler.Setup);
            Class1.PCall(StandHandler.Setup);
            Class1.PCall(PatchHandler.Setup);
            Class1.PCall(FirstPerTurnHandler.Setup);
            Class1.PCall(ForcedTurnHandler.Setup);
            Class1.PCall(KafkaHandler.Setup);
            Class1.PCall(DeterminedHandler.Setup);
            Class1.PCall(ComaHandler.Setup);
            Class1.PCall(DivineProtMod.Setup);
            Class1.PCall(LinkedMod.Setup);
            Class1.PCall(JoyHandler.Setup);
            Class1.PCall(SaturnHandler.Setup);
            Class1.PCall(RotcoreHandler.Setup);
            Class1.PCall(PauseHook.Setup);
            Class1.PCall(HeavenFix.Add);
            Class1.PCall(TempRankUpCloser.Setup);
            Class1.PCall(BlockFromShops.Setup);
            Class1.PCall(WontKillDamageHook.Setup);
            Class1.PCall(PermanentPassiveHandler.Setup);

            Class1.PCall(Anesthetics.Add);
            Class1.PCall(Power.Add);
            Class1.PCall(Determined.Add);
            Class1.PCall(Pale.Add);
            Class1.PCall(Pimples.Add);
            Class1.PCall(Haste.Add);
            Class1.PCall(Acid.Add);
            Class1.PCall(Dodge.Add);
            Class1.PCall(Entropy.Add);
            Class1.PCall(Water.Add);
            Class1.PCall(Drowning.Add);
            Class1.PCall(Terror.Add);
            Class1.PCall(Karma.Add);
            Class1.PCall(Inverted.Add);

            Class1.PCall(PatchSetup.Setup);
            Class1.PCall(ScrapBomb.Add);

            Class1.PCall(Merced.Add);
            Class1.PCall(Cora.Add);
            Class1.PCall(Xet.Add);
            Class1.PCall(Saline.Add);
            Class1.PCall(Didion.Add);
            Class1.PCall(Rose.Add);
            Class1.PCall(Prayer.Add);
            Class1.PCall(Kafka.Add);
            Class1.PCall(Bodybag.Add);
            Class1.PCall(Hangman.Add);
            Class1.PCall(Saturn.Add);
            Class1.PCall(Hare.Add);
            Class1.PCall(Moon.Add);
            Class1.PCall(Clerk.Add);
            Class1.PCall(Esther.Add);
            Class1.PCall(Rhys.Add);
            Class1.PCall(Wtmiyr.Add);
            Class1.PCall(Joy.Add);
            Class1.PCall(Patch.Add);
            Class1.PCall(Six.Add);
            Class1.PCall(Rotcore.Add);
            Class1.PCall(Catten.Add);
            Class1.PCall(Sunflower.Add);
            Class1.PCall(Saea.Add);
            Class1.PCall(Alpha.Add);
            Class1.PCall(Defacer.Add);

            Class1.PCall(FreeFool.Setup);
            Class1.PCall(Defacer.AddDialogueEmote);

            Class1.PCall(CascadingDamageItemHandler.Setup);
            Class1.PCall(ClownKiller.Setup);
            Class1.PCall(Anatomy.Add);
            Class1.PCall(Dog.Add);
            Class1.PCall(Amoeba.Add);

            Class1.PCall(Merced.Items);
            Class1.PCall(Cora.Items);
            Class1.PCall(Xet.Items);
            Class1.PCall(Saline.Items);
            Class1.PCall(Didion.Items);
            Class1.PCall(Rose.Items);
            Class1.PCall(Prayer.Items);
            Class1.PCall(Kafka.Items);
            Class1.PCall(Bodybag.Items);
            Class1.PCall(Hangman.Items);
            Class1.PCall(Saturn.Items);
            Class1.PCall(Hare.Items);
            Class1.PCall(Moon.Items);
            Class1.PCall(Clerk.Items);
            Class1.PCall(Esther.Items);
            Class1.PCall(Rhys.Items);
            Class1.PCall(Wtmiyr.Items);
            Class1.PCall(Joy.Items);
            Class1.PCall(Patch.Items);
            Class1.PCall(Six.Items);
            Class1.PCall(Rotcore.Items);
            Class1.PCall(Catten.Items);
            Class1.PCall(Sunflower.Items);
            Class1.PCall(Saea.Items);
            Class1.PCall(Alpha.Items);
            Class1.PCall(Defacer.Items);

            Class1.PCall(Merced.Unlocks);
            Class1.PCall(Cora.Unlocks);
            Class1.PCall(Xet.Unlocks);
            Class1.PCall(Saline.Unlocks);
            Class1.PCall(Didion.Unlocks);
            Class1.PCall(Rose.Unlocks);
            Class1.PCall(Prayer.Unlocks);
            Class1.PCall(Kafka.Unlocks);
            Class1.PCall(Bodybag.Unlocks);
            Class1.PCall(Hangman.Unlocks);
            Class1.PCall(Saturn.Unlocks);
            Class1.PCall(Hare.Unlocks);
            Class1.PCall(Moon.Unlocks);
            Class1.PCall(Clerk.Unlocks);
            Class1.PCall(Esther.Unlocks);
            Class1.PCall(Rhys.Unlocks);
            Class1.PCall(Wtmiyr.Unlocks);
            Class1.PCall(Joy.Unlocks);
            Class1.PCall(Patch.Unlocks);
            Class1.PCall(Six.Unlocks);
            Class1.PCall(Rotcore.Unlocks);
            Class1.PCall(Catten.Unlocks);
            Class1.PCall(Sunflower.Unlocks);
            Class1.PCall(Saea.Unlocks);
            Class1.PCall(Alpha.Unlocks);
            Class1.PCall(Defacer.Unlocks);

            Class1.PCall(CustomDialogueHandler.Setup);
            Class1.PCall(Quests.Add);
            Class1.PCall(FreeFool.Add_Saea);

            Class1.PCall(PostLoading.Setup);

            Class1.PCall(ObliterateHook.Setup);
            Class1.PCall(IgnoreShieldHandler.Setup);
            Class1.PCall(SidesDamagedHandler.Setup);
            Class1.PCall(SidesWillApplyDamageHandler.Setup);
            Class1.PCall(AlarmHandler.Setup);
            Class1.PCall(YouVoodooDollHandler.Setup);

            Logger.LogInfo("day At Foolss");


        }
    }
}
