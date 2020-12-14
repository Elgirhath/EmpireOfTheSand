using System.Linq;
using Assets.Util;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Map
{
    public class GameMap : MonoBehaviour
    {
        private GridLayout gridLayout;
        public TileTypeRetriever TileTypeRetriever { get; private set; }

        public Tilemap[] Tilemaps { get; private set; }
        public static GameMap Instance { get; private set; }
        public TileMatrix Matrix { get; private set; }

        private void Awake()
        {
            Instance = this;
            gridLayout = GetComponent<GridLayout>();
            Tilemaps = GetComponentsInChildren<Tilemap>();
            TileTypeRetriever = GetComponentInParent<TileTypeRetriever>();
            Matrix = TileMatrix.Generate(Tilemaps, TileTypeRetriever);
        }


        //void Update()
        //{
        //    for (int x = 0; x < Matrix.size.x; x++)
        //    {
        //        for (int y = 0; y < Matrix.size.y; y++)
        //        {
        //            var tile = Matrix.tiles[x, y];
        //            var color = tile.structure == null ? Color.blue : Color.red;
        //            DebugExtension.DrawCross(tile.tile.position, 0.1f, color);
        //        }
        //    }
        //}

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
                        type = TileTypeRetriever.GetTileType(tile)
                    };
                }
            }

            return null;
        }

    }
}
