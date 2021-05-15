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

        public static Texture2D[] _leftControlTexMap;
        public static Texture2D[] _rightControlTexMap;

        public static Texture2D LeftControlTex { get { return GetControlTex(_leftControlTexMap, isLeftDoorClosed, isLeftLightOn); } }
        public static Texture2D RightControlTex { get { return GetControlTex(_rightControlTexMap, isRightDoorClosed, isRightLightOn); } }

        public static Prop LeftControl;
        public static Prop RightControl;

        public static int leftDoorTimer = 0;
        public static int rightDoorTimer = 0;

        public static bool isLeftDoorClosed = false;
        public static bool isRightDoorClosed = false;

        public static bool isLeftLightOn = false;
        public static bool isRightLightOn = false;

        public static int controlHeight = 280;

        public static int leftXPos = 10; 
        public static int rightXPos = 1480;


        public static bool hasBonnie = true;
        public static bool hasChica = true;

        public static int xScroll = 150;

        public static int numSlices = 60;

        public static double[] yPosMap;

        public static Rectangle[] srcPosMap;

        public static Texture2D OfficeTex { get
        {
            if (isLeftLightOn) return _officeTexMap[hasBonnie ? 3 : 1];
            if (isRightLightOn) return _officeTexMap[hasChica ? 4 : 2];

            return _officeTexMap[0];
        } }

        private static Texture2D GetControlTex(Texture2D[] texMap, bool door, bool light)
        {
            return texMap[door ? ((light) ? 3 : 2) : ((light) ? 1 : 0)];
        }
    }
}
