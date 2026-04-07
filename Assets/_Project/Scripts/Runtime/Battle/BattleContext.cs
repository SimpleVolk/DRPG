using DRPG.Data;
using DRPG.Entities;

namespace DRPG.Battle
{
    public sealed class BattleContext
    {
        public BattleContext(
            CombatantRuntime player,
            CombatantRuntime enemy,
            DiceDefinition playerDiceDefinition,
            int enemyAttackDamage)
        {
            Player = player;
            Enemy = enemy;
            PlayerDiceDefinition = playerDiceDefinition;
            EnemyAttackDamage = enemyAttackDamage;
        }

        public CombatantRuntime Player { get; }
        public CombatantRuntime Enemy { get; }
        public DiceDefinition PlayerDiceDefinition { get; }
        public int EnemyAttackDamage { get; }
    }
}
