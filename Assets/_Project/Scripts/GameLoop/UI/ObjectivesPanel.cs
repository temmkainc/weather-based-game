using UnityEngine;
using Zenject;
using GameLoop;
using System.Collections.Generic;
using Inventory;

public class ObjectivesPanel : PanelBase
{
    [Inject] private ObjectiveListItem _objectiveListItemPrefab;
    [Inject] private InventoryModel _inventoryModel;

    private List<ObjectiveListItem> _listItems = new List<ObjectiveListItem>();

    public void Initialize(RoundObjectives round)
    {
        Clear();
        foreach (var goal in round.Goals)
        {
            ObjectiveListItem listItem = Instantiate(_objectiveListItemPrefab, transform);
            listItem.Initialize(goal.Item.Icon, goal.AmountRequired, goal.Item);
            _listItems.Add(listItem);
        }

        UpdateAllCounts();
        _inventoryModel.OnItemChanged += OnItemChanged;
    }

    private void OnDestroy()
    {
        _inventoryModel.OnItemChanged -= OnItemChanged;
    }

    private void OnItemChanged(int slotIndex, InventoryItem item)
    {
        UpdateAllCounts();
    }

    private void UpdateAllCounts()
    {
        foreach (var listItem in _listItems)
        {
            int total = 0;
            foreach (var slot in _inventoryModel.Items)
            {
                if (slot != null && slot.Data == listItem.Item)
                    total += slot.Quantity;
            }

            listItem.UpdateCount(total);
        }
    }

    private void Clear()
    {
        foreach (var listItem in _listItems)
        {
            if (listItem != null)
                Destroy(listItem.gameObject);
        }
        _listItems.Clear();
    }
}
