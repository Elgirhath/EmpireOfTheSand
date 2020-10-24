using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Map
{
    public class GameMap : MonoBehaviour
    {
        private GridLayout gridLayout;
        private TileTypeRetriever tileTypeRetriever;

        public Tilemap[] Tilemaps { get; private set; }
        public static GameMap Instance { get; private set; }

        private void Start()
        {
            Instance = this;
            gridLayout = GetComponent<GridLayout>();
            Tilemaps = GetComponentsInChildren<Tilemap>();
            tileTypeRetriever = GetComponentInParent<TileTypeRetriever>();
        }

        public Vector3 GetCellCenterWorld(Vector3Int cellIndex) => Tilemaps[0].GetCellCenterWorld(cellIndex);

        public Vector3Int GetCellPosition(Vector3 worldPosition)
        {
            worldPosition.z = transform.position.z;
            return gridLayout.WorldToCell(worldPosition);
        }

        public Vector3 SnapToGrid(Vector3 worldPosition)
        {
            return GetCellCenterWorld(GetCellPosition(worldPosition));
        }

        public CustomTile GetTileAtPosition(Vector3 worldPosition)
        {
            foreach (var tilemap in Tilemaps.Reverse())
            {
                var tile = tilemap.GetTile(GetCellPosition(worldPosition));
                if (tile != null)
                {
                    return new CustomTile
                    {
                        position = tilemap.GetCellCenterWorld(GetCellPosition(worldPosition)),
                        @base = tile,
                        type = tileTypeRetriever.GetTileType(tile)
                    };
                }
            }

            return null;
        }
    }
}
