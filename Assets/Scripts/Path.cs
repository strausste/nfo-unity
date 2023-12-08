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

    public static int ComputePathCost(List<Vector2Int> path, int orthogonalMovementCost, int diagonalMovementCost)
    {
        int cost = 0;
        
        foreach (var cube in path)
        {
            int currentIndex = path.IndexOf(cube);
            
            // For all the cubes in path but the last (the destination)
            if (path.IndexOf(cube) != path.Count - 1)
            {
                Vector2Int nextCube = path[currentIndex + 1];

                int dx = cube.x - nextCube.x;
                int dy = cube.y - nextCube.y;
                
                // Add up to cost the correct one based on the direction we're moving to the next cube
                cost += Directions.IsIndexOrthogonal(Directions.GetDeltaIndex(dx, dy)) ? orthogonalMovementCost : diagonalMovementCost;
            }
        }

        return cost;
    }
    
    // ====================================================================================
}
