using Inventory.UI;
using System;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    public class InventoryHotbarManager
    {
        public readonly InventoryHotbarView HotbarView;
        public int SelectedSlotIndex => _selectedSlotIndex;
        public int Size => _size;
        public event Action<int> OnSlotSelected;

        private readonly InventoryModel _inventory;
        private readonly int _size;
        private int _selectedSlotIndex;

        public InventoryHotbarManager(InventoryModel inventory, int size, InventoryHotbarView hotbarView)
        {
            _inventory = inventory;
            Debug.Log(_inventory.Capacity);
            _size = size;
            HotbarView = hotbarView;
        }

        public InventoryItem[] Hotbar =>
            _inventory.Items
                .Take(_size)
                .Concat(Enumerable.Repeat<InventoryItem>(null, _size))
                .Take(_size)
                .ToArray();

        public void SelectSlot(int index)
        {
            if (index < 0 || index >= Hotbar.Length) return;
            _selectedSlotIndex = index;
            OnSlotSelected?.Invoke(index);
            _inventory.SelectItem(Hotbar[index]);
        }
    }
}