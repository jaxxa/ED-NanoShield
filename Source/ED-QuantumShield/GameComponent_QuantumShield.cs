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

        public GameComponent_QuantumShield(Game game)
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
