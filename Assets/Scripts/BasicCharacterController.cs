using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacterController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //quick and dirty movement for now
        if (Input.GetKey(KeyCode.W))
            transform.position -= new Vector3(0.0f, -0.1f, 0.0f);
        if (Input.GetKey(KeyCode.A))
            transform.position -= new Vector3(0.1f, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.S))
            transform.position -= new Vector3(0.0f, 0.1f, 0.0f);
        if (Input.GetKey(KeyCode.D))
            transform.position -= new Vector3(-0.1f, 0.0f, 0.0f);

        Vector3 mousePos = transform.worldToLocalMatrix * Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float lookAngle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        //transform.localEulerAngles = new Vector3(0, 0, lookAngle);
        transform.GetChild(0).rotation = Quaternion.Euler(0.0f, 0.0f, lookAngle - 90);
        //GetComponentInChildren<LightSource>().Angle = lookAngle;
    }
}
