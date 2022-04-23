using System.Threading;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject deathParticlePrefab;
        [SerializeField]
        private GameObject foodPrefab;
        [SerializeField]
        private GameObject cellPrefab;
        [SerializeField]
        private int foodTimer;
        [SerializeField]
        private int totalTimer;
        [SerializeField]
        private float initialRadius;
        [SerializeField]
        private float currRadius;


        private int dropFoodCount;
        private bool isFirstCell;

        // Start is called before the first frame update
        private void Start()
        {
            Debug.Log("STARTING!");
            initialRadius = 300;
            currRadius = 20;
            foodTimer = 0;
            totalTimer = 0;
            isFirstCell = true;
            PlayerController.OnCellCreatedTriggerEnter += OnCellDivider;
            PlayerController.OnCellDestroyedTriggerEnter += OnCellDestroyer;
            dropFoodCount = foodTimer;
            int avgPoints = Globals.TotalEvoPoints / Globals.TotalProperties;
            Cell genesis = new Cell(avgPoints, avgPoints, avgPoints, avgPoints, avgPoints, avgPoints, Globals.BeginningFullness);
            PlayerController.CellCreatedTriggerEnter(genesis, new Vector3(1.5f, 0.5f, 1.5f), Globals.BeginningFullness);
        }

        public static event System.Action<Cell> SMOnCellCreatedTriggerEnter;
        public static void SMCellCreatedTriggerEnter(Cell cellParams)
        {
            // Process the triggering here
            if (SMOnCellCreatedTriggerEnter != null)
            {
                SMOnCellCreatedTriggerEnter(cellParams);
            }
        }

        public static event System.Action<Cell> SMOnCellDestroyedTriggerEnter;
        private static void SMCellDestroyedTriggerEnter(Cell cellParams)
        {
            // Process the triggering here
            if (SMOnCellDestroyedTriggerEnter != null)
            {
                SMOnCellDestroyedTriggerEnter(cellParams);
            }
        }

        private void OnCellDestroyer(Cell cellParams, Vector3 location, bool isEaten)
        {
            if (isEaten)
            {
                var rotation = Quaternion.identity;
                rotation.eulerAngles = new Vector3(-90, 0, 0);
                var deathInstance = Instantiate(
                                    deathParticlePrefab,
                                    location + new Vector3(0, 1, 0),
                                    rotation
                                   );
                deathInstance.transform.localScale = new Vector3(1, 1, 1) * 5;
            }
            SMCellDestroyedTriggerEnter(cellParams);
        }

        private void OnCellDivider(Cell originCellParams, Vector3 cellLocation, int originFullness)
        {
            var newCell = new Cell(
                originCellParams.MaxFullnessPoints,
                originCellParams.SpeedPoints,
                originCellParams.ReplicationRatePoints,
                originCellParams.FoodWorthPoints,
                originCellParams.SightRadiusPoints,
                originCellParams.SizePoints,
                originFullness
            );

            if (!isFirstCell) newCell.MutateRandomProperty();
            else isFirstCell = false;

            CreateCell(cellLocation, newCell);
        }

        // Update is called once per frame
        private void Update()
        {
            totalTimer++;

            if (totalTimer % 4000 == 0)
            {
                foodTimer++;
            }
            if (totalTimer % 10 == 0)
            {
                currRadius = (currRadius >= Globals.MaxRadius) ? Globals.MaxRadius : currRadius + 1;
            }
            if(totalTimer % 5 == 0)
            {
                GenerateRandomFood();
            }

            
        }

        private void GenerateRandomFood()
        {
            float randRad = currRadius * Mathf.Sqrt(Random.value);
            float theta = Random.value * 2 * Mathf.PI;
            float mineX = (float)(randRad * System.Math.Cos(theta));
            float mineY = (float)(randRad * System.Math.Sin(theta));
            var foodInstance = Instantiate(foodPrefab, new Vector3(mineX, 3, mineY), Quaternion.identity);
            foodInstance.transform.parent = GameObject.Find("Foods").transform;
        }

        public float[] GetNormallizedList(float[] originalArray)
        {
            double sumSquares = 0;
            for (int i = 0; i < originalArray.Length; i++)
            {
                sumSquares += System.Math.Pow(originalArray[i], 2);
            }

            double root = System.Math.Sqrt(sumSquares);

            float[] normallizedArray = new float[] { };

            for (int i = 0; i < originalArray.Length; i++)
            {
                normallizedArray[i] = (float)(originalArray[i] / sumSquares);
            }

            return new float[] { };
        }

        public void CreateCell(Vector3 cellLocation, Cell newCell)
        {
            var newInstance =
                Instantiate(
                    cellPrefab,
                    cellLocation + new Vector3(1.5f, 0, 1.5f),
                    Quaternion.identity
                );

            newInstance.GetComponent<PlayerController>().CellParams.SetCellParams(newCell);
            newInstance.transform.localScale = new Vector3(1, 2, 1) * newCell.SizePoints * 0.5f;
            newInstance.transform.parent = GameObject.Find("Cells").transform;
            newInstance.GetComponent<Renderer>().material.color = Cell.GetPropertyColor(newCell.GetBestProperty());
            SMCellCreatedTriggerEnter(newCell);
        }
    }
}