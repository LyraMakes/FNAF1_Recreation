using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNAF1_Recreation
{
    class UI
    {
        public Texture2D loadTex;
        public Texture2D[] _usageTexMap;

        public Texture2D camTex;

        public int power;
        public int powerUsage;

        public int time;
        public int timeSeconds;

        double startTime;

        public bool BonnieInc = false;
        public bool ChicaInc = false;
        public bool FoxyInc = false;


        private int powerSecs;

        public UI()
        {
            power = 999;
            powerUsage = 1;
            time = 12;
            timeSeconds = 0;
            
            powerSecs = 0;
        }

        public void SetStartTime(GameTime gameTime) => startTime = gameTime.TotalGameTime.TotalSeconds;

        public void Reset()
        {
            power = 999;
            powerUsage = 1;
            time = 12;
            timeSeconds = 0;

            powerSecs = 0;
        }

        public void OnTick(GameTime gameTime)
        {
            // Reset Increment triggers
            BonnieInc = false; ChicaInc = false; FoxyInc = false;

            if (startTime + 1 < gameTime.TotalGameTime.TotalSeconds)
            {
                timeSeconds++;
                startTime = gameTime.TotalGameTime.TotalSeconds;
                power -= powerUsage;

            } 

            if (timeSeconds == 90 && time == 12)
            {
                timeSeconds = 0;
                time = 1;
            }

            if (timeSeconds == 89 && time != 12)
            {
                timeSeconds = 0;
                time++;

                BonnieInc = time == 2 || time == 3 || time == 4;
                ChicaInc = time == 3 || time == 4;
                FoxyInc = time == 3 || time == 4;
            }

            powerUsage = 1;
            if (Office.isLeftDoorClosed) powerUsage++;
            if (Office.isRightDoorClosed) powerUsage++;
            if (Office.isLeftLightOn || Office.isRightLightOn) powerUsage++;
            if (Office.isCamUp) powerUsage++;
        }
    }
}
