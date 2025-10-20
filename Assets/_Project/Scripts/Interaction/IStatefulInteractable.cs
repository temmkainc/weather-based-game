using System;
namespace Common
{
    public interface IStatefulInteractable : IInteractable
    {
        event Action StateChangedEvent;
    }
}