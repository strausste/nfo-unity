using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Heuristics
{
    // ====================================================================================
    // Class attributes
    // ====================================================================================

    public enum HeuristicName
    {
        Euclidean,
        Manhattan,
        Chebyshev,
        OctileDistance
    }
    
    // Used for Octile Distance (double check AStarManager.cs has the same costs when using that heuristic)
    private static int OrthogonalMovementCost = 10;
    private static int DiagonalMovementCost = 14;
    
    // ====================================================================================
    
    
    // ====================================================================================
    // Class methods
    // ====================================================================================

    public static int CalculateHeuristic(HeuristicName heuristic, Vector2Int first, Vector2Int second)
    {
        switch (heuristic)
        {
            case HeuristicName.Euclidean:
                return EuclideanDistance(first, second);
            case HeuristicName.Manhattan:
                return ManhattanDistance(first, second);
            case HeuristicName.Chebyshev:
                return ChebyshevDistance(first, second);
            case HeuristicName.OctileDistance:
                return OctileDiagonalDistance(first, second);
            default:
                throw new ArgumentOutOfRangeException(nameof(heuristic), heuristic, null);
        }
    }
    
    public static int EuclideanDistance(Vector2Int first, Vector2Int second)
    {
        float dx = Math.Abs(first.x - second.x);
        float dy = Math.Abs(first.y - second.y);
        return (int) Math.Sqrt( (dx * dx) + (dy * dy) );
    }
    
    public static int ManhattanDistance(Vector2Int first, Vector2Int second)
    {
            return Math.Abs(first.x - second.x) + Math.Abs(first.y - second.y);
    }

    public static int ChebyshevDistance(Vector2Int first, Vector2Int second)
    {
        int dx = Mathf.Abs(first.x - second.x);
        int dy = Mathf.Abs(first.y - second.y);

        return Math.Max(dx, dy);
    }
    
    public static int OctileDiagonalDistance(Vector2Int first, Vector2Int second)
    {
        int dx = Mathf.Abs(first.x - second.x);
        int dy = Mathf.Abs(first.y - second.y);

        return Math.Max(dx, dy) + (DiagonalMovementCost - OrthogonalMovementCost) * Math.Min(dx, dy);
    }

    // ====================================================================================
}
