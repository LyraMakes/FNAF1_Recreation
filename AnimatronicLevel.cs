using System;
using System.Collections.Generic;
using System.Text;

namespace FNAF1_Recreation
{
    class AnimatronicLevel
    {
        private int value;

        public AnimatronicLevel() => value = 0;

        public AnimatronicLevel(int val) => value = BoundValue(val);

        private static int BoundValue(int val) => (val < 0) ? 0 : (val > 20) ? 20 : val;

        public static AnimatronicLevel[] NightPreset(int night, Rand r)
        {
            bool res = r.RandInt(1, 2) % 2 == 0;
            AnimatronicLevel[] freddy = new AnimatronicLevel[] { 0, 0, 1, (res) ? 1 : 2, 3, 4 };
            AnimatronicLevel[] bonnie = new AnimatronicLevel[] { 0, 3, 0, 2, 5, 10 };
            AnimatronicLevel[] chica = new AnimatronicLevel[] { 0, 1, 5, 4, 7, 12 };
            AnimatronicLevel[] foxy = new AnimatronicLevel[] { 0, 1, 2, 6, 5, 16 };

            return new AnimatronicLevel[] {freddy[night-1], bonnie[night - 1], chica[night - 1], foxy[night - 1] };

        }

        //Casting Overloads
        public static implicit operator int(AnimatronicLevel aL) => aL.value;
        public static implicit operator AnimatronicLevel(int i) => new AnimatronicLevel(i);

        public static AnimatronicLevel operator ++(AnimatronicLevel aL) => aL.value = BoundValue(aL.value + 1);
        public static AnimatronicLevel operator --(AnimatronicLevel aL) => aL.value = BoundValue(aL.value - 1);

        public static AnimatronicLevel operator +(AnimatronicLevel aL) => aL;
        public static AnimatronicLevel operator -(AnimatronicLevel _) => new AnimatronicLevel();

        public static AnimatronicLevel operator +(AnimatronicLevel aL1, AnimatronicLevel aL2)
            => new AnimatronicLevel(aL1.value + aL2.value);
        public static AnimatronicLevel operator -(AnimatronicLevel aL1, AnimatronicLevel aL2)
            => new AnimatronicLevel(aL1.value - aL2.value);

        public static AnimatronicLevel operator *(AnimatronicLevel aL1, AnimatronicLevel aL2)
            => new AnimatronicLevel(aL1.value * aL2.value);
        public static AnimatronicLevel operator /(AnimatronicLevel aL1, AnimatronicLevel aL2)
            => new AnimatronicLevel(aL1.value / aL2.value);

        public static AnimatronicLevel operator %(AnimatronicLevel aL1, AnimatronicLevel aL2)
            => new AnimatronicLevel(aL1.value % aL2.value);

        public static bool operator ==(AnimatronicLevel aL1, AnimatronicLevel aL2) => aL1.value == aL2.value;
        public static bool operator !=(AnimatronicLevel aL1, AnimatronicLevel aL2) => aL1.value != aL2.value;

        public static bool operator <=(AnimatronicLevel aL1, AnimatronicLevel aL2) => aL1.value <= aL2.value;
        public static bool operator >=(AnimatronicLevel aL1, AnimatronicLevel aL2) => aL1.value >= aL2.value;

        public static bool operator <(AnimatronicLevel aL1, AnimatronicLevel aL2) => aL1.value < aL2.value;
        public static bool operator >(AnimatronicLevel aL1, AnimatronicLevel aL2) => aL1.value > aL2.value;

        public static AnimatronicLevel operator <<(AnimatronicLevel aL, int offset)
            => new AnimatronicLevel(aL.value << offset);
        public static AnimatronicLevel operator >>(AnimatronicLevel aL, int offset)
            => new AnimatronicLevel(aL.value >> offset);

        public static bool operator true(AnimatronicLevel aL) => aL.value != 0;
        public static bool operator false(AnimatronicLevel aL) => aL.value == 0;

        public override string ToString() => $"{value}";
        public override bool Equals(object obj) => obj is AnimatronicLevel aL && value == aL.value;
        public override int GetHashCode() => value.GetHashCode();
        
    }
}
