using CameraSystem;
using GameLoop;
using UnityEngine;
using WeatherSystem;
using Zenject;

namespace Common
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private CameraConfig _cameraConfig;

        [SerializeField] private ObjectiveListItem _objectiveListItemPrefab;
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<CameraRotatedSignal>();
            Container.DeclareSignal<WeatherDataReadySignal>();
            Container.DeclareSignal<RoundCompletedSignal>();
            Container.DeclareSignal<RoundFailedSignal>();
            Container.DeclareSignal<NextRoundRequestedSignal>();
            Container.DeclareSignal<RestartGameSignal>();

            Container.BindInterfacesAndSelfTo<WeatherService>().AsSingle();

            Container.BindInstance(_cameraConfig).AsSingle();

            Container.Bind<TimerPanel>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ObjectivesPanel>().FromComponentInHierarchy().AsSingle();

            Container.Bind<ObjectiveListItem>().FromComponentInNewPrefab(_objectiveListItemPrefab).AsSingle().NonLazy();
        }
    }
}