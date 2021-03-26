public class LayerMask
{
    public const int Default = 1;
    public const int TransparentFx = 1 << Layer.TransparentFx;
    public const int IgnoreRaycast = 1 << Layer.IgnoreRaycast;
    //public const int three = 1 << Layer.three; // not used
    public const int Water = 1 << Layer.Water;
    public const int UI = 1 << Layer.UI;
    //public const int six = 1 << 6; // not used
    //public const int seven = 1 << 7; // not used
    public const int Buildable = 1 << Layer.Buildable;
    public const int AiPathing = 1 << Layer.AiPathing;
    public const int Enemy = 1 << Layer.Enemy;
}

public class Layer
{
    public const int Default = 0;
    public const int TransparentFx = 1;
    public const int IgnoreRaycast = 2;
    //public const int three = 3; // not used
    public const int Water = 4;
    public const int UI = 5;
    //public const int six = 6; // not used
    //public const int seven = 7; // not used
    public const int Buildable = 8;
    public const int AiPathing = 9;
    public const int Enemy = 10;
}

public class Tags
{
    public const string Generated = "Generated";
    public const string Finish = "Finish";
    public const string Enemy = "Enemy";
}

public class GameObjectNames
{
    public const string RangeCollider = "Range Collider";
    public const string TowerModel = "Tower Model";
}