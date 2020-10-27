using System.Collections.Generic;
using Assets.Unit.Movement;
using Pathfinding;
using UnityEngine;

public class DynamicObstacle : MonoBehaviour
{
    public float boundsExpansion;
    public float updateFrequency;
    public float positionThreshold;

    private readonly IList<GraphNode> nodesChanged = new List<GraphNode>();
    private Vector3 lastPosition;

    void Start()
    {
        InvokeRepeating(nameof(UpdateGraph), Random.value/2f, updateFrequency);
    }

    void UpdateGraph()
    {
        if (Vector2.Distance(transform.position, lastPosition) < positionThreshold) return;

        var bounds = GetComponent<Collider2D>().bounds;
        bounds.Expand(boundsExpansion);

        AstarPath.active.AddWorkItem(new AstarWorkItem(ctx =>
        {
            RestoreNodes();
            ModifyNodes(bounds);
        }));

        lastPosition = transform.position;
    }

    private void RestoreNodes()
    {
        var gg = AstarPath.active.data.gridGraph;
        foreach (var node in nodesChanged)
        {
            var shouldRecalculate = DynamicObstacleNodeOccupationRegistry.Pull(node);
            if (!shouldRecalculate) continue;

            var gridNode = (GridNodeBase) node;
            gg.CalculateConnectionsForCellAndNeighbours(gridNode.XCoordinateInGrid, gridNode.ZCoordinateInGrid);
        }

        nodesChanged.Clear();
    }

    private void ModifyNodes(Bounds bounds)
    {
        var gg = AstarPath.active.data.gridGraph;
        var nodes = gg.GetNodesInRegion(bounds);
        foreach (var node in nodes)
        {
            nodesChanged.Add(node);
            var shouldModify = DynamicObstacleNodeOccupationRegistry.Push(node);

            if (!shouldModify) continue;

            node.Walkable = false;
            var gridNode = (GridNodeBase)node;
            gg.CalculateConnectionsForCellAndNeighbours(gridNode.XCoordinateInGrid, gridNode.ZCoordinateInGrid);
        }
    }
}
