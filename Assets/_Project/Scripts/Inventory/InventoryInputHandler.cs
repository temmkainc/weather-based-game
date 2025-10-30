using UnityEngine;
using Zenject;

namespace Inventory.UI
{
    public class InventoryInputHandler : MonoBehaviour
    {
        [Inject] private InventoryFullView _inventoryView;
        [Inject] private InventoryHotbarView _hotbarView;

        private bool _isOpen;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _isOpen = !_isOpen;
                if (_isOpen)
                {
                    _hotbarView.Hide();
                    _inventoryView.Show();
                }
                else
                {
                    _hotbarView.Show();
                    _inventoryView.Hide();
                }
            }
        }
    }
}