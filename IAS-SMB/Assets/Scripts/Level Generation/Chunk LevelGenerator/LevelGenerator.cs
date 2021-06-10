using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{

    public SizeSelector levelSizeSelector;
    [SerializeField] int tilemapCount;
    [SerializeField] int levelSize;
    [SerializeField] int minLevelSize;
    [SerializeField] int maxLevelSize;
    [SerializeField] Vector2Int chunkSize = Vector2Int.one;

    [SerializeField] bool useStartChunk = false;
    [SerializeField] bool useEndChunk = false;

    #region ChunkElement properties

        #region Platform properties

        [SerializeField] TileBase platformTile;
        [SerializeField] int maxHeightDifference = 3;
        int platformCount { get => platformCountSelector == SizeSelector.RandomRange ? Random.Range(platformMinCount, platformMaxCount + 1) : platformFixedCount; }

        public SizeSelector platformCountSelector;
        [Min(0)]
        [SerializeField] int platformFixedCount;
        [Min(0)]
        [SerializeField] int platformMinCount;
        [Min(0)]
        [SerializeField] int platformMaxCount;

        int platformHeight { get => platformHeightSelector == SizeSelector.RandomRange ? Random.Range(platformMinHeight, platformMaxHeight + 1) : platformFixedHeight; }

        public SizeSelector platformHeightSelector;
        [Min(1)]
        [SerializeField] int platformFixedHeight;
        [Min(1)]
        [SerializeField] int platformMinHeight;
        [Min(2)]
        [SerializeField] int platformMaxHeight;
        #endregion

        #region Gap properties

        [SerializeField] ChunkElementData gap;
        [SerializeField] TileBase gapTile;
        [Min(0)]
        [SerializeField] int numberOfGaps;

        int gapWidth { get => gapWidthSelector == SizeSelector.RandomRange ? Random.Range(gapMinWidth, gapMaxWidth + 1) : gapFixedWidth; }

        public SizeSelector gapWidthSelector;
        [Min(1)]
        [SerializeField] int gapFixedWidth;
        [Min(1)]
        [SerializeField] int gapMinWidth;
        [Min(2)]
        [SerializeField] int gapMaxWidth;
        #endregion

        #region Tube properties

        [SerializeField] TileBase tubeTile;
        [Min(0)]
        [SerializeField] int numberOfTubes;
        [SerializeField] int tubeMaxIteration;
        int tubeHeight { get => tubeHeightSelector == SizeSelector.RandomRange ? Random.Range(tubeMinHeight, tubeMaxHeight + 1) : tubeFixedHeight; }

        public SizeSelector tubeHeightSelector;
        [Min(1)]
        [SerializeField] int tubeFixedHeight;
        [Min(1)]
        [SerializeField] int tubeMinHeight;
        [Min(2)]
        [SerializeField] int tubeMaxHeight;
        #endregion

        #region Cannon properties

        [SerializeField] TileBase cannonTile;
        [Min(0)]
        [SerializeField] int numberOfCannons;
        [SerializeField] int cannonMaxIteration;
        int cannonHeight { get => cannonHeightSelector == SizeSelector.RandomRange ? Random.Range(cannonMinHeight, cannonMaxHeight + 1) : cannonFixedHeight; }

        public SizeSelector cannonHeightSelector;
        [Min(1)]
        [SerializeField] int cannonFixedHeight;
        [Min(1)]
        [SerializeField] int cannonMinHeight;
        [Min(2)]
        [SerializeField] int cannonMaxHeight;
    #endregion

        #region Coin properties
        #endregion
        #region Enemy properties
        #endregion
    #endregion


    public Chunk[] level;

    public RandomChunk rndChk;

    public void CreateLevel()
    {
        //Init seed
        SeedInitializer seedInitializer;
        if (TryGetComponent(out seedInitializer))
        {
            seedInitializer.InitSeed();
            Debug.Log("Creating level with seed " + seedInitializer.Seed);
        }
        

        //Build level 
        int size;
        switch (levelSizeSelector)
        {
            default:
                return;
            case SizeSelector.Fixed:
                size = levelSize;
                break;
            case SizeSelector.RandomRange:
                size = Random.Range(minLevelSize, maxLevelSize+1);
                break;
        }

        level = new Chunk[size];
        ChunkElement.SetChunkSize(chunkSize);

        for (int i = 0; i < level.Length; i++)
        {
            level[i] = CreateChunk(i);
        }



        //Draw level
        TilemapGenerator tilemapGenerator;
        if (TryGetComponent(out tilemapGenerator))
        {
            tilemapGenerator.GenerateTilemap(level, -chunkSize.x / 2, -chunkSize.y / 2);
        }
    }


    public Chunk CreateChunk(int chunkIndex)
    {
        List<ChunkElement> chunkElements = new List<ChunkElement>();
        int groundHeight = platformHeight;
        int startGroundHeight = 0;
        int endGroundHeight = 0;
        List<ChunkElement> platforms = new List<ChunkElement>();

        if (useStartChunk && chunkIndex == 0)
        {
            //Draw start chunk
            ChunkElement platform = new ChunkElement(0, platformTile, new Vector2Int(0, 0), new Vector2Int(chunkSize.x, platformHeight));
            platforms.Add(platform);
            chunkElements.AddRange(platforms);


        }
        else if (useEndChunk && chunkIndex == level.Length - 1)
        {
            //Draw end chunk
            ChunkElement platform = new ChunkElement(0, platformTile, new Vector2Int(0, 0), new Vector2Int(chunkSize.x, platformHeight));
            platforms.Add(platform);
            chunkElements.AddRange(platforms);
        }
        else 
        {
            //Draw random chunk

            #region Create platforms
            int platformNumber = platformCount;
            int extraTiles = chunkSize.x % platformNumber;
            int platformWidth = chunkSize.x / platformNumber;
            List<RectInt> platformBreakpointsRect = new List<RectInt>();

            for (int i = 0; i < platformNumber; i++)
            {
                int height = platformHeight;
                if (chunkIndex > 0)
                {
                    if (i > 0)
                    {
                        //clamp random height between height of the last platform in the same chunk +- maxHeightGap
                        height = Mathf.Clamp(height, platforms[platforms.Count - 1].Rect.height - maxHeightDifference, platforms[platforms.Count - 1].Rect.height + maxHeightDifference);
                    }
                    //clamp random height between height of the last platform of the last chunk +- maxHeightGap
                    else height = Mathf.Clamp(height, ((RandomChunk)level[chunkIndex - 1]).endGroundHeight - maxHeightDifference, ((RandomChunk)level[chunkIndex - 1]).endGroundHeight + maxHeightDifference);
                }
                

                //Get the platform breakpoints inside a chunk
                int breakPoint = (i + 1) * platformWidth + extraTiles;
                if (i > 0 && height != platforms[platforms.Count - 1].Rect.height)
                {
                    platformBreakpointsRect.Add(new RectInt(i* platformWidth + extraTiles, 0, 2, chunkSize.y));
                }
               
                
                RectInt platformRect = new RectInt(i * platformWidth + (i > 0 ? extraTiles : 0), 0, platformWidth + (i == 0 ? extraTiles : 0), height);
                ChunkElement platform = new ChunkElement(0, platformTile, platformRect);
                platforms.Add(platform);
            }
            chunkElements.AddRange(platforms);
            #endregion

            #region Create gaps
            List<ChunkElement> gaps = new List<ChunkElement>();
            for (int i = 0; i < numberOfGaps; i++)
            {
                int width = gapWidth;
                ChunkElement gap = new ChunkElement(0, gapTile, new Vector2Int(Random.Range(1, chunkSize.x - (width + 1)), 0), new Vector2Int(width, chunkSize.y));
                gaps.Add(gap);
            }
            chunkElements.AddRange(gaps);
            #endregion

            #region Create cannons
            List<ChunkElement> cannons = new List<ChunkElement>();
            int cannonWidth = 1;
            for (int i = 0, n = 0; i < cannonMaxIteration && n < numberOfCannons; i++)
            {
                int x = Random.Range(1, chunkSize.x - cannonWidth);
                RectInt cannonRect = new RectInt(x, GetChunkLocalHeight(x, platforms), cannonWidth, cannonHeight);
                ChunkElement cannon = new ChunkElement(0, cannonTile, cannonRect); 
                if (!cannon.isOverlapping(chunkElements) && !cannon.isOverlapping(platformBreakpointsRect))
                {
                    n++;
                    cannons.Add(cannon);
                }
            }
            chunkElements.AddRange(cannons);
            #endregion

            #region Create tubes
            List<ChunkElement> tubes = new List<ChunkElement>();
            int tubeWidth = 2;
            for (int i = 0, n = 0 ; i < tubeMaxIteration && n < numberOfTubes; i++)
            {
                int x = Random.Range(1, chunkSize.x - tubeWidth);
                RectInt tubeRect = new RectInt(x, GetChunkLocalHeight(x, platforms), tubeWidth, tubeHeight);
                ChunkElement tube = new ChunkElement(0, tubeTile, tubeRect);
                if (!tube.isOverlapping(chunkElements) && !tube.isOverlapping(tubes) && !tube.isOverlapping(platformBreakpointsRect))
                {
                    n++;
                    tubes.Add(tube);
                }
            }
            Debug.Log(tubeMaxIteration);
            chunkElements.AddRange(tubes);
            #endregion

            #region Create coins
            #endregion

            #region Create enemies
            #endregion
        }

        startGroundHeight = GetChunkLocalHeight(0, platforms);
        endGroundHeight = GetChunkLocalHeight(chunkSize.x - 1, platforms);

        Chunk chunk = new RandomChunk(chunkSize, startGroundHeight, endGroundHeight, chunkElements);
        return chunk;
    }
    int GetChunkLocalHeight(int xPos, List<ChunkElement> elementList)
    {
        foreach (var elem in elementList)
        {
            if (new RectInt(xPos, 0, 1, 1).Overlaps(elem.Rect))
            {
                return elem.Rect.height;
            }
        }
        Debug.LogError("No matching height was found.");
        return -1;
    }


    private void OnValidate()
    {
        if (chunkSize.x <= 0)
        {
            chunkSize.x = 1;
        }

        if (chunkSize.y <= 0)
        {
            chunkSize.y = 1;
        }


        if (useStartChunk != useEndChunk)
        {
            if (levelSize < 1) levelSize = 1;
            if (minLevelSize < 1) minLevelSize = 1;
        }


        if (useStartChunk && useEndChunk)
        {
            if (levelSize < 2) levelSize = 2;
            if (minLevelSize < 2) minLevelSize = 2;
        }


        if (minLevelSize > maxLevelSize) maxLevelSize = minLevelSize + 1;
    }

    private void OnDrawGizmosSelected()
    {
        if (level != null)
        {
            for (int i = 0; i < level.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(new Vector3(chunkSize.x * i, 0, 0), new Vector3(chunkSize.x, chunkSize.y, 0));
            }
        }
    }
}


