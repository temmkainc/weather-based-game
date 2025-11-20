using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class RoundWinPanel : PanelBase
{
    [field: SerializeField] public Button NextRoundButton { get; private set; }
    
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public override void Open()
    {
        base.Open();
        NextRoundButton.onClick.AddListener(On_NextRoundButtonClick);
    }

    public override void Close()
    {
        base.Close();
        NextRoundButton.onClick.RemoveListener(On_NextRoundButtonClick);
    }

    public void On_NextRoundButtonClick()
    {
        _signalBus.Fire<NextRoundRequestedSignal>();
        Close();
    }
}
