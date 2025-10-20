using Common;
using TMPro;
using UnityEngine;

namespace PlayerSystem
{
    [RequireComponent(typeof(Player))]
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tooltipTMP;

        private IInteractable _currentInteractable;
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IStatefulInteractable>(out var stateful))
            {
                _currentInteractable = stateful;
                ShowTooltip(_currentInteractable.GetInteractionName());
                stateful.StateChangedEvent += On_StateChanged;
                return;
            }

            if (other.TryGetComponent<IInteractable>(out var interactable))
            {
                _currentInteractable = interactable;
                ShowTooltip(_currentInteractable.GetInteractionName());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IStatefulInteractable>(out var stateful) && _currentInteractable == stateful)
            {
                stateful.StateChangedEvent -= On_StateChanged;
                _currentInteractable = null;
                HideTooltip();
                return;
            }

            if (other.TryGetComponent<IInteractable>(out var interactable) && _currentInteractable == interactable)
            {
                _currentInteractable = null;
                HideTooltip();
            }
        }

        private void On_StateChanged()
        {
            if (_currentInteractable == null)
                return;
            ShowTooltip(_currentInteractable.GetInteractionName());
        }

        private void Update()
        {
            if (_currentInteractable != null && Input.GetKeyDown(KeyCode.F))
            {
                _currentInteractable.Interact(_player);
                ShowTooltip(_currentInteractable.GetInteractionName());
            }
        }

        private void ShowTooltip(string text)
        {
            _tooltipTMP.text = $"Press [F] to {text}";
        }

        private void HideTooltip()
        {
            _tooltipTMP.text = string.Empty;
        }
    }
}
