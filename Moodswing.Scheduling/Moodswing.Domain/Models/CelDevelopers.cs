using System.Collections.Generic;

namespace Moodswing.Domain.Models
{
    public class CelDevelopers
    {
        public IEnumerable<Developer> Developers { get; set; }

        public CelDevelopers(IEnumerable<Developer> developers)
        {
            Developers = developers;
        }
    }
}
