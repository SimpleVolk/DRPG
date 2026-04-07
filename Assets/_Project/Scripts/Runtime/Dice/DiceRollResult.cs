using System;

namespace DRPG.Dice
{
    public sealed class DiceRollResult
    {
        public static readonly DiceRollResult Empty = new(Array.Empty<int>(), 0, false);

        public DiceRollResult(int[] values, int sum, bool isBust)
        {
            Values = values ?? Array.Empty<int>();
            Sum = sum;
            IsBust = isBust;
        }

        public int[] Values { get; }
        public int Sum { get; }
        public bool IsBust { get; }
    }
}
