using UnityEngine;

namespace Assets
{
    public class CameraController : MonoBehaviour
    {
        public int moveCameraThreshold = 50;
        public float moveSpeed = 1f;
        private Camera cam;

        private void Start()
        {
            cam = GetComponent<Camera>();
        }

        private void Update()
        {
            var moveDirection = GetMoveDirection();

            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }

        private Vector3 GetMoveDirection()
        {
            Vector3 direction = Vector3.zero;

            if (Input.mousePosition.x < moveCameraThreshold)
            {
                direction += Vector3.left;
            }

            if (Input.mousePosition.y < moveCameraThreshold)
            {
                direction += Vector3.down;
            }

            if (Input.mousePosition.x > cam.pixelWidth - moveCameraThreshold)
            {
                direction += Vector3.right;
            }

            if (Input.mousePosition.y > cam.pixelHeight - moveCameraThreshold)
            {
                direction += Vector3.up;
            }

            return direction;
        }
    }
}
