using CameraSystem;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Common
{
    public class BillboardToCamera : MonoBehaviour
    {
        [Inject] private CameraConfig _config;
        [Inject] private SignalBus _signalBus;

        private CancellationTokenSource _cts;

        private void OnEnable() => _signalBus.Subscribe<CameraRotatedSignal>(OnCameraRotated);
        private void OnDisable() => _signalBus.Unsubscribe<CameraRotatedSignal>(OnCameraRotated);

        private async void OnCameraRotated(CameraRotatedSignal signal)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;

            float targetY = signal.RotationY;

            try
            {
                while (!Mathf.Approximately(transform.rotation.eulerAngles.y, targetY))
                {
                    token.ThrowIfCancellationRequested();

                    Vector3 currentEuler = transform.rotation.eulerAngles;
                    float newY = Mathf.MoveTowardsAngle(currentEuler.y, targetY, _config.RotationSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0f, newY, 0f);

                    await UniTask.Yield(token);
                }
            }
            catch (OperationCanceledException)
            {
                // Rotation canceled by a new signal - nothing to do
            }
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
