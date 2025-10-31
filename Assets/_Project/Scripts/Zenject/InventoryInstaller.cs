using Inventory.UI;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Inventory
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] private InventoryModel _playerInventory;
        [SerializeField] private List<ItemData> _startingItems;
        [SerializeField] private int _initialHotbarCapacity = 9;
        [SerializeField] private int _initialCapacity = 18;

        public override void InstallBindings()
        {
            Container.Bind<InventoryModel>().AsSingle()
                     .WithArguments(_initialCapacity, _startingItems);

            Container.Bind<InventoryFullView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryInputHandler>().AsSingle();

            Container.Bind<InventoryHotbarView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<InventoryHotbarManager>().AsSingle()
                     .WithArguments(_initialHotbarCapacity);
        }
    }
}