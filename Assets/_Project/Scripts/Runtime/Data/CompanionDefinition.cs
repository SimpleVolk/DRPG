using UnityEngine;

namespace DRPG.Data
{
    [CreateAssetMenu(menuName = "DRPG/Data/Companion Definition", fileName = "CompanionDefinition")]
    public sealed class CompanionDefinition : DefinitionBase
    {
        [SerializeField] private string displayName = string.Empty;
        [SerializeField] private int baseHp = 10;
        [SerializeField] private DiceDefinition[] startingDice = { };

        public string DisplayName => displayName;
        public int BaseHp => baseHp;
        public DiceDefinition[] StartingDice => startingDice;
    }
}
