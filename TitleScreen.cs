using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNAF1_Recreation
{
    static class TitleScreen
    {
        public static Texture2D bgTex;
        public static Texture2D[] _texMap;

        public static Texture2D staticTex;
        public static Texture2D[] _staticTexMap;
        
        public static Texture2D staticBTex;
        public static Texture2D[] _staticBTexMap;

        public static Texture2D staticLineTex;
        public static int barPos = -20;

        public static int staticTimer = 0;
        public static bool staticOn = false;


        public static int selected = 0;
        public static int currentNight = 1;
        public static int numStars = 0;

        public static bool Night6Unlocked = false;
        public static bool Night7Unlocked = false;

        public static int texChanged;

        public static List<string> menuOptions;
    }
}
