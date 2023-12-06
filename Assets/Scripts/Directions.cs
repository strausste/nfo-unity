using System;
using System.Collections;

public class Directions 
{
    // ====================================================================================
    // Class attributes
    // ====================================================================================
    
    /** These directions correspond to the index of the deltas arrays dx and dy */
    enum Deltas
    {
        LEFT,
        UP_LEFT,
        UP,
        UP_RIGHT,
        RIGHT,
        DOWN_RIGHT,
        DOWN,
        DOWN_LEFT
    }
 
    // Directions:
    private static readonly int[] _dx = { -1, -1, 0, 1, 1, 1, 0, -1 };
    private static readonly int[] _dy = { 0, 1, 1, 1, 0, -1, -1, -1 };
    
    // Number of directions is the number of values in the Delta enum
    private static readonly int _numberOfDirections = Enum.GetValues(typeof(Deltas)).Length;
    
    // ====================================================================================
    
    
    // ====================================================================================
    // Class methods
    // ====================================================================================

    public static int[] GetDx()
    {
        return _dx;
    }

    public static int[] GetDy()
    {
        return _dy;
    }

    public static int GetNumberOfDirections()
    {
        return _numberOfDirections;
    }

    // ====================================================================================
    
    
}
