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

        public int power;
        public int time;
        int timeSeconds;

        double startTime;

        public UI()
        {
            power = 100;
            time = 12;
        }

        public void SetStartTime(GameTime gameTime)
        {
            startTime = gameTime.TotalGameTime.TotalSeconds;
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
        }
    }
}
