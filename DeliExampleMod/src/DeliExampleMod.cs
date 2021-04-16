using BepInEx.Configuration;
using Deli;
using FistVR;
using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace H3VR.SloMoMod
{
    public class SloMoMod : DeliBehaviour
    {
        public static ConfigEntry<float> SpeedUpAmount;
        public static ConfigEntry<float> SlowDownAmount;
        
        private void Awake()
        {
            Logger.LogInfo("Started H3VR Slo Mo Mod"); 
            SpeedUpAmount = Config.Bind("General", "Speed up amount", 0.1f, "Amount to speed up");
            SlowDownAmount = Config.Bind("General", "Slow down amount", 0.1f, "Amount to slow down");

            var patch = new Harmony("SloMoMod");
            patch.PatchAll(typeof(SloMoMod));
            
        }

        [HarmonyPatch(typeof(FVRMovementManager), "TurnCounterClockWise")]
        [HarmonyPrefix]
        static void SlowDown()
        {
            Debug.Log("Slowing Down");

            Time.timeScale -= SlowDownAmount.Value;
            Time.fixedDeltaTime = Time.timeScale / SteamVR.instance.hmd_DisplayFrequency;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
        
        [HarmonyPatch(typeof(FVRMovementManager), "TurnClockWise")]
        [HarmonyPrefix]
        static void SpeedUp()
        {
            Debug.Log("Speeding Up");
            //Logger.LogInfo("Speeding Up");
            
            
            Time.timeScale += SpeedUpAmount.Value;
            Time.fixedDeltaTime = Time.timeScale / SteamVR.instance.hmd_DisplayFrequency;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        }
        
        
        [HarmonyPatch(typeof(AudioSource), "pitch", MethodType.Setter)]
        [HarmonyPrefix]
        public static void FixPitch(ref float value)
        {
            value *= Time.timeScale;
        }
    }
}