using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayUtils
{
    // ========== Public methods ==========
    public static void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = Random.Range(0, n);
            n -= 1;
            T temp = array[k];
            array[k] = array[n];
            array[n] = temp;
        }
    }
}
