using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts
{
    public class Cell
    {
        private static readonly System.Random Randator = new System.Random();
        public int SpeedPoints { get; set; }
        public float CellSpeed { get { return SpeedPoints * Globals.SpeedUnits; } }
        public int ReplicationRatePoints { get; set; }
        public int CellReplicationRate { get { return Globals.MaxReplicationRate - (ReplicationRatePoints * Globals.ReplicationRateUnits) + Random.Range(0, ReplicationRatePoints); } }
        public int FoodWorthPoints { get; set; }

        public int CellFoodWorth { get { return FoodWorthPoints * Globals.FoodWorthUnits; } }
        public int SizePoints { get; set; }
        public int CellSize { get { return SizePoints * Globals.SizeUnits; } }
        public int SightRadiusPoints { get; set; }
        public int CellSightRadius { get { return SightRadiusPoints * Globals.SightRadiusUnits; } }
        public int MaxFullnessPoints { get; set; }
        public int CellMaxFullness { get { return MaxFullnessPoints * Globals.MaxFullnessUnits; } }

        public int BeginningFullness { get; set; }

        public Cell(int maxFullness, int speed, int replicationRate, int foodWorth, int sightRadius, int size, int beginningFullness)
        {
            MaxFullnessPoints = maxFullness;
            SpeedPoints = speed;
            ReplicationRatePoints = replicationRate;
            FoodWorthPoints = foodWorth;
            SightRadiusPoints = sightRadius;
            SizePoints = size;
            BeginningFullness = beginningFullness;
        }

        // Return the name of the property with the highest rate.
        public string GetBestProperty()
        {
            (string, int)[] propertiesArray =
                new[]
                {
                    ("MaxFullnessPoints", MaxFullnessPoints),
                    ("SpeedPoints", SpeedPoints),
                    ("ReplicationRatePoints", ReplicationRatePoints),
                    ("FoodWorthPoints", FoodWorthPoints),
                    ("SightRadiusPoints", SightRadiusPoints),
                };

            (string, int)[] sortedArray = propertiesArray.OrderByDescending(x => x.Item2).ToArray();
            if (sortedArray[0].Item2 == sortedArray[propertiesArray.Length - 1].Item2)
            {
                return "AllEqual";
            }

            return sortedArray[0].Item1;
        }

        public string CellToString()
        {
            var cellString =
                "MaxFullness: " + MaxFullnessPoints +
                ", Speed: " + SpeedPoints +
                ", ReplicationRate: " + ReplicationRatePoints +
                ", FoodWorth: " + FoodWorthPoints +
                ", SightRadius: " + SightRadiusPoints +
                ", SizePoints: " + SizePoints;
            return cellString;
        }

        public void SetCellParams(Cell cellParams)
        {
            BeginningFullness = cellParams.BeginningFullness;
            MaxFullnessPoints = cellParams.MaxFullnessPoints;
            SpeedPoints = cellParams.SpeedPoints;
            ReplicationRatePoints = cellParams.ReplicationRatePoints;
            FoodWorthPoints = cellParams.FoodWorthPoints;
            SizePoints = cellParams.SizePoints;
            SightRadiusPoints = cellParams.SightRadiusPoints;
        }

        public static Color GetPropertyColor(string propName)
        {
            switch (propName)
            {
                case "MaxFullnessPoints":
                    return Color.white;
                case "SpeedPoints":
                    return Color.yellow;
                case "ReplicationRatePoints":
                    return Color.red;
                case "FoodWorthPoints":
                    return Color.green;
                case "SightRadiusPoints":
                    return Color.magenta;
                case "SizePoints":
                    return Color.blue;
                case "AllEqual":
                    return Color.gray;
            }

            return Color.black;
        }

        private string GetRandomProperty()
        {
            var randInt = Randator.Next(1, 6);
            switch (randInt)
            {
                case 1:
                    return "MaxFullnessPoints";
                case 2:
                    return "SpeedPoints";

                // case 3:
                //    return "ReplicationRatePoints";
                case 3:
                    return "FoodWorthPoints";
                case 4:
                    return "SightRadiusPoints";
                case 5:
                    return "SizePoints";
            }

            return "NoProperty";
        }

        public void MutateRandomProperty()
        {
            string property1 = GetRandomProperty();
            string property2 = GetRandomProperty();

            PropertyInfo property1Info = this.GetType().GetProperty(property1);
            PropertyInfo property2Info = this.GetType().GetProperty(property2);

            if (
                property1Info.Name == property2Info.Name ||
                    (property1Info.Name == "ReplicationRatePoints" &&
                     ReplicationRatePoints > Globals.TotalEvoPoints * 0.8)
               )
            {
                return;
            }

            int p1CurrValue = (int)property1Info.GetValue(this);
            int p2CurrValue = (int)property2Info.GetValue(this);

            if (p1CurrValue < Globals.TotalEvoPoints && p2CurrValue > 0)
            {
                property1Info.SetValue(this, p1CurrValue + 1);
                property2Info.SetValue(this, p2CurrValue - 1);
            }
        }
    }
}
