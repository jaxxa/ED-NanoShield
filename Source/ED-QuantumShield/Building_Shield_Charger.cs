using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;
using Enhanced_Development.PersonalShields.Nano;

namespace ED_QuantumShield
{
    [StaticConstructorOnStartup]
    public class Building_QuantumShield_Charger : Building
    {
        #region Variables

        public float MAX_DISTANCE = 5.0f;
        public bool flag_charge = false;
        CompPowerTrader power;
        //NanoConnector nanoConnector;

        private static Texture2D UI_UPGRADE;
        private static Texture2D UI_CHARGE_OFF;
        private static Texture2D UI_CHARGE_ON;

        #endregion

        //Constructor
        static Building_QuantumShield_Charger()
        {
            //Log.Message("Getting graphics");
            UI_UPGRADE = ContentFinder<Texture2D>.Get("UI/Upgrade", true);
            UI_CHARGE_OFF = ContentFinder<Texture2D>.Get("UI/ChargeOFF", true);
            UI_CHARGE_ON = ContentFinder<Texture2D>.Get("UI/ChargeON", true);
        }

        #region Overrides

        //Dummy override
        public override void PostMake()
        {
            base.PostMake();
        }
        public override void Draw()
        {
            base.Draw();
        }

        //On spawn, get the power component reference
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.power = base.GetComp<CompPowerTrader>();
        }

        public override void Tick()
        {
            //Log.Message("Tick");
            base.Tick();

            //if (this.power.PowerOn == true)
            //{
            //    NanoManager.tick();
            //}

            //This no longer requires Power inorder to be usable in Caravans.

            if (this.flag_charge)
            {
                this.rechargePawns();
            }
        }
        
        public override void DrawExtraSelectionOverlays()
        {
            GenDraw.DrawRadiusRing(base.Position, this.MAX_DISTANCE);
        }

        //public override string GetInspectString()
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    //stringBuilder.Append(base.GetInspectString());
        //    ///stringBuilder.Append(shieldField.GetInspectString());

        //    /*
        //    for (int i = 0, l = sparksParticle.Length; i < l; i++)
        //    {
        //        stringBuilder.AppendLine("   " + (i + 1) + ". " + sparksParticle[i].currentDir + " -> " + sparksParticle[i].currentStep);
        //    }*/

        //    string text;

        //    text = "Nano Charge: " + NanoManager.getCurrentCharge() + " / " + NanoManager.getMaxCharge();
        //    stringBuilder.AppendLine(text);

        //    if (power != null)
        //    {
        //        text = power.CompInspectStringExtra();
        //        if (!text.NullOrEmpty())
        //        {
        //            stringBuilder.AppendLine(text);
        //        }
        //    }


        //    return stringBuilder.ToString();
        //}

        //Saving game
        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref flag_charge, "flag_charge");
            Scribe_Values.Look(ref NanoManager.currentCharge, "currentCharge");
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            //Add the stock Gizmoes
            foreach (var g in base.GetGizmos())
            {
                yield return g;
            }

            if (true)
            {
                Command_Action act = new Command_Action();
                //act.action = () => Designator_Deconstruct.DesignateDeconstruct(this);
                act.action = () => this.upgradePawns();
                act.icon = UI_UPGRADE;
                act.defaultLabel = "Upgrade Pawn";
                act.defaultDesc = "Upgrade Pawn";
                //act.activateSound = SoundDef.Named("Click");
                //act.hotKey = KeyBindingDefOf.DesignatorDeconstruct;
                //act.groupKey = 689736;
                yield return act;
            }

            if (flag_charge)
            {
                Command_Action act = new Command_Action();
                //act.action = () => Designator_Deconstruct.DesignateDeconstruct(this);
                act.action = () => this.SwitchCharge();
                act.icon = UI_CHARGE_ON;
                act.defaultLabel = "Charge Shields";
                act.defaultDesc = "On";
                // act.activateSound = SoundDef.Named("Click");
                //act.hotKey = KeyBindingDefOf.DesignatorDeconstruct;
                //act.groupKey = 689736;
                yield return act;
            }
            else
            {
                Command_Action act = new Command_Action();
                //act.action = () => Designator_Deconstruct.DesignateDeconstruct(this);
                act.action = () => this.SwitchCharge();
                act.icon = UI_CHARGE_OFF;
                act.defaultLabel = "Charge Shields";
                act.defaultDesc = "Off";
                //act.activateSound = SoundDef.Named("Click");
                //act.hotKey = KeyBindingDefOf.DesignatorDeconstruct;
                //act.groupKey = 689736;
                yield return act;
            }
        }

        #endregion
        
        private void SwitchCharge()
        {
            flag_charge = !flag_charge;
        }


        private IEnumerable<CompQuantumShield> ShieldCompsInRangeAndOfFaction()
        {
            IEnumerable<Pawn> _Pawns = this.Map.mapPawns.PawnsInFaction(Faction.OfPlayer).Where<Pawn>(t => t.Position.InHorDistOf(this.Position, this.MAX_DISTANCE));
            IEnumerable<CompQuantumShield> _Comps = _Pawns.Select(p => p.TryGetComp<CompQuantumShield>());
            return _Comps;
        }


        private void upgradePawns()
        {
            Log.Message("Upgrade");
            IEnumerable<Pawn> closePawns = Enhanced_Development.Utilities.Utilities.findPawnsInColony(this.Position, this.Map, this.MAX_DISTANCE);

            bool _AnyUpgraded = false;


            foreach (CompQuantumShield _ShieldComp in this.ShieldCompsInRangeAndOfFaction())
            {
                Log.Message("Adding");
                if (!_ShieldComp.QuantumShieldActive)
                {
                    _ShieldComp.QuantumShieldActive = true;
                    _AnyUpgraded = true;
                }
            }

            //if (closePawns != null)
            //{
            //    foreach (Pawn currentPawn in closePawns.ToList())
            //    {
            //        CompQuantumShield _ShieldComp = currentPawn.TryGetComp<CompQuantumShield>();
            //        if (null != _ShieldComp)
            //        {
            //            Log.Message("Adding");
            //            if (!_ShieldComp.QuantumShieldActive)
            //            {
            //                _ShieldComp.QuantumShieldActive = true;
            //                _AnyUpgraded = true;
            //            }
            //        }
            //    }
            //}

            if (!_AnyUpgraded)
            {
                Log.Message("No Paws found to add Quantum Shields to.");
            }

            return;
        }

        public void rechargePawns()
        {
            int currentTick = Find.TickManager.TicksGame;
            //Only every 10 ticks
            if (currentTick % 10 == 0)
            {
                
                foreach (CompQuantumShield _ShieldComp in this.ShieldCompsInRangeAndOfFaction())
                {
                    if (_ShieldComp.QuantumShieldActive)
                    {                        
                        _ShieldComp.QuantumShieldChargeLevelCurrent += 10;
                    }
                }
            }
        }

    }
}