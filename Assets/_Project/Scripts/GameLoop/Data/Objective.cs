using Inventory;
using System;

namespace GameLoop
{
    [Serializable]
    public class Objective
    {
        public ItemData Item { get; set; }
        public int AmountRequired { get; set; }
    }
}
