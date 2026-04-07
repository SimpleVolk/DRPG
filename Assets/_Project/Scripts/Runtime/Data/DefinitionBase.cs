using UnityEngine;

namespace DRPG.Data
{
    public abstract class DefinitionBase : ScriptableObject
    {
        [SerializeField] private string id = string.Empty;

        public string Id => id;
    }
}
