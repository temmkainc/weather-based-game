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
        private IStatefulInteractable _currentStateful;
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IStatefulInteractable stateful))
            {
                SetCurrentInteractable(stateful);
                stateful.StateChangedEvent += On_StateChanged;
                return;
            }

            if (other.TryGetComponent(out IInteractable interactable))
            {
                SetCurrentInteractable(interactable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_currentStateful != null && other.TryGetComponent(out IStatefulInteractable stateful) && stateful == _currentStateful)
            {
                stateful.StateChangedEvent -= On_StateChanged;
                ClearInteractable();
                return;
            }

            if (other.TryGetComponent(out IInteractable interactable) && interactable == _currentInteractable)
            {
                ClearInteractable();
            }
        }

        private void Update()
        {
            if (_currentInteractable == null) return;

            var requiredType = _currentInteractable.GetRequiredToolType();
            var currentTool = _player.ToolManager.CurrentTool;

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (requiredType != null && (currentTool == null || currentTool.GetType() != requiredType))
                {
                    Debug.Log("You can`t do that with that tool");
                    return;
                }

                _currentInteractable.Interact(_player);
            }
        }

        private void SetCurrentInteractable(IInteractable interactable)
        {
            _currentInteractable = interactable;
            _currentStateful = interactable as IStatefulInteractable;
            RefreshTooltip();
        }

        private void ClearInteractable()
        {
            _currentInteractable = null;
            _currentStateful = null;
            HideTooltip();
        }

        private void RefreshTooltip()
        {
            if (_currentInteractable == null) return;

            string text = _currentInteractable.GetInteractionName();
            if (string.IsNullOrEmpty(text))
            {
                HideTooltip();
                return;
            }

            ShowTooltip(text);
        }

        private void On_StateChanged()
        {
            RefreshTooltip();
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
