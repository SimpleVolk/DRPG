using System.Collections.Generic;
using DRPG.Entities;

namespace DRPG.Roster
{
    public sealed class RosterModel
    {
        private readonly List<CompanionInstance> _companions = new();

        public IReadOnlyList<CompanionInstance> Companions => _companions;

        public bool AddCompanion(CompanionInstance companion)
        {
            if (companion == null || _companions.Exists(x => x.InstanceId == companion.InstanceId))
            {
                return false;
            }

            _companions.Add(companion);
            return true;
        }
    }
}
