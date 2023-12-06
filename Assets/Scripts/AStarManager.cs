using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AStarManager : MonoBehaviour
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
    
    private GameObject[,] _grid;
    
    private int _rows;
    private int _columns;
    
    private int[,] _distances;
    private bool[,] _isVisited;

    private Vector2Int[,] _pred; // matrix of Vector2Int coordinates

    private Vector2Int _sourcePosition;
    private Vector2Int _destinationPosition;

    private Vector2Int[] _obstaclesPosition;

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

        _obstaclesPosition = gridManager.GetObstaclesPosition();

        _rows = gridManager.GetGridSize().x;
        _columns = gridManager.GetGridSize().y;

        _distances = new int[_rows, _columns];
        _isVisited = new bool[_rows, _columns];
        _pred = new Vector2Int[_rows, _columns];

        _numberOfSteps = 0;
        
        // ====================================================================================
        
        // A*
        
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
            
            // Check neighbors (in each direction), similar to checking leaving arcs
            for (int i = 0; i < Directions.GetNumberOfDirections(); i++)
            {
                int neighborX = x + Directions.GetDx()[i];
                int neighborY = y + Directions.GetDy()[i];
                
                // Check if the neighbor is inside the grid
                if ((neighborX >= 0 && neighborX < _rows) && (neighborY >= 0 && neighborY < _columns))
                {
                    // Calculate the distance (based on the neighbor's type (orthogonal or diagonal)
                    int distance = Directions.IsIndexOrthogonal(i) ? orthogonalCost : diagonalCost;

                    int heuristic = Convert.ToInt32(Heuristics.ManhattanDistance((i % 2) + 1,currentPosition, _destinationPosition));
                    distance += heuristic;
                    
                    // TODO: change 'distance' to 'cost' lol
                    Debug.Log("===");
                    Debug.Log("Current position: " + currentPosition.x + "," + currentPosition.y);
                    Debug.Log("Current Neighbor: " + neighborX + "," + neighborY );
                    Debug.Log("Distance value: " + (distance - heuristic));
                    Debug.Log("Heuristic value: + " + heuristic);
                    Debug.Log("===");

                    // Check optimality condition
                    if (!_isVisited[neighborX, neighborY] && distance < _distances[neighborX, neighborY])
                    {
                        // Update distance if this is shorter
                        _distances[neighborX, neighborY] = distance;
                        
                        // Update predecessor
                        _pred[neighborX, neighborY] = currentPosition;
                        
                        // Add neighbor to the priority queue
                        priorityQueue.Enqueue((neighborX, neighborY));
                    }
                }

                _numberOfSteps++;
            }
        }
        
        // ====================================================================================
        
        Debug.Log("(A*) number of steps: " + _numberOfSteps);
        
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
        
        
        // ====================================================================================
        
        // Update scenario:
        
        _path.ForEach(t => Debug.Log(t));
        
        Debug.Log("(Reconstruct path) number of steps: " + _numberOfSteps);

        // Show in the grid the cubes the algorithm visited
        if (displayVisited)
        {
            for (int x = 0; x < _rows; x++)
            {
                for (int y = 0; y < _columns; y++)
                {
                    if (_isVisited[x,y])
                    {
                        gridManager.DeleteCube(x, y);
                        gridManager.CreateCube(gridManager.GetVisitedPrefab(), x, y);
                    }
                }
            }
        }

        // Create path cubes
        foreach (var cube in _path)
        {
            // Leave source and destination visible
            if (cube == _sourcePosition || cube == _destinationPosition)
            {
                continue; 
            }

            gridManager.DeleteCube(cube.x, cube.y);
            gridManager.CreateCube(gridManager.GetPathPrefab(), cube.x, cube.y);
        }

        // ====================================================================================
        
    }
    
    // ====================================================================================
}
