using Common;
using PlayerSystem;

namespace Farming.Tools
{
    public interface ITool
    {
        bool CanUseOn(IInteractable interactable);
        void Use(Player player, IInteractable interactable);
    }
}
