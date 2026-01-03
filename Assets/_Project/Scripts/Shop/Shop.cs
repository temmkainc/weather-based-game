using Common;
using PlayerSystem;
using System;
using TMPro;
using UnityEngine;
using Zenject;

public class Shop : MonoBehaviour, IInteractable
{
    [Inject] private ShopPanel _panel;

    public string GetInteractionName()
    {
        return "Enter the Shop";
    }

    public Type GetRequiredToolType()
    {
        return null;
    }

    public void Interact(Player player)
    {
        _panel.On_Interacted();
    }
}
