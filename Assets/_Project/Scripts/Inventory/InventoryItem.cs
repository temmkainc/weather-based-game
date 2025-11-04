using Farming.Tools;
using System;

namespace Inventory
{
    [Serializable]
    public class InventoryItem
    {
        public ItemData Data;
        public int Quantity;
        public ToolData Tool => Data != null && Data.Type == ItemType.Tool ? Data.ToolReference : null;
    }
}