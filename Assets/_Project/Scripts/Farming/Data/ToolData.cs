using Common;
using PlayerSystem;
using System;
using UnityEngine;

namespace Farming.Tools
{
    public class ToolData : ScriptableObject, ITool
    {
        public bool HasValue;
        public event Action<float> OnValueChanged;
        protected void NotifyValueChanged(float percentage) => OnValueChanged?.Invoke(percentage);
        public virtual float GetValuePercentage() { return 0; }
        public virtual bool CanUseOn(IInteractable interactable) => true;
        public virtual void Use(Player player, IInteractable interactable) { }

    }
}
