using System;
using System.Collections.Generic;

namespace Queries
{
    public static class MyLinq
    {
        // Func<T,bool> predicate: a function that returns true if the condition/inline lambda expression
        // to delcare it from the calleer is true i.e to declare what's going to be filtered.
        public static IEnumerable<T> Filter<T> (this IEnumerable<T> source, Func<T,bool> predicate)
        {

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }

        }
    }
}
