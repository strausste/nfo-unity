using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Tilemap tileMap;

    [SerializeField] private Tile walkableTile;
    [SerializeField] private Tile obstacleTile;
    [SerializeField] private Tile sourceTile;
    [SerializeField] private Tile destinationTile;

    private List<Vector3Int> _walkablePositions = new List<Vector3Int>();
    private List<Vector3Int> _obstaclePositions = new List<Vector3Int>();
    private Vector3Int _originPosition;
    private Vector3Int _destinationPosition;

    // Start is called before the first frame update
    void Start()
    {
        tileMap.CompressBounds();
        
        // Get all the tiles' positions based on their type
        BoundsInt bounds = tileMap.cellBounds;
        
        Debug.Log(bounds);
        
        TileBase[] allTiles = tileMap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                for (int z = 0; z < bounds.size.z; z++)
                {
                    TileBase tile = allTiles[x + y * bounds.size.x + z * bounds.size.x * bounds.size.y];

                    if (tile == walkableTile)
                    {
                        _walkablePositions.Add(new Vector3Int(x, y, z));
                    }
                    else if (tile == obstacleTile)
                    {
                        _obstaclePositions.Add(new Vector3Int(x, y, z));
                    }
                    else if (tile == sourceTile)
                    {
                        _originPosition = new Vector3Int(x, y, z);
                    }
                    else if (tile == destinationTile)
                    {
                        _destinationPosition = new Vector3Int(x, y, z);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var o in _obstaclePositions)
            {
                Debug.Log(o + ",," + tileMap.CellToWorld(o));
                // Instantiate(cube, tileMap.CellToWorld(o), Quaternion.identity); // cube was a default Unity's cube prefab, but now it's been deleted.
            }
        }
    }
}
