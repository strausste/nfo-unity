using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private GameObject[,] _grid;

    [Header("Grid settings")] [SerializeField]
    private int gridSizeX = 64;

    [SerializeField] private int gridSizeY = 64;

    [Header("Positions")] [SerializeField] private Vector2Int[] obstaclesPosition;
    [SerializeField] private Vector2Int sourcePosition;
    [SerializeField] private Vector2Int destinationPosition;

    [Header("Prefabs")] [SerializeField] private GameObject gridParent;
    [SerializeField] private GameObject walkablePrefab;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject sourcePrefab;
    [SerializeField] private GameObject destinationPrefab;

    [Header("Scripts")] [SerializeField] private InputManager inputManager;


    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
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
            Destroy(_grid[op.x, op.y]);

            // Create obstacle cubes instead
            CreateCube(obstaclePrefab, op.x, op.y);
        }

        // Source
        Destroy(_grid[sourcePosition.x, sourcePosition.y]);
        CreateCube(sourcePrefab, sourcePosition.x, sourcePosition.y);

        // Destination
        Destroy(_grid[destinationPosition.x, destinationPosition.y]);
        CreateCube(destinationPrefab, destinationPosition.x, destinationPosition.y);
    }

    /** Instantiates and stores the desired prefab in the _grid*/
    private void CreateCube(GameObject prefab, int x, int z)
    {
        Vector3 cubePosition = new Vector3(x, 0, z); // y is set to 0 because all cubes have the same height

        // Instantiate the prefab
        GameObject cube = Instantiate(prefab, cubePosition, Quaternion.identity);
        cube.transform.parent = gridParent.transform;

        // Storing the cube in the grid array
        _grid[x, z] = cube;
    }
}
