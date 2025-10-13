using CameraSystem;
using UnityEngine;
using Zenject;

namespace Common
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private CameraConfig _cameraConfig;
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<CameraRotatedSignal>();

            Container.BindInstance(_cameraConfig).AsSingle();
        }
    }
}