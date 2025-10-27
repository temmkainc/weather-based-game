using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory
{
    public class InventoryModel
    {
        private readonly List<InventoryItem> _items;

        public IReadOnlyList<InventoryItem> Items => _items;

        public event Action<InventoryItem> OnItemAdded;
        public event Action<InventoryItem> OnItemRemoved;
        public event Action<InventoryItem> OnItemSelected;

        public InventoryModel(IEnumerable<ItemData> startingItems = null)
        {
            _items = new List<InventoryItem>();

            if (startingItems != null)
            {
                foreach (var item in startingItems.Where(i => i != null))
                    AddItem(item, 1);
            }
        }
        public void AddItem(ItemData data, int amount = 1)
        {
            var existing = _items.FirstOrDefault(i => i.Data == data);
            if (existing != null)
                existing.Quantity += amount;
            else
                _items.Add(new InventoryItem { Data = data, Quantity = amount });

            OnItemAdded?.Invoke(existing ?? _items[^1]);
        }

        public bool RemoveItem(ItemData data, int amount = 1)
        {
            var existing = _items.FirstOrDefault(i => i.Data == data);
            if (existing == null || existing.Quantity < amount)
                return false;

            existing.Quantity -= amount;
            if (existing.Quantity <= 0)
                _items.Remove(existing);

            OnItemRemoved?.Invoke(existing);
            return true;
        }

        public IEnumerable<InventoryItem> GetItemsByType(ItemType type)
        {
            return _items.Where(i => i.Data.Type == type);
        }

        public void SelectItem(InventoryItem item)
        {
            if (!_items.Contains(item)) return;
            OnItemSelected?.Invoke(item);
        }
    }
}
