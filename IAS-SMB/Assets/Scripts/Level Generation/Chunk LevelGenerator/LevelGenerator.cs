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

        #region Blocks properties
        [SerializeField] TileBase blockTile;
        [SerializeField] TileBase brickBlockTile;
        [SerializeField] TileBase yellowBlockTile;
        [SerializeField][Range(0,1)] float yellowBlockProbability;
        [SerializeField] int blockHeightFromPlatform;
        int blockWidth { get => blockWidthSelector == SizeSelector.RandomRange ? Random.Range(blockMinWidth, blockMaxWidth + 1) : blockFixedWidth; }

        public SizeSelector blockWidthSelector;
        [Min(1)]
        [SerializeField] int blockFixedWidth;
        [Min(1)]
        [SerializeField] int blockMinWidth;
        [Min(2)]
        [SerializeField] int blockMaxWidth;

    #endregion

        #region Coin properties
        [SerializeField] TileBase coinTile;
        [SerializeField][Range(0,1)] float extraCoinProbability;
        #endregion

        #region Enemy properties
        TileBase[] enemyTiles;
        [SerializeField] TileBase goombaTile;
        [SerializeField] TileBase koopaTile;
        int enemyCount { get => enemyCountSelector == SizeSelector.RandomRange ? Random.Range(enemyMinCount, enemyMaxCount + 1) : enemyFixedCount; }

        public SizeSelector enemyCountSelector;
        [Min(0)]
        [SerializeField] int enemyFixedCount;
        [Min(0)]
        [SerializeField] int enemyMinCount;
        [Min(0)]
        [SerializeField] int enemyMaxCount;
        #endregion

        #region Structures properties
        [SerializeField] TileBase playerSpawnerTile;
        [SerializeField] TileBase endFlagTile;
        [SerializeField] TileBase endCastleTile;
        #endregion

    #endregion


    public Chunk[] level;

    public RandomChunk rndChk;

    private void SetupEnemyPool()
    {
        enemyTiles = new TileBase[2];
        enemyTiles[0] = goombaTile;
        enemyTiles[1] = koopaTile;
    }

    public void CreateLevel()
    {
        //Init seed
        SeedInitializer seedInitializer;
        if (TryGetComponent(out seedInitializer))
        {
            seedInitializer.InitSeed();
            Debug.Log("Creating level with seed " + seedInitializer.Seed);
        }

        SetupEnemyPool();

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
        int endGroundHeight = 0;
        List<ChunkElement> platforms = new List<ChunkElement>();
        List<ChunkElement> structures = new List<ChunkElement>();

        if (useStartChunk && chunkIndex == 0)
        {
            //Draw start chunk
            int height = platformHeight;
            ChunkElement platform = new ChunkElement(0, platformTile, Vector2Int.zero, new Vector2Int(chunkSize.x, height));
            platforms.Add(platform);
            chunkElements.AddRange(platforms);

            ChunkElement playerSpawner = new ChunkElement(0, playerSpawnerTile, new Vector2Int(chunkSize.x / 2, height), Vector2Int.one);
            chunkElements.Add(playerSpawner);
        }
        else if (useEndChunk && chunkIndex == level.Length - 1)
        {
            //Draw end chunk
            int lastChunkHeight = ((RandomChunk)level[level.Length - 2]).endGroundHeight;
            ChunkElement platform = new ChunkElement(0, platformTile, Vector2Int.zero, new Vector2Int(chunkSize.x, lastChunkHeight));
            platforms.Add(platform);
            chunkElements.AddRange(platforms);

            ChunkElement endFlagBlock = new ChunkElement(0, blockTile, new Vector2Int(chunkSize.x/3, lastChunkHeight), new Vector2Int(1, 1));
            structures.Add(endFlagBlock);
            ChunkElement endFlag = new ChunkElement(0, endFlagTile, new Vector2Int(chunkSize.x/3, lastChunkHeight + 1), new Vector2Int(1, 7));
            structures.Add(endFlag);
            ChunkElement endCastle = new ChunkElement(0, endCastleTile, new Vector2Int(2 * chunkSize.x / 3, lastChunkHeight), Vector2Int.one);
            structures.Add(endCastle);
            chunkElements.AddRange(structures);
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
            chunkElements.AddRange(tubes);
            #endregion

            #region Create coins
            List<ChunkElement> coins = new List<ChunkElement>();
            int coinHeight = 1;
            for (int i = 0; i < gaps.Count; i++)
            {
                RectInt coinRect = new RectInt(gaps[i].Rect.x, GetChunkLocalHeight(gaps[i].Rect.x-1, platforms) + 4 , gaps[i].Rect.width, coinHeight);
                ChunkElement coinRow = new ChunkElement(0, coinTile, coinRect);
                coins.Add(coinRow);
                if (Random.Range(0f, 1f) < extraCoinProbability)
                {
                    coinRect = new RectInt(gaps[i].Rect.x-1, GetChunkLocalHeight(gaps[i].Rect.x - 1, platforms) + 3, 1, coinHeight);
                    ChunkElement frontCoin = new ChunkElement(0, coinTile, coinRect);
                    coins.Add(frontCoin);
                }
                if (Random.Range(0f, 1f) < extraCoinProbability)
                {
                    coinRect = new RectInt(gaps[i].Rect.x+ gaps[i].Rect.width, GetChunkLocalHeight(gaps[i].Rect.x - 1, platforms) + 3, 1, coinHeight);
                    ChunkElement backCoin = new ChunkElement(0, coinTile, coinRect);
                    coins.Add(backCoin);
                }
            }
            chunkElements.AddRange(coins);
            #endregion

            #region Create blocks
            List<ChunkElement> blocks = new List<ChunkElement>();
            List<ChunkElement> yellowBlocks = new List<ChunkElement>();
            if ((numberOfTubes + numberOfCannons - (tubes.Count + cannons.Count)) > 0)
            {
                int blockHeight = 1;
                for (int i = 0; i < 2; i++)
                {
                    int width = blockWidth;
                    int xPos = Random.Range(1, chunkSize.x - (width + 1));
                    int yPos = GetChunkLocalHeight(xPos, platforms) + blockHeightFromPlatform;
                    RectInt blockRect = new RectInt(xPos, yPos, width, blockHeight);
                    ChunkElement blockRow = new ChunkElement(0, brickBlockTile, blockRect);

                    if (!blockRow.isOverlapping(chunkElements) && !blockRow.isOverlapping(blocks))
                    {
                        blocks.Add(blockRow);
                    }
                }

                foreach (var blockRow in blocks)
                {
                    if (Random.Range(0f, 1f) < yellowBlockProbability)
                    {
                        RectInt yellowBlockRect = new RectInt(Random.Range(blockRow.Rect.x, blockRow.Rect.x + blockRow.Rect.width), blockRow.Rect.y, 1, 1);
                        ChunkElement yellowBlock = new ChunkElement(0, yellowBlockTile, yellowBlockRect);
                        yellowBlocks.Add(yellowBlock);
                    }
                }
                chunkElements.AddRange(blocks);
                chunkElements.AddRange(yellowBlocks);
            }
            #endregion

            #region Create enemies
            List<ChunkElement> enemySpawners = new List<ChunkElement>();
            List<ChunkElement> spawnableElements = new List<ChunkElement>();
            spawnableElements.AddRange(platforms);
            spawnableElements.AddRange(tubes);
            spawnableElements.AddRange(blocks);
            for (int i = 0; i < enemyCount; i++)
            {
                ChunkElement spawnElement = spawnableElements[Random.Range(0, spawnableElements.Count)];
                int xPos = Random.Range(spawnElement.Rect.x, spawnElement.Rect.x + spawnElement.Rect.width);
                RectInt enemyRect = new RectInt(xPos, spawnElement.Rect.y + spawnElement.Rect.height, 1, 1);
                ChunkElement enemySpawner = new ChunkElement(0, enemyTiles[Random.Range(0, enemyTiles.Length)], enemyRect);
                if (!enemySpawner.isOverlapping(enemySpawners) && !enemySpawner.isOverlapping(chunkElements))
                {
                    enemySpawners.Add(enemySpawner);
                }
            }
            chunkElements.AddRange(enemySpawners);
            #endregion
        }
        
        endGroundHeight = GetChunkLocalHeight(chunkSize.x - 1, platforms);

        Chunk chunk = new RandomChunk(chunkSize, endGroundHeight, chunkElements);
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
}


