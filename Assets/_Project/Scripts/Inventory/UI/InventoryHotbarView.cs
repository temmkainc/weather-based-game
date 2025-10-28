using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Inventory.UI
{
    public class InventoryHotbarView : MonoBehaviour, IInventoryView
    {
        [Inject] private InventoryHotbarManager _hotbarManager;

        [Header("References")]
        [SerializeField] private HotbarInputHandler _inputHandler;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Transform _selectedOutline;

        private Image[] _slotImages;

        private void Start()
        {
            _hotbarManager.OnSlotSelected += On_SlotSelected;

            _selectedOutline.gameObject.SetActive(false);
            BuildSlots();
            Refresh();
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void On_SlotSelected(int index)
        {
            if (index < 0 || index >= _slotImages.Length) return;

            _selectedOutline.transform.position = _slotImages[index].transform.position;
            _selectedOutline.gameObject.SetActive(true);

            Refresh();
        }

        private void On_SlotClicked(int index)
        {
            _hotbarManager.SelectSlot(index);
        }

        public void Refresh()
        {
            var hotbar = _hotbarManager.Hotbar;

            for (int i = 0; i < _slotImages.Length; i++)
            {
                var image = _slotImages[i];
                if (i < hotbar.Length && hotbar[i] != null)
                {
                    image.sprite = hotbar[i].Data.Icon;
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
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            var hotbar = _hotbarManager.Hotbar;
            _slotImages = new Image[hotbar.Length];

            for (int i = 0; i < hotbar.Length; i++)
            {
                var slot = Instantiate(_slotPrefab, transform);
                _slotImages[i] = slot.GetComponentsInChildren<Image>(includeInactive: true)
                    .FirstOrDefault(img => img.gameObject != slot);

                int index = i;
                var button = slot.GetComponent<Button>();
                button.onClick.AddListener(() => On_SlotClicked(index));
            }
        }

        private void OnDestroy()
        {
            _hotbarManager.OnSlotSelected -= On_SlotSelected;
        }
    }
}
