using System;
using System.Collections.Generic;

public static class ListUtil
{
    public static void Shuffle<T>(this List<T> list)
    {
        var random = new Random();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);

            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
