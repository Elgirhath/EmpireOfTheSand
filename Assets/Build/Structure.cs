using Map;
using Units.Attacking;
using UnityEngine;

namespace Build
{
    public abstract class Structure : MonoBehaviour, IAttackable
    {
        public CustomTile tile;
        public int hp;

        protected virtual void Start()
        {
            tile = GameMap.Instance.GetTileAtPosition(transform.position);
            var x = 42;
        }

        public void Attack(int attackStrength)
        {
            hp--;
            if (hp <= 0)
            {
                Destroy();
            }
        }

        public virtual void Destroy()
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