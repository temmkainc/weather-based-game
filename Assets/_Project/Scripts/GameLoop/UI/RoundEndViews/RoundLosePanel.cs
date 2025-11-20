using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RoundLosePanel : PanelBase
{
    [field: SerializeField] public Button RestartGameButton { get; private set; }
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public override void Open()
    {
        base.Open();
        RestartGameButton.onClick.AddListener(On_RestartGameButtonClick);
    }

    public override void Close()
    {
        base.Close();
        RestartGameButton.onClick.RemoveListener(On_RestartGameButtonClick);
    }

    public void On_RestartGameButtonClick()
    {
        _signalBus.Fire<RestartGameSignal>();
        Close();
    }
}
