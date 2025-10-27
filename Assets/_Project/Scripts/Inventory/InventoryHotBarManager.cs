using System.Linq;

namespace Inventory
{
    public class InventoryHotbarManager
    {
        private readonly InventoryModel _inventory;
        private readonly int _size;

        public InventoryHotbarManager(InventoryModel inventory, int size)
        {
            _inventory = inventory;
            _size = size;
        }

        public InventoryItem[] Hotbar => _inventory.Items.Take(_size).ToArray();

        public void SelectSlot(int index)
        {
            if (index < 0 || index >= Hotbar.Length) return;
            _inventory.SelectItem(Hotbar[index]);
        }
    }
}