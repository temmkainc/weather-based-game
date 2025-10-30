using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Inventory.UI
{
    public class InventoryFullView : MonoBehaviour, IInventoryView
    {
        [Inject] private InventoryModel _inventory;
        [Inject] private InventoryHotbarManager _hotbarManager;

        [Header("References")]
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Transform _gridParent;
        [SerializeField] private Image _dragIcon;

        private Image[] _slotImages;
        private TMP_Text[] _slotIndexLabels;
        private int _capacity;

        private int? _selectedSlotIndex;

        private void Start()
        {
            _capacity = _inventory.Capacity;
            BuildSlots();
            Refresh();
            Hide();

            _inventory.OnItemAdded += On_ItemUpdated;
            _inventory.OnItemRemoved += On_ItemUpdated;
            _inventory.OnItemChanged += On_ItemUpdated;

            _dragIcon.enabled = false;
        }

        private void Update()
        {
            if (_dragIcon.enabled)
                _dragIcon.transform.position = Input.mousePosition;
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void Refresh()
        {
            var items = _inventory.Items;

            for (int i = 0; i < _slotImages.Length; i++)
            {
                var image = _slotImages[i];
                if (i < items.Count && items[i] != null)
                {
                    image.sprite = items[i].Data.Icon;
                    image.color = Color.white;
                }
                else
                {
                    image.sprite = null;
                    image.color = Color.clear;
                }
            }
        }

        private void BuildSlots()
        {
            foreach (Transform child in _gridParent)
                Destroy(child.gameObject);

            _slotImages = new Image[_capacity];
            _slotIndexLabels = new TMP_Text[_capacity];

            for (int i = 0; i < _capacity; i++)
            {
                var slot = Instantiate(_slotPrefab, _gridParent);
                _slotImages[i] = slot.GetComponentsInChildren<Image>(includeInactive: true)
                    .FirstOrDefault(img => img.gameObject != slot);

                _slotIndexLabels[i] = slot.GetComponentsInChildren<TMP_Text>(includeInactive: true)
                    .FirstOrDefault(tmp => tmp.gameObject.name.Contains("Index"));

                var button = slot.GetComponent<Button>();
                if (button != null)
                {
                    int index = i;
                    button.onClick.AddListener(() => On_SlotSelected(index));
                }

                if (i <= _hotbarManager.Size - 1 && _slotIndexLabels[i] != null)
                {
                    _slotIndexLabels[i].gameObject.SetActive(true);
                    _slotIndexLabels[i].text = (i + 1).ToString();
                }
            }
        }

        public void On_SlotSelected(int index)
        {
            var item = _inventory.Items[index];
            _slotImages[index].enabled = false;

            if (_selectedSlotIndex == null)
            {
                if (item == null)
                    return;

                _selectedSlotIndex = index;

                _dragIcon.sprite = item.Data.Icon;
                _dragIcon.color = Color.white;
                _dragIcon.enabled = true;
                return;
            }

            int prevIndex = _selectedSlotIndex.Value;

            _inventory.SwapItems(prevIndex, index);
            _selectedSlotIndex = null;

            _dragIcon.enabled = false;

            _slotImages[prevIndex].enabled = true;
            _slotImages[index].enabled = true;

            Refresh();
            _hotbarManager.HotbarView.Refresh();
        }

        private void On_ItemUpdated(int index, InventoryItem item)
        {
            if (index < 0 || index >= _slotImages.Length)
                return;

            var image = _slotImages[index];

            if (item != null)
            {
                image.sprite = item.Data.Icon;
                image.color = Color.white;
            }
            else
            {
                image.sprite = null;
                image.color = Color.clear;
            }

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
