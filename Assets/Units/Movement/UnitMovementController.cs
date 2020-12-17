using Pathfinding;
using UnityEngine;

namespace Units.Movement
{
    // Using dynamic rigidbody to allow pushing one unit by another
    // This lowers the chance of units getting stuck
    public class UnitMovementController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float distanceToFocusOnNextWaypoint;

        public float otherUnitRecalculateDistance;
        public float maxDistanceToPath;
        public float collisionCheckFrequency;

        public bool IsMoving { get; private set; }

        private Path path;
        private int currentWaypoint;

        private Seeker seeker;
        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            seeker = GetComponent<Seeker>();
        }

        private void Update()
        {
            if (path == null) return;
            if (currentWaypoint >= path.vectorPath.Count) return;

            Move();
        }

        private void Move()
        {
            Vector2 currentDestination = path.vectorPath[currentWaypoint];

            if (currentWaypoint == path.vectorPath.Count - 1)
            {
                var distanceToWaypoint = Vector2.Distance(currentDestination, rb.position);
                if (distanceToWaypoint < 0.1f)
                {
                    Stop();
                    return;
                }
            }
            else if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < distanceToFocusOnNextWaypoint)
            {
                currentDestination = path.vectorPath[++currentWaypoint];
            }

            var vectorToDestination = currentDestination - rb.position;
            var direction = vectorToDestination.normalized;
            var moveVector = direction * moveSpeed * Time.deltaTime;

            var movePosition = rb.position + moveVector;
            rb.MovePosition(movePosition);
        }

        private void OnPathCalculated(Path path)
        {
            if (path.error) return;

            this.path = path;
            currentWaypoint = 0;
        }

        public void Stop()
        {
            if (IsCalculatingPath())
            {
                seeker.CancelCurrentPathRequest();
            }
            CancelInvoke(nameof(HandleCollision));
            path = null;
            IsMoving = false;
        }

        private bool IsCalculatingPath()
        {
            return path == null && IsMoving;
        }

        private void HandleCollision() => UnitCollisionHandler.HandleCollision(this, path);

        public void SetDestination(Vector3 dest)
        {
            seeker.StartPath(rb.position, dest, OnPathCalculated);
            IsMoving = true;
            InvokeRepeating(nameof(HandleCollision), collisionCheckFrequency, collisionCheckFrequency);
        }
    }
}
