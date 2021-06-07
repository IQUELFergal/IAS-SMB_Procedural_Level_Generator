using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New GameElementData", menuName = "LevelGenerator/GameElementData", order = 2)]
public class GameElementData : ScriptableObject
{
    [SerializeField] TileBase tile;
    public SizeSelector sizeSelector;
    Vector2Int size;
    [SerializeField] Vector2Int fixedSize;
    [SerializeField] Vector2Int minSize;
    [SerializeField] Vector2Int maxSize;

    public Vector2Int Size { get => (sizeSelector == SizeSelector.RandomRange ? new Vector2Int(Random.Range(minSize.x, maxSize.x + 1), Random.Range(minSize.y, maxSize.y + 1)):fixedSize); }
    public TileBase Tile { get => tile; set => tile = value; }
}
