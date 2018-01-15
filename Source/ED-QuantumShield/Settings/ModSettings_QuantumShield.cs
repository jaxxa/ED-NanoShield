using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ED_QuantumShield.Settings
{
    class ModSettings_ED_QuantumShield : ModSettings
    {


        public override void ExposeData()
        {
            base.ExposeData();

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

