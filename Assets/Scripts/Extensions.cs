using UnityEngine;

/// <summary>
/// Basic class that contains various extension methods
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Multiplies this vector's dimensions with another's, x*x, y*y, z*z
    /// </summary>
    public static Vector3 Times(this Vector3 me, Vector3 them)
    {
        return new Vector3(
            me.x * them.x,
            me.y * them.y,
            me.z * them.z
        );
    }

    /// <summary>
    /// Divides this vector's dimensions with another's, x/x, y/y, z/z
    /// </summary>
    public static Vector3 Divided(this Vector3 me, Vector3 them)
    {
        return new Vector3(
            me.x / them.x,
            me.y / them.y,
            me.z / them.z
        );
    }

    /// <summary>
    /// Modulos this vector's dimensions by modAmount, x % modAmount, y % modAmount, z % modAmount
    /// </summary>
    public static Vector3 Mod(this Vector3 me, float modAmount)
    {
        return new Vector3(
            me.x % modAmount,
            me.y % modAmount,
            me.z % modAmount
        );
    }

    /// <summary>
    /// Converts a float's integer value in seconds to a formatted string
    /// </summary>
    public static string SecondsToString(this float me)
    {
        int minutes = (int)me / 60;
        int seconds = (int)me % 60;

        return $"{minutes:00}:{seconds:00}";
    }
}