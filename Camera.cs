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
    }
}
