using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayUtils
{
    public string getClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private static ArrayUtils _instance;

    public static ArrayUtils instance
    {
        get
        {
            if (_instance == null)
                _instance = new ArrayUtils();
            return _instance;
        }
    }

    // ========== Public methods ==========
    public void Shuffle<T>(T[] array)
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
