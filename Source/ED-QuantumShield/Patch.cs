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

            //Log.Message("QuantumShieldPatch");
            //PawnKindDef _Def = PawnKindDefOf.Colonist;
            //CompProperties_QuantumShield _CompProp = new CompProperties_QuantumShield();
            //_Def.comps.Add(_CompProp);
            //Log.Message("QuantumShieldPatch Done");


            Log.Message("#ED_QuantumShield.ApplyPatches() Starting");

            //Get the Origional CheckSpring Method
            //MethodInfo _PawnComponentsUtility_AddComponentsForSpawn = typeof(RimWorld.PawnComponentsUtility).GetMethod("AddComponentsForSpawn", BindingFlags.Public | BindingFlags.Static);

            //MethodInfo _PawnComponentsUtility_AddComponentsForSpawn = typeof(Verse.Pawn).GetMethod("SpawnSetup", BindingFlags.Public | BindingFlags.Instance);
            //Patch.LogNULL(_PawnComponentsUtility_AddComponentsForSpawn, "_PawnComponentsUtility_AddComponentsForSpawn", true);

            MethodInfo _PawnComponentsUtility_AddComponentsForSpawn = typeof(Verse.ThingWithComps).GetMethod("InitializeComps", BindingFlags.Public | BindingFlags.Instance);
            Patch.LogNULL(_PawnComponentsUtility_AddComponentsForSpawn, "_PawnComponentsUtility_AddComponentsForSpawn", true);

            //Get the Prefix Patch
            MethodInfo _AddCompPrefix = typeof(Patch).GetMethod("AddCompPrefix", BindingFlags.Public | BindingFlags.Static);
            Patch.LogNULL(_AddCompPrefix, "_AddCompPrefix", true);

            //Apply the Prefix Patch
            _Harmony.Patch(_PawnComponentsUtility_AddComponentsForSpawn, new HarmonyMethod(_AddCompPrefix), null);

            Log.Message("#ED_QuantumShield.ApplyPatches() Completed");
        }



        // prefix
        // - wants instance, result and count
        // - wants to change count
        // - returns a boolean that controls if original is executed (true) or not (false)
        public static bool AddCompPrefix(ThingWithComps __instance)
        {
            CompProperties_QuantumShield _CompProp = new CompProperties_QuantumShield();
            //Log.Message("DefName " + __instance.def.defName);
            //string.Equals(__instance.def.thingClass, "Pawn") &&
            
            if (__instance is Pawn && !__instance.def.comps.Any(x => string.Equals(x.compClass.FullName, _CompProp.compClass.FullName)))
            {
            //Log.Message(__instance.def.comps.Count.ToString());
            __instance.def.comps.Add(_CompProp);
            }
            //Log.Message(__instance.def.comps.Count.ToString());

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
