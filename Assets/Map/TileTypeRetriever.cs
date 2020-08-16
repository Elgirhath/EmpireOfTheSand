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
            if (tile == sandTile) return TileType.SAND;
            if (tile == waterTile) return TileType.WATER;
            return TileType.DEFAULT;
        }
    }
}