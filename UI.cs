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

        public UI()
        {
            power = 99;
            powerUsage = 1;
            time = 12;
            timeSeconds = 0;
        }

        public void SetStartTime(GameTime gameTime)
        {
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void Reset()
        {
            power = 99;
            powerUsage = 1;
            time = 12;
            timeSeconds = 0;
        }

        public void OnTick(GameTime gameTime)
        {
            if (startTime + 1 < gameTime.TotalGameTime.TotalSeconds)
            {
                timeSeconds++;
                startTime = gameTime.TotalGameTime.TotalSeconds;
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
            }

            powerUsage = 1;
            if (Office.isLeftDoorClosed) powerUsage++;
            if (Office.isRightDoorClosed) powerUsage++;
            if (Office.isLeftLightOn || Office.isRightLightOn) powerUsage++;
            if (Office.isCamUp) powerUsage++;
        }
    }
}
