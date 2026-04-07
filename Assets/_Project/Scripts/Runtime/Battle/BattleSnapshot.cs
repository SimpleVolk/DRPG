using DRPG.Dice;

namespace DRPG.Battle
{
    public sealed class BattleSnapshot
    {
        public BattleSnapshot(
            BattlePhase phase,
            int playerHp,
            int playerMaxHp,
            int enemyHp,
            int enemyMaxHp,
            int accumulatedDamage,
            DiceRollResult lastRoll,
            bool isBust,
            bool isPlayerTurn)
        {
            Phase = phase;
            PlayerHp = playerHp;
            PlayerMaxHp = playerMaxHp;
            EnemyHp = enemyHp;
            EnemyMaxHp = enemyMaxHp;
            AccumulatedDamage = accumulatedDamage;
            LastRoll = lastRoll ?? DiceRollResult.Empty;
            IsBust = isBust;
            IsPlayerTurn = isPlayerTurn;
        }

        public BattlePhase Phase { get; }
        public int PlayerHp { get; }
        public int PlayerMaxHp { get; }
        public int EnemyHp { get; }
        public int EnemyMaxHp { get; }
        public int AccumulatedDamage { get; }
        public DiceRollResult LastRoll { get; }
        public bool IsBust { get; }
        public bool IsPlayerTurn { get; }
    }
}
