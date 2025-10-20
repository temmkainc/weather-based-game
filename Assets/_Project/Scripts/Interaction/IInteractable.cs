using PlayerSystem;

namespace Common
{
    public interface IInteractable
    {
        string GetInteractionName();
        void Interact(Player player);
    }
}
