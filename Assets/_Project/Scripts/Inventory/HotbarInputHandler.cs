using Inventory.UI;
using UnityEngine;
using Zenject;

namespace Inventory
{
    public class HotbarInputHandler : MonoBehaviour
    {
        private InventoryHotbarManager _hotbarManager;
        private InventoryHotbarView _hotbarView;

        [Inject]
        public void Construct(InventoryHotbarManager hotbarManager, InventoryHotbarView view)
        {
            _hotbarManager = hotbarManager;
            _hotbarView = view;
        }

        private void Update()
        {
            if (_hotbarManager == null || !_hotbarView.gameObject.activeSelf) return;

            for (int i = 0; i < _hotbarManager.Hotbar.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                    _hotbarManager.SelectSlot(i);
            }
        }
    }
}
