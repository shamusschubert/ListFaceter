using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ListFaceter
{
    public static class CustomListExtensions
    {
        public static FacetedList<T> ToFacetedList<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            FacetedList<T> ret = new FacetedList<T>();

            foreach (var item in source)
                ret.Add(item);

            return ret;
        }
    }
}
