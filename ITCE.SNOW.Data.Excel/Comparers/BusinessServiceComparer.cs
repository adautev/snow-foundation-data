using System.Collections.Generic;
using ITCE.SNOW.Dtos.V1;

namespace ITCE.SNOW.Data.Excel.Comparers
{
    public class BusinessServiceComparer : IEqualityComparer<BusinessService>
    {
        public bool Equals(BusinessService x, BusinessService y)
        {
            return x.Name == y.Name && x.Country == y.Country;
        }

        public int GetHashCode(BusinessService obj)
        {
            return $"{obj.Name}{obj.Country}".GetHashCode();
        }
    }
}