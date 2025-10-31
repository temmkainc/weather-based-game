using Common;
using PlayerSystem;
using UnityEngine;

namespace Farming.Tools
{
    public class ToolData : ScriptableObject, ITool
    {
        public virtual bool CanUseOn(IInteractable interactable) => true;
        public virtual void Use(Player player, IInteractable interactable) { }

    }
}
