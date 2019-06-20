using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triple<T, U, V>
{
    public Triple()
    {
    }

    public Triple(T first, U second, V third)
    {
        this.First = first;
        this.Second = second;
        this.Third = third;
    }

    public T First { get; set; }
    public U Second { get; set; }
    public V Third { get; set; }
};

public class PlayerAI : MonoBehaviour
{
    Transform target;
    Transform zombieParent;
    float targetDuration;
    const float forceRetargetTime = 5;
    Weapon weapon;
    NavAgent navAgent;
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        zombieParent = transform.parent;
        rb2d = transform.GetComponentInChildren<Rigidbody2D>();
        weapon = transform.GetComponentInChildren<Weapon>();
        navAgent = transform.GetComponentInChildren<NavAgent>();
        ReTarget();
    }

    // Update is called once per frame
    void Update()
    {
        targetDuration += Time.deltaTime;
        if (targetDuration > forceRetargetTime || target == null)
            ReTarget();

        Vector2 direction = (target.position - transform.position).normalized;
        Vector2 moveVector = new Vector2(0, 0);
        if ((target.position - transform.position).magnitude < 5)
        {
            moveVector = (-direction);
        }

        float lookAngleOffset = Vector2.SignedAngle(direction, (Vector2)transform.up);
        rb2d.MoveRotation(transform.rotation * Quaternion.AngleAxis((-lookAngleOffset * Time.fixedDeltaTime) / 0.1f, Vector3.forward));

        //determine difference from the way we want to move to our current direction
        float moveAngleOffset = Vector2.SignedAngle(moveVector, (Vector2)transform.up);
        float adjustedMoveForce;

        if (moveAngleOffset > 90 || moveAngleOffset < -90)//moving backwards or backsideways
        {
            float modifiedAngle = Mathf.Abs(moveAngleOffset) - 180;
            adjustedMoveForce = Mathf.Clamp01((0.5f * Mathf.Cos(modifiedAngle / 60) + 0.5f) * 1) * 0.5f;
        }
        else//moving forward or sideways
        {
            adjustedMoveForce = Mathf.Clamp01((0.5f * Mathf.Cos(moveAngleOffset / 60) + 0.5f) * 1);
        }

        rb2d.AddForce(moveVector * adjustedMoveForce, ForceMode2D.Impulse);

        if (lookAngleOffset < 15)
            weapon.Fire(transform.up);
    }

    void ReTarget()
    {
        //score, distance, visible
        Triple<float, float, bool>[] scoreParameters = new Triple<float, float, bool>[zombieParent.transform.childCount];
        for (int i = 0; i < zombieParent.transform.childCount; i++)
        {
            Transform selection = zombieParent.GetChild(i);
            scoreParameters[i].Second = (transform.position - selection.position).magnitude;
            RaycastHit hit;
            Physics.Raycast(transform.position, (selection.position - transform.position), out hit, 100);
            scoreParameters[i].Third = hit.transform == selection;
            //score = distance + 5 if visible, 0 if not
            scoreParameters[i].First = scoreParameters[i].Second + (scoreParameters[i].Third == true ? 5 : 0);
        }

        int best = 0;
        for (int i = 1; i < scoreParameters.Length; i++)
        {
            if (scoreParameters[i].First > scoreParameters[best].First)
                best = i;
        }
        target = zombieParent.transform.GetChild(best);
        targetDuration = 0;
    }
}
