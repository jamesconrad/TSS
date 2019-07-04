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
    private Vector3 basePosition;
    private float interpolateVal;
    private Health health;
    private static bool navMeshRebuildQueued = false;
	// Use this for initialization
	void Start ()
    {
        health = GetComponent<Health>();
        baseStrength = strength;
        basePosition = transform.position;
        //calculate corners of localspace bounds
        bounds = new Bounds(transform.localPosition, transform.localScale);
        wallPoints = new Vector2[2];
        wallPoints[0] = bounds.center + bounds.extents;
        wallPoints[1] = bounds.center - bounds.extents;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position != basePosition)
        {
            interpolateVal += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, basePosition, interpolateVal);
        }
        Debug.DrawLine(transform.parent.rotation * wallPoints[0] + transform.parent.position, transform.parent.rotation * wallPoints[1] + transform.parent.position, Color.red);
	}

    public void OnBulletImpact(float bulletDamage, Vector3 bulletDirection)
    {
        float percentDamage = health.ChangeHP(-bulletDamage);
        UpdateStrength(percentDamage);
        //0.1f = max move, ie if 100% damge will only move x units
        transform.position += bulletDirection * 0.1f * percentDamage;
        interpolateVal = 0;
        //impact particle
    }

    //called on collision with damaging object
    public void UpdateStrength(float healthPercent)
    {
        strength = baseStrength * healthPercent;
        if (strength == 0)
        {
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<NavMeshStaticFeature>());
            Destroy(GetComponent<Collider2D>());
            StartCoroutine(DelayedNavMeshRebuild());
            RubbleSpawner rs = GetComponent<RubbleSpawner>();
            rs.GenerateRubble();
            //gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private IEnumerator DelayedNavMeshRebuild(float delay = 1)
    {
        if (navMeshRebuildQueued) yield break;
        navMeshRebuildQueued = true;
        yield return new WaitForSeconds(delay);
        GenerateNavMesh2D.Instance.RebuildNavMesh();
        navMeshRebuildQueued = false;
    }
}
