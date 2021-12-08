using MelonLoader;
using UnityEngine;
using HarmonyLib;
using StressLevelZero.Interaction;
using StressLevelZero.Props.Weapons;
using ModThatIsNotMod;
using ModThatIsNotMod.BoneMenu;

namespace BetterLaunching
{
    public static class BuildInfo
    {
        public const string Name = "BetterLaunching";
        public const string Author = "DarkSwitchPro";
        public const string Company = null;
        public const string Version = "1.0.0";
        public const string DownloadLink = null;
    }

    public class BetterLaunching : MelonMod
    {

        private static float velocityImpulseFactor = 1f;

        [HarmonyPatch(typeof(StressLevelZero.Props.Weapons.GravityGun), "Blast")]
        public static class GravityGunBlastPatch
        {
            public static Vector3 preVelocity;

            public static void Prefix(StressLevelZero.Props.Weapons.GravityGun __instance)
            {
                preVelocity = __instance.m_GrabbedRigidbody.velocity;
            }

            public static void Postfix(StressLevelZero.Props.Weapons.GravityGun __instance)
            {
                Vector3 postVelocity = __instance.m_GrabbedRigidbody.velocity;
                Rigidbody grabbedRigidbody = __instance.m_GrabbedRigidbody;
                if (grabbedRigidbody == null)
                    return;
                Transform root = grabbedRigidbody.GetComponentInParent<InteractableHostManager>()?.transform ?? grabbedRigidbody.GetComponentInParent<InteractableHost>()?.transform ?? grabbedRigidbody.transform;
                Rigidbody[] rigidbodies = root.GetComponentsInChildren<Rigidbody>();

                foreach (Rigidbody rb in rigidbodies)
                {
                    rb.AddForce(postVelocity * velocityImpulseFactor, ForceMode.Impulse);
                }
            }
        }

        public override void OnApplicationStart()
        {
            HarmonyLib.Harmony harmonyInstance = new HarmonyLib.Harmony("DSP.BetterLaunching.Harmony");
            harmonyInstance.PatchAll();

            MenuCategory Betterlaunching = MenuManager.CreateCategory("Better Launching", Color.blue);
            Betterlaunching.CreateFloatElement("Launch Multiplier", Color.white, velocityImpulseFactor, (value) => velocityImpulseFactor = value, 0.5f, 1f, 2f, true);
        }
    }
}