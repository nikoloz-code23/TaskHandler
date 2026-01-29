using System;
using System.Collections.Generic;

namespace TaskTracker.Utilities;

public static class ListUtilities
{
    public static int IterateUntilPredicate<T>(List<T> list, Func<T, bool> predicate)
    {
        int listSize = list.Count;

        for(int i = 0; i < listSize; i++)
        {
            if (predicate(list[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static int IterateUntilPredicateReverse<T>(List<T> list, Func<T, bool> predicate)
    {
        int listSize = list.Count;

        for(int i = listSize-1; i >= 0; i--)
        {
            if (predicate(list[i]))
            {
                return i;
            }
        }

        return -1;
    }
}