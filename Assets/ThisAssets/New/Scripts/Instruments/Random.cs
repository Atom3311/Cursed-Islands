using MathRandom = Unity.Mathematics.Random;
namespace CursedIsland
{
    public static class Random
    {
        public static MathRandom MainRandom = new MathRandom(1);
        public static int GetRandomNumber(int statr, int end)
        {
            int targetNumber = MainRandom.NextInt(statr, end);
            return targetNumber;
        }
    }
}