using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FNAF1_Recreation
{
    class Room
    {
        public static Room[] rooms;

        public string name;
        public Texture2D roomTex;
        public List<Animatronic> animatronics;

        public Room(string n)
        {
            name = n;
            animatronics = new List<Animatronic>();
        }

        public void AddAnimatronic(Animatronic a)
        {
            animatronics.Add(a);
        }

        public void RemoveAnimatronic(Animatronic a)
        {
            animatronics.Remove(a);
        }

        public void RemoveAnimatronic(int indx)
        {
            animatronics.RemoveAt(indx);
        }

        public void SetTexture(Texture2D tex) => roomTex = tex;

        public override string ToString() => name;
    }
}
