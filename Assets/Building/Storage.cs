using Assets.Map;
using Assets.Unit.ResourceGathering;
using UnityEngine;

namespace Assets.Building
{
    public class Storage : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private TileType type;

        [SerializeField]
        private int capacity;
#pragma warning restore 649

        public TileType Type => type;

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