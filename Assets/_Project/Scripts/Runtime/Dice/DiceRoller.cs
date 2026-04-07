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

        public DiceRollResult Roll(int diceCount, int sides)
        {
            if (diceCount <= 0 || sides <= 1)
            {
                return DiceRollResult.Empty;
            }

            var results = new int[diceCount];
            var sum = 0;
            var isBust = false;

            for (var i = 0; i < diceCount; i++)
            {
                var value = _random.Next(1, sides + 1);
                results[i] = value;
                sum += value;

                if (value == 1)
                {
                    isBust = true;
                }
            }

            return new DiceRollResult(results, sum, isBust);
        }
    }
}
