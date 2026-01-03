using Inventory;
using NUnit.Framework;
using PlayerSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopPanel : Panel
{
    [SerializeField] private BuyListItem _buyListItemPrefab;
    [SerializeField] private BoughtListItem _boughtListItemPrefab;

    [SerializeField] private List<ItemData> _shopItems;
    [SerializeField] private BuyListItem[] _buyListItems;

    [SerializeField] private Transform _shopItemsLayoutGroup;
    [SerializeField] private Transform _boughtItemsLayoutGroup;

    [SerializeField] private Player _player;
    [SerializeField] private Color _canAffordColor;
    [SerializeField] private Color _cannotAffordColor;

    [SerializeField] private Button _closeButton;
    [SerializeField] private TMP_Text _playerMoneyTMP;

    [Inject] private InventoryModel _inventoryModel;
    private readonly List<ItemData> _stagedItems = new();
    private int _stagedMoneyDelta;

    private bool _isOpened = false;

    private void Start()
    {
        _buyListItems = new BuyListItem[_shopItems.Count];

        for (int i = 0; i < _shopItems.Count; i++)
        {
            var buyListItem = Instantiate(_buyListItemPrefab, _shopItemsLayoutGroup);
            buyListItem.Initialize(_shopItems[i]);
            buyListItem.Clicked += OnBuyItemClicked;

            _buyListItems[i] = buyListItem;
        }
    }

    public override void Open()
    {
        base.Open();
        _closeButton.onClick.AddListener(On_Interacted);
        UpdateMoneyUI();
        Time.timeScale = 0f;
    }

    public override void Close()
    {
        base.Close();
        _closeButton.onClick.RemoveListener(On_Interacted);
        Time.timeScale = 1f;

        CommitTransaction();
    }
    private void CommitTransaction()
    {
        if (_stagedItems.Count == 0)
            return;

        _player.Money += _stagedMoneyDelta;

        foreach (var item in _stagedItems)
        {
            _inventoryModel.AddItemToFirstFreeSlot(item);
        }

        foreach (Transform child in _boughtItemsLayoutGroup)
        {
            Destroy(child.gameObject);
        }

        _stagedItems.Clear();
        _stagedMoneyDelta = 0;
    }

    public void On_Interacted()
    {
        _isOpened = !_isOpened;

        if (_isOpened)
        {
            Open();
            return;
        }
        Close();
    }

    private void OnBuyItemClicked(ItemData itemData)
    {
        if (_player.Money + _stagedMoneyDelta < itemData.BuyPrice)
            return;

        _stagedItems.Add(itemData);
        _stagedMoneyDelta -= itemData.BuyPrice;

        SpawnBoughtItemUI(itemData);
        UpdateMoneyUI();
    }
    private void OnBoughtItemClicked(ItemData itemData, BoughtListItem uiItem)
    {
        _stagedItems.Remove(itemData);
        _stagedMoneyDelta += itemData.BuyPrice;

        Destroy(uiItem.gameObject);
        UpdateMoneyUI();
    }
    private void SpawnBoughtItemUI(ItemData itemData)
    {
        var boughtItem = Instantiate(_boughtListItemPrefab, _boughtItemsLayoutGroup);
        boughtItem.Initialize(itemData);

        boughtItem.Clicked += (ItemData) => OnBoughtItemClicked(itemData, boughtItem);
    }

    private void UpdateMoneyUI()
    {
        int displayedMoney = _player.Money + _stagedMoneyDelta;
        _playerMoneyTMP.text = displayedMoney.ToString();

        for (int i = 0; i < _buyListItems.Length; i++)
        {
            bool canAfford = displayedMoney >= _shopItems[i].BuyPrice;
            _buyListItems[i].RefreshUI(canAfford ? _canAffordColor : _cannotAffordColor);
        }
    }

}
