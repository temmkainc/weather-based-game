using Farming.Tools;
using System;
using System.Collections.Generic;
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
        protected Image[] _itemValueBarImages;
        protected TMP_Text[] _slotCountLabels;
        private readonly Dictionary<int, (ToolData tool, Action<float> handler)> _valueChangedHandlers = new();
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
            _itemValueBarImages = new Image[slotCount];

            for (int i = 0; i < slotCount; i++)
            {
                var slot = Instantiate(_slotPrefab, parent);

                _slotImages[i] = slot.GetComponentsInChildren<Image>(includeInactive: true)
                    .FirstOrDefault(img => img.gameObject.name.Contains("ItemIcon"));

                var itemValueBar = slot.GetComponentsInChildren<Image>(includeInactive: true)
                    .FirstOrDefault(img => img.gameObject.name.Contains("ValueBar"));
                var texts = slot.GetComponentsInChildren<TMP_Text>(includeInactive: true);
                var indexTMP = texts.FirstOrDefault(tmp => tmp.gameObject.name.Contains("Index"));
                var countTMP = texts.FirstOrDefault(tmp => tmp.gameObject.name.Contains("Count"));

                if (countTMP != null)
                    _slotCountLabels[i] = countTMP;

                if (itemValueBar != null)
                {
                    _itemValueBarImages[i] = itemValueBar;
                }

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
            if (_valueChangedHandlers.TryGetValue(index, out var existing))
            {
                if (existing.tool != null)
                    existing.tool.OnValueChanged -= existing.handler;

                _valueChangedHandlers.Remove(index);
            }

            var image = _slotImages[index];
            var countLabel = _slotCountLabels[index];
            var valueBarImage = _itemValueBarImages[index];

            if (item == null)
            {
                image.sprite = null;
                image.color = Color.clear;
                countLabel.gameObject.SetActive(false);
                valueBarImage.gameObject.SetActive(false);
                return;
            }

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

            valueBarImage.gameObject.SetActive(false);

            if (item.Tool != null && valueBarImage != null)
            {
                valueBarImage.gameObject.SetActive(true);
                valueBarImage.fillAmount = item.Tool.GetValuePercentage();

                Action<float> handler = percentage => On_ItemValueChanged(index, percentage);
                _valueChangedHandlers[index] = (item.Tool, handler);

                item.Tool.OnValueChanged += handler;
            }
        }

        private void On_ItemValueChanged(int slotIndex, float percentage)
        {
            if (slotIndex < 0 || slotIndex >= _itemValueBarImages.Length) return;

            var valueBarImage = _itemValueBarImages[slotIndex];
            if (valueBarImage != null)
                valueBarImage.fillAmount = percentage;
        }
    }
}
