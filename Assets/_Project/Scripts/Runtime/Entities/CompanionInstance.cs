namespace DRPG.Entities
{
    public sealed class CompanionInstance
    {
        public CompanionInstance(string instanceId, string definitionId, int level = 1, int xp = 0)
        {
            InstanceId = instanceId;
            DefinitionId = definitionId;
            Level = level;
            Xp = xp;
        }

        public string InstanceId { get; }
        public string DefinitionId { get; }
        public int Level { get; private set; }
        public int Xp { get; private set; }

        public void GrantXp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            Xp += amount;
        }
    }
}
