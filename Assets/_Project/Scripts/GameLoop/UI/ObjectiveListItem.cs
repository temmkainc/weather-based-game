using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveListItem : MonoBehaviour
{
    [field: SerializeField] public Image Icon { get; private set; }
    [field: SerializeField] public TMP_Text TMP { get; private set; }

    private int _required;
    private ItemData _item;

    public void Initialize(Sprite icon, int countRequired, ItemData item)
    {
        Icon.sprite = icon;
        _required = countRequired;
        _item = item;
        UpdateCount(0);
    }

    public void UpdateCount(int currentAmount)
    {
        TMP.text = $"{currentAmount} / {_required}";
    }

    public ItemData Item => _item;
    public int Required => _required;
}
