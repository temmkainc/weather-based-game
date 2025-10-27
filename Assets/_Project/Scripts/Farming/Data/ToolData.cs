using Common;
using PlayerSystem;
using UnityEngine;

namespace Farming.Tools
{
    public class ToolData : ScriptableObject, ITool
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        public virtual bool CanUseOn(IInteractable interactable) => true;

        public virtual void Use(Player player, IInteractable interactable)
        {
            Debug.Log($"Used {Name}");
        }
    }
}
