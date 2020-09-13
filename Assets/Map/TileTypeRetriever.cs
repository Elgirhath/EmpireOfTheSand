using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Map
{
    public class TileTypeRetriever : MonoBehaviour
    {
        public Tile sandTile;
        public Tile waterTile;

        public TileType GetTileType(TileBase tile)
        {
            if (tile == sandTile) return TileType.Sand;
            if (tile == waterTile) return TileType.Water;
            return TileType.Default;
        }
    }
}