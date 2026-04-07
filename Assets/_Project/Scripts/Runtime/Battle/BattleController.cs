using DRPG.Dice;
using UnityEngine;

namespace DRPG.Battle
{
    public sealed class BattleController : MonoBehaviour
    {
        private BattleEngine _engine;

        private void Awake()
        {
            _engine = new BattleEngine(new BattleStateMachine(), new DiceRoller());
        }

        public void StartBattle(BattleContext context)
        {
            _engine.StartBattle(context);
        }
    }
}
