using FNAF1_Recreation.AudioSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace FNAF1_Recreation
{
    static class Office
    {
        public static Texture2D[] _officeTexMap;

        public static Texture2D[] _leftControlTexMap;
        public static Texture2D[] _rightControlTexMap;

        public static Texture2D LeftControlTex { get { return GetControlTex(_leftControlTexMap, isLeftDoorClosed, isLeftLightOn); } }
        public static Texture2D RightControlTex { get { return GetControlTex(_rightControlTexMap, isRightDoorClosed, isRightLightOn); } }

        public static Texture2D[] _leftDoorTexMap;
        public static Texture2D[] _rightDoorTexMap;


        public static Prop LeftControl;
        public static Prop RightControl;

        public static Prop LeftDoor;
        public static Prop RightDoor;


        public static int leftDoorTimer = 0;
        public static int rightDoorTimer = 0;

        public static int leftDoorTex = 0;
        public static int rightDoorTex = 0;

        public static bool changeLeftDoor = true;
        public static bool changeRightDoor = true; 

        public static bool isLeftDoorClosed = false;
        public static bool isRightDoorClosed = false;

        public static bool isLeftLightOn = false;
        public static bool isRightLightOn = false;

        public static bool isCamUp = false;
        public static bool hasLeftCamBox = true;

        public static int controlHeight = 280;

        public static int leftXPos = 10; 
        public static int rightXPos = 1480;

        public static bool doorHasBonnie = false;
        public static bool doorHasChica = false;

        public static int xScroll = 150;

        public static int numSlices = 2048;

        public static double[] yPosMap;

        public static Rectangle[] srcPosMap;

        public static Texture2D OfficeTex { get
        {
            if (isLeftLightOn) return _officeTexMap[doorHasBonnie ? 3 : 1];
            if (isRightLightOn) return _officeTexMap[doorHasChica ? 4 : 2];

            return _officeTexMap[0];
        } }

        private static Texture2D GetControlTex(Texture2D[] texMap, bool door, bool light)
        {
            return texMap[door ? ((light) ? 3 : 2) : ((light) ? 1 : 0)];
        }

        public static bool ToggleCam(SoundEffect sfx, AudioChannel chn)
        {
            Audio.Play(sfx, chn);
            isCamUp = !isCamUp;
            return isCamUp;
        }
    }
}
