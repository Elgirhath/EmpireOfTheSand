using UnityEngine;

namespace Assets.Unit
{
    // Using dynamic rigidbody to allow pushing one unit by another
    // This lowers the chance of units getting stuck
    public class UnitMovementController : MonoBehaviour
    {
        public float moveSpeed = 10f;

        public bool IsMoving { get; set; } = false;
        private Vector3 destination;

        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (IsMoving)
            {
                Move();
            }
        }

        private void Move()
        {
            var vectorToDestination = (Vector2)destination - rb.position;
            var direction = vectorToDestination.normalized;
            var moveVector = direction * moveSpeed * Time.deltaTime;

            if (moveVector.magnitude > vectorToDestination.magnitude)
            {
                moveVector = vectorToDestination;
                IsMoving = false;
            }

            var movePosition = rb.position + moveVector;
            rb.MovePosition(movePosition);
        }

        public void SetDestination(Vector3 dest)
        {
            destination = dest;
            IsMoving = true;
        }
    }
}
