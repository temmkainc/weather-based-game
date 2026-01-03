using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Inventory;
using System;
using UnityEngine.EventSystems;
using System.Collections;

public class BuyListItem : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [field: SerializeField] public Image Image { get; private set; }
    [field: SerializeField] public Button Button { get; private set; }
    [field: SerializeField] public TMP_Text Text { get; private set; }

    [Header("Scale Feedback")]
    [SerializeField] private float _hoverScale = 1.05f;
    [SerializeField] private float _pressedScale = 0.95f;
    [SerializeField] private float _scaleSpeed = 12f;

    public event Action<ItemData> Clicked;

    private ItemData _itemData;
    private Vector3 _baseScale;
    private Coroutine _scaleRoutine;

    private void Awake()
    {
        _baseScale = transform.localScale;
    }

    public void Initialize(ItemData data)
    {
        _itemData = data;

        Image.sprite = data.Icon;
        Text.text = $"{data.BuyPrice}";

        Button.onClick.AddListener(OnClicked);
    }
    public void RefreshUI(Color textColor) { Text.color = textColor; }
    public void OnPointerEnter(PointerEventData eventData)
    {
        AnimateScale(_baseScale * _hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnimateScale(_baseScale);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AnimateScale(_baseScale * _pressedScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AnimateScale(_baseScale * _hoverScale);
    }


    private void AnimateScale(Vector3 target)
    {
        if (_scaleRoutine != null)
            StopCoroutine(_scaleRoutine);

        _scaleRoutine = StartCoroutine(ScaleRoutine(target));
    }

    private IEnumerator ScaleRoutine(Vector3 target)
    {
        while (Vector3.Distance(transform.localScale, target) > 0.001f)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                target,
                Time.unscaledDeltaTime * _scaleSpeed
            );
            yield return null;
        }

        transform.localScale = target;
    }

    private void OnClicked()
    {
        Clicked?.Invoke(_itemData);
    }

    private void OnDestroy()
    {
        Button.onClick.RemoveListener(OnClicked);
    }
}
