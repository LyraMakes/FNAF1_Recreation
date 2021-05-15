using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNAF1_Recreation
{
    class Prop
    {
        private Texture2D tex;

        private Rectangle[] srcMap;

        public Prop() { }

        public Prop(Texture2D tex, int width) => SetTex(tex, width);

        public void SetTex(Texture2D tex, int width)
        {
            this.tex = tex;

            int numRects = tex.Width / width;

            srcMap = new Rectangle[numRects + 1];

            for (int i = 0; i < numRects + 1; i++)
            {
                srcMap[i] = new Rectangle(i * width, 0, width, tex.Height);
            }
            //srcMap[numRects]
        }

        public void Draw (SpriteBatch spriteLayer, Vector2 pos, int numSlices)
        {
            for (int i = 0; i < srcMap.Length; i++)
            {
                //if (pos.X + tex.Width < i * srcMap[0].Width) continue;
                //if (pos.X > i * srcMap[0].Width) continue;

                float yOffset = Math.Abs((numSlices / 2) - i) / (float)(numSlices / 2);
                Vector2 newPos = new Vector2(srcMap[i].X + pos.X, pos.Y + tex.Height / 2);
                spriteLayer.Draw(
                    tex,
                    newPos,
                    srcMap[i],
                    Color.White,
                    0,
                    new Vector2(0, tex.Height / 2),
                    new Vector2(1, LMath.GetYScale(yOffset)),
                    SpriteEffects.None,
                    0
                );
            }
        }
    }
}
