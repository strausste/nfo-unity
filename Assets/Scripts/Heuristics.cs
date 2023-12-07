using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heuristics
{
    // ====================================================================================
    // Class attributes
    // ====================================================================================
    
    // Euclidean (2 directions)
    // Manhattan (4 directions)
    // Diagonal distance (8 directions)
    
    // Source: https://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
    
    // ====================================================================================
    
    
    // ====================================================================================
    // Class methods
    // ====================================================================================
    
    public static double EuclideanDistance(Vector2Int first, Vector2Int second)
    {
        float dx = Math.Abs(first.x - second.x);
        float dy = Math.Abs(first.y - second.y);
        return Math.Sqrt( (dx * dx) + (dy * dy) );
    }
    
    public static decimal ManhattanDistance(Vector2Int first, Vector2Int second)
    {
        return Math.Abs(first.x - second.x) + Math.Abs(first.y - second.y);
    }
    
    // ====================================================================================
}
