using DRPG.Battle;
using UnityEngine;

namespace DRPG.UI
{
    public sealed class BattleHudPresenter : MonoBehaviour
    {
        [SerializeField] private BattleController battleController;
        [SerializeField] private BattleHudView hudView;

        private void OnEnable()
        {
            BindController();
            BindView();
            RefreshFromCurrentSnapshot();
        }

        private void OnDisable()
        {
            UnbindView();
            UnbindController();
        }

        private void HandleSnapshotUpdated(BattleSnapshot snapshot)
        {
            if (hudView == null)
            {
                return;
            }

            hudView.Render(snapshot);
        }

        private void HandleRollRequested()
        {
            battleController?.RollButtonPressed();
        }

        private void HandleStopRequested()
        {
            battleController?.StopButtonPressed();
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

        private void BindView()
        {
            if (hudView != null)
            {
                hudView.RollRequested += HandleRollRequested;
                hudView.StopRequested += HandleStopRequested;
            }
        }

        private void UnbindView()
        {
            if (hudView != null)
            {
                hudView.RollRequested -= HandleRollRequested;
                hudView.StopRequested -= HandleStopRequested;
            }
        }

        private void RefreshFromCurrentSnapshot()
        {
            if (battleController?.CurrentSnapshot != null)
            {
                HandleSnapshotUpdated(battleController.CurrentSnapshot);
            }
        }
    }
}
