using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapLocation
{
    public int x;
    public int y;

    public MapLocation(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}

/// <summary>
/// If position is 0 then draw room if 1 then empty
/// </summary>
public class Maze : MonoBehaviour
{
    [SerializeField] private GameObject gameSession;
    [SerializeField] private Recursion recursion;
    public GameObject positionHolder;

    public List<MapLocation> directions = new List<MapLocation>() {
                                           new MapLocation(1, 0),
                                           new MapLocation(0, 1),
                                           new MapLocation(-1, 0),
                                           new MapLocation(0, -1) };

    public List<MapLocation> mainPath = new List<MapLocation>();

    /// <summary>
    /// Rooms:
    /// 0 RoomLR
    /// 1 RoomBT
    /// 2 RoomLB
    /// 3 RoomLT
    /// 4 RoomRB
    /// 5 RoomRT
    /// 6 RoomLRB
    /// 7 RoomLRT
    /// 8 RoomRBT
    /// 9 RoomLBT
    /// 10 RoomLRBT
    /// </summary>
    public GameObject[] rooms;
    [SerializeField] private GameObject[] walls;
    [SerializeField] private float moveAmount;

    public int width = 5;
    public int height = 5;
    public float scale = 10f;

    private float timeBetweenRoom;
    public float startTimeBetweenRoom = 0.25f;

    public byte[,] map;

    public bool mapInitialized = false;
    public bool stopGeneration;

    public LayerMask layerMask;

    public void Start()
    {
        InitializeMap();
    }

    public void Update()
    {
        
    }

    public void InitializeMap()
    {
        Vector2 newPos;

        // Add bordering to match size of dungeon
        width = width + 2;
        height = height + 2;

        map = new byte[width, height];
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, y] = 1;

                newPos = new Vector2(5 + (x * moveAmount), 5 + (y * moveAmount));

                GameObject instance = (GameObject)Instantiate(positionHolder, newPos, Quaternion.identity); // Instantiate all position holders
                instance.transform.SetParent(transform);
                instance.GetComponent<SpawnRandRoom>().recursion = recursion;
                instance.GetComponent<SpawnRandRoom>().xPos = x;
                instance.GetComponent<SpawnRandRoom>().yPos = y;
                instance.GetComponent<SpawnRandRoom>().xMax = width;
                instance.GetComponent<SpawnRandRoom>().yMax = height;
            }
        }

        // Call Generate
        Generate();
        DrawMap();
        mapInitialized = true;
    }

    /// <summary>
    /// Virtual generate function for recursion
    /// </summary>
    public virtual void Generate()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    public void DrawMap()
    {
        if (mapInitialized == true)
        {
            for (int i = 0; i < mainPath.Count; i++)
            {
                int x = mainPath[i].x;
                int y = mainPath[i].y;

                if (map[x, y] == 1)
                {
                    return;
                }
                else if (search2D(x, y, new int[] {
                                                    5, 0, 5,
                                                    1, 0, 1,
                                                    5, 0, 5 }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[5], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    5, 1, 5,
                                                    0, 0, 0,
                                                    5, 1, 5
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[4], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    1, 0, 1,
                                                    0, 0, 0,
                                                    1, 0, 1
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[14], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    5, 0, 1,
                                                    1, 0, 0,
                                                    5, 1, 5
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[9], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    1, 0, 5,
                                                    0, 0, 1,
                                                    5, 1, 5
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[7], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    5, 1, 5,
                                                    1, 0, 0,
                                                    5, 0, 1
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[8], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    5, 1, 5,
                                                    0, 0, 1,
                                                    1, 0, 5
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[6], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    1, 0, 1,
                                                    0, 0, 0,
                                                    5, 1, 5
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[11], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    5, 1, 5,
                                                    0, 0, 0,
                                                    1, 0, 1
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[10], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    5, 0, 1,
                                                    1, 0, 0,
                                                    5, 0, 1
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[12], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    1, 0, 5,
                                                    0, 0, 1,
                                                    1, 0, 5
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[13], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    5, 1, 5,
                                                    0, 0, 1,
                                                    5, 1, 5
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[0], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    5, 1, 5,
                                                    1, 0, 0,
                                                    5, 1, 5
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[1], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    5, 1, 5,
                                                    1, 0, 1,
                                                    5, 0, 5
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[2], pos, Quaternion.identity);
                }
                else if (search2D(x, y, new int[] {
                                                    5, 0, 5,
                                                    1, 0, 1,
                                                    5, 1, 5
                                                    }))
                {
                    Vector2 pos = new Vector2(5 + (x * scale), 5 + (y * scale));
                    Instantiate(rooms[3], pos, Quaternion.identity);
                }
            }
        }

        stopGeneration = true;
        Vector2 startingPos = new Vector2(15f, 45f);
        gameSession.GetComponent<GameSession>().InstantiatePlayer(startingPos);
    }

    bool search2D(int col, int row, int[] pattern)
    {
        int count = 0;
        int pos = 0;

        for (int y = 1; y > -2; y--)
        {
            for (int x = -1; x < 2; x++)
            {
                if (pattern[pos] == map[col + x, row + y] || pattern[pos] == 5)
                {
                    count++;
                }
                pos++;
            }
        }

        return (count == 9);
    }

    /// <summary>
    /// Counts number of square neighbors
    /// </summary>
    public int countSquareNeighbors(int x, int y)
    {
        int count = 0;

        if (x <= 0 || x >= width - 1 || y <= 0 || y >= height - 1) return 5;
        if (map[x - 1, y] == 0) count++;
        if (map[x + 1, y] == 0) count++;
        if (map[x, y - 1] == 0) count++;
        if (map[x, y + 1] == 0) count++;

        return count;
    }
}
