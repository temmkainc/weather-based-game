using PlayerSystem;
using System;

namespace Common
{
    public interface IInteractable
    {
        Type GetRequiredToolType();
        string GetInteractionName();
        void Interact(Player player);
    }
}
