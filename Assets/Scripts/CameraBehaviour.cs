using UnityEngine;

namespace Assets.Scripts
{
    public class CameraBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject auraPrefab;

        private GameObject auraInstance;

        //public Rigidbody Rigid;
        //public float MouseSensitivity;
        //public float MoveSpeed;
        //public float JumpForce;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (hit.transform != null && hit.collider.tag == "Cell")
                    {
                        PlayerController cell = hit.transform.gameObject.GetComponent<PlayerController>();
                        Cell cellParams = cell.CellParams;
                        string cellParamsString = cellParams.CellToString();
                        cellParamsString = cellParamsString.Replace(",", System.Environment.NewLine);
                        SetStatisticsPointerData(cellParamsString, cell, "Cell");
                        ChangeAura(hit.transform);

                    } else if (hit.transform != null && hit.collider.tag == "Food")
                    {
                        Debug.Log("Food clicked");
                        ChangeAura(hit.transform);
                        SetStatisticsPointerData("Just some food", null, "Food");
                    }
                }
            }

            //MoveSpeed = 10.0f;

            //Rigid.MoveRotation(Rigid.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("Mouse X") * MouseSensitivity, 0)));
            //Rigid.MoveRotation(Rigid.rotation * Quaternion.Euler(new Vector3(Input.GetAxis("Mouse Y") * MouseSensitivity, 0, 0)));
            //Rigid.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * MoveSpeed) + (transform.right * Input.GetAxis("Horizontal") * MoveSpeed));
            
            //if (Input.GetKeyDown("space"))
            //    Rigid.AddForce(transform.up * JumpForce);


            // transform.Rotate(0, 3 * Time.deltaTime, 0);
        }

        private void SetStatisticsPointerData(string infoString, PlayerController currCell, string chosenObjectType)
        {
            Statistics.infoString = infoString;
            Statistics.CurrCell = currCell;
            Statistics.clickedItemType = chosenObjectType;
        }

        private void ChangeAura(Transform objectTransform)
        {
            // Create aura around cell if none exists
            if (!auraInstance)
            {
                auraInstance = Instantiate(auraPrefab, new Vector3(1, 1, 1), Quaternion.identity);
            }
            auraInstance.transform.position = objectTransform.position;
            auraInstance.transform.parent = objectTransform.transform;
        }



    }
}
