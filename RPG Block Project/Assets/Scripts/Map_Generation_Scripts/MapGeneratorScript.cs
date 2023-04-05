using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MapGeneratorScript : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform navMeshPrefab;

    private NavMeshSurface surface;

    private string tileHolderName = "GeneratedMap";
    private string obstacleHolderName = "GeneratedObstacles";

    public Vector2 mapSize;
    public Vector2 maxMapSize;

    public int seed = 0;

    [Range(0, 1)]
    public float obstaclePercent;

    public float tileSize = 1;

    private List<TileCoordinates> arrayTileCoordinates;
    private Queue<TileCoordinates> arrayShuffledTileCoordinates;

    TileCoordinates mapCentre;

    public void GenerateMap()
    {

        arrayTileCoordinates = GenerateMapArray();
        arrayShuffledTileCoordinates = GenerateShuffledArrayObjects(arrayTileCoordinates);

        mapCentre = new TileCoordinates((int)mapSize.x / 2, (int)mapSize.y / 2);

        DestroyExistingHolderObjects(tileHolderName);
        GenerateNewMapHolder(tileHolderName);

        DestroyExistingHolderObjects(obstacleHolderName);
        GenerateObstacleHolderObjects(obstacleHolderName);

        GenerateParentNewNavMeshHolder();
    }

    private Queue<TileCoordinates> GenerateShuffledArrayObjects(List<TileCoordinates> arrayToShuffle)
    {
        Queue<TileCoordinates> shuffledArray = new Queue<TileCoordinates>();

        return shuffledArray = new Queue<TileCoordinates>(ShuffleArrayScript.ShuffleArray(arrayToShuffle.ToArray(), seed));
    }

    private void GenerateObstacleHolderObjects(string obstacleHolderName)
    {
        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

        int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
        int currentObstacleCount = 0;

        Transform obstacleHolderObject = new GameObject(obstacleHolderName).transform;
        obstacleHolderObject.parent = transform;

        for (int i = 0; i < obstacleCount; i++)
        {
            TileCoordinates obstacleCoordinate = GetRandomCoordinate();
            obstacleMap[obstacleCoordinate.x, obstacleCoordinate.y] = true;

            currentObstacleCount++;

            if (obstacleCoordinate != mapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
            {
                Vector3 obstaclePosition = ConvertCoordinateToVector3(obstacleCoordinate.x, obstacleCoordinate.y);
                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity) as Transform;
                newObstacle.localScale = Vector3.one * tileSize;
                newObstacle.parent = obstacleHolderObject;
            }
            else
            {
                obstacleMap[obstacleCoordinate.x, obstacleCoordinate.y] = false;
                currentObstacleCount--;
            }           
        }
    }

    private void GenerateNewMapHolder(string holderObjectName)
    {
        Transform newMapHolderObject = new GameObject(holderObjectName).transform;
        newMapHolderObject.parent = transform;
        newMapHolderObject.gameObject.layer = LayerMask.NameToLayer("Ground");
        newMapHolderObject.gameObject.isStatic = true;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = ConvertCoordinateToVector3(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * tileSize;
                newTile.parent = newMapHolderObject;
                newTile.gameObject.layer = newMapHolderObject.gameObject.layer;
                newTile.gameObject.isStatic = newMapHolderObject.gameObject.isStatic;
            }
        }
    }

    private void GenerateParentNewNavMeshHolder()
    {
        surface = transform.gameObject.GetComponentInParent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<TileCoordinates> queue = new Queue<TileCoordinates>();
        queue.Enqueue(mapCentre);
        mapFlags[mapCentre.x, mapCentre.y] = true;

        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            TileCoordinates tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new TileCoordinates(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    private void DestroyExistingHolderObjects(string holderObjectName)
    {
        if (transform.Find(holderObjectName))
        {
            DestroyImmediate(transform.Find(holderObjectName).gameObject);
        }
    }

    private List<TileCoordinates> GenerateMapArray()
    {
        List<TileCoordinates> generatedArray = new List<TileCoordinates>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                generatedArray.Add(new TileCoordinates(x, y));
            }
        }

        return generatedArray;
    }


    public TileCoordinates GetRandomCoordinate()
    {
        TileCoordinates selectedTileCoordinate = arrayShuffledTileCoordinates.Dequeue();
        arrayShuffledTileCoordinates.Enqueue(selectedTileCoordinate);
        return selectedTileCoordinate;
    }

    public struct TileCoordinates
    {
        public int x;
        public int y;

        public TileCoordinates(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public static bool operator ==(TileCoordinates c1, TileCoordinates c2)
        {
            return c1.x == c2.x && c1.y == c2.y;
        }

        public static bool operator !=(TileCoordinates c1, TileCoordinates c2)
        {
            return !(c1 == c2);
        }
    }

    public void GenerateNewSeedValue (int value)
    {
        seed = value;
    }

    public Vector3 GetMapCenter()
    {
        return ConvertCoordinateToVector3(mapCentre.x, mapCentre.y);
    }

    private Vector3 ConvertCoordinateToVector3(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f  + y) * tileSize;       
    }

    private void Start()
    {
        GenerateMap();
    }
}
