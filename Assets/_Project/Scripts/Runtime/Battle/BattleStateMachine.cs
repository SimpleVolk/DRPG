namespace DRPG.Battle
{
    public sealed class BattleStateMachine
    {
        public BattlePhase CurrentPhase { get; private set; } = BattlePhase.BattleInit;

        public bool TrySetPhase(BattlePhase phase)
        {
            if (CurrentPhase == phase)
            {
                return false;
            }

            CurrentPhase = phase;
            return true;
        }
    }
}
