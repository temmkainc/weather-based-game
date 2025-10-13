using UnityEngine;
using Zenject;

namespace CameraSystem
{
    public class CameraRotator : MonoBehaviour
    {
        [Inject] private CameraConfig _config;
        [Inject] private SignalBus _signalBus;

        private float _rotationSpeed;
        private float _rotationStep;

        private float _currentY;
        private CameraFollow2P5D _cameraFollow;

        private bool _isRotating = false;

        private void Awake()
        {
            _cameraFollow = GetComponent<CameraFollow2P5D>();
            _currentY = _cameraFollow.rotationY;

            _rotationSpeed = _config.RotationSpeed;
            _rotationStep = _config.RotationStep;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Rotate(-_rotationStep);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Rotate(_rotationStep);
            }

            _cameraFollow.rotationY = Mathf.MoveTowardsAngle(
                _cameraFollow.rotationY,
                _currentY,
                _rotationSpeed * Time.deltaTime
            );

            if (_isRotating && Mathf.Approximately(_cameraFollow.rotationY, _currentY))
            {
                _isRotating = false;
            }
        }

        private void Rotate(float delta)
        {
            if (_isRotating)
                return;
            _currentY += delta;
            _signalBus.Fire(new CameraRotatedSignal(_currentY));
            _isRotating = true;
        }
    }
}
