using System;
using DRPG.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace DRPG.UI
{
    public sealed class BattleHudView : MonoBehaviour
    {
        [Header("Actions")]
        [SerializeField] private Button rollButton;
        [SerializeField] private Button stopButton;

        [Header("Readouts")]
        [SerializeField] private Text playerHpText;
        [SerializeField] private Text enemyHpText;
        [SerializeField] private Text accumulatedDamageText;
        [SerializeField] private Text lastRollText;
        [SerializeField] private Text bustStateText;
        [SerializeField] private Text phaseText;

        public event Action RollRequested;
        public event Action StopRequested;

        private void OnEnable()
        {
            if (rollButton != null)
            {
                rollButton.onClick.AddListener(HandleRollClicked);
            }

            if (stopButton != null)
            {
                stopButton.onClick.AddListener(HandleStopClicked);
            }
        }

        private void OnDisable()
        {
            if (rollButton != null)
            {
                rollButton.onClick.RemoveListener(HandleRollClicked);
            }

            if (stopButton != null)
            {
                stopButton.onClick.RemoveListener(HandleStopClicked);
            }
        }

        public void Render(BattleSnapshot snapshot)
        {
            SetText(playerHpText, $"Player HP: {snapshot.PlayerHp}/{snapshot.PlayerMaxHp}");
            SetText(enemyHpText, $"Enemy HP: {snapshot.EnemyHp}/{snapshot.EnemyMaxHp}");
            SetText(accumulatedDamageText, $"Accumulated Damage: {snapshot.AccumulatedDamage}");
            SetText(lastRollText, $"Last Roll: {FormatRollValues(snapshot)}");
            SetText(bustStateText, snapshot.IsBust ? "State: BUST" : "State: SAFE");
            SetText(phaseText, $"Phase: {snapshot.Phase}");

            var battleOver = snapshot.Phase == BattlePhase.Victory || snapshot.Phase == BattlePhase.Defeat;
            SetButtonsInteractable(snapshot.IsPlayerTurn && !battleOver);
        }

        private void HandleRollClicked()
        {
            RollRequested?.Invoke();
        }

        private void HandleStopClicked()
        {
            StopRequested?.Invoke();
        }

        private void SetButtonsInteractable(bool canAct)
        {
            if (rollButton != null)
            {
                rollButton.interactable = canAct;
            }

            if (stopButton != null)
            {
                stopButton.interactable = canAct;
            }
        }

        private static string FormatRollValues(BattleSnapshot snapshot)
        {
            if (snapshot.LastRoll?.Values == null || snapshot.LastRoll.Values.Length == 0)
            {
                return "-";
            }

            return string.Join(",", snapshot.LastRoll.Values);
        }

        private static void SetText(Text target, string value)
        {
            if (target != null)
            {
                target.text = value;
            }
        }
    }
}
