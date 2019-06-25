using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacterController : MonoBehaviour {

    public float moveForce = 1;
    public float rotationSpeed = 1;
    public float cameraOffset = 2;

    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate ()
    {
        Vector2 moveVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
            moveVector.y += 1;
        if (Input.GetKey(KeyCode.A))
            moveVector.x -= 1;
        if (Input.GetKey(KeyCode.S))
            moveVector.y -= 1;
        if (Input.GetKey(KeyCode.D))
            moveVector.x += 1;
        moveVector.Normalize(); ;

        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookVector = (mousePoint - transform.position).normalized;

        Debug.DrawRay(transform.position, lookVector, Color.red);
        Debug.DrawRay(transform.position, transform.up, Color.blue);
        //Debug.DrawLine(transform.position, transform.up, Color.red);

        //determine difference from look direction to our current direction
        float lookAngleOffset = Vector2.SignedAngle(lookVector, (Vector2)transform.up);
        rb2d.MoveRotation(transform.rotation * Quaternion.AngleAxis((-lookAngleOffset * Time.fixedDeltaTime) / 0.1f, Vector3.forward));

        //determine difference from the way we want to move to our current direction
        float moveAngleOffset = Vector2.SignedAngle(moveVector, (Vector2)transform.up);
        float adjustedMoveForce;

        if (moveAngleOffset > 90 || moveAngleOffset < -90)//moving backwards or backsideways
        {
            float modifiedAngle = Mathf.Abs(moveAngleOffset) - 180;
            adjustedMoveForce = Mathf.Clamp01((0.5f * Mathf.Cos(modifiedAngle / 60) + 0.5f) * moveForce) * 0.5f;
        }
        else//moving forward or sideways
        {
            adjustedMoveForce = Mathf.Clamp01((0.5f * Mathf.Cos(moveAngleOffset / 60) + 0.5f) * moveForce);
        }

        rb2d.AddForce(moveVector * adjustedMoveForce, ForceMode2D.Impulse);
        Vector3 cameraPos = transform.position + transform.up * cameraOffset;
        cameraPos.z = -10;
        Camera.main.transform.position = cameraPos;
    }
}
