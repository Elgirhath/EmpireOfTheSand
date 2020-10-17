using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Map
{
    public class GameMap : MonoBehaviour
    {
        private GridLayout gridLayout;
        private TileTypeRetriever tileTypeRetriever;

        public Tilemap tilemap { get; private set; }
        public static GameMap Instance { get; private set; }

        private void Start()
        {
            Instance = this;
            gridLayout = GetComponentInParent<GridLayout>();
            tilemap = GetComponentInParent<Tilemap>();
            tileTypeRetriever = GetComponentInParent<TileTypeRetriever>();
        }

        public Vector3Int GetCellPosition(Vector3 worldPosition)
        {
            worldPosition.z = transform.position.z;
            return gridLayout.WorldToCell(worldPosition);
        }

        public Vector3 SnapToGrid(Vector3 worldPosition)
        {
            var cellPosition = GetCellPosition(worldPosition);
            return tilemap.GetCellCenterWorld(cellPosition);
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
}
