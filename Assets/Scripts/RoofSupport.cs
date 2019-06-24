using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofSupport : MonoBehaviour {

    [Tooltip("When distance = strength, impact will be 0")]
    public float strength = 1;
    private float baseStrength;
    //localspace, 0 is max, 1 is min
    public Vector2[] wallPoints;
    public Bounds bounds;
	// Use this for initialization
	void Start ()
    {
        baseStrength = strength;

        //calculate corners of localspace bounds
        bounds = new Bounds(transform.localPosition, transform.localScale);
        wallPoints = new Vector2[2];
        wallPoints[0] = bounds.center + bounds.extents;
        wallPoints[1] = bounds.center - bounds.extents;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.DrawLine(transform.parent.rotation * wallPoints[0] + transform.parent.position, transform.parent.rotation * wallPoints[1] + transform.parent.position, Color.red);
	}

    public void UpdateStrength(float healthPercent)
    {
        strength = baseStrength * healthPercent;
        if (strength == 0)
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
