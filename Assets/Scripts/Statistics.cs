using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Assets.Scripts
{
    public class Statistics : MonoBehaviour
    {
        [SerializeField]
        private Text myText;
        private int time = 0;

        public static string infoString;
        public static PlayerController CurrCell;
        public static string clickedItemType = "NONE";

        [SerializeField]
        private Text info;
        private static int numCells;
        private const string cellInfoString = "Cell Info:\n";

        private static int maxFullnessPoints;
        private static int speedPoints;
        private static int replicationRatePoints;
        private static int foodWorthPoints;
        private static int sightRadiusPoints;
        private static int sizePoints;

        private void Awake()
        {
            // guard = new object();
            numCells = 0;
            maxFullnessPoints = 0;
            speedPoints = 0;
            replicationRatePoints = 0;
            foodWorthPoints = 0;
            sightRadiusPoints = 0;
            sizePoints = 0;
        }

        // Start is called before the first frame update
        private void Start()
        {
            time = 0;
            myText.text = "Starting Game...";
            SpawnManager.SMOnCellCreatedTriggerEnter += OnCellCreated;
            SpawnManager.SMOnCellDestroyedTriggerEnter += OnCellDestroyed;
        }

        private static void OnCellCreated(Cell createdCellParams)
        {
            Interlocked.Increment(ref numCells);
            Interlocked.Add(ref maxFullnessPoints, createdCellParams.MaxFullnessPoints);
            Interlocked.Add(ref speedPoints, createdCellParams.SpeedPoints);
            Interlocked.Add(ref replicationRatePoints, createdCellParams.ReplicationRatePoints);
            Interlocked.Add(ref foodWorthPoints, createdCellParams.FoodWorthPoints);
            Interlocked.Add(ref sightRadiusPoints, createdCellParams.SightRadiusPoints);
            Interlocked.Add(ref sizePoints, createdCellParams.SizePoints);
        }

        private static void OnCellDestroyed(Cell destroyedCellParams)
        {
            Interlocked.Decrement(ref numCells);
            Interlocked.Add(ref maxFullnessPoints, -destroyedCellParams.MaxFullnessPoints);
            Interlocked.Add(ref speedPoints, -destroyedCellParams.SpeedPoints);
            Interlocked.Add(ref replicationRatePoints, -destroyedCellParams.ReplicationRatePoints);
            Interlocked.Add(ref foodWorthPoints, -destroyedCellParams.FoodWorthPoints);
            Interlocked.Add(ref sightRadiusPoints, -destroyedCellParams.SightRadiusPoints);
            Interlocked.Add(ref sizePoints, -destroyedCellParams.SizePoints);
        }

        private void Update()
        {
            time++;
            var colliders = Physics.OverlapSphere(new Vector3(0, 0, 0), 1000);
            var filteredColliders = colliders.Where(c => c.CompareTag("Cell")).ToArray();
            if (numCells != filteredColliders.Length) {
                Debug.Log("numCells:  " + numCells + "filteredColliders.Length: " + filteredColliders.Length);
            }

            var totSpeed = Enumerable.Sum(filteredColliders, cell => cell.GetComponent<PlayerController>().CellParams.SpeedPoints);
            // Debug.Log("totSpeed: " + totSpeed);

            info.text = cellInfoString;

            if (CurrCell && clickedItemType == "Cell")
            {
                info.text = cellInfoString + infoString;
                info.text += "\nFullness: " + CurrCell.Fullness + "\nTime To Reproduce: " + CurrCell.TimeToReplication;
            } else if(clickedItemType == "Food")
            {
                info.text += infoString;
            } else
            {
                info.text += "No cell chosen";
            }

            myText.text =
                "Number of cells: " + numCells.ToString() + "\n" +
                "MaxFullnessPoints: " + ((float)maxFullnessPoints / numCells).ToString("n2") + "\n" +
                "SpeedPoints:" + ((float)speedPoints / numCells).ToString("n2") + "\n" +
                "ReplicationRatePoints: " + ((float)replicationRatePoints / numCells).ToString("n2") + "\n" +
                "FoodWorthPoints: " + ((float)foodWorthPoints / numCells).ToString("n2") + "\n" +
                "SightRadiusPoints: " + ((float)sightRadiusPoints / numCells).ToString("n2") + "\n" +
                "SizePoints: " + ((float)sizePoints / numCells).ToString("n2") + "\n" +
                "AVG: " + ((float)(sizePoints + sightRadiusPoints + foodWorthPoints + replicationRatePoints + speedPoints + maxFullnessPoints) / numCells).ToString("n2");

            if(time % 100 == 0)
            {
                WriteInfo("stats.csv");
            }


        }

        void WriteInfo(string filePath)
        {
            var records = new List<Foo>
        {
            new Foo { time = time, numCells = numCells }
        };
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(filePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(records);
            }
        }
    }

}

public class Foo
{
    public int time { get; set; }
    public int numCells { get; set; }
}