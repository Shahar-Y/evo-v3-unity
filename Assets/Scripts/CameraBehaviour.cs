using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
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
                        Cell cellParams = hit.transform.gameObject.GetComponent<PlayerController>().CellParams;
                        string objString = JsonConvert.SerializeObject(cellParams);
                        objString = objString.Replace(",", System.Environment.NewLine);
                        objString = objString.Replace("{", "");
                        objString = objString.Replace("}", "");
                        objString = objString.Replace("\"", "");
                        Statistics.infoString = objString;
                        // hit.transform.gameObject.GetComponent<Renderer>().material.color = new Color(100, 100, 100);
                        if( !auraInstance )
                        {
                            auraInstance = Instantiate(auraPrefab, new Vector3(1, 1, 1), Quaternion.identity);
                        }
                        auraInstance.transform.position = hit.transform.position;
                        auraInstance.transform.parent = hit.transform;
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
    }
}
