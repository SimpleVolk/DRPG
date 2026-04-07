namespace DRPG.Core
{
    public sealed class GameStateMachine
    {
        public GameScene CurrentScene { get; private set; } = GameScene.Boot;

        public bool TryTransitionTo(GameScene nextScene)
        {
            if (CurrentScene == nextScene)
            {
                return false;
            }

            CurrentScene = nextScene;
            return true;
        }
    }
}
