using Assets.Map;
using Assets.Unit.ResourceGathering;
using UnityEngine;

namespace Assets.Building
{
    public class Storage : MonoBehaviour
    {
        [SerializeField]
        private TileType type;
        public TileType Type => type;

        [SerializeField]
        private int capacity;

        public int Capacity => capacity;

        [SerializeField]
        private int size;

        public int Size
        {
            get => size;
            set => size = value;
        }
    }
}