using DRPG.Entities;

namespace DRPG.Battle
{
    public sealed class BattleContext
    {
        public BattleContext(CombatantRuntime player, CombatantRuntime enemy)
        {
            Player = player;
            Enemy = enemy;
        }

        public CombatantRuntime Player { get; }
        public CombatantRuntime Enemy { get; }
    }
}
