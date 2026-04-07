using System;

namespace DRPG.Dice
{
    public sealed class DiceRoller
    {
        private readonly Random _random;

        public DiceRoller(int? seed = null)
        {
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        public int[] Roll(int diceCount)
        {
            if (diceCount <= 0)
            {
                return Array.Empty<int>();
            }

            var results = new int[diceCount];
            for (var i = 0; i < diceCount; i++)
            {
                results[i] = _random.Next(1, 7);
            }

            return results;
        }
    }
}
