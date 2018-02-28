using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ED_QuantumShield
{
    class ModSettings_QuantumShield : ModSettings
    {
        //Mod_QuantumShield.Settings.BuildingChargeDelay
        public int ShieldChargeLevelMax;

        public int BuildingChargeDelay;
        public int BuildingChargeAmount;
        public int BuildingReservePowerMax;
        

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref ShieldChargeLevelMax, "ShieldChargeLevelMax", 200);

            Scribe_Values.Look(ref BuildingChargeDelay, "BuildingChargeDelay", 100);
            Scribe_Values.Look(ref BuildingChargeAmount, "BuildingChargeAmount", 100);

            Scribe_Values.Look(ref BuildingReservePowerMax, "BuildingReservePowerMax", 400);

            //            Scribe_Values.Look<bool>(ref ShowLettersThreatBig, "ShowLettersThreatBig", true, true);
        }


        public void DoSettingsWindowContents(Rect canvas)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.ColumnWidth = 250f;
            listing_Standard.Begin(canvas);
            //listing_Standard.set_ColumnWidth(rect.get_width() - 4f);

            listing_Standard.Label("Sections Starting with '*' only apply after Restart.");
            listing_Standard.GapLine(12f);
            listing_Standard.Label("Letter Suppression:");
            listing_Standard.Gap(12f);

            listing_Standard.End();
        }
    }
}

