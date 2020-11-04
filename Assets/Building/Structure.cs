using Assets.Map;
using Assets.Units.Attacking;
using UnityEngine;

namespace Assets.Building
{
    public abstract class Structure : MonoBehaviour, IAttackable
    {
        public CustomTile tile;
        public int hp;

        protected virtual void Start()
        {
            tile = GameMap.Instance.GetTileAtPosition(transform.position);
        }

        public void Attack(int attackStrength)
        {
            hp--;
            if (hp <= 0)
            {
                Destroy();
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public Vector2 Position => transform.position;

        private void OnDestroy()
        {
            IsDestroyed = true;
        }

        public bool IsDestroyed { get; private set; }
    }
}