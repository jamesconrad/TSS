using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

// Tagging component for use with the LocalNavMeshBuilder
// Supports mesh-filter and terrain - can be extended to physics and/or primitives
[DefaultExecutionOrder(-200)]
public class NavMeshSourceTag : MonoBehaviour
{
    // Global containers for all active mesh/terrain tags
    public static List<KeyValuePair<MeshFilter, int>> m_Meshes = new List<KeyValuePair<MeshFilter, int>>();
    public static List<Terrain> m_Terrains = new List<Terrain>();
    public int NavmeshLayer = 0;

    void OnEnable()
    {
        var m = GetComponent<MeshFilter>();
        if (m != null)
        {
            m_Meshes.Add(new KeyValuePair<MeshFilter, int>(m, NavmeshLayer));
        }

        var t = GetComponent<Terrain>();
        if (t != null)
        {
            m_Terrains.Add(t);
        }
    }

    void OnDisable()
    {
        var m = GetComponent<MeshFilter>();
        if (m != null)
        {
            m_Meshes.Remove(new KeyValuePair<MeshFilter, int>(m, NavmeshLayer));
        }

        var t = GetComponent<Terrain>();
        if (t != null)
        {
            m_Terrains.Remove(t);
        }
    }

    // Collect all the navmesh build sources for enabled objects tagged by this component
    public static void Collect(ref List<NavMeshBuildSource> sources)
    {
        sources.Clear();

        for (var i = 0; i < m_Meshes.Count; ++i)
        {
            var mf = m_Meshes[i];
            if (mf.Key == null) continue;

            var m = mf.Key.sharedMesh;
            if (m == null) continue;

            var s = new NavMeshBuildSource();
            s.shape = NavMeshBuildSourceShape.Mesh;
            s.sourceObject = m;
            s.transform = mf.Key.transform.localToWorldMatrix;
            s.area = mf.Value;
            sources.Add(s);
        }

        for (var i = 0; i < m_Terrains.Count; ++i)
        {
            var t = m_Terrains[i];
            if (t == null) continue;

            var s = new NavMeshBuildSource();
            s.shape = NavMeshBuildSourceShape.Terrain;
            s.sourceObject = t.terrainData;
            // Terrain system only supports translation - so we pass translation only to back-end
            s.transform = Matrix4x4.TRS(t.transform.position, Quaternion.identity, Vector3.one);
            s.area = 0;
            sources.Add(s);
        }
    }
}
