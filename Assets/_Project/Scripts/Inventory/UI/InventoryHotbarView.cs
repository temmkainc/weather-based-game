using UnityEngine;
using Zenject;

namespace Inventory.UI
{
    public class InventoryHotbarView : InventoryViewBase
    {
        [Inject] private InventoryHotbarManager _hotbarManager;

        [Header("References")]
        [SerializeField] private Transform _selectedOutline;

        private void Start()
        {
            _hotbarManager.OnSlotSelected += On_SlotSelected;
            _selectedOutline.gameObject.SetActive(false);

            BuildSlots(transform, _hotbarManager.Hotbar.Length, On_SlotClicked, _hotbarManager.Size);
            Refresh();
        }

        public override void Show() { gameObject.SetActive(true); _selectedOutline.gameObject.SetActive(true); }
        public override void Hide() { gameObject.SetActive(false); _selectedOutline.gameObject.SetActive(false); }

        public override void Refresh()
        {
            var hotbar = _hotbarManager.Hotbar;
            for (int i = 0; i < hotbar.Length; i++)
                SetSlotVisual(i, hotbar[i]);
        }

        private void On_SlotClicked(int index)
        {
            _hotbarManager.SelectSlot(index);
        }

        public override void On_SlotSelected(int index)
        {
            if (index < 0 || index >= _slotImages.Length) return;
            _selectedOutline.transform.position = _slotImages[index].transform.position;
            _selectedOutline.gameObject.SetActive(true);
            Refresh();
        }

        private void OnDestroy()
        {
            _hotbarManager.OnSlotSelected -= On_SlotSelected;
        }
    }
}
