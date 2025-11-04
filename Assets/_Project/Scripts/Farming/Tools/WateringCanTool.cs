using Common;
using PlayerSystem;
using UnityEngine;

namespace Farming.Tools
{
    [CreateAssetMenu(fileName = "WateringCan", menuName = "Farming/Tool/Watering Can")]
    public class WateringCanTool : ToolData
    {
        [field: SerializeField] public float Capacity { get; private set; }
        [field: SerializeField] public float CurrentWaterAmount { get; private set; }
        [field: SerializeField] public float UsePerPlantWaterAmount { get; private set; }

        public override float GetValuePercentage() => CurrentWaterAmount / Capacity;

        private void Notify()
        {
            NotifyValueChanged(GetValuePercentage());
        }

        private void OnEnable()
        {
            CurrentWaterAmount = Capacity;
        }

        public override bool CanUseOn(IInteractable interactable)
        {
            return interactable is PotBase pot && pot.CurrentState == PotState.Planted || interactable is WaterSourceBase;
        }

        public override void Use(Player player, IInteractable interactable)
        {
            if (interactable is WaterSourceBase)
            {
                Refill();
                return;
            }

            if (interactable is not PotBase pot)
                return;

            if (CurrentWaterAmount < UsePerPlantWaterAmount)
                return;

            CurrentWaterAmount -= UsePerPlantWaterAmount;
            Notify();

            // TODO: Give feedback that there is not enough water


            pot.WaterCrop();
        }

        public void Refill()
        {
            CurrentWaterAmount = Capacity;
            Notify();
        }
    }
}
