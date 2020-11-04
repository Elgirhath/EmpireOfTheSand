using Assets.Map;
using UnityEngine;

namespace Assets.Building
{
    public abstract class Structure : MonoBehaviour
    {
        public CustomTile tile;

        protected virtual void Start()
        {
            tile = GameMap.Instance.GetTileAtPosition(transform.position);
        }
    }
}