using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNAF1_Recreation
{
    class Camera
    {
        public static Texture2D[] _onTex;
        public static Texture2D[] _offTex;

        public static Texture2D GetCamTex(int cam, bool isActive) => isActive ? _onTex[cam] : _offTex[cam];
        public static Room CurrentRoom;
        public static int scroll;

        public static Texture2D[] TabletTexMap;
        public static int TabletTexOffset = -1;
        public static Texture2D TabletTex { get { return TabletTexMap[TabletTexOffset]; } }
        public static bool DrawCam { get { return TabletTexOffset != -1 && TabletTexOffset != 11; } }
    }
}
