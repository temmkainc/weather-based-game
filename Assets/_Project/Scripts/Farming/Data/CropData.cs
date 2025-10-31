using Inventory;
using System;
using UnityEngine;

namespace Farming
{
    [CreateAssetMenu(menuName = "Farming/Crop Data")]
    public class CropData : ScriptableObject
    {
        [field: SerializeField] public string CropName { get; private set; }
        [field: SerializeField] public GrowthStage[] GrowthStages { get; private set; }
        [field: SerializeField] public float WaterNeededToGrow { get; private set; }
        [field: SerializeField] public ItemData[] HarvestItemData { get; private set; }
    }

    [Serializable]
    public struct GrowthStage
    {
        public Sprite Sprite;
        public float TimeToNextStage;
    }
}
