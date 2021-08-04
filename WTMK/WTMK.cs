using System.Collections.Generic;

public sealed class WTMK
{
    private static readonly WTMK _Instance = new WTMK();
    private System.Random _Rando = new System.Random();

    public static WTMK Instance 
    { 
        get { return _Instance; }
    }

    public System.Random Rando
    {
        get { return _Rando; }
    }

    public int Pick(int max)
    {
        return _Rando.Next(max);
    }

    public int Range(int min, int max)
    {
        return _Rando.Next(min,max);
    }

    public void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _Rando.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}