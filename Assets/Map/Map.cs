using Assets.Map;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    private GridLayout gridLayout;
    private Tilemap tilemap;
    private TileTypeRetriever tileTypeRetriever;

    private void Start()
    {
        gridLayout = GetComponentInParent<GridLayout>();
        tilemap = GetComponentInParent<Tilemap>();
        tileTypeRetriever = GetComponentInParent<TileTypeRetriever>();
    }

    public Vector3Int GetCellPosition(Vector3 worldPosition)
    {
        worldPosition.z = transform.position.z;
        return gridLayout.WorldToCell(worldPosition);
    }

    public CustomTile GetTileAtPosition(Vector3 worldPosition)
    {
        var tile = tilemap.GetTile(GetCellPosition(worldPosition));
        return new CustomTile
        {
            position = tilemap.GetCellCenterWorld(GetCellPosition(worldPosition)),
            @base = tile,
            type = tileTypeRetriever.GetTileType(tile)
        };
    }
}
