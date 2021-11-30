using MelonLoader;
using UnityEngine;
using StressLevelZero.Props.Weapons;
using HarmonyLib;

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
        [HarmonyPatch(typeof(GravityGun), "Blast")]
        public static class GravityGunBlastPatch
        {
            public static Vector3 preVelocity;

            public static void Prefix(GravityGun __instance)
            {
                preVelocity = __instance.m_GrabbedRigidbody.gameObject.GetComponent<Rigidbody>().velocity;
            }

            public static void Postfix(GravityGun __instance)
            {
                Vector3 postVelocity = __instance.m_GrabbedRigidbody.gameObject.GetComponent<Rigidbody>().velocity;

                Rigidbody grabbedRigidbody = __instance.m_GrabbedRigidbody;
                Transform root = grabbedRigidbody.transform.root;
                Rigidbody[] rigidbodies = root.GetComponentsInChildren<Rigidbody>();
                float velocityImpulseFactor = 2f;

                foreach (Rigidbody rb in rigidbodies)
                {
                    if (rb != null)
                    {
                        rb.AddForce(postVelocity * velocityImpulseFactor, ForceMode.Impulse);
                    }
                }
            }
        }

        public override void OnApplicationStart()
        {
            HarmonyLib.Harmony harmonyInstance = new HarmonyLib.Harmony("DSP.BetterLaunching.Harmony");
            harmonyInstance.PatchAll();
        }
    }
}