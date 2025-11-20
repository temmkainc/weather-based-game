using Inventory;
using System;

namespace GameLoop
{
    public class RoundInventoryProcessor
    {
        private readonly InventoryModel _inventory;

        public RoundInventoryProcessor(InventoryModel inventory)
        {
            _inventory = inventory;
        }

        public void ConsumeItems(RoundObjectives round)
        {
            foreach (var goal in round.Goals)
            {
                int remaining = goal.AmountRequired;

                for (int i = 0; i < _inventory.Items.Count; i++)
                {
                    var slot = _inventory.Items[i];
                    if (slot == null || slot.Data != goal.Item)
                        continue;

                    int removeAmount = Math.Min(remaining, slot.Quantity);

                    _inventory.RemoveItemAtSlot(i, removeAmount);
                    remaining -= removeAmount;

                    if (remaining <= 0)
                        break;
                }
            }
        }
    }
}
