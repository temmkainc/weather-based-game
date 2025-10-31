using Common;
using Farming.Tools;
using Inventory;
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

        public PotState CurrentState => _currentState;

        [Inject] private DiContainer _diContainer;
        [Inject] PotSceneContainer _sceneContainer;
        [Inject] InventoryModel _inventoryModel;

        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private CropBase _currentCrop;
        [SerializeField] private Transform _cropSlot;

        private Sprite _emptySprite;
        private Sprite _dugSprite;
        private ItemData[] _harvestCropData;
        private PotState _currentState = PotState.Empty;

        private void Start()
        {
            var random = DeterministicRandom.Next(0, _sceneContainer.SpriteSets.Length);

            _emptySprite = _sceneContainer.SpriteSets[random].EmptySprite;
            _dugSprite = _sceneContainer.SpriteSets[random].DugSprite;

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
                PotState.Dug => typeof(SeedsTool),
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
            _renderer.sprite = _dugSprite;
            SetState(PotState.Dug);
        }

        public void WaterCrop()
        {
            _currentCrop.Water();
        }

        public void PlantSeeds(CropData cropData)
        {
            GameObject crop = _diContainer.InstantiatePrefab(_sceneContainer.CropBasePrefab, _cropSlot);
            _currentCrop = crop.GetComponent<CropBase>();
            _harvestCropData = cropData.HarvestItemData;
            _currentCrop.Plant(cropData);

            _currentCrop.GrowUpEvent += On_CropGrowUp;
            _currentCrop.DieEvent += On_CropDied;

            SetState(PotState.Planted);
        }

        private void Harvest()
        {
            for (int i = 0; i < _harvestCropData.Length; i++)
            {
                _inventoryModel.AddItemToFirstFreeSlot(_harvestCropData[i], 1);
            }
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
