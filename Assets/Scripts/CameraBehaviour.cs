using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    // private void Start()
    // {
    // }

    // Update is called once per frame
    //public Rigidbody Rigid;
    //public float MouseSensitivity;
    //public float MoveSpeed;
    //public float JumpForce;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    Debug.Log(hit.transform.gameObject.name);
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
