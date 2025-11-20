using UnityEngine;
using Zenject;

public class GameUIController : MonoBehaviour
{

    [SerializeField] private RoundWinPanel _winPanel;
    [SerializeField] private RoundLosePanel _losePanel;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        signalBus.Subscribe<RoundCompletedSignal>(OnRoundCompleted);
        signalBus.Subscribe<RoundFailedSignal>(OnRoundFailed);
    }

    private void OnRoundCompleted(RoundCompletedSignal signal)
    {
        _winPanel.Open();
    }

    private void OnRoundFailed(RoundFailedSignal signal)
    {
        _losePanel.Close();
    }
}
