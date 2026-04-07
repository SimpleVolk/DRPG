using DRPG.Battle;
using UnityEngine;

namespace DRPG.UI
{
    public sealed class BattleHudPresenter : MonoBehaviour
    {
        [SerializeField] private BattleController battleController;

        public BattleController BattleController => battleController;

        // TODO: Bind runtime battle snapshot data to HUD widgets.
    }
}
