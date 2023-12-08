using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class GridManager : MonoBehaviour
{
    // ====================================================================================
    // Class attributes
    // ====================================================================================

    private GameObject[,] _grid;

    [Header("Grid settings")] 
    [SerializeField] private int gridSizeX = 64;
    [SerializeField] private int gridSizeY = 64;

    [Header("Positions")] 
    [SerializeField] private Vector2Int[] obstaclesPosition;
    [SerializeField] private Vector2Int sourcePosition;
    [SerializeField] private Vector2Int destinationPosition;

    [Header("Prefabs")] 
    [SerializeField] private GameObject gridParent;
    
    [SerializeField] private GameObject walkablePrefab;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject sourcePrefab;
    [SerializeField] private GameObject destinationPrefab;
    
    [SerializeField] private GameObject pathPrefab;
    [SerializeField] private GameObject visitedPrefab;

    // Static reference to the instance (singleton pattern)
    private static GridManager _instance;

    // ====================================================================================


    // ====================================================================================
    // Class methods
    // ====================================================================================

    // Public property to access the instance
    public static GridManager GetInstance()
    {
        // If the instance doesn't exist, find or create it
        if (_instance == null)
        {
            _instance = FindObjectOfType<GridManager>();

            // If no instance exists in the scene, create a new GameObject and add the script
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject(nameof(GridManager));
                _instance = singletonObject.AddComponent<GridManager>();
            }
        }

        return _instance;
    }

    public Vector2Int GetGridSize()
    {
        return new Vector2Int(gridSizeX, gridSizeY);
    }

    public GameObject[,] GetGrid()
    {
        return this._grid;
    }

    public Vector2Int GetSourcePosition()
    {
        return this.sourcePosition;
    }
    
    public Vector2Int GetDestinationPosition()
    {
        return this.destinationPosition;
    }

    public Vector2Int[] GetObstaclesPosition()
    {
        return this.obstaclesPosition;
    }

    public GameObject GetWalkablePrefab()
    {
        return this.walkablePrefab;
    }
    
    public GameObject GetObstaclePrefab()
    {
        return this.obstaclePrefab;
    }
    
    public GameObject GetSourcePrefab()
    {
        return this.sourcePrefab;
    }
    
    public GameObject GetPathPrefab()
    {
        return this.pathPrefab;
    }
    
    public GameObject GetVisitedPrefab()
    {
        return this.visitedPrefab;
    }
    
    public GameObject GetDestinationPrefab()
    {
        return this.destinationPrefab;
    }
    
    private void CreateGrid()
    {
        _grid = new GameObject[gridSizeX, gridSizeY];

        // Create all cubes walkable
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeY; z++)
            {
                CreateCube(walkablePrefab, x, z);
            }
        }

        // Obstacles:
        foreach (var op in obstaclesPosition)
        {
            // Destroy walkable cubes in that position
            DeleteCube(op.x, op.y);

            // Create obstacle cubes instead
            CreateCube(obstaclePrefab, op.x, op.y);
        }

        // Source
        DeleteCube(sourcePosition.x, sourcePosition.y);
        CreateCube(sourcePrefab, sourcePosition.x, sourcePosition.y);

        // Destination
        DeleteCube(destinationPosition.x, destinationPosition.y);
        CreateCube(destinationPrefab, destinationPosition.x, destinationPosition.y);
    }

    public void UpdateScenarioAfterPathComputation(List<Vector2Int> path, bool displayVisited, bool[,] isVisited)
    {
        // Show in the grid the cubes the algorithm visited
        if (displayVisited)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector2Int current = new Vector2Int(x, y);
                    
                    // Leave obstacles, source and destination visible
                    if (current == sourcePosition || current == destinationPosition || obstaclesPosition.Contains(current))
                    {
                        continue;
                    }
                    
                    if (isVisited[x,y])
                    {
                        DeleteCube(x, y);
                        CreateCube(visitedPrefab, x, y);
                    }
                }
            }
        }

        // Create path cubes
        foreach (var cube in path)
        {
            // Leave obstacles, source and destination visible
            if (cube == sourcePosition || cube == destinationPosition || obstaclesPosition.Contains(cube))
            {
                continue; 
            }

            DeleteCube(cube.x, cube.y);
            CreateCube(GetPathPrefab(), cube.x, cube.y);
        }
    }

    /** Instantiates and stores the desired prefab in the _grid */
    public void CreateCube(GameObject prefab, int x, int z)
    {
        Vector3 cubePosition = new Vector3(x, 0, z); // y is set to 0 because all cubes have the same height

        // Instantiate the prefab
        GameObject cube = Instantiate(prefab, cubePosition, Quaternion.identity);
        cube.transform.parent = gridParent.transform;

        // Storing the cube in the grid array
        _grid[x, z] = cube;
    }

    public void DeleteCube(int x, int z)
    {
        Destroy(_grid[x, z]);
    }
    
    // ====================================================================================
    
    
    // ====================================================================================
    // MonoBehaviour methods
    // ====================================================================================

    private void Awake()
    {
        // Ensure there's only one instance, and persist it between scenes
        // This is useless for the project's purposes, but this is how you implement singleton pattern in Unity
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance already exists, destroy this one
            Destroy(gameObject);
        }
        
        CreateGrid();
    }

    // ====================================================================================
}
