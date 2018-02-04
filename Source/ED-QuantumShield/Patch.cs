using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace ED_QuantumShield
{

    [StaticConstructorOnStartup]
    static class Patch
    {
        static Patch()
        {

            HarmonyInstance _Harmony = HarmonyInstance.Create("EnhancedDevelopment.QuantumShield");

            Log.Message("#ED_QuantumShield.ApplyPatches() Starting");

            MethodInfo _ThingWithComps_InitializeComps = typeof(Verse.ThingWithComps).GetMethod("InitializeComps", BindingFlags.Public | BindingFlags.Instance);
            Patch.LogNULL(_ThingWithComps_InitializeComps, "_ThingWithComps_InitializeComps", true);

            //Get the Prefix Patch
            MethodInfo _InitializeCompsPrefix = typeof(Patch).GetMethod("InitializeCompsPrefix", BindingFlags.Public | BindingFlags.Static);
            Patch.LogNULL(_InitializeCompsPrefix, "_AddCompPrefix", true);

            //Apply the Prefix Patch
            _Harmony.Patch(_ThingWithComps_InitializeComps, new HarmonyMethod(_InitializeCompsPrefix), null);

            Log.Message("#ED_QuantumShield.ApplyPatches() Completed");
        }

        // prefix
        // - wants instance, result and count
        // - wants to change count
        // - returns a boolean that controls if original is executed (true) or not (false)
        public static bool InitializeCompsPrefix(ThingWithComps __instance)
        {
            //Bail out if not dealing with a Pawn
            if (!(__instance is Pawn))
            {
                return true;
            }

            CompProperties_QuantumShield _CompProp = new CompProperties_QuantumShield();

            if (__instance is Pawn && !__instance.def.comps.Any(x => string.Equals(x.compClass.FullName, _CompProp.compClass.FullName)))
            {
                __instance.def.comps.Add(_CompProp);
            }

            return true;
        }

        /// <summary>
        /// Debug Logging Helper
        /// </summary>
        /// <param name="objectToTest"></param>
        /// <param name="name"></param>
        /// <param name="logSucess"></param>
        public static void LogNULL(object objectToTest, String name, bool logSucess = false)
        {
            if (objectToTest == null)
            {
                Log.Error(name + " Is NULL.");
            }
            else
            {
                if (logSucess)
                {
                    Log.Message(name + " Is Not NULL.");
                }
            }
        }
    }
}
