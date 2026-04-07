using UnityEngine;

namespace DRPG.Data
{
    [CreateAssetMenu(menuName = "DRPG/Data/Enemy Definition", fileName = "EnemyDefinition")]
    public sealed class EnemyDefinition : DefinitionBase
    {
        [SerializeField] private string displayName = string.Empty;
        [SerializeField] private int baseHp = 8;

        public string DisplayName => displayName;
        public int BaseHp => baseHp;
    }
}
