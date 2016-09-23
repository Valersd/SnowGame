using System;
using System.Collections.Generic;

namespace Snow.Core
{
    public static class HelperMethods
    {
        public static void Fill(this IList<int> collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i] = i;
            }
        }

        public static void Shuffle(this IList<int> collection, Random random)
        {
            for (int i = 0; i < collection.Count - 1; i++)
            {
                int positionForSwitch = random.Next(i + 1, collection.Count);
                int temp = collection[i];
                collection[i] = collection[positionForSwitch];
                collection[positionForSwitch] = temp;
            }
        }
    }
}
