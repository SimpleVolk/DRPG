using System;
using DRPG.Data;
using DRPG.Dice;
using DRPG.Entities;
using UnityEngine;

namespace DRPG.Battle
{
    public sealed class BattleController : MonoBehaviour
    {
        [Header("Battle Setup")]
        [SerializeField] private bool startOnAwake = true;
        [SerializeField] private int playerMaxHp = 20;
        [SerializeField] private int playerDiceCount = 2;
        [SerializeField] private int enemyMaxHp = 18;
        [SerializeField] private int enemyAttackDamage = 3;
        [SerializeField] private DiceDefinition playerDiceDefinition;

        private BattleEngine _engine;

        public event Action<BattleSnapshot> SnapshotUpdated;

        private void Awake()
        {
            _engine = new BattleEngine(new BattleStateMachine(), new DiceRoller());

            if (startOnAwake)
            {
                StartBattle(CreateDefaultContext());
            }
        }

        public void StartBattle(BattleContext context)
        {
            var snapshot = _engine.StartBattle(context);
            SnapshotUpdated?.Invoke(snapshot);
        }

        public void RollButtonPressed()
        {
            var snapshot = _engine.RollPlayerDice();
            SnapshotUpdated?.Invoke(snapshot);
        }

        public void StopButtonPressed()
        {
            var snapshot = _engine.StopPlayerTurn();
            SnapshotUpdated?.Invoke(snapshot);
        }

        private BattleContext CreateDefaultContext()
        {
            var player = new CombatantRuntime("player", playerMaxHp, playerDiceCount);
            var enemy = new CombatantRuntime("enemy", enemyMaxHp, 0);
            return new BattleContext(player, enemy, playerDiceDefinition, enemyAttackDamage);
        }
    }
}
