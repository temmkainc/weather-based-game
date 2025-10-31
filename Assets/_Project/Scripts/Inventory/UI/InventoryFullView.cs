using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Inventory.UI
{
    public class InventoryFullView : InventoryViewBase
    {
        [Inject] private InventoryModel _inventory;
        [Inject] private InventoryHotbarManager _hotbarManager;

        [Header("References")]
        [SerializeField] private Transform _gridParent;
        [SerializeField] private Image _dragIcon;
        private TMP_Text _dragQuantityTMP;

        private int? _selectedSlotIndex;

        private void Start()
        {
            _dragQuantityTMP = _dragIcon.GetComponentInChildren<TMP_Text>(includeInactive: true);
            _dragIcon.enabled = false;

            BuildSlots(_gridParent, _inventory.Capacity, On_SlotSelected, _hotbarManager.Size);
            Refresh();
            Hide();

            _inventory.OnItemAdded += On_ItemUpdated;
            _inventory.OnItemRemoved += On_ItemUpdated;
            _inventory.OnItemChanged += On_ItemUpdated;
        }

        private void Update()
        {
            if (_dragIcon.enabled)
                _dragIcon.transform.position = Input.mousePosition;
        }

        public override void Show() { gameObject.SetActive(true); Refresh(); }
        public override void Hide() => gameObject.SetActive(false);

        public override void Refresh()
        {
            var items = _inventory.Items;
            for (int i = 0; i < _slotImages.Length; i++)
                SetSlotVisual(i, items[i]);
        }

        public override void On_SlotSelected(int index)
        {
            var item = _inventory.Items[index];
            _slotImages[index].enabled = false;
            _slotCountLabels[index].enabled = false;

            if (_selectedSlotIndex == null)
            {
                if (item == null) return;
                _selectedSlotIndex = index;
                _dragIcon.sprite = item.Data.Icon;
                _dragIcon.color = Color.white;
                _dragIcon.enabled = true;
                _dragQuantityTMP.text = item.Quantity > 1 ? item.Quantity.ToString() : "";
                _dragQuantityTMP.enabled = true;
                return;
            }

            int prevIndex = _selectedSlotIndex.Value;
            _inventory.SwapItems(prevIndex, index);
            _selectedSlotIndex = null;

            _slotImages[index].enabled = _slotImages[prevIndex].enabled = true;
            _slotCountLabels[index].enabled = _slotCountLabels[prevIndex].enabled = true;

            _dragIcon.enabled = false;
            _dragQuantityTMP.enabled = false;
            Refresh();
            _hotbarManager.HotbarView.Refresh();
        }

        private void On_ItemUpdated(int index, InventoryItem item)
        {
            if (index < 0 || index >= _slotImages.Length) return;
            SetSlotVisual(index, item);
            if (index < _hotbarManager.Size)
                _hotbarManager.HotbarView.Refresh();
        }

        private void OnDestroy()
        {
            _inventory.OnItemAdded -= On_ItemUpdated;
            _inventory.OnItemRemoved -= On_ItemUpdated;
            _inventory.OnItemChanged -= On_ItemUpdated;
        }
    }
}
