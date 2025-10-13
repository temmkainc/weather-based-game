using UnityEngine;

namespace CameraSystem
{
    public class CameraFollow2P5D : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;

        [Header("Camera Settings")]
        public Vector3 offset = new Vector3(10f, 10f, -10f);
        public float followSpeed = 5f;
        public bool useOrthographic = true;
        public float orthographicSize = 10f;
        public float rotationX = 45f;
        public float rotationY = 45f;

        private Camera _cam;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
            if (useOrthographic)
            {
                _cam.orthographic = true;
                _cam.orthographicSize = orthographicSize;
            }
        }

        private void LateUpdate()
        {
            if (!target) return;

            Quaternion desiredRotation = Quaternion.Euler(rotationX, rotationY, 0);
            transform.rotation = desiredRotation;

            Vector3 desiredPosition = target.position + desiredRotation * offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }
}
