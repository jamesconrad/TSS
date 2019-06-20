﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofSupport : MonoBehaviour {

    public float strength = 1;
    //worldspace, 0 is max, 1 is min
    public Vector2[] wallPoints;
    public Bounds bounds;
	// Use this for initialization
	void Start ()
    {
        //fetch quad and build supports
        Collider2D collider = GetComponent<Collider2D>();
        bounds = collider.bounds;
        wallPoints = new Vector2[2];
        wallPoints[0] = collider.bounds.center + collider.bounds.extents;
        wallPoints[1] = collider.bounds.center - collider.bounds.extents;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.DrawLine(wallPoints[0], wallPoints[1], Color.red);
	}
}
