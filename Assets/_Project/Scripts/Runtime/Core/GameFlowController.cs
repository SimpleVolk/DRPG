using UnityEngine;

namespace DRPG.Core
{
    public sealed class GameFlowController : MonoBehaviour
    {
        private GameFlowService _flowService;

        private void Awake()
        {
            var stateMachine = new GameStateMachine();
            _flowService = new GameFlowService(stateMachine);

            // TODO: Replace direct wiring with a persistent runtime composition root in Boot.
            _flowService.StartGame();
        }
    }
}
