using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace ED_QuantumShield
{
    class GameComponent_QuantumCharge : Verse.GameComponent
    {

        public GameComponent_QuantumCharge(Game game)
        {
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

            //Log.Message("GameCompTick");
        }
    }
}
