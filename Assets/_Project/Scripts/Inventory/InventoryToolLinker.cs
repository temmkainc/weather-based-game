using Farming.Tools;
using UnityEngine;
using Zenject;

namespace Inventory
{
    public class InventoryToolLinker : MonoBehaviour
    {
        [Inject] private InventoryModel _inventory;
        [SerializeField] private ToolManager _toolManager;

        private void OnEnable()
        {
            _inventory.OnItemSelected += HandleItemSelected;
            _inventory.OnItemChanged += HandleItemChanged;
        }

        private void OnDisable()
        {
            _inventory.OnItemSelected -= HandleItemSelected;
            _inventory.OnItemChanged -= HandleItemChanged;
        }

        private void HandleItemChanged(int slot, InventoryItem item)
        {
            if (_inventory.SelectedItem == null)
                _toolManager.SetTool(null);
        }

        private void HandleItemSelected(InventoryItem item)
        {
            if (item == null || !(item.Data.Type == ItemType.Tool || item.Data.Type == ItemType.Seeds))
            {
                _toolManager.SetTool(null);
                return;
            }

            _toolManager.SetTool(item.Data.ToolReference);
        }
    }
}
