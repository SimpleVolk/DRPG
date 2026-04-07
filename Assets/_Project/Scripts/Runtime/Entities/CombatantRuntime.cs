namespace DRPG.Entities
{
    public sealed class CombatantRuntime
    {
        public CombatantRuntime(string id, int maxHp, int diceCount)
        {
            Id = id;
            MaxHp = maxHp;
            CurrentHp = maxHp;
            DiceCount = diceCount;
        }

        public string Id { get; }
        public int MaxHp { get; }
        public int CurrentHp { get; private set; }
        public int DiceCount { get; }

        public void ApplyDamage(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            CurrentHp -= amount;
            if (CurrentHp < 0)
            {
                CurrentHp = 0;
            }
        }
    }
}
