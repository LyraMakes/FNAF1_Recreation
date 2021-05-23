using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace FNAF1_Recreation.Animatronics
{
    class Foxy : Animatronic
    {
        public double startTime;
        public int phase;
        public float frozenDelay;
        public int chargeNum;

        public double chargeStartTime;

        public Foxy(Rand rand) : base(rand)
        {
            movementOffset = 5.01;

            startTime = 0f;
            phase = 1;
            frozenDelay = 0f;
            chargeNum = 0;
            chargeStartTime = 0f;
        }

        public void OnTick(GameTime gameTime)
        {
            if (Office.isCamUp && frozenDelay == 0f) frozenDelay = rand.RandFloat(0.83f, 16.67f);

            double gTTGTTS = gameTime.TotalGameTime.TotalSeconds;
            if (startTime + frozenDelay < gTTGTTS)
            {
                startTime = gTTGTTS;
                frozenDelay = 0f;
            }

            if (moveStartTime + movementOffset < gTTGTTS) TryMove();
        }

        public void TryMove()
        {
            if (frozenDelay == 0 && MovementOpportunity())
            {
                if (phase < 4) phase++;
                else
                {
                    chargeNum++;
                }
            }
        }
    }
}
