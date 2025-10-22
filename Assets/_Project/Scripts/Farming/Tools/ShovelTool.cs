using Common;
using PlayerSystem;

namespace Farming.Tools
{
    public class ShovelTool : ITool
    {
        public string Name => "Shovel";

        public bool CanUseOn(IInteractable interactable)
        {
            return interactable is PotBase pot && pot.CurrentState == PotState.Empty;
        }

        public void Use(Player player, IInteractable interactable)
        {
            if (interactable is not PotBase pot)
                return;

            pot.Dig();
        }
    }
}
