using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {

        public Cell CellParams { get; set; }
        private Rigidbody rb;

        [SerializeField]
        private int timeToReplication = 9000;
        [SerializeField]
        private int fullness;
        private int directionCount;
        private Vector3 currDirection;
        private static object ccLock = new object();
        private static bool triggerOn = false;

        public string MyName { get; set; } = "";
        public bool IsDead { get; set; } = false;
        public readonly object CellGuard = new object();

        // Start is called before the first frame update
        private void Awake()
        {
            int avgPoints = Globals.TotalEvoPoints / Globals.TotalProperties;
            CellParams = new Cell(avgPoints, avgPoints, avgPoints, avgPoints, avgPoints, avgPoints, Globals.BeginningFullness);
        }

        private void Start()
        {
            MyName = System.Guid.NewGuid().ToString();
            rb = GetComponent<Rigidbody>();
            timeToReplication =
                CellParams.CellReplicationRate + Random.Range(Globals.ReplicationRateUnits, -Globals.ReplicationRateUnits);
            fullness = CellParams.BeginningFullness;
            directionCount = 0;
            currDirection = new Vector3(
                Random.Range(-1.0f, 1.0f),
                0,
                Random.Range(-1.0f, 1.0f)
            ).normalized;
        }

        public static event System.Action<Cell, Vector3, int> OnCellCreatedTriggerEnter;
        public static void CellCreatedTriggerEnter(Cell cellParams, Vector3 location, int originFullness)
        {
            // Process the triggering here
            if (OnCellCreatedTriggerEnter != null)
            {
                OnCellCreatedTriggerEnter(cellParams, location, originFullness);
            }
        }

        public static event System.Action<Cell, Vector3, bool> OnCellDestroyedTriggerEnter;
        private static void CellDestroyedTriggerEnter(Cell cellParams, Vector3 location, bool isEaten)
        {
            // Process the triggering here
            if (OnCellDestroyedTriggerEnter != null)
            {
                OnCellDestroyedTriggerEnter(cellParams, location, isEaten);
            }
        }

        public static event System.Action<Vector3, Cell, int> OnCellDivideTriggerEnter;

        // Update is called once per frame
        private void Update()
        {
            triggerOn = false;
            fullness -= 1;
            timeToReplication -= 1;
            if (fullness <= 0)
            {
                lock (CellGuard)
                {
                    CellDestroyedTriggerEnter(CellParams, transform.position, false);
                    IsDead = true;
                    Destroy(gameObject);
                }
            }

            if (timeToReplication <= 0 && fullness > CellParams.CellMaxFullness / 2)
            {
                lock (CellGuard)
                {
                    fullness /= 2;
                    timeToReplication = CellParams.CellReplicationRate >= Globals.MinReplicationRate ?
                                        CellParams.CellReplicationRate : Globals.MinReplicationRate;
                    CellCreatedTriggerEnter(CellParams, transform.position, fullness);
                }
            }

            ChooseDirection();
            rb.velocity = currDirection * CellParams.CellSpeed;
        }

        private void ChangeToRandomDirection()
        {
            currDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
            directionCount = 0;
        }

        // Chooses the direction for the cell to choose given it's location and the world.
        // Inserts the new direction to the field `currDirection`.
        private void ChooseDirection()
        {
            var colliders = Physics.OverlapSphere(transform.position, CellParams.CellSightRadius);
            List<Collider> dangerCells = new List<Collider>();
            List<Collider> foodCells = new List<Collider>();
            List<Collider> foods = new List<Collider>();

            for (int i = 0; i < colliders.Length; i++) {
                Collider currCollider = colliders[i];
                switch (currCollider.tag)
                {
                    case "Food":
                        foods.Add(currCollider);
                        break;

                    case "Cell":
                        Cell otherCellParams = currCollider.GetComponent<PlayerController>().CellParams;
                        if (IsEdible(otherCellParams))
                        {
                            foodCells.Add(currCollider);
                        } else if (IsDanger(otherCellParams))
                        {
                            dangerCells.Add(currCollider);
                        }

                        break;

                    default:
                        break;
                }
            }

            // if no colliders in sight.
            // foodCells 
            if (!dangerCells.Any() && !foodCells.Any() && !foods.Any())
            {
                if (directionCount < 100)
                {
                    directionCount++;
                } else
                {
                    ChangeToRandomDirection();
                }

                return;
            }

            dangerCells.OrderBy(cell => GetDistance(cell.transform.position));
            foodCells.OrderBy(cell => GetDistance(cell.transform.position));
            foods.OrderBy(food => GetDistance(food.transform.position));

            Vector3 firstEnemyLocation = dangerCells.Any() ? dangerCells.First().transform.position : Globals.FurthestLocation;
            Vector3 firstFoodCellLocation = foodCells.Any() ? foodCells.First().transform.position : Globals.FurthestLocation;
            Vector3 firstFoodsLocation = foods.Any() ? foods.First().transform.position : Globals.FurthestLocation;

            float firstEnemyDistance = GetDistance(firstEnemyLocation);
            float firstFoodCellDistance = GetDistance(firstFoodCellLocation);
            float firstFoodsDistance = GetDistance(firstFoodsLocation);

            // If the enemy is near, RUN!
            // Otherwise, if hungry go to the closest source of food.
            if (firstEnemyDistance < firstFoodCellDistance && firstEnemyDistance < firstFoodsDistance)
            {
                currDirection = -(firstEnemyLocation - transform.position).normalized;
                return;
            }
            else if (fullness + CellParams.CellFoodWorth < CellParams.CellMaxFullness)
            {
                currDirection = (firstFoodsDistance < firstFoodCellDistance) ?
                    (firstFoodsLocation - transform.position).normalized :
                    (firstFoodCellLocation - transform.position).normalized;
            }
        }

        private float GetDistance(Vector3 otherObjectLocation)
        {
            return (transform.position - otherObjectLocation).sqrMagnitude;
        }

        private bool IsEdible(Cell otherCell)
        {
            return otherCell.CellSize + Globals.EdibleMinDiff <= CellParams.CellSize;
        }

        private bool IsDanger(Cell otherCell)
        {
            return otherCell.CellSize - Globals.EdibleMinDiff >= CellParams.CellSize;
        }

        private void ConsumeFood(string FoodType)
        {
            int foodWorth = CellParams.CellFoodWorth;

            switch (FoodType)
            {
                case "Food":
                    break;

                case "Cell":
                    foodWorth *= Globals.MeatWorth;
                    break;
            }

            fullness = fullness + foodWorth;

            // After food is consumed, change direction.
            ChangeToRandomDirection();
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Food"))
            {
                // If cell is full, ignore the food
                if (fullness + CellParams.CellFoodWorth > CellParams.CellMaxFullness)
                {
                    return;
                }

                ConsumeFood("Food");
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Cell"))
            {
                PlayerController otherCell = other.GetComponent<PlayerController>();

                lock (otherCell.CellGuard)
                {
                    if (IsEdible(otherCell.CellParams))
                    {
                        otherCell = other.GetComponent<PlayerController>();
                        if (otherCell.IsDead)
                        {
                            return;
                        }

                        otherCell.IsDead = true;

                        ConsumeFood("Cell");

                        CellDestroyedTriggerEnter(otherCell.CellParams, transform.position, true);
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }
}