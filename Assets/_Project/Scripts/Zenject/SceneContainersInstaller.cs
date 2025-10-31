using Farming;
using UnityEngine;
using Zenject;

namespace Common
{
    public class SceneContainersInstaller : MonoInstaller
    {
        [SerializeField] private PotSceneContainer _potSceneContainer;
        public override void InstallBindings()
        {
            Container.BindInstance(_potSceneContainer).AsSingle();
        }
    }
}