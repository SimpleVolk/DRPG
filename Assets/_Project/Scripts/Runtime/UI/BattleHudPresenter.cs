using DRPG.Battle;
using UnityEngine;

namespace DRPG.UI
{
    public sealed class BattleHudPresenter : MonoBehaviour
    {
        [SerializeField] private BattleController battleController;

        [Header("Debug HUD")]
        [SerializeField] private string playerHpText;
        [SerializeField] private string enemyHpText;
        [SerializeField] private string accumulatedDamageText;
        [SerializeField] private string lastRollText;
        [SerializeField] private string bustText;

        public BattleController BattleController => battleController;

        private void OnEnable()
        {
            BindController();
        }

        private void OnDisable()
        {
            UnbindController();
        }

        private void HandleSnapshotUpdated(BattleSnapshot snapshot)
        {
            playerHpText = FormatPlayerHp(snapshot);
            enemyHpText = FormatEnemyHp(snapshot);
            accumulatedDamageText = FormatAccumulatedDamage(snapshot);
            lastRollText = FormatLastRoll(snapshot);
            bustText = snapshot.IsBust ? "BUST" : "Safe";

            Debug.Log($"{playerHpText} | {enemyHpText} | {accumulatedDamageText} | {lastRollText} | {bustText}");
        }

        private void BindController()
        {
            if (battleController != null)
            {
                battleController.SnapshotUpdated += HandleSnapshotUpdated;
            }
        }

        private void UnbindController()
        {
            if (battleController != null)
            {
                battleController.SnapshotUpdated -= HandleSnapshotUpdated;
            }
        }

        private static string FormatPlayerHp(BattleSnapshot snapshot)
        {
            return $"Player HP: {snapshot.PlayerHp}/{snapshot.PlayerMaxHp}";
        }

        private static string FormatEnemyHp(BattleSnapshot snapshot)
        {
            return $"Enemy HP: {snapshot.EnemyHp}/{snapshot.EnemyMaxHp}";
        }

        private static string FormatAccumulatedDamage(BattleSnapshot snapshot)
        {
            return $"Accumulated Damage: {snapshot.AccumulatedDamage}";
        }

        private static string FormatLastRoll(BattleSnapshot snapshot)
        {
            return $"Last Roll: {FormatRollValues(snapshot)}";
        }

        private static string FormatRollValues(BattleSnapshot snapshot)
        {
            if (snapshot.LastRoll?.Values == null || snapshot.LastRoll.Values.Length == 0)
            {
                return "-";
            }

            return string.Join(",", snapshot.LastRoll.Values);
        }
    }
}
