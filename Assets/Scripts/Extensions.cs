using UnityEngine;

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