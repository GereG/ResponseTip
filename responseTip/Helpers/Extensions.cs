using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace responseTip.Helpers
{
    public static class Extensions
    {
        public static ICollection<T> Clone<T>(this ICollection<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
