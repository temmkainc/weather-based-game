using Farming;
using Farming.Tools;
using Inventory;
using System;
using TMPro;
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
        public int Money
        {
            get => _money;
            set
            {
                if (_money == value)
                    return;

                _money = value;
                RefreshMoneyUI();
            }
        }

        [SerializeField] private int _money;
        [SerializeField] private TMP_Text _moneyText;

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
            RefreshMoneyUI();
        }

        private void RefreshMoneyUI()
        {
            _moneyText.text = $"{Money}";
        }
    }
}
