using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.Mathf;

public class MovePiece : MonoBehaviour
{
    Vector3 startPos;
    Rigidbody rigidBody;

    //Stupid smoothed movement coefficients >:(
    float moveVarA = 0.5f;
    float moveVarB = 10f;
    float moveVarC = 7.5f;

    //Rotation coefficients
    private float turnSpeed = 300.0f;
    private float direction;

    void Start() {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        direction = 0;
    }

    //On click, set the start position to objects current position, and move object to layer 2 (Unity default ignore raycast layer)
    void OnMouseDown()
    {
        startPos = transform.position;
        gameObject.layer = 2;
    }

    void OnMouseDrag()
    {
        //Calculate new position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
 
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            //New position is hit position. If right mouse is pressed, y is 0.5 above ground. Else, y is 2 above ground.
            Vector3 newPos = new Vector3(
                                        hit.point.x, 
                                        hit.point.y + (float)(Input.GetMouseButton(1) ? 0.5 : 2),
                                        hit.point.z);

            //Smoothed movement code 
            Vector3 deltaPos = newPos - transform.position;
            rigidBody.velocity = deltaPos * ((moveVarB / (float)Mathf.Pow(deltaPos.magnitude, -moveVarA)) + moveVarC);

            //Simple movement code
            //rigidBody.MovePosition(new Vector3(hit.point.x, hit.point.y, hit.point.z));
        }

        //Change rotation if scroll
        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            direction = (direction + Input.GetAxisRaw("Mouse ScrollWheel") * 900) % 360;
        }


        //Correct rotation
        transform.eulerAngles = new Vector3(Mathf.MoveTowardsAngle(transform.eulerAngles.x, 0, turnSpeed * Time.deltaTime),
                                            Mathf.MoveTowardsAngle(transform.eulerAngles.y, direction, turnSpeed * Time.deltaTime),
                                            Mathf.MoveTowardsAngle(transform.eulerAngles.z, 0, turnSpeed * Time.deltaTime));
    }

    void OnMouseUp() {
        gameObject.layer = 0;
    }
}
