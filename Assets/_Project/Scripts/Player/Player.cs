using Farming;
using Farming.Tools;
using Inventory;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
    public class Player : MonoBehaviour
    {
        public ToolManager ToolManager { get; private set; }
        public PlayerMovement Movement { get; private set; }
        public InventoryModel InventoryModel { get; private set; }
        public InventoryHotbarManager InventoryHotbarManager { get; private set; }

        public CropBase CropPrefab;

        [Inject]
        public void Construct(InventoryModel inventoryModel, InventoryHotbarManager inventoryHotbarManager)
        {
            InventoryModel = inventoryModel;
            InventoryHotbarManager = inventoryHotbarManager;
        }

        private void Awake()
        {
            ToolManager = GetComponent<ToolManager>();
            Movement = GetComponent<PlayerMovement>();
        }
    }
}
