using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Map
{
    public class CustomTile
    {
        public Vector3 position;
        public TileBase @base;
        public TileType type;
        public GameObject structure;
    }
}