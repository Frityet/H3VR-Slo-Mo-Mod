using Deli;
using FistVR;
using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace H3VR.Slomo
{
    public class SloMoMod : DeliBehaviour
    {
        private void Awake()
        {
            Logger.LogInfo("Started H3VR Slo Mo Mod");
            Logger.LogWarning("https://media.discordapp.net/attachments/705223076652253302/800139596573704192/EFK38CWXsAM2sAM.png");

            var patch = new Harmony("SloMoMod");
            patch.PatchAll(typeof(SloMoMod));
            
        }

        [HarmonyPatch(typeof(FVRMovementManager), "TurnCounterClockWise")]
        [HarmonyPrefix]
        static bool SlowDown()
        {
           // Logger.LogInfo("Slowing Down");
            Debug.Log("Slowing Down");
            
            Time.timeScale -= 0.1f;
            Time.fixedDeltaTime = Time.timeScale / SteamVR.instance.hmd_DisplayFrequency;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            
            return false;
        }
        
        [HarmonyPatch(typeof(FVRMovementManager), "TurnClockWise")]
        [HarmonyPrefix]
        static bool SpeedUp()
        {
            Debug.Log("Speeding Up");
            //Logger.LogInfo("Speeding Up");
            
            
            Time.timeScale += 0.1f;
            Time.fixedDeltaTime = Time.timeScale / SteamVR.instance.hmd_DisplayFrequency;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            
            
            return false;

        }
        
        
        [HarmonyPatch(typeof(AudioSource), "pitch", MethodType.Setter)]
        [HarmonyPrefix]
        public static void FixPitch(ref float value)
        {
            value *= Time.timeScale;
        }


/*
        public static void PlayAudio(AudioClip Audio, float pitch = 1f)
        {
            AudioSource source = new();


        }
*/
    }
}