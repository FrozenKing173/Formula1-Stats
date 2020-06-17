using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCode.Models;

namespace TestCode.Comparer
{
    public class TeamValueComparer : IComparer<TeamValue>
    {
        public int Compare(TeamValue x, TeamValue y)
        {
            if (x.TeamWinsPercentage == y.TeamWinsPercentage)
                return 0;
            if (x.TeamWinsPercentage > y.TeamWinsPercentage)
                return -1;
            else
                return 1;
        }
    }
}
