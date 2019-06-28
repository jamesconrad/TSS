using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleSlow : MonoBehaviour
{
    public float LinearDrag;
    public float DefaultDrag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health other = collision.gameObject.GetComponent<Health>();
        if (other && other.physMat == Health.physicalMaterial.FLESH)
            other.GetComponent<Rigidbody2D>().drag = LinearDrag;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Health other = collision.gameObject.GetComponent<Health>();
        if (other && other.physMat == Health.physicalMaterial.FLESH)
            other.GetComponent<Rigidbody2D>().drag = DefaultDrag;
    }
}
