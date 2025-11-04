using Common;
using Farming.Tools;
using PlayerSystem;
using System;
using UnityEngine;

namespace Farming
{
    public class WaterSourceBase : MonoBehaviour, IInteractable
    {
        public string GetInteractionName()
        {
            return "Refill";
        }

        public Type GetRequiredToolType()
        {
            return typeof(WateringCanTool);
        }

        public void Interact(Player player)
        {
            // Any Interaction without tool
        }
    }
}
