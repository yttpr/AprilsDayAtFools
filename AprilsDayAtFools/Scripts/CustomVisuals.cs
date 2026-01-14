using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class CustomVisuals
    {
        public static Dictionary<string, AttackVisualsSO> Visuals;
        public static void Prepare()
        {
            Visuals = new Dictionary<string, AttackVisualsSO>();
        }
        public static void LoadVisuals(string name, AssetBundle bundle, string path, string sound, bool full = false)
        {
            try
            {
                AttackVisualsSO ret = ScriptableObject.CreateInstance<AttackVisualsSO>();
                ret.name = name;
                ret.animation = bundle.LoadAsset<AnimationClip>(path);
                ret.audioReference = sound;
                ret.isAnimationFullScreen = full;
                if (Visuals == null) Prepare();
                if (!Visuals.ContainsKey(name)) Visuals.Add(name, ret);
                else Debug.LogWarning("animation for " + name + " already exists!");
            }
            catch (Exception ex)
            {
                Debug.LogError("visuals failed to load: " + name);
                Debug.LogError("asset path: " + path);
                Debug.LogError("audio path: " + sound);
                Debug.LogError(ex.ToString());
            }
        }
        public static AttackVisualsSO GetVisuals(string name)
        {
            if (Visuals == null) Prepare();
            if (Visuals.TryGetValue(name, out AttackVisualsSO ret)) return ret;
            else Debug.LogWarning("missing animation for " + name);
            return null;
        }
        public static void Duplicate(string newname, string oldname, string audio)
        {
            try
            {
                AttackVisualsSO old = GetVisuals(oldname);
                if (old == null) return;
                AttackVisualsSO ret = ScriptableObject.CreateInstance<AttackVisualsSO>();
                ret.name = newname;
                ret.animation = old.animation;
                ret.audioReference = audio;
                ret.isAnimationFullScreen = old.isAnimationFullScreen;
                if (Visuals == null) Prepare();
                if (!Visuals.ContainsKey(newname)) Visuals.Add(newname, ret);
                else Debug.LogWarning("animation for " + newname + " already exists!");
            }
            catch
            {
                Debug.LogError("visuals failed to load: " + newname);
                Debug.LogError("failed to copy off: " + oldname);
            }
        }

        public static void Setup()
        {
            LoadVisuals("Salt/Static", Joyce.Attacks, "assets/pretty/StaticAnim.anim", "event:/Lunacy/Attack/Static", true);
            LoadVisuals("Salt/Rose", Joyce.Attacks, "assets/pretty/FlowerAttackAnim.anim", "event:/Lunacy/Attack/FlowerBell");
            LoadVisuals("Salt/Sprout", Joyce.Attacks, "assets/pretty/FlowerBoneBreak.anim", "event:/Lunacy/Attack/FlowerBone");
            LoadVisuals("Salt/Cannon", Joyce.Attacks, "assets/pretty/CannonAnim.anim", "event:/Lunacy/Attack/Cannon");
            LoadVisuals("Salt/Gaze", Joyce.Attacks, "assets/pretty/EyeScare.anim", "event:/Lunacy/Attack/EyeScare");
            LoadVisuals("Salt/Decapitate", Joyce.Attacks, "assets/pretty/CutAnim.anim", "event:/Lunacy/Attack/Decapitate");
            LoadVisuals("Salt/Class", Joyce.Attacks, "assets/pretty/ClassAnim.anim", "event:/Lunacy/Attack/Class");
            LoadVisuals("Salt/Needle", Joyce.Attacks, "assets/pretty/NeedleAnim.anim", "event:/Lunacy/Attack/Needle");
            LoadVisuals("Salt/Claws", Joyce.Attacks, "assets/pretty/ClawingAnim.anim", "event:/Lunacy/Attack/Clawing");
            LoadVisuals("Salt/Stars", Joyce.Attacks, "assets/pretty/StarryAnim.anim", "event:/Lunacy/Attack/PointGet");
            Duplicate("Salt/Skyloft/Stars", "Salt/Stars", "event:/Lunacy/Hurt/BirdSound");
            LoadVisuals("Salt/Hung", Joyce.Attacks, "assets/pretty/HUNG.anim", "event:/Lunacy/Attack/Noosed");
            LoadVisuals("Salt/Crush", Joyce.Attacks, "assets/Attack2/PressAnim.anim", "event:/Lunacy/Attack2/Press");
            LoadVisuals("Salt/Ads", Joyce.Attacks, "assets/Attack2/Ads.anim", "event:/Lunacy/Attack2/Popup");
            LoadVisuals("Salt/Door", Joyce.Attacks, "assets/Attack2/DoorAnim.anim", "event:/Lunacy/Attack2/DoorSlam");
            LoadVisuals("Salt/Keyhole", Joyce.Attacks, "assets/Attack2/KeyholeAnim.anim", "event:/Lunacy/Attack2/Blink");
            LoadVisuals("Salt/Notif", Joyce.Attacks, "assets/Attack2/AlertAnim.anim", "event:/Lunacy/Attack2/Quest");
            LoadVisuals("Salt/Wheel", Joyce.Attacks, "assets/Attack2/Sail.anim", "event:/Lunacy/Attack2/Wheel");
            LoadVisuals("Salt/Swirl", Joyce.Attacks, "assets/Attack2/Waves1Anim.anim", "event:/Lunacy/Attack2/Waves1");
            LoadVisuals("Salt/Pop", Joyce.Attacks, "assets/Attack2/Waves2Anim.anim", "event:/Lunacy/Attack2/Waves2");
            LoadVisuals("Salt/Smile", Joyce.Attacks, "assets/Attack2/SmileScare.anim", "event:/Lunacy/Attack2/Smiley");
            LoadVisuals("Salt/Fog", Joyce.Attacks, "assets/Attack2/FoggyLens.anim", "event:/Lunacy/Attack2/Fog", true);
            LoadVisuals("Salt/Ash", Joyce.Attacks, "assets/Attack2/AshAnim.anim", "event:/Lunacy/Attack2/Ash");
            LoadVisuals("Salt/Four", Joyce.Attacks, "assets/Attack2/FourAnim.anim", "event:/Lunacy/Attack2/Four");
            LoadVisuals("Salt/Ribbon", Joyce.Attacks, "assets/Attack2/Ribbon.anim", "event:/Lunacy/Attack2/Ribbon");
            LoadVisuals("Salt/Bullet", Joyce.Attacks, "assets/Attack2/BulletsAnim.anim", "event:/Lunacy/Attack2/Gun");
            LoadVisuals("Salt/Shatter", Joyce.Attacks, "assets/Attack2/ShatterAnim.anim", "event:/Lunacy/Attack2/Shatter");
            LoadVisuals("Salt/Insta/Shatter", Joyce.Attacks, "assets/Attack2/ImmediateShatter.anim", "event:/Lunacy/Attack2/Shatter");
            LoadVisuals("Salt/Zap", Joyce.Attacks, "assets/Attack2/Electric.anim", "event:/Lunacy/Attack2/Zap");
            LoadVisuals("Salt/Alarm", Joyce.Attacks, "assets/Attack2/ClockAnim.anim", "event:/Lunacy/Attack2/WakeUp");
            LoadVisuals("Salt/Piano", Joyce.Attacks, "assets/Attack2/HammerKeys.anim", "event:/Lunacy/Attack2/Piano");
            LoadVisuals("Salt/Think", Joyce.Attacks, "assets/Attack2/IdeaAnim.anim", "event:/Lunacy/Attack2/Thought");
            LoadVisuals("Salt/Whisper", Joyce.Attacks, "assets/Attack2/Speak.anim", "event:/Lunacy/Attack2/Whisper");
            LoadVisuals("Salt/Cube", Joyce.Attacks, "assets/Attack2/CubeAnim.anim", "event:/Lunacy/Attack2/Construct");
            LoadVisuals("Salt/Snap", Joyce.Attacks, "assets/Attack2/SnapRose.anim", "event:/Lunacy/Attack2/Snaps");
            LoadVisuals("Salt/Rain", Joyce.Attacks, "assets/Attack2/RainingAnim.anim", "event:/Lunacy/Attack2/Rainy");
            LoadVisuals("Salt/Coda", Joyce.Attacks, "assets/Attack2/CodaAnim.anim", "event:/Lunacy/Attack2/Coda");
            LoadVisuals("Salt/Forest", Joyce.Attacks, "assets/Attack2/TheForest.anim", "event:/Lunacy/Attack2/Forest", true);
            LoadVisuals("Salt/Shush", Joyce.Attacks, "assets/Attack2/ShushAnim.anim", "event:/Lunacy/Attack2/Shush");
            LoadVisuals("Salt/Lens", Joyce.Attacks, "assets/Attack2/Picture.anim", "event:/Lunacy/Attack2/Shutter");
            LoadVisuals("Salt/Train", Joyce.Attacks, "assets/train/HitAndRun.anim", "event:/Lunacy/Attack3/FUCKINGTRAIN");
            LoadVisuals("Salt/Censor", Joyce.Attacks, "assets/Attack3/CensoredAnimation.anim", "event:/Lunacy/Attack3/Censored");
            LoadVisuals("Salt/Unlock", Joyce.Attacks, "assets/Attack3/UnlockAnim.anim", "event:/Lunacy/Attack3/Unlocking");
            LoadVisuals("Salt/Spotlight", Joyce.Attacks, "assets/Attack3/SpotlightAnim.anim", "event:/Lunacy/Attack3/Spotlight");
            LoadVisuals("Salt/Scorch", Joyce.Attacks, "assets/16/ScorchAnim.anim", "event:/Lunacy/Attack3/Scorch");
            LoadVisuals("Salt/Curse", Joyce.Attacks, "assets/ani/thecurse.anim", LoadedAssetsHandler.GetEnemy("UnfinishedHeir_BOSS").abilities[2].ability.visuals.audioReference);
            LoadVisuals("Salt/Nailing", Joyce.Attacks, "Assets/ani/Nailing.anim", "event:/Lunacy/Attack3/Nailing");
            LoadVisuals("Salt/Stop", Joyce.Attacks, "Assets/train/NewTrain/StopSignAnim.anim", "event:/Lunacy/Attack3/Stop");
            LoadVisuals("Salt/Sign", Joyce.Attacks, "Assets/train/NewTrain/SignSlamAnim.anim", "event:/Lunacy/Attack3/Sign");
            LoadVisuals("Salt/Gears", Joyce.Attacks, "Assets/attacks3/Gears.anim", LoadedAssetsHandler.GetEnemyAbility("Crush_A").visuals.audioReference);
            LoadVisuals("Salt/Reload", Joyce.Attacks, "Assets/attacks3/Reload.anim", "event:/Lunacy/Attack4/Reload");
            LoadVisuals("Salt/Gunshot", Joyce.Attacks, "Assets/attacks3/Gunshot.anim", "event:/Lunacy/Attack4/Gunshot");
            LoadVisuals("Salt/StarBomb", Joyce.Attacks, "Assets/attacks3/StarBomb.anim", "event:/Lunacy/Attack4/StarBomb");
            LoadVisuals("Salt/Call", Joyce.Attacks, "Assets/attacks3/Call.anim", "event:/Lunacy/Attack2/WakeUp");
            LoadVisuals("Salt/Drill", Joyce.Attacks, "Assets/attacks3/Drill.anim", "event:/Lunacy/Attack3/Stop");
            LoadVisuals("Salt/YinYang", Joyce.Attacks, "Assets/attacks3/YinYang.anim", "event:/Lunacy/Attack4/ReverseSwirl");
            LoadVisuals("Salt/Curtains", Joyce.Attacks, "Assets/attacks3/Curtains.anim", "event:/Lunacy/Attack4/Curtains", true);
            LoadVisuals("Salt/Monster", Joyce.Attacks, "Assets/attacks3/Monster.anim", LoadedAssetsHandler.GetCharacterAbility("Oil_1_A").visuals.audioReference);
        }
    }
}
