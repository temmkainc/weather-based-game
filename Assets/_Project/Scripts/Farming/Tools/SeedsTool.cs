using Common;
using PlayerSystem;
using UnityEngine;

namespace Farming.Tools
{
    [CreateAssetMenu(fileName = "Seeds", menuName = "Farming/Tool/Seeds")]
    public class SeedsTool : ToolData
    {
        public override bool CanUseOn(IInteractable interactable)
        {
            return interactable is PotBase pot && pot.CurrentState == PotState.Dug;
        }

        public override void Use(Player player, IInteractable interactable)
        {
            if (interactable is not PotBase pot)
                return;

            var inventoryModel = player.InventoryModel;
            var seedsItem = inventoryModel.SelectedItem;
            Debug.Log($"Planting seeds: ");

            if (seedsItem.Data.CropSeeds == null)
                return;

            inventoryModel.RemoveItemAtSlot(player.InventoryHotbarManager.SelectedSlotIndex, 1);
            pot.PlantSeeds(seedsItem.Data.CropSeeds);
        }
    }
}
