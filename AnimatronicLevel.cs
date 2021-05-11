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
