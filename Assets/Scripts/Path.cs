using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    // ====================================================================================
    // Class methods
    // ====================================================================================
    
    public static List<Vector2Int> ReconstructPath(Vector2Int[,] predecessors, Vector2Int source, Vector2Int destination)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        // Starting from destination
        Vector2Int current = destination; 
        
        // To source
        while (!current.Equals(source))
        {
            path.Add(current);

            // TODO: there is no safety check (pred is inside the grid's boundaries)

            current = predecessors[current.x, current.y];
        }

        // Add the source to the path
        path.Add(source);

        // Reverse to get the path from source to destination
        path.Reverse();

        return path;
    }
    
    // ====================================================================================
}
