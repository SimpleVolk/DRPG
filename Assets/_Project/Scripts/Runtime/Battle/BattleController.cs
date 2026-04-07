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
        private BattleSnapshot _currentSnapshot;

        public event Action<BattleSnapshot> SnapshotUpdated;

        public BattleSnapshot CurrentSnapshot => _currentSnapshot;

        private void Awake()
        {
            _engine = new BattleEngine(new BattleStateMachine(), new DiceRoller());

            if (startOnAwake)
            {
                StartDefaultBattle();
            }
        }

        public void StartBattle(BattleContext context)
        {
            PublishSnapshot(_engine.StartBattle(context));
        }

        public void StartDefaultBattle()
        {
            StartBattle(CreateDefaultBattleContext());
        }

        public void RollButtonPressed()
        {
            PublishSnapshot(_engine.RollPlayerDice());
        }

        public void StopButtonPressed()
        {
            PublishSnapshot(_engine.StopPlayerTurn());
        }

        private BattleContext CreateDefaultBattleContext()
        {
            var player = new CombatantRuntime("player", playerMaxHp, playerDiceCount);
            var enemy = new CombatantRuntime("enemy", enemyMaxHp, 0);
            return new BattleContext(player, enemy, playerDiceDefinition, enemyAttackDamage);
        }

        private void PublishSnapshot(BattleSnapshot snapshot)
        {
            _currentSnapshot = snapshot;
            SnapshotUpdated?.Invoke(snapshot);
        }
    }
}
