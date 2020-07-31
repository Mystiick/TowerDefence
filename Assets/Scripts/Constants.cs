using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

public class LayerMask
{
    public const int Default = 1;
    public const int Buildable = 1 << 8;
}

public class Tag
{
    public const string Generated = "Generated";
}

public static class Extensions
{
    public static Vector3 Times(this Vector3 me, Vector3 them)
    {
        return new Vector3(
            me.x * them.x,
            me.y * them.y,
            me.z * them.z
        );
    }

    public static Vector3 Mod(this Vector3 me, float modAmount)
    {
        return new Vector3(
            me.x % modAmount, 
            me.y % modAmount,
            me.z % modAmount
        );
    }
}