using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Build and update a localized navmesh from the sources marked by NavMeshSourceTag
[DefaultExecutionOrder(-102)]
public class GenerateNavMesh2D : Singleton<GenerateNavMesh2D>
{
    public Vector2 PlaneDimensions = new Vector2(100f, 100f);

    public float AgentRadiusToVoxelMultiplier = 0.3f;

    private Vector3 boundsDimensions;
    private Vector3 boundsCentre = Vector3.zero;

    public int TileSize = 256;

    /// <summary>
    /// prefab of the XY-plane nav mesh surface.
    /// </summary>
    public NavMeshSurface ArbitraryPlane;

    /// <summary>
    /// on the 'Navigation' window, in the 'Areas' tab, this is the index of the 'Not Walkable' area type.
    /// </summary>
    public int AreaNotWalkable = 1;
    
    [HideInInspector]
    private NavMeshBuildSettings[] buildSettings;
    public NavMeshBuildSettings[] NavMeshBuildSettings { get { return buildSettings; } }
    NavMeshSurface[] surfaces;

    List<NavMeshBuildSource> allSources = new List<NavMeshBuildSource>();
    public bool build = false;

    private void Start()
    {
        InitializeNavMesh();
        RebuildNavMesh();
    }
    private void Update()
    {
        if (build)
        {
            build = false;
            RebuildNavMesh();
        }
    }

    public void InitializeNavMesh()
    {

        int navMeshBuildSettingCount = NavMesh.GetSettingsCount();
        buildSettings = new NavMeshBuildSettings[navMeshBuildSettingCount];
        for (int i = 0; i < navMeshBuildSettingCount; i++)
        {
            //get the Unity-stored build setting
            NavMeshBuildSettings current = NavMesh.GetSettingsByIndex(i);
            //cache it for later access
            buildSettings[i] = current;
        }
        
        // Construct and add navmesh surfaces
        surfaces = new NavMeshSurface[buildSettings.Length];
        for (int i = 0; i < buildSettings.Length; i++)
        {
            var current = buildSettings[i];
            //create a surface for this navmeshagent
            var surf = Instantiate(ArbitraryPlane);

            surf.agentTypeID = current.agentTypeID;
            UpdateSurfaceSettings(surf);

            //remove any pre-existing data on it
            surf.RemoveData();
            surf.navMeshData = null;
            
            //and build it
            surf.BuildNavMesh(overrideSettings: current);
            
            surfaces[i] = surf;
        }

    }

    
    public void RebuildNavMesh()
    {
        var allStaticFeatures = GameObject.FindObjectsOfType<NavMeshStaticFeature>();
        allSources.Clear();
        allSources.AddRange(allStaticFeatures.Select(x => x.GenerateBuildSource()));
        
        
        for(int i = 0; i < surfaces.Length; i++)
        {
            var surface = surfaces[i];
            var buildSetting = buildSettings[i];

            //update the surface to be the correct size, in case it's changed
            UpdateSurfaceSettings(surface);
            //update tile size, etc
            UpdateBuildSettings(ref buildSetting);
            //ensure the changes are saved to the cached copy
            buildSettings[i] = buildSetting;

            //rebuild the navmesh
            surface.BuildNavMesh(allSources, overrideSettings: buildSetting);
        }
        
    }

    private void UpdateSurfaceSettings(NavMeshSurface surface)
    {
        //planes are of dimensions 10x10, not 1x1!
        float planeSize = 10f;
        surface.transform.localScale = new Vector3((PlaneDimensions.x) / planeSize, 1f, (PlaneDimensions.y) / planeSize);
    }
    
    private void UpdateBuildSettings(ref NavMeshBuildSettings settings)
    {
        settings.overrideTileSize = true;
        settings.tileSize = TileSize;
        settings.overrideVoxelSize = true;
        settings.voxelSize = settings.agentRadius * AgentRadiusToVoxelMultiplier;
    }

}
