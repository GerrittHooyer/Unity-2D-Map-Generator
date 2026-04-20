using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class MapAlgorithm : ScriptableObject
{
    [SerializeField] private string _name;
    protected MapGenerator _generator;

    public MapGenerator Generator { set { _generator = value; } }

    public abstract void Generate();
}
