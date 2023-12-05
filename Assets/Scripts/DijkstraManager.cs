using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DijkstraManager : MonoBehaviour
{
    
    // ====================================================================================
    // Class attributes
    // ====================================================================================
    
    [SerializeField] private GridManager gridManager;
    private GameObject[,] _grid;
    
    private int _rows;
    private int _columns;
    
    private int[,] _distances;
    private bool[,] _isVisited;

    private Vector2Int _sourcePosition;
    private Vector2Int _destinationPosition;
    
    // ====================================================================================
    
    
    // ====================================================================================
    // MonoBehaviour methods:
    // ====================================================================================
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialization:
        _grid = gridManager.GetGrid();

        _sourcePosition = gridManager.GetSourcePosition();
        _destinationPosition = gridManager.GetDestinationPosition();

        _rows = gridManager.GetGridSize().x;
        _columns = gridManager.GetGridSize().y;

        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                _distances[i, j] = int.MaxValue; // Instead of Infinity, we give integer's max value
                _isVisited[i, j] = false; // All nodes are temporary before execution
            }
        }
        
        // Distance from source itself is 0
        _distances[_sourcePosition.x, _sourcePosition.y] = 0;
        
        // Priority queue to store nodes (cubes)'s coordinates based on their distances
        PriorityQueue<(int, int)> priorityQueue = new PriorityQueue<(int, int)>((a, b) =>
            _distances[a.Item1, a.Item2].CompareTo(_distances[b.Item1, b.Item2]));
        
        priorityQueue.Enqueue((_sourcePosition.x, _sourcePosition.y));
        
        // TODO: something that returns cube based on its coordinate
    }
    
    // ====================================================================================
}
