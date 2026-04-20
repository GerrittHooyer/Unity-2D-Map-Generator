using System.Threading;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[CreateAssetMenu(fileName = "CellularAutomata", menuName = "Scriptable Objects/MapAlgorithm/CellularAutomata")]
public class CellularAutomata : MapAlgorithm
{
    [SerializeField] private int _iterations = 1;
    [SerializeField] bool _resetMap = true;
    [SerializeField] private float _fillPercentage = 0.5f;
    [SerializeField] private int _threshold = 5;


    public override void Generate()
    {
        if (_resetMap)
        {
            _generator.RandomizeGrid(_fillPercentage);
        }
        for (int i = 0; i < _iterations; i++)
        {
            EvaluateTiles();
        }
    }

    

    public void EvaluateTiles()
    {
        for (int x = _generator.BoundarySize.x; x < _generator.MapSize.x - _generator.BoundarySize.x; x++)
        {
            for (int y = _generator.BoundarySize.y; y < _generator.MapSize.y - _generator.BoundarySize.y; y++)
            {
                _generator.Map[x,y] = EvaluateTile(x, y);
            }
        }
    }

    public MapTileID EvaluateTile(int x, int y)
    {
        MapTileID id;

        int count = 0;
        if (_generator.Map[x, y] == MapTileID.Floor && _generator.IsPointValid(new Vector2Int(x,y)))
        {
            count++;
        }
        foreach (Vector2Int direction in _generator.ValidDirections)
        {
            Vector2Int pos = new Vector2Int(x, y) + direction;
            
            if (_generator.Map[pos.x, pos.y] == MapTileID.Floor && _generator.IsPointValid(pos))
            {
                count++;
            }
        }

        if (count >= _threshold)
        {
            id = MapTileID.Floor;
        }
        else
        {
            id = MapTileID.Wall;
        }

        return id;
    }
}   
