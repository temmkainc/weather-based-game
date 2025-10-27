using Common;
using PlayerSystem;
using UnityEngine;

namespace Farming.Tools
{
    [CreateAssetMenu(fileName = "WateringCan", menuName = "Farming/Tool/Watering Can")]
    public class WateringCanTool : ToolData
    {
        public override bool CanUseOn(IInteractable interactable)
        {
            return interactable is PotBase pot && pot.CurrentState == PotState.Planted;
        }

        public override void Use(Player player, IInteractable interactable)
        {
            if (interactable is not PotBase pot)
                return;

            pot.WaterCrop();
        }
    }
}
