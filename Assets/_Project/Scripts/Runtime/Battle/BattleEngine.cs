using DRPG.Dice;

namespace DRPG.Battle
{
    public sealed class BattleEngine
    {
        private readonly BattleStateMachine _stateMachine;
        private readonly DiceRoller _diceRoller;

        public BattleEngine(BattleStateMachine stateMachine, DiceRoller diceRoller)
        {
            _stateMachine = stateMachine;
            _diceRoller = diceRoller;
        }

        public void StartBattle(BattleContext context)
        {
            _stateMachine.TrySetPhase(BattlePhase.RoundStart);
            _diceRoller.Roll(context.Player.DiceCount);
        }

        // TODO: Move round processing to explicit command methods (Roll/Resolve/EndTurn).
    }
}
