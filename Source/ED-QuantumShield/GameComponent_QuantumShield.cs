using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace ED_QuantumShield
{
    class GameComponent_QuantumShield : Verse.GameComponent
    {

        #region Variables
        //--Saved
        public static int ChargeLevelCurrent = 200;

        //--Not Saved
        public static int ChargeLevelMax = 1000;

        //public static GameComponent_QuantumShield GameComp;
        #endregion

        public GameComponent_QuantumShield(Game game)
        {
            // GameComponent_QuantumShield.GameComp = this;
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();

            //Only Run every 20 Ticks.
            int currentTick = Find.TickManager.TicksGame;
            if (currentTick % 20 != 0)
            {
                return;
            }

            GameComponent_QuantumShield.ReturnCharge(1);
            //Log.Message("GameCompTick");

        }

        public static int RequestCharge(int chargeToRequest)
        {
            if (GameComponent_QuantumShield.ChargeLevelCurrent > chargeToRequest)
            {
                GameComponent_QuantumShield.ChargeLevelCurrent -= chargeToRequest;
                return chargeToRequest;
            }
            else
            {
                int _Temp = GameComponent_QuantumShield.ChargeLevelCurrent;
                GameComponent_QuantumShield.ChargeLevelCurrent = 0;
                return _Temp;
            }
        }

        public static void ReturnCharge(int chargeLevel)
        {
            GameComponent_QuantumShield.ChargeLevelCurrent += chargeLevel;

            if (GameComponent_QuantumShield.ChargeLevelCurrent > GameComponent_QuantumShield.ChargeLevelMax)
            {
                GameComponent_QuantumShield.ChargeLevelCurrent = GameComponent_QuantumShield.ChargeLevelMax;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look<int>(ref GameComponent_QuantumShield.ChargeLevelCurrent, "ChargeLevelCurrent");
        }

        public static string GetInspectStringStatus()
        {
            return "Global Quantum Charge: " + GameComponent_QuantumShield.ChargeLevelCurrent.ToString() + " / " + GameComponent_QuantumShield.ChargeLevelMax.ToString();
        }
    }
}
