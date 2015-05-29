using UnityEngine;
using System.Collections;
using Rnd = System.Random;

public class Random
{
    //private int _seed;
    private static Rnd _rnd;

    public Random(int seed)
    {
        //_seed = seed;
        _rnd = new Rnd(seed);
    }

    public bool RandomBernoulli(float p)
    {
        return _rnd.NextDouble() <= p;
    }

    public float RandomUniform(float a, float b)
    {
        return Range(a, b);
    }

    public static float Range(float a, float b)
    {
        if (_rnd == null) _rnd = new Rnd();
        return (float)_rnd.NextDouble() * (b - a) + a;
    }
}
