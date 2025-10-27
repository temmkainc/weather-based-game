using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Inventory
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] private InventoryModel _playerInventory;
        [SerializeField] private List<ItemData> _startingItems;

        public override void InstallBindings()
        {
            Container.Bind<InventoryModel>().AsSingle()
                     .WithArguments(_startingItems);

            Container.Bind<InventoryHotbarManager>().AsSingle()
                     .WithArguments(5);
        }
    }
}