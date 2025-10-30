using UnityEngine;
using Zenject;

namespace Inventory
{
    public class HotbarInputHandler : MonoBehaviour
    {
        private InventoryHotbarManager _hotbarManager;

        [Inject]
        public void Construct(InventoryHotbarManager hotbarManager)
        {
            _hotbarManager = hotbarManager;
        }

        private void Update()
        {
            if (_hotbarManager == null) return;

            for (int i = 0; i < _hotbarManager.Hotbar.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                    _hotbarManager.SelectSlot(i);
            }
        }
    }
}
