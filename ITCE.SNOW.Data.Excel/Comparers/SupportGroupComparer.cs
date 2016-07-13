using System.Collections.Generic;
using ITCE.SNOW.Dtos.V1;

namespace ITCE.SNOW.Data.Excel.Comparers
{
    public class SupportGroupComparer : IEqualityComparer<SupportGroup>
    {
        public bool Equals(SupportGroup x, SupportGroup y)
        {
            return x.Name == y.Name && x.Country == y.Country;
        }

        public int GetHashCode(SupportGroup obj)
        {
            return $"{obj.Name}{obj.Country}".GetHashCode();
        }
    }
}