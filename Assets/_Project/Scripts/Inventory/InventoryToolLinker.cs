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
        }

        private void OnDisable()
        {
            _inventory.OnItemSelected -= HandleItemSelected;
        }

        private void HandleItemSelected(InventoryItem item)
        {
            if (item.Data.ToolReference != null)
            {
                _toolManager.SetTool(item.Data.ToolReference);
            }
        }
    }
}
