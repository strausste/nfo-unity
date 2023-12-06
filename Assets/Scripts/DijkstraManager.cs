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
                // Instead of Infinity, we give integer's max value
                _distances[i, j] = int.MaxValue; 
                
                // All nodes are temporary before execution
                _isVisited[i, j] = false; 
            }
        }
        
        // Distance from source itself is 0
        _distances[_sourcePosition.x, _sourcePosition.y] = 0;
        
        // Priority queue to store nodes (cubes)'s coordinates based on their distances
        PriorityQueue<(int, int)> priorityQueue = new PriorityQueue<(int, int)>((a, b) =>
            _distances[a.Item1, a.Item2].CompareTo(_distances[b.Item1, b.Item2])); // C# Tuples, and C#'s default CompareTo()
        
        // Add source to the priority queue
        priorityQueue.Enqueue((_sourcePosition.x, _sourcePosition.y));

        // While there are elements in the priority queue
        while (priorityQueue.Count() > 0)
        {
            // Extract the cube with the smallest distance
            (int x, int y) = priorityQueue.Dequeue();
            
            // Mark the cube as visited
            _isVisited[x, y] = true;
            
            // Check neighbors (in each direction)
            for (int i = 0; i < Directions.GetNumberOfDirections(); i++)
            {
                int neighborX = x + Directions.GetDx()[i];
                int neighborY = y + Directions.GetDy()[i];
                
                // Check if the neighbor is inside the grid
                if ((neighborX >= 0 && neighborX < _rows) && (neighborY >= 0 && neighborY < _columns))
                {
                    // Calculate the distance
                    int distance = _distances[x, y] + 1; // [we are assigning 1 to each neighbor!]
                    // TODO: different distances for diagonal and h/v movements?
                    
                    if (!_isVisited[neighborX, neighborY] && distance < _distances[neighborX, neighborY])
                    {
                        _distances[neighborX, neighborY] = distance;
                        priorityQueue.Enqueue((neighborX, neighborY));
                    }
                }
            }
        }
    }
    
    // ====================================================================================
}
