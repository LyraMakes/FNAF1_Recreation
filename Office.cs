using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNAF1_Recreation
{
    class Office
    {
        public static Texture2D[] _officeTexMap;

        public static Texture2D officeTex;

        public static bool isLeftLightOn = false;
        public static bool isRightLightOn = false;

        public static bool hasBonnie = false;
        public static bool hasChica  = false;

        public static int xScroll = 150;

        public static int numSlices = 60;

        public static double[] yPosMap;

        public static Rectangle[] srcPosMap;


        public static Texture2D GetOfficeTex()
        {
            if (isLeftLightOn ) return _officeTexMap[hasBonnie ? 3 : 1];
            if (isRightLightOn) return _officeTexMap[hasChica  ? 4 : 2];

            return _officeTexMap[0];
        }
    }
}
