using System;
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
            if (battleController != null)
            {
                battleController.SnapshotUpdated += HandleSnapshotUpdated;
            }
        }

        private void OnDisable()
        {
            if (battleController != null)
            {
                battleController.SnapshotUpdated -= HandleSnapshotUpdated;
            }
        }

        private void HandleSnapshotUpdated(BattleSnapshot snapshot)
        {
            playerHpText = $"Player HP: {snapshot.PlayerHp}/{snapshot.PlayerMaxHp}";
            enemyHpText = $"Enemy HP: {snapshot.EnemyHp}/{snapshot.EnemyMaxHp}";
            accumulatedDamageText = $"Accumulated Damage: {snapshot.AccumulatedDamage}";
            lastRollText = $"Last Roll: {FormatRoll(snapshot)}";
            bustText = snapshot.IsBust ? "BUST" : "Safe";

            Debug.Log($"{playerHpText} | {enemyHpText} | {accumulatedDamageText} | {lastRollText} | {bustText}");
        }

        private static string FormatRoll(BattleSnapshot snapshot)
        {
            if (snapshot.LastRoll?.Values == null || snapshot.LastRoll.Values.Length == 0)
            {
                return "-";
            }

            return string.Join(",", snapshot.LastRoll.Values);
        }
    }
}
