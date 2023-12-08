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

    [Header("Costs")] 
    [SerializeField] private int orthogonalCost = 1;
    [SerializeField] private int diagonalCost = 1;

    [Header("Booleans")] 
    [SerializeField] private bool displayVisited;
    
    [Header("Scripts")] 
    [SerializeField] private GridManager gridManager;
    [SerializeField] private UIManager uIManager;
    
    private GameObject[,] _grid;
    
    private int _rows;
    private int _columns;
    
    private int[,] _distances;
    private bool[,] _isVisited; // used by algorithm

    private bool[,] _neighborsVisited; // used for visualization only

    private Vector2Int[,] _pred; // matrix of Vector2Int coordinates

    private Vector2Int _sourcePosition;
    private Vector2Int _destinationPosition;

    private Vector2Int[] _obstaclesPosition;

    private List<Vector2Int> _path;

    private int _numberOfSteps;
    
    // ====================================================================================
    
    
    // ====================================================================================
    // MonoBehaviour methods
    // ====================================================================================
    
    private void OnEnable()
    {
        // ====================================================================================
        // Initialization
        // ====================================================================================
        
        _grid = gridManager.GetGrid();

        _sourcePosition = gridManager.GetSourcePosition();
        _destinationPosition = gridManager.GetDestinationPosition();

        _obstaclesPosition = gridManager.GetObstaclesPosition();

        _rows = gridManager.GetGridSize().x;
        _columns = gridManager.GetGridSize().y;

        _distances = new int[_rows, _columns];
        _isVisited = new bool[_rows, _columns];
        _pred = new Vector2Int[_rows, _columns];

        _neighborsVisited = new bool[_rows, _columns];
        _numberOfSteps = 0;
        
        // ====================================================================================
        
        
        // ====================================================================================
        // Dijkstra
        // ====================================================================================

        // Start execution time measurement
        var watch = System.Diagnostics.Stopwatch.StartNew(); 
        
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

            Vector2Int currentPosition = new Vector2Int(x, y);
            
            // Break the loop if we reach destination position
            if (currentPosition == _destinationPosition)
            {
                break;
            }

            // Check if this is an obstacle cube
            bool isObstacle = false;
            
            foreach (var obstacle in _obstaclesPosition)
            {
                if (currentPosition == obstacle)
                {
                    isObstacle = true;
                }
            }

            // Skip this iteration if we reach obstacle position
            if (isObstacle)
            {
                continue;
            }
            
            // Mark the cube as visited
            _isVisited[x, y] = true;
            
            // Check neighbors in each direction (similar to checking leaving arcs)
            for (int i = 0; i < Directions.GetNumberOfDirections(); i++)
            {
                int neighborX = x + Directions.GetDx()[i];
                int neighborY = y + Directions.GetDy()[i];
                
                // Check if the neighbor is inside the grid
                if ((neighborX >= 0 && neighborX < _rows) && (neighborY >= 0 && neighborY < _columns))
                {
                    // Calculate the distance (based on the neighbor's type (orthogonal or diagonal)
                    int movementCost = Directions.IsIndexOrthogonal(i) ? orthogonalCost : diagonalCost;
                    int distance = _distances[x,y] + movementCost;
                    
                    // Check optimality condition
                    if (distance < _distances[neighborX, neighborY])
                    {
                        // Update distance if this is shorter
                        _distances[neighborX, neighborY] = distance;
                        
                        // Update predecessor
                        _pred[neighborX, neighborY] = currentPosition;
                        
                        // Add neighbor to the priority queue
                        priorityQueue.Enqueue((neighborX, neighborY));
                    }
                    
                    // (For visualization purpose only! Not part of Dijkstra's algorithm)
                    _neighborsVisited[neighborX, neighborY] = true;
                }

                _numberOfSteps++;
            }
        }
        
        // ====================================================================================
        
        
        // ====================================================================================
        // Reconstruct path
        // ====================================================================================
        
        _path = Path.ReconstructPath(_pred, _sourcePosition, _destinationPosition);
        
        // Stop execution time measurement
        watch.Stop();
        
        // ====================================================================================
        
        
        // ====================================================================================
        // UI
        // ====================================================================================
        
        uIManager.SetAlgorithmText(UIManager.Algorithms.DIJKSTRA);
        uIManager.SetPathCostText(Path.ComputePathCost(_path, orthogonalCost, diagonalCost));
        uIManager.SetNumberOfStepsText(_numberOfSteps);
        uIManager.SetExecutionTimeText((int)watch.ElapsedMilliseconds);
        
        // ====================================================================================
        
        
        // ====================================================================================
        // Update scenario
        // ====================================================================================
        
        // Merge isVisited and _neighborsVisited together to visualize them both
        for (int x = 0; x < _rows; x++)
        {
            for (int y = 0; y < _columns; y++)
            {
                if (_neighborsVisited[x, y]) _isVisited[x, y] = true;
            }
        }
        
        gridManager.UpdateScenarioAfterPathComputation(_path, displayVisited, _isVisited);

        // ====================================================================================
        
        
        // ====================================================================================
        // Debug.Log
        // ====================================================================================
        
        Debug.Log("(Dijkstra's) Number of steps: " + _numberOfSteps);
        
        int visitedCubesCount = Enumerable.Range(0, _rows)
            .SelectMany(x => Enumerable.Range(0, _columns).Select(y => new { X = x, Y = y }))
            .Count(coord => _isVisited[coord.X, coord.Y]);
            
        Debug.Log("(Dijkstra's) " + "Number of visited cubes: " + visitedCubesCount);
        
        // ====================================================================================
    }
    
    /** Restore the world as it was before the algorithm's execution */
    private void OnDisable()
    {
        // Restore Grid
        gridManager.RecreateGrid();
        
        // Restore UI
        uIManager.RestoreTexts();
    }
    
    // ====================================================================================
}
