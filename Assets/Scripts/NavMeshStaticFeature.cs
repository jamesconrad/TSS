using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Custom-made class, so we can easily identify objects that we intend to incorporate into
/// the NavMesh as walls or boundaries. Also allows us to encapsulate all the code required to
/// turn the Collider2D into a NavMeshBuildSource, which is fed into the NavMesh build operation.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class NavMeshStaticFeature : MonoBehaviour {

    const int AreaNotWalkable = 1;
    private new Collider2D collider2D;
    private PolygonCollider2D polygonCollider2D;
    private CircleCollider2D circleCollider2D;
    private BoxCollider2D boxCollider2D;
    private Mesh generatedStaticFeatureMesh;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        if (null == collider2D)
            throw new Exception("Must have a collider2D attached.");

        if (collider2D.isTrigger)
            throw new Exception("Trigger colliders are not valid for being static features.");

        polygonCollider2D = collider2D as PolygonCollider2D;
        //if we've got a PolygonCollider2D, generate a mesh to represent the collider.
        if (null != polygonCollider2D)
            GenerateAndCacheMesh();

        circleCollider2D = collider2D as CircleCollider2D;
        boxCollider2D = collider2D as BoxCollider2D;

        if (null == boxCollider2D && null == circleCollider2D && null == polygonCollider2D)
            throw new Exception("NavMeshStaticFeature has only been coded to handle PolygonCollider2D, BoxCollider2D, and CircleCollider2D.");
    }

    /// <summary>
    /// Takes the series of points specified in the PolygonCollider2D, and creates a mesh.
    /// </summary>
    private void GenerateAndCacheMesh()
    {
        int[] indices = Triangulator.Triangulate(polygonCollider2D.points);
        var vertices = polygonCollider2D.points.Select(p => (Vector3)p).ToArray();

        // create mesh
        generatedStaticFeatureMesh = new Mesh
        {
            vertices = vertices,
            triangles = indices
        };
        generatedStaticFeatureMesh.RecalculateBounds();
    }

    /// <summary>
    /// Don't cache this because any movement of the gameobject will cause the buildsource to be invalidated.
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    public NavMeshBuildSource GenerateBuildSource()
    {
        var localPosition = collider2D.offset;
        var worldPositionIncludingOffset = collider2D.transform.TransformPoint(localPosition);

        NavMeshBuildSource source = default(NavMeshBuildSource);

        if (null != polygonCollider2D)
        {
            source = new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Mesh,
                sourceObject = generatedStaticFeatureMesh,
                area = AreaNotWalkable,
                transform = Matrix4x4.TRS(worldPositionIncludingOffset, collider2D.transform.rotation, collider2D.transform.lossyScale)
            };
        }

        if(null != boxCollider2D)
        {
            source = new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Box,
                size = boxCollider2D.size,
                area = AreaNotWalkable,
                transform = Matrix4x4.TRS(worldPositionIncludingOffset, collider2D.transform.rotation, collider2D.transform.lossyScale)
            };
        }

        if (null != circleCollider2D)
        {
            float diameter = circleCollider2D.radius * 2f;
            source = new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Sphere,
                size = new Vector3(diameter, diameter, diameter),
                area = AreaNotWalkable,
                transform = Matrix4x4.TRS(worldPositionIncludingOffset, collider2D.transform.rotation, collider2D.transform.lossyScale)
            };
        }

        return source;
    }
}
