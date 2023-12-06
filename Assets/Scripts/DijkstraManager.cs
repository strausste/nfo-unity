using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private Vector2Int[,] _pred; // matrix of Vector2Int coordinates

    private Vector2Int _sourcePosition;
    private Vector2Int _destinationPosition;

    private List<Vector2Int> _path;

    private int _numberOfSteps;
    
    // ====================================================================================
    
    
    // ====================================================================================
    // MonoBehaviour methods:
    // ====================================================================================
    
    // Start is called before the first frame update
    void Start()
    {
        // ====================================================================================
        
        // Initialization:
        _grid = gridManager.GetGrid();

        _sourcePosition = gridManager.GetSourcePosition();
        _destinationPosition = gridManager.GetDestinationPosition();

        _rows = gridManager.GetGridSize().x;
        _columns = gridManager.GetGridSize().y;

        _distances = new int[_rows, _columns];
        _isVisited = new bool[_rows, _columns];
        _pred = new Vector2Int[_rows, _columns];

        _numberOfSteps = 0;
        
        // ====================================================================================
        
        // Dijkstra's
        
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
        
        // Predecessor of source is source itself
        _pred[_sourcePosition.x, _sourcePosition.y] = _sourcePosition;
        
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
            
            // Break the loop if we reach destination position
            if (new Vector2Int(x, y) == _destinationPosition)
            {
                break;
            }
            
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
                    
                    // Check optimality condition
                    if (!_isVisited[neighborX, neighborY] && distance < _distances[neighborX, neighborY])
                    {
                        // Update distance if this is shorter
                        _distances[neighborX, neighborY] = distance;
                        
                        // Update predecessor
                        _pred[neighborX, neighborY] = new Vector2Int(x, y);
                        
                        // Add neighbor to the priority queue
                        priorityQueue.Enqueue((neighborX, neighborY)); // TODO: !!!
                    }
                }

                _numberOfSteps++;
            }
        }
        
        // ====================================================================================
        
        Debug.Log("(Dijkstra's) number of steps: " + _numberOfSteps);
        
        // ====================================================================================
        
        // Reconstruct path

        _numberOfSteps = 0;
        
        _path = new List<Vector2Int>();
        
        // Starting from destination
        Vector2Int current = new Vector2Int(_destinationPosition.x, _destinationPosition.y) ;
        
        
        // To source
        while (!current.Equals(new Vector2Int(_sourcePosition.x, _sourcePosition.y)))
        {
            _path.Add(current);

            // Check if current is out of bounds to avoid potential issues
            if (current.x < 0 || current.x >= _rows || current.y < 0 || current.y >= _columns)
            {
                Debug.LogError("Path reconstruction encountered out-of-bounds coordinates.");
                break;
            }

            current = _pred[current.x, current.y];

            _numberOfSteps++;
        }

        // Add the source to the path
        _path.Add(_sourcePosition);

        // Reverse to get the path from source to destination
        _path.Reverse();

        // ====================================================================================
        
        _path.ForEach(t => Debug.Log(t));
        
        Debug.Log("(Reconstruct path) number of steps: " + _numberOfSteps);
    }
    
    // ====================================================================================
}
