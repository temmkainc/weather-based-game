using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory
{
    public class InventoryModel
    {
        public int Capacity { get; }
        public InventoryItem SelectedItem { get; private set; }

        private readonly InventoryItem[] _items;
        public IReadOnlyList<InventoryItem> Items => _items;

        public event Action<int, InventoryItem> OnItemAdded;
        public event Action<int, InventoryItem> OnItemRemoved;
        public event Action<int, InventoryItem> OnItemChanged;
        public event Action<InventoryItem> OnItemSelected;

        public InventoryModel(int capacity, IEnumerable<ItemData> startingItems = null)
        {
            Capacity = capacity;
            _items = new InventoryItem[capacity];

            if (startingItems != null)
            {
                int index = 0;
                foreach (var data in startingItems.Where(i => i != null))
                {
                    if (index >= capacity) break;
                    AddItemToSlot(index, data, 1);
                    index++;
                }
            }
        }

        public bool AddItemToSlot(int index, ItemData data, int amount = 1)
        {
            if (index < 0 || index >= Capacity || data == null)
                return false;

            var item = _items[index];
            if (item == null)
            {
                _items[index] = new InventoryItem { Data = data, Quantity = amount };
                OnItemAdded?.Invoke(index, _items[index]);
                OnItemChanged?.Invoke(index, _items[index]);
                return true;
            }

            if (item.Data == data)
            {
                item.Quantity += amount;
                OnItemChanged?.Invoke(index, item);
                return true;
            }

            return false;
        }

        public bool SwapItems(int indexA, int indexB)
        {
            if (indexA < 0 || indexA >= Capacity || indexB < 0 || indexB >= Capacity || indexA == indexB)
                return false;

            if (_items[indexA] == null && _items[indexB] == null)
                return false;

            var temp = _items[indexA];
            _items[indexA] = _items[indexB];
            _items[indexB] = temp;

            OnItemChanged?.Invoke(indexA, _items[indexA]);
            OnItemChanged?.Invoke(indexB, _items[indexB]);

            return true;
        }

        public bool AddItemToFirstFreeSlot(ItemData data, int amount = 1)
        {
            if (data == null) return false;

            for (int i = 0; i < Capacity; i++)
            {
                var existing = _items[i];
                if (existing != null && existing.Data == data && data.IsStackable)
                {
                    existing.Quantity += amount;
                    OnItemChanged?.Invoke(i, existing);
                    return true;
                }
            }

            for (int i = 0; i < Capacity; i++)
            {
                if (_items[i] == null)
                {
                    return AddItemToSlot(i, data, amount);
                }
            }

            return false;
        }


        public bool RemoveItemAtSlot(int index, int amount = 1)
        {
            if (index < 0 || index >= Capacity) return false;
            var item = _items[index];
            if (item == null) return false;

            item.Quantity -= amount;
            if (item.Quantity <= 0)
            {
                _items[index] = null;
                OnItemRemoved?.Invoke(index, item);
                OnItemChanged?.Invoke(index, null);
            }
            else
            {
                OnItemChanged?.Invoke(index, item);
            }

            return true;
        }

        public void SelectItem(InventoryItem item)
        {
            if (!_items.Contains(item))
            {
                SelectedItem = null;
                OnItemSelected?.Invoke(null);
                return;
            }

            SelectedItem = item;
            OnItemSelected?.Invoke(item);
        }

        public void Clear()
        {
            for (int i = 0; i < Capacity; i++)
            {
                _items[i] = null;
                OnItemChanged?.Invoke(i, null);
            }
        }
    }
}
