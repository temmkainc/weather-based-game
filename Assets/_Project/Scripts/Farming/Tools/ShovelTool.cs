using Common;
using PlayerSystem;
using UnityEngine;

namespace Farming.Tools
{
    [CreateAssetMenu(fileName = "Shovel", menuName = "Farming/Tool/Shovel")]
    public class ShovelTool : ToolData
    {
        public override bool CanUseOn(IInteractable interactable)
        {
            return interactable is PotBase pot && pot.CurrentState == PotState.Empty;
        }

        public override void Use(Player player, IInteractable interactable)
        {
            if (interactable is not PotBase pot)
                return;
            pot.Dig();
        }
    }
}
