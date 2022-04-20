using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Statistics : MonoBehaviour
    {
        [SerializeField]
        private Text myText;
        private static int numCells;

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

            var colliders = Physics.OverlapSphere(new Vector3(0, 0, 0), 1000);
            var filteredColliders = colliders.Where(c => c.CompareTag("Cell")).ToArray();
            if (numCells != filteredColliders.Length) {
                Debug.Log("numCells:  " + numCells + "filteredColliders.Length: " + filteredColliders.Length);
            }

            var totSpeed = Enumerable.Sum(filteredColliders, cell => cell.GetComponent<PlayerController>().CellParams.SpeedPoints);
            // Debug.Log("totSpeed: " + totSpeed);

            myText.text =
                "Number of cells: " + numCells.ToString() + "\n" +
                "MaxFullnessPoints: " + ((float)maxFullnessPoints / numCells).ToString("n2") + "\n" +
                "SpeedPoints:" + ((float)speedPoints / numCells).ToString("n2") + "\n" +
                "ReplicationRatePoints: " + ((float)replicationRatePoints / numCells).ToString("n2") + "\n" +
                "FoodWorthPoints: " + ((float)foodWorthPoints / numCells).ToString("n2") + "\n" +
                "SightRadiusPoints: " + ((float)sightRadiusPoints / numCells).ToString("n2") + "\n" +
                "SizePoints: " + ((float)sizePoints / numCells).ToString("n2") + "\n" +
                "AVG: " + ((float)(sizePoints + sightRadiusPoints + foodWorthPoints + replicationRatePoints + speedPoints + maxFullnessPoints) / numCells).ToString("n2");
        }
    }
}