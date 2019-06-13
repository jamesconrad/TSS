using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofSupport : MonoBehaviour {

    public float strength = 1;
    public Vector2[] wallPoints; //worldspace

	// Use this for initialization
	void Start ()
    {
        //fetch quad and build supports
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        wallPoints = new Vector2[2];
        //fetch points in local space, z is always 0 so ignored
        Vector3 offset;
        if (mesh.bounds.extents.x * transform.localScale.x > mesh.bounds.extents.y * transform.localScale.y)
            offset = mesh.bounds.center + new Vector3(mesh.bounds.extents.x, 0, 0);
        else
            offset = mesh.bounds.center + new Vector3(0, mesh.bounds.extents.y, 0);

        wallPoints[0] = transform.TransformPoint(mesh.bounds.center + offset);
        wallPoints[1] = transform.TransformPoint(mesh.bounds.center - offset);
    }
	
	// Update is called once per frame
	void Update ()
    {

	}
}
