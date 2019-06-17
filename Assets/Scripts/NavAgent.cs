using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class NavAgent : MonoBehaviour
{
    //[HideInInspector]
    public int AgentTypeID;
    //[HideInInspector]
    public float AgentTypeRadius;

    private Vector2 destination;

    private NavMeshPath path3D;

    public List<Vector2> PathPoints = new List<Vector2>();

    public float DistanceToClearPoint = 1f;

    private Rigidbody2D rb2d;

    public float MaxMoveForce = 1f;
    public float TurnRate = 1f;

    [Tooltip("How often should we pathfind, in case obstacles obstruct or clear our path?")]
    public float RepathDelaySeconds = 0.5f;

    private float repathTime;

    // Use this for initialization
    void Start()
    {
        path3D = new NavMeshPath();
        rb2d = GetComponent<Rigidbody2D>();

        repathTime = Time.time + RepathDelaySeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (PathPoints.Count > 1 && repathTime < Time.time)
        {
            CalculatePath();
            repathTime = Time.time + RepathDelaySeconds;
        }
    }

    private void FixedUpdate()
    {
        //NOTE: the first element in the path is always our position.
        if (PathPoints.Count > 0)
            PathPoints[0] = this.transform.position;

        //iterate through points sequentially, and if we're within a specified distance to the next-closest point, remove it. break out as soon as the next point is far enough away.
        for (int i = 1; i < PathPoints.Count; i++)
        {
            float distanceToNextPoint = Vector2.Distance(PathPoints[1], (Vector2)this.transform.position);
            if (distanceToNextPoint < DistanceToClearPoint)
                PathPoints.RemoveAt(1);
            else
                break;
        }

        //move towards next point
        if (PathPoints.Count > 1)
        {
            Vector2 vectorToNextPointNormalised = (PathPoints[1] - (Vector2)this.transform.position).normalized;
            float angleOffset = Vector2.SignedAngle(vectorToNextPointNormalised, (Vector2)transform.up);
            rb2d.MoveRotation(transform.rotation * Quaternion.AngleAxis((-angleOffset * Time.fixedDeltaTime)/0.5f, Vector3.forward));
            //Debug.DrawLine(transform.position, PathPoints[1], Color.cyan);
            //Debug.DrawRay(transform.position, transform.up, Color.green);
            //formula: movespeed = 0.5cos(x/30)+0.5
            float MoveForce = Mathf.Clamp01((0.5f * Mathf.Cos(angleOffset/30) + 0.5f) * MaxMoveForce);
            Vector2 moveForceVector = vectorToNextPointNormalised * MoveForce;
            //print("MoveForce: " + MoveForce);
            Debug.DrawRay(transform.position, moveForceVector, Color.yellow);
            rb2d.AddForce(moveForceVector, ForceMode2D.Force);
        }
    }


    public bool SetDestination(Vector2 destination)
    {
        this.destination = destination;
        return CalculatePath();
    }

    private bool CalculatePath()
    {
        NavMeshQueryFilter filter = new NavMeshQueryFilter
        {
            agentTypeID = AgentTypeID,
            areaMask = NavMesh.AllAreas
        };
        bool result = NavMesh.CalculatePath(this.transform.position, destination, filter, path3D);

        PathPoints.Clear();
        PathPoints.AddRange(path3D.corners.Select(x => (Vector2)x));
        return result;
    }
}
