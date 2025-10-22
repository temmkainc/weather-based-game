using Common;
using PlayerSystem;

namespace Farming.Tools
{
    public class WateringCanTool : ITool
    {
        public string Name => "Watering Can";

        public bool CanUseOn(IInteractable interactable)
        {
            return interactable is PotBase pot && pot.CurrentState == PotState.Planted;
        }

        public void Use(Player player, IInteractable interactable)
        {
            if (interactable is PotBase pot)
                pot.WaterCrop();
        }
    }
}