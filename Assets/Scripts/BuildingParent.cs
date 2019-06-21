using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingParent : MonoBehaviour
{
    //Can only construct box shaped buildings
    public Vector2Int sizeInTiles = new Vector2Int(2,2);
    public Bounds bounds;
    private Vector2 tileSize;
    private Vector2[] tileCenters;
    private float[] tileStrengths;
    private RoofSupport[] children;
    
    // Start is called before the first frame update
    void Start()
    {
        //calcualte the bounds of all support children
        Vector2 averagePoint = new Vector2(0, 0);
        int supportWalls = 0;
        children = GetComponentsInChildren<RoofSupport>();
        Vector2 max = children[0].wallPoints[0];
        for (int i = 0; i < children.Length; i++)
        {
            supportWalls++;
            averagePoint += children[i].wallPoints[0] + children[i].wallPoints[1];
            foreach (Vector2 point in children[i].wallPoints)
            {
                if (max.x < point.x)
                    max.x = point.x;
                if (max.y < point.y)
                    max.y = point.y;
            }
        }
        Vector2 center = averagePoint / (supportWalls * 2);
        bounds = new Bounds(center, (max - center) * 2);

        //build roof
        tileSize = (Vector2)bounds.extents * 2 / sizeInTiles;
        Vector2 tileHalfSize = tileSize / 2;
        tileCenters = new Vector2[sizeInTiles.x * sizeInTiles.y];
        tileStrengths = new float[tileCenters.Length];
        for (int i = 0; i < tileCenters.Length; i++)
        {
            Vector2 tc = new Vector2(0, 0);
            tc.x = tileHalfSize.x + (i % sizeInTiles.x) * tileSize.x;
            tc.y = tileHalfSize.y + (i / sizeInTiles.x) * tileSize.y;
            tileCenters[i] = (Vector2)bounds.min + tc;
            tileStrengths[i] = CalculatePointStrength(tileCenters[i]);
        }
        print(tileCenters);
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 tileHalfSize = tileSize / 2;
        foreach (Vector2 p in tileCenters)
        {
            Debug.DrawLine(p + new Vector2(-tileHalfSize.x, -tileHalfSize.y), p + new Vector2(tileHalfSize.x, tileHalfSize.y), Color.green);
            Debug.DrawLine(p + new Vector2(-tileHalfSize.x, tileHalfSize.y), p + new Vector2(tileHalfSize.x, -tileHalfSize.y), Color.green);
        }
    }

    float CalculatePointStrength(Vector2 point)
    {
        Vector2[] directions = { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
        float strength = 0;

        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(point, directions[i], bounds.max.magnitude, ~LayerMask.NameToLayer("BuildingSupport"));
            if (hit.transform != null)
            {
                float hitStrength = hit.transform.GetComponent<RoofSupport>().strength;
                strength += Mathf.Max(0, (hit.distance * hit.distance) / (-hitStrength * hitStrength) + 1);
            }
        }

        return Mathf.Max(0,strength);//children.Length;
    }

    void OnDrawGizmos()
    {
        if (tileCenters != null)
            for (int i = 0; i < tileCenters.Length; i++)
                UnityEditor.Handles.Label(tileCenters[i], tileStrengths[i].ToString());
    }
}
