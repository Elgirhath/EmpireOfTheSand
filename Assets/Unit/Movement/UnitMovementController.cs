using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Assets.Unit.Movement
{
    // Using dynamic rigidbody to allow pushing one unit by another
    // This lowers the chance of units getting stuck
    public class UnitMovementController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float distanceToFocusOnNextWaypoint;
        public float smoothingThreshold;

        public bool IsMoving { get; private set; }

        private bool isCalculationInterrupted = false;

        private IList<Vector3> waypointList;
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
            if (waypointList == null) return;
            if (currentWaypoint >= waypointList.Count) return;

            Move();
        }

        private void Move()
        {
            Vector2 currentDestination = waypointList[currentWaypoint];

            if (currentWaypoint == waypointList.Count - 1)
            {
                var distanceToWaypoint = Vector2.Distance(currentDestination, rb.position);
                if (distanceToWaypoint < 0.1f)
                {
                    Stop();
                    return;
                }
            }
            else if (Vector2.Distance(rb.position, waypointList[currentWaypoint]) < distanceToFocusOnNextWaypoint)
            {
                currentDestination = waypointList[++currentWaypoint];
            }

            var vectorToDestination = currentDestination - rb.position;
            var direction = vectorToDestination.normalized;
            var moveVector = direction * moveSpeed * Time.deltaTime;

            var movePosition = rb.position + moveVector;
            rb.MovePosition(movePosition);
        }

        private void OnPathCalculated(Path path)
        {
            if (isCalculationInterrupted)
            {
                isCalculationInterrupted = false;
                return;
            }

            if (path.error) return;

            waypointList = MovementPathInterpolator.SmoothOut(path.vectorPath, smoothingThreshold);
            currentWaypoint = 0;
        }

        public void Stop()
        {
            if (IsCalculatingPath())
            {
                isCalculationInterrupted = true;
            }
            waypointList = null;
            IsMoving = false;
        }

        private bool IsCalculatingPath()
        {
            return waypointList == null && IsMoving;
        }

        public void SetDestination(Vector3 dest)
        {
            seeker.StartPath(rb.position, dest, OnPathCalculated);
            IsMoving = true;
        }
    }
}
