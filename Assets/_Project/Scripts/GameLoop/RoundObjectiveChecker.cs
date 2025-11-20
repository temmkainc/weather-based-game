using Inventory;

namespace GameLoop
{
    public class RoundObjectiveChecker
    {
        private readonly InventoryModel _inventory;

        public RoundObjectiveChecker(InventoryModel inventory)
        {
            _inventory = inventory;
        }

        public bool AllObjectivesCompleted(RoundObjectives round)
        {
            foreach (var goal in round.Goals)
            {
                int collected = 0;

                foreach (var slot in _inventory.Items)
                {
                    if (slot != null && slot.Data == goal.Item)
                    {
                        collected += slot.Quantity;
                        if (collected >= goal.AmountRequired)
                            break;
                    }
                }

                if (collected < goal.AmountRequired)
                    return false;
            }

            return true;
        }
    }
}
