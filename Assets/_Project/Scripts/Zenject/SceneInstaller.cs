using Farming;
using PlayerSystem;
using UnityEngine;
using Zenject;

namespace Common
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private PotSceneContainer _potSceneContainer;
        [SerializeField] private ShopPanel _shopPanel;
        [SerializeField] private Player _player;
        public override void InstallBindings()
        {
            Container.BindInstance(_potSceneContainer).AsSingle();
            Container.BindInstance(_shopPanel).AsSingle();
            Container.BindInstance(_player).AsSingle();
        }
    }
}