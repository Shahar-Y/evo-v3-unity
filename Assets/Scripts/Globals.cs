using UnityEngine;

public static class Globals
{
    public static float SpeedUnits { get; } = 1.5f;
    public static int ReplicationRateUnits { get; } = -30;
    public static int FoodWorthUnits { get; } = 40;
    public static int SightRadiusUnits { get; } = 2;
    public static int MaxFullnessUnits { get; } = 200;
    public static int SizeUnits { get; } = 1;
    public static int TotalEvoPoints { get; } = 50;
    public static int TotalProperties { get; } = 5;
    public static int MaxReplicationRate { get; } = (int)(-0.5 * TotalEvoPoints * ReplicationRateUnits);
    public static int BeginningFullness { get; } = 1000;
    public static int MinReplicationRate { get; } = 100;
    public static int EdibleMinDiff { get; } = 2;
    public static Vector3 FurthestLocation { get; } = new Vector3(10000, 10000, 10000);
    public static int MeatWorth { get; } = 5;
    public static int MaxRadius { get; } = 500;
    

    public static int SelfDestructAnimation { get; } = 50;
}