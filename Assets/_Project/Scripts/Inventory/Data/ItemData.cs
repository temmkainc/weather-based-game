using Farming.Tools;
using UnityEngine;

namespace Inventory
{
    public enum ItemType
    {
        Seed,
        Crop,
        Tool
    }

    [CreateAssetMenu(fileName = "Item", menuName = "Farming/ItemData")]
    public class ItemData : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public ItemType Type { get; private set; }
        [field: SerializeField] public bool IsStackable { get; private set; }

        [field: SerializeField] public ToolData ToolReference { get; private set; }
    }
}
