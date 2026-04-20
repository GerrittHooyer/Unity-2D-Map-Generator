using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiMapAlgorithm", menuName = "Scriptable Objects/MapAlgorithm/MultiMapAlgorithm")]
public class MultiMapAlgorithm : MapAlgorithm
{
    [SerializeField] List<MapAlgorithm> _algorithms;

    public override void Generate()
    {
        foreach (var algorithm in _algorithms)
        {
            algorithm.Generator = _generator;
            algorithm.Generate();
        }
    }
}
