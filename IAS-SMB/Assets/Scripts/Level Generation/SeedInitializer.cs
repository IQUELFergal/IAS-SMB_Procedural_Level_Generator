using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedInitializer : MonoBehaviour
{
    [SerializeField] string stringSeed = "Seed string";
    [SerializeField] bool useStringSeed;
    [SerializeField] int seed;
    [SerializeField] bool randomizeSeed;

    public int Seed { get => seed; set => seed = value; }

    public void InitSeed()
    {
        if (useStringSeed)
        {
            Seed = stringSeed.GetHashCode();
        }

        if (randomizeSeed)
        {
            Seed = Random.Range(-9999999, 9999999);
        }

        Random.InitState(Seed);
    }
}
