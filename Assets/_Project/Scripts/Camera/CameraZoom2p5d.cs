using UnityEngine;

namespace CameraSystem
{
    [RequireComponent(typeof(CameraFollow2P5D))]
    [RequireComponent(typeof(Camera))]
    public class CameraZoom2P5D : MonoBehaviour
    {
        [Header("Zoom Settings")]
        public float zoomSpeed = 5f;
        public float smoothTime = 0.15f;
        public float minZoom = 6f;
        public float maxZoom = 14f;

        private Camera _cam;
        private float _targetZoom;
        private float _velocity;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
            _targetZoom = _cam.orthographic ? _cam.orthographicSize : _cam.fieldOfView;
        }

        private void Update()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            _targetZoom -= scroll * zoomSpeed * 100f * Time.deltaTime;

            _targetZoom = Mathf.Clamp(_targetZoom, minZoom, maxZoom);

            if (_cam.orthographic)
            {
                _cam.orthographicSize = Mathf.SmoothDamp(
                    _cam.orthographicSize,
                    _targetZoom,
                    ref _velocity,
                    smoothTime
                );
            }
            else
            {
                _cam.fieldOfView = Mathf.SmoothDamp(
                    _cam.fieldOfView,
                    _targetZoom,
                    ref _velocity,
                    smoothTime
                );
            }
        }
    }
}
