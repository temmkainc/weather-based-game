using Common;
using Farming.Tools;
using PlayerSystem;
using System;
using UnityEngine;
using Zenject;

namespace Farming
{
    public enum PotState
    {
        Empty,
        Dug,
        Planted,
        ReadyToHarvest,
        Dead
    }

    public class PotBase : MonoBehaviour, IStatefulInteractable
    {
        public event Action StateChangedEvent;

        [SerializeField] private SpriteRenderer _renderer;

        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private Sprite _dugSprite;

        [SerializeField] private CropBase _currentCrop;
        [SerializeField] private Transform _cropSlot;
        [Inject] private DiContainer _diContainer;

        private PotState _currentState = PotState.Empty;

        public PotState CurrentState => _currentState;

        private void Start()
        {
            _renderer.sprite = _emptySprite;
        }

        public string GetInteractionName()
        {
            return _currentState switch
            {
                PotState.Empty => "Dig",
                PotState.Dug => "Plant Seeds",
                PotState.Planted => "Water",
                PotState.ReadyToHarvest => "Harvest",
                PotState.Dead => "Clear",
                _ => string.Empty,
            };
        }

        public Type GetRequiredToolType()
        {
            return _currentState switch
            {
                PotState.Empty => typeof(ShovelTool),
                PotState.Dug => null,
                PotState.Planted => typeof(WateringCanTool),
                PotState.ReadyToHarvest => null,
                PotState.Dead => typeof(ShovelTool),
                _ => null,
            };
        }

        public void Interact(Player player)
        {
            switch (_currentState)
            {
                case PotState.Empty:
                    Dig();
                    break;
                case PotState.Dug:
                    PlantSeeds(player.CropPrefab);
                    break;
                case PotState.Planted:
                    WaterCrop();
                    break;
                case PotState.ReadyToHarvest:
                    Harvest();
                    break;
                case PotState.Dead:
                    ClearPlant();
                    break;
            }
        }

        public void Dig()
        {
            Debug.Log("Digged the pot");
            _renderer.sprite = _dugSprite;
            SetState(PotState.Dug);
        }

        public void WaterCrop()
        {
            Debug.Log("Watered the pot");
            _currentCrop.Water();
        }

        private void PlantSeeds(CropBase crop)
        {
            Debug.Log("Planted seeds");
            GameObject cropGO = _diContainer.InstantiatePrefab(crop.gameObject, _cropSlot);
            _currentCrop = cropGO.GetComponent<CropBase>();
            _currentCrop.Plant();

            _currentCrop.GrowUpEvent += On_CropGrowUp;
            _currentCrop.DieEvent += On_CropDied;

            SetState(PotState.Planted);
        }

        private void Harvest()
        {
            Debug.Log("Harvested the plant");
            ClearPlant();
        }

        private void ClearPlant()
        {
            Destroy(_currentCrop.gameObject);
            _currentCrop = null;
            _renderer.sprite = _emptySprite;
            SetState(PotState.Empty);
        }

        private void On_CropGrowUp()
        {
            SetState(PotState.ReadyToHarvest);
        }

        private void On_CropDied()
        {
            SetState(PotState.Dead);
        }

        private void SetState(PotState newState)
        {
            _currentState = newState;
            StateChangedEvent?.Invoke();
        }
    }
}
