namespace DRPG.Core
{
    public sealed class GameFlowService
    {
        private readonly GameStateMachine _stateMachine;

        public GameFlowService(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public bool StartGame() => _stateMachine.TryTransitionTo(GameScene.MainMenu);
        public bool EnterHub() => _stateMachine.TryTransitionTo(GameScene.Hub);
        public bool EnterBattle() => _stateMachine.TryTransitionTo(GameScene.Battle);
        public bool ReturnToHub() => _stateMachine.TryTransitionTo(GameScene.Hub);
    }
}
