using UnityEngine;
using System.Collections.Generic;
using UnityEngine.LightTransport.PostProcessing;
using System.Security.Cryptography;

[CreateAssetMenu(fileName = "DrunkardsWalk", menuName = "Scriptable Objects/MapAlgorithm/DrunkardsWalk")]
public class DrunkardsWalk : MapAlgorithm
{
    [SerializeField] int _iterations;
    [SerializeField] Vector2Int _startPoint;
    [SerializeField] int _maxSteps;
    [SerializeField] bool _resetMap = true;
    
    private List<Vector2Int> _visited;

    public void Awake()
    {
        _visited = new List<Vector2Int>();
    }

    public override void Generate()
    {
        _visited = new List<Vector2Int>();
        if (_resetMap)
        {
            _generator.FillGrid();
        }

        int iteration = 0;
        Vector2Int pos = _startPoint;


        SetTile(pos, MapTileID.Floor);
        while (iteration < _iterations)
        {
            RandomWalk(pos);
            pos = GetRandomStartFromVisited();
            iteration++;
        }
    }

    private void RandomWalk(Vector2Int pos)
    {
        int steps = Random.Range(0, _maxSteps);
        for (int s = 0; s < steps; s++)
        {
            Vector2Int direction = GetDirection();
            pos = MovePosition(pos, direction);
            SetTile(pos, MapTileID.Floor);
            SetSurroundingTile(pos, MapTileID.Floor);
        }
    }

    private Vector2Int MovePosition(Vector2Int pos, Vector2Int direction)
    {
        pos += direction;
        if (pos.x < _generator.BoundarySize.x) pos.x = _generator.BoundarySize.x;
        if (pos.y < _generator.BoundarySize.y) pos.y = _generator.BoundarySize.y;
        if (pos.x >= _generator.MapSize.x - _generator.BoundarySize.x) pos.x = _generator.MapSize.x - _generator.BoundarySize.x;
        if (pos.y >= _generator.MapSize.y - _generator.BoundarySize.y) pos.y = _generator.MapSize.y - _generator.BoundarySize.y;
        return pos;
    }


    private void SetSurroundingTile(Vector2Int pos, MapTileID tile)
    {
        foreach (Vector2Int nearbyTileDirection in _generator.ValidDirections)
        {
            Vector2Int nPos = MovePosition(pos, nearbyTileDirection);
            SetTile(nPos, tile);
        }
    }

    private void SetTile(Vector2Int pos, MapTileID tile)
    {
        _generator.Map[pos.x, pos.y] = tile;
        if (!_visited.Contains(pos)) _visited.Add(pos);
    }

    private Vector2Int GetDirection()
    {
        int i = Random.Range(0, _generator.ValidDirections.Length);
        return _generator.ValidDirections[i];
    }

    private Vector2Int GetRandomStartFromVisited()
    {
        int i = Random.Range(0, _visited.Count);      
        return _visited[i];
    }
       
}
