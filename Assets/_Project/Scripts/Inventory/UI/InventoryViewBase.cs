using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public abstract class InventoryViewBase : MonoBehaviour, IInventoryView
    {
        [Header("Common References")]
        [SerializeField] protected GameObject _slotPrefab;

        protected Image[] _slotImages;
        protected TMP_Text[] _slotCountLabels;

        public abstract void Show();
        public abstract void Hide();
        public abstract void Refresh();
        public abstract void On_SlotSelected(int index);

        protected void BuildSlots(Transform parent, int slotCount, System.Action<int> onClick, int indexesToShow)
        {
            foreach (Transform child in parent)
                Destroy(child.gameObject);

            _slotImages = new Image[slotCount];
            _slotCountLabels = new TMP_Text[slotCount];

            for (int i = 0; i < slotCount; i++)
            {
                var slot = Instantiate(_slotPrefab, parent);

                _slotImages[i] = slot.GetComponentsInChildren<Image>(includeInactive: true)
                    .FirstOrDefault(img => img.gameObject != slot);

                var texts = slot.GetComponentsInChildren<TMP_Text>(includeInactive: true);
                var indexTMP = texts.FirstOrDefault(tmp => tmp.gameObject.name.Contains("Index"));
                var countTMP = texts.FirstOrDefault(tmp => tmp.gameObject.name.Contains("Count"));

                if (countTMP != null)
                    _slotCountLabels[i] = countTMP;

                if (i < indexesToShow && indexTMP != null)
                {
                    indexTMP.gameObject.SetActive(true);
                    indexTMP.text = (i + 1).ToString();
                }

                var button = slot.GetComponent<Button>();
                if (button != null)
                {
                    int index = i;
                    button.onClick.AddListener(() => onClick(index));
                }
            }
        }

        protected void SetSlotVisual(int index, InventoryItem item)
        {
            var image = _slotImages[index];
            var countLabel = _slotCountLabels[index];

            if (item != null)
            {
                image.sprite = item.Data.Icon;
                image.color = Color.white;

                if (item.Quantity > 1)
                {
                    countLabel.text = item.Quantity.ToString();
                    countLabel.gameObject.SetActive(true);
                }
                else
                {
                    countLabel.gameObject.SetActive(false);
                }
            }
            else
            {
                image.sprite = null;
                image.color = Color.clear;
                countLabel.gameObject.SetActive(false);
            }
        }
    }
}
