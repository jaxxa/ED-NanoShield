using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ED_NanoShield
{
    [StaticConstructorOnStartup]
    class CompQuantumShield : ThingComp
    {
        private static Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            base.PostPreApplyDamage(dinfo, out absorbed);
            if (absorbed)
                return;
            absorbed = true;
        }

        public override void PostDraw()
        {
            base.PostDraw();
            float num1 = Mathf.Lerp(1.2f, 1.55f, 100f);
            Vector3 drawPos = this.parent.DrawPos;
            drawPos.y = Altitudes.AltitudeFor(AltitudeLayer.Blueprint);
            int num2 = 7;
            if (num2 < 8)
            {
                float num3 = (float)((double)(8 - num2) / 8.0 * 0.0500000007450581);
                num1 -= num3;
            }
            float angle = (float)Rand.Range(0, 360);
            Vector3 s = new Vector3(num1, 1f, num1);
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetTRS(drawPos, Quaternion.AngleAxis(angle, Vector3.up), s);
            Graphics.DrawMesh(MeshPool.plane10, matrix, CompQuantumShield.BubbleMat, 0);
        }

        public CompProperties_QuantumShield Props
        {
            get
            {
                return (CompProperties_QuantumShield)this.props;
            }
        }
    }

    class CompProperties_QuantumShield : CompProperties
    {
        public CompProperties_QuantumShield()
        {
            this.compClass = typeof(CompQuantumShield);
        }
    }
}
