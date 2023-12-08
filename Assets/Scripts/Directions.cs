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

    /** Returns the common index of this class' _dx and _dy array given the dx and dy parameters */
    public static int GetDeltaIndex(int dx, int dy)
    {
        if ((dx > 1 || dx < -1) || (dy > 1 || dy < -1))
        {
            throw new ArgumentOutOfRangeException();
        }

        for (int i = 0; i < _numberOfDirections; i++)
        {
            // Return the index that mathces _dx with dx and _dy with dy
            if (_dx[i] == dx && _dy[i] == dy)
            {
                return i;
            }
        }

        // In case of error return -1
        return -1;
    }

    /** Returns true iff deltaIndex is associated to an orthogonal movement, false otherwise*/
    public static bool IsIndexOrthogonal(int deltaIndex)
    {
        // Orthogonal movements are in the even positions of the Deltas enum
        return deltaIndex % 2 == 0;
    }
    
    /** Returns true iff deltaIndex is associated to a diagonal movement, false otherwise*/
    public static bool IsIndexDiagonal(int deltaIndex)
    {
        // Diagonal movements are in the odd positions of the Deltas enum
        return deltaIndex % 2 == 1;
    }
    
    // ====================================================================================
    
    
}
