using System;
using DRPG.Dice;

namespace DRPG.Battle
{
    public sealed class BattleEngine
    {
        private readonly BattleStateMachine _stateMachine;
        private readonly DiceRoller _diceRoller;

        private BattleContext _context;
        private DiceRollResult _lastRoll = DiceRollResult.Empty;
        private int _accumulatedDamage;
        private bool _isBust;
        private bool _isPlayerTurn;

        public BattleEngine(BattleStateMachine stateMachine, DiceRoller diceRoller)
        {
            _stateMachine = stateMachine;
            _diceRoller = diceRoller;
        }

        public BattleSnapshot StartBattle(BattleContext context)
        {
            _context = context;
            _lastRoll = DiceRollResult.Empty;
            _accumulatedDamage = 0;
            _isBust = false;
            _isPlayerTurn = true;

            _stateMachine.TrySetPhase(BattlePhase.RoundStart);
            _stateMachine.TrySetPhase(BattlePhase.PlayerRoll);

            return BuildSnapshot();
        }

        public BattleSnapshot RollPlayerDice()
        {
            if (!CanPlayerAct())
            {
                return BuildSnapshot();
            }

            var diceCount = Math.Clamp(_context.Player.DiceCount, 1, 3);
            var sides = _context.PlayerDiceDefinition == null ? 6 : Math.Max(2, _context.PlayerDiceDefinition.Sides);
            _lastRoll = _diceRoller.Roll(diceCount, sides);

            if (_lastRoll.IsBust)
            {
                _isBust = true;
                _accumulatedDamage = 0;
                ResolveEnemyTurn();
                return BuildSnapshot();
            }

            _isBust = false;
            _accumulatedDamage += _lastRoll.Sum;
            _stateMachine.TrySetPhase(BattlePhase.PlayerResolve);
            _stateMachine.TrySetPhase(BattlePhase.PlayerRoll);

            return BuildSnapshot();
        }

        public BattleSnapshot StopPlayerTurn()
        {
            if (!CanPlayerAct())
            {
                return BuildSnapshot();
            }

            _context.Enemy.ApplyDamage(_accumulatedDamage);
            _accumulatedDamage = 0;
            _isBust = false;

            if (_context.Enemy.CurrentHp <= 0)
            {
                _stateMachine.TrySetPhase(BattlePhase.Victory);
                _isPlayerTurn = false;
                return BuildSnapshot();
            }

            ResolveEnemyTurn();
            return BuildSnapshot();
        }

        // TODO: Integrate companion turn ordering when party combat is implemented.
        // TODO: Support advanced dice types/modifiers (rerolls, weighted faces, effects).
        // TODO: Apply status effect hooks during roll, resolve, and enemy turn.
        // TODO: Trigger capture system checks when enemy HP reaches threshold.

        private bool CanPlayerAct()
        {
            return _context != null &&
                   _isPlayerTurn &&
                   _stateMachine.CurrentPhase != BattlePhase.Victory &&
                   _stateMachine.CurrentPhase != BattlePhase.Defeat;
        }

        private void ResolveEnemyTurn()
        {
            _stateMachine.TrySetPhase(BattlePhase.EnemyTurn);
            _isPlayerTurn = false;

            _context.Player.ApplyDamage(_context.EnemyAttackDamage);

            if (_context.Player.CurrentHp <= 0)
            {
                _stateMachine.TrySetPhase(BattlePhase.Defeat);
                return;
            }

            _stateMachine.TrySetPhase(BattlePhase.RoundEnd);
            _stateMachine.TrySetPhase(BattlePhase.RoundStart);
            _stateMachine.TrySetPhase(BattlePhase.PlayerRoll);
            _isPlayerTurn = true;
        }

        private BattleSnapshot BuildSnapshot()
        {
            if (_context == null)
            {
                return new BattleSnapshot(
                    _stateMachine.CurrentPhase,
                    0,
                    0,
                    0,
                    0,
                    _accumulatedDamage,
                    _lastRoll,
                    _isBust,
                    _isPlayerTurn);
            }

            return new BattleSnapshot(
                _stateMachine.CurrentPhase,
                _context.Player.CurrentHp,
                _context.Player.MaxHp,
                _context.Enemy.CurrentHp,
                _context.Enemy.MaxHp,
                _accumulatedDamage,
                _lastRoll,
                _isBust,
                _isPlayerTurn);
        }
    }
}
