using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    Tilemap tilemap;
    public bool destroyTile = false;
    public Vector2 offset = Vector2.zero;
    public GameObject gameObjectToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = GameObject.Find("SpawnedGameObjects");
        Instantiate(gameObjectToSpawn, transform.position + (Vector3)offset, Quaternion.identity, parent.transform);
        if (destroyTile)
        {
            tilemap = Object.FindObjectOfType(typeof(Tilemap)) as Tilemap;
            tilemap.SetTile(tilemap.WorldToCell(transform.position), null);
        }
    }
}
