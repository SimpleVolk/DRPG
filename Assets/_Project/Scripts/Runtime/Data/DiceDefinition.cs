using UnityEngine;

namespace DRPG.Data
{
    [CreateAssetMenu(menuName = "DRPG/Data/Dice Definition", fileName = "DiceDefinition")]
    public sealed class DiceDefinition : DefinitionBase
    {
        [SerializeField] private int sides = 6;

        public int Sides => sides;
    }
}
