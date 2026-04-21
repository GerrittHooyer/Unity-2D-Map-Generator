using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public enum MapTileID
{
    Floor = 0,
    Wall = 1
}

public class MapGenerator : MonoBehaviour
{
    private Tilemap _tilemap;
    [SerializeField] private List<TileBase> _wallTiles;
    [SerializeField] private List<TileBase> _floorTiles;
    [SerializeField] private Vector2Int _mapSize;
    [SerializeField] private Vector2Int _boundarySize;
    [SerializeField] private MapAlgorithm _algorithm;
    [SerializeField] string _seed;
    private bool _initialized = false;
    private MapTileID[,] _map;
    static readonly private Vector2Int[] _validDirections =
    {
        new Vector2Int(0, 1),      // North
        new Vector2Int(1, 0),      // East
        new Vector2Int(0, -1),     // South
        new Vector2Int(-1, 0),     // West
        new Vector2Int(1, 1),      // North East
        new Vector2Int(1, -1),     // South East
        new Vector2Int(-1, -1),    // South West
        new Vector2Int(-1, 1)     // North West
    };

    public Tilemap Tilemap { get { return _tilemap; } }
    public List<TileBase> WallTiles { get { return _wallTiles; } }
    public List<TileBase> FloorTiles { get { return _floorTiles; } }
    public Vector2Int MapSize { get { return _mapSize; } }

    public Vector2Int BoundarySize { get { return _boundarySize; } }
    public Vector2Int[] ValidDirections
    {
        get
        {
            return _validDirections;
        }
    }

    [SerializeField]
    public int WalkableTileCount
    {
        get
        {
            int count = 0;

            for (int x = 0; x < _map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < _map.GetUpperBound(1); y++)
                {
                    if (_map[x, y] == MapTileID.Floor) count++;
                }
            }
            return count;
        }
    }

    public MapTileID[,] Map { get { return _map; } set { _map = value; } }


    void Awake()
    {
        if (_seed.Length != 0)
        {
            Random.InitState(_seed.GetHashCode());
        }
        _tilemap = GetComponent<Tilemap>();
        _algorithm.Generator = this;
        _map = new MapTileID[MapSize.x, MapSize.y];

    }

    void Update()
    {
        if (!_initialized)
        {
            _algorithm.Generate();
            RenderMap();
            _initialized = true;
        }
    }

    public void RenderMap()
    {
        //Clear the map (ensures we dont overlap)
        _tilemap.ClearAllTiles();

        for (int x = 0; x < _map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < _map.GetUpperBound(1); y++)
            {
                if (_map[x, y] == MapTileID.Floor)
                {
                    SetTile(x, y, GetRandomTile(_floorTiles));
                }
                else if (_map[x, y] == MapTileID.Wall)
                {
                    SetTile(x, y, GetRandomTile(_wallTiles));
                }

            }
        }
    }

    public TileBase GetRandomTile(List<TileBase> tiles)
    {
        int i = Random.Range(0, tiles.Count);
        return tiles[i];
    }

    public void SetTile(int x, int y, TileBase tile)
    {
        _tilemap.SetTile(new Vector3Int(x, y, 0), tile);
    }

    public void FillGrid()
    {
        for (int x = 0; x < Map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < Map.GetUpperBound(1); y++)
            {
                Map[x, y] = MapTileID.Wall;
            }
        }
    }

    public void RandomizeGrid(float fillPercentage)
    {
        FillGrid();
        for (int x = BoundarySize.x; x < MapSize.x - BoundarySize.x; x++)
        {
            for (int y = BoundarySize.y; y < MapSize.y - BoundarySize.y; y++)
            {
                float c = Random.value;
                if (c < fillPercentage)
                {
                    Map[x, y] = MapTileID.Wall;
                }
                else
                {
                    Map[x, y] = MapTileID.Floor;
                }
            }
        }
    }

    public void DebugLogMapArray()
    {
        foreach (var tile in _map)
        {
            Debug.Log(tile);
        }
    }

    public bool IsPointValid (Vector2Int pos)
    {
        return pos.x > BoundarySize.x && pos.y > BoundarySize.y && pos.x <= MapSize.x - BoundarySize.x && pos.y <= MapSize.y - BoundarySize.y;
    }
}
