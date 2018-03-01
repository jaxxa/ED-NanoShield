﻿using ED_QuantumShield;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ED_QuantumShield
{
    [StaticConstructorOnStartup]
    class CompQuantumShield : ThingComp
    {
        #region Variables
        //--Saved
        public bool QuantumShieldActive = false;
        public int QuantumShieldChargeLevelCurrent = 200;

        //--Not Saved
        private int KeepDisplayingTicks = 1000;
        private int lastKeepDisplayTick = -9999;
        private Vector3 impactAngleVect;

        private static Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
        #endregion

        #region Overrides

        public override string GetDescriptionPart()
        {
            if (this.QuantumShieldActive)
            {
                return "Quantum Shield:" + QuantumShieldChargeLevelCurrent + " / " + Mod_QuantumShield.Settings.ShieldChargeLevelMax +
                        Environment.NewLine + base.GetDescriptionPart();
            }
            return base.GetDescriptionPart();
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            using (IEnumerator<Gizmo> enumerator = base.CompGetGizmosExtra().GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    Gizmo c = enumerator.Current;
                    yield return c;
                    ;
                }
            }

            if (this.QuantumShieldActive)
            {
                Gizmo_QuantumShieldStatus opt1 = new Gizmo_QuantumShieldStatus(this);
                yield return opt1;
            }
        }

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            base.PostPreApplyDamage(dinfo, out absorbed);

            //If the shield is not Active or it was alreadt absorbed or it is out of charge, return
            if (!QuantumShieldActive || absorbed || this.QuantumShieldChargeLevelCurrent <= 0)
            {
                return;
            }

            //TODO Filter on Damage Type

            this.QuantumShieldChargeLevelCurrent -= dinfo.Amount;

            absorbed = true;


            this.lastKeepDisplayTick = Find.TickManager.TicksGame;

            SoundDefOf.EnergyShieldAbsorbDamage.PlayOneShot(new TargetInfo(this.parent.Position, base.parent.Map, false));
            this.impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
            Vector3 loc = base.parent.TrueCenter() + this.impactAngleVect.RotatedBy(180f) * 0.5f;
            float num = Mathf.Min(10f, 2f + (float)dinfo.Amount / 10f);
            MoteMaker.MakeStaticMote(loc, base.parent.Map, ThingDefOf.Mote_ExplosionFlash, num);
            int num2 = (int)num;
            for (int i = 0; i < num2; i++)
            {
                MoteMaker.ThrowDustPuff(loc, base.parent.Map, Rand.Range(0.8f, 1.2f));
            }
        }

        //public override void CompTick()
        //{
        //    //Log.Message("CompTick");
        //    base.CompTick();
        //}

        public override void PostDraw()
        {
            base.PostDraw();

            if (this.QuantumShieldActive && this.ShouldDisplay)
            {
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
        }
        
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref QuantumShieldActive, "QuantumShieldActive");
            Scribe_Values.Look(ref QuantumShieldChargeLevelCurrent, "QuantumShieldChargeLevelCurrent");
        }
        #endregion

        public CompProperties_QuantumShield Props
        {
            get
            {
                return (CompProperties_QuantumShield)this.props;
            }
        }
        
        private bool ShouldDisplay
        {
            get
            {
                if (!QuantumShieldActive)
                {
                    return false;
                }

                Pawn wearer = this.parent as Pawn;

                if (wearer != null && wearer.Spawned && !wearer.Dead && !wearer.Downed)
                {
                    if (this.QuantumShieldChargeLevelCurrent <= 0)
                    {
                        return false;
                    }
                    if (wearer.InAggroMentalState)
                    {
                        return true;
                    }
                    if (wearer.Drafted)
                    {
                        return true;
                    }
                    if (wearer.Faction.HostileTo(Faction.OfPlayer) && !wearer.IsPrisoner)
                    {
                        return true;
                    }
                    if (Find.TickManager.TicksGame < this.lastKeepDisplayTick + this.KeepDisplayingTicks)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        public int RechargeShield(int chargeAvalable)
        {
            if(!this.QuantumShieldActive || this.QuantumShieldChargeLevelCurrent >= Mod_QuantumShield.Settings.ShieldChargeLevelMax)
            {
                return 0;
            }

            this.QuantumShieldChargeLevelCurrent += chargeAvalable;

            if (this.QuantumShieldChargeLevelCurrent <= Mod_QuantumShield.Settings.ShieldChargeLevelMax)
            {
                return chargeAvalable;
            }
            else
            {
                int _Overcharge = this.QuantumShieldChargeLevelCurrent - Mod_QuantumShield.Settings.ShieldChargeLevelMax;
                this.QuantumShieldChargeLevelCurrent -= _Overcharge;
                return chargeAvalable - _Overcharge;
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
