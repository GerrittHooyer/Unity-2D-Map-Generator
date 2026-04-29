using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "PerlinNoise", menuName = "Scriptable Objects/MapAlgorithm/PerlinNoise")]
public class PerlinNoise : MapAlgorithm
{
    [SerializeField, Range(0.0f, 1.0f)] float _threshold = 0.5f;
    [SerializeField] float _scale = 1.0f;

    public override void Generate() {
        
        _generator.FillGrid();
        for (int i = _generator.BoundarySize.x; i < _generator.MapSize.x - _generator.BoundarySize.x; i++)
        {
            for (int j = _generator.BoundarySize.y; j < _generator.MapSize.y - _generator.BoundarySize.y; j++)
            {
                float x = (float)i / (float)_generator.MapSize.x * _scale;
                float y = (float)j / (float)_generator.MapSize.y * _scale;
                float v = Mathf.PerlinNoise(x, y);
                MapTileID tileID = v > _threshold ? MapTileID.Floor : MapTileID.Wall;
                _generator.Map[i,j] = tileID;
            }
        }
    }
}
