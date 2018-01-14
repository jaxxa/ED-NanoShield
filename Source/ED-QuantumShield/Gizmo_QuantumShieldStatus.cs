﻿using ED_NanoShield;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ED_QuantumShield
{
    
    [StaticConstructorOnStartup]
    internal class Gizmo_QuantumShieldStatus : Gizmo
    {
        public CompQuantumShield QuantumShield;

        private static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

        private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

        public override float Width
        {
            get
            {
                return 140f;
            }
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft)
        {
            Rect overRect = new Rect(topLeft.x, topLeft.y, this.Width, 75f);
            Find.WindowStack.ImmediateWindow(984688, overRect, WindowLayer.GameUI, delegate
            {
                Rect rect = overRect.AtZero().ContractedBy(6f);
                Rect rect2 = rect;
                rect2.height = overRect.height / 2f;
                Text.Font = GameFont.Tiny;
               // Widgets.Label(rect2, this.QuantumShield.LabelCap);
                Widgets.Label(rect2, "Temp");
                Rect rect3 = rect;
                rect3.yMin = overRect.height / 2f;
                //float fillPercent = this.QuantumShield.Energy / Mathf.Max(1f, this.QuantumShield.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true));
                float fillPercent = ( 50 / 100);
                Widgets.FillableBar(rect3, fillPercent, Gizmo_QuantumShieldStatus.FullShieldBarTex, Gizmo_QuantumShieldStatus.EmptyShieldBarTex, false);
                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(rect3, (50 * 100f).ToString("F0") + " / " + (100 * 100f).ToString("F0"));
                Text.Anchor = TextAnchor.UpperLeft;
            }, true, false, 1f);
            return new GizmoResult(GizmoState.Clear);
        }
    }
}
