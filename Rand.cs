using System;

namespace FNAF1_Recreation
{
    class Rand
    {
        private readonly Random random;
        public Rand()
        {
            random = new Random();
        }

        public Rand(int seed)
        {
            random = new Random(seed);
        }

        public int RandInt(int min, int max) => random.Next(min, max + 1);

        public int Next() => RandInt(1, 20);

        public float RandFloat(float min, float max) => (float)((random.NextDouble() * (max + 0.01f - min)) + min);
    }
}
