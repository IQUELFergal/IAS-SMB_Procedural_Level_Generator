using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameElementQuantifier
{
    [SerializeField] ChunkElementData data;
    [SerializeField] int tilemapIndex;
    [SerializeField] int minQuantity;
    [SerializeField] int maxQuantity;
}
