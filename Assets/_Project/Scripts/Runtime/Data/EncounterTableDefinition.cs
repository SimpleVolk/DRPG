using UnityEngine;

namespace DRPG.Data
{
    [CreateAssetMenu(menuName = "DRPG/Data/Encounter Table Definition", fileName = "EncounterTableDefinition")]
    public sealed class EncounterTableDefinition : DefinitionBase
    {
        [SerializeField] private Entry[] entries = { };

        public Entry[] Entries => entries;

        [System.Serializable]
        public struct Entry
        {
            public EnemyDefinition Enemy;
            public int Weight;
        }
    }
}
