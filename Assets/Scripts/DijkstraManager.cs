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
    [SerializeField] private int orthogonalCost = 10;
    [SerializeField] private int diagonalCost = 14;

    [Header("Booleans")] 
    [SerializeField] private bool displayVisited;
    
    [Header("Scripts")] 
    [SerializeField] private GridManager gridManager;
    [SerializeField] private UIManager uIManager;
    
    private GameObject[,] _grid;
    
    private int _rows;
    private int _columns;
    
    private int[,] _distances;

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
        _pred = new Vector2Int[_rows, _columns];
        
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
            }
        }
        
        // Distance from source itself is 0
        _distances[_sourcePosition.x, _sourcePosition.y] = 0;
        
        // Predecessor of source is source itself
        _pred[_sourcePosition.x, _sourcePosition.y] = _sourcePosition;
        
        // Priority queue to store nodes (cubes)'s coordinates based on their distances [OPEN LIST]
        PriorityQueue<(int, int)> temporaryCubes = new PriorityQueue<(int, int)>((a, b) =>
            _distances[a.Item1, a.Item2].CompareTo(_distances[b.Item1, b.Item2])); // C# Tuples, and C#'s default CompareTo()
    
        // [CLOSED LIST]
        HashSet<(int, int)> permanentCubes = new HashSet<(int, int)>(); 

        // Add source to the priority queue
        temporaryCubes.Enqueue((_sourcePosition.x, _sourcePosition.y));

        // While there are elements in the priority queue
        while (temporaryCubes.Count() > 0)
        {
            // Extract the cube with the smallest distance
            (int x, int y) = temporaryCubes.Dequeue();
            
            if (permanentCubes.Contains((x, y))) continue; 
            
            // Make this cube permanent:
            permanentCubes.Add((x, y));

            Vector2Int currentPosition = new Vector2Int(x, y);
            
            // Break the loop if we reach destination position
            if (currentPosition == _destinationPosition)
            {
                break;
            }
            
            // ===================================
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
            // ===================================

            // Check neighbors in each direction (similar to checking leaving arcs)
            for (int i = 0; i < Directions.GetNumberOfDirections(); i++)
            {
                int neighborX = x + Directions.GetDx()[i];
                int neighborY = y + Directions.GetDy()[i];
                
                // Check if the neighbor is inside the grid
                if ((neighborX >= 0 && neighborX < _rows) && (neighborY >= 0 && neighborY < _columns))
                {
                    // Compute the distance (based on the neighbor's type (orthogonal or diagonal)
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
                        temporaryCubes.Enqueue((neighborX, neighborY));
                    }
                }
                // _numberOfSteps++;
            }
            _numberOfSteps++;
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
        
        gridManager.UpdateScenarioAfterPathComputation(_path, displayVisited, permanentCubes);

        // ====================================================================================
        
        
        // ====================================================================================
        // Debug.Log
        // ====================================================================================
        
        Debug.Log("(Dijkstra's) Number of steps: " + _numberOfSteps);
        
        Debug.Log("(Dijkstra's) " + "Number of visited cubes: " + permanentCubes.Count());
        
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
