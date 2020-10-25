using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class DynamicObstacle : MonoBehaviour
{
    private AstarPath astar;
    private IList<Bounds> boundsUpdated = new List<Bounds>();
    public float boundsExpansion;
    public float updateFrequency;

    void Start()
    {
        astar = AstarPath.active;
    }

    void Update()
    {
        InvokeRepeating(nameof(UpdateGraph), 0f, updateFrequency);
    }

    void UpdateGraph()
    {
        var bounds = GetComponent<Collider2D>().bounds;
        bounds.Expand(boundsExpansion);

        var updateObject = new GraphUpdateObject(bounds)
        {
            setWalkability = false, 
            modifyWalkability = true
        };

        RestorePreviousNodes();

        astar.UpdateGraphs(updateObject);
        boundsUpdated.Add(bounds);
    }

    void RestorePreviousNodes()
    {
        foreach (var bounds in boundsUpdated)
        {
            var guo = new GraphUpdateObject(bounds)
            {
                modifyWalkability = false
            };
            astar.UpdateGraphs(guo);
        }
        boundsUpdated.Clear();
    }
}
