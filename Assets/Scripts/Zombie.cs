using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public bool debugMode = true;

    public GameObject target;
    //swarmai
    private NavAgent navAgent;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        navAgent = GetComponent<NavAgent>();
        if (navAgent == null)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //debug stuff first
        if (debugMode)
        {
            //Debug.DrawLine(transform.position, target.transform.position,Color.red);
        }
        navAgent.SetDestination(target.transform.position);
    }
}
