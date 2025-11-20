using Common;
using Inventory;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameLoop
{
    public class RoundManager : MonoBehaviour
    {
        [field: SerializeField] public List<ItemData> PossibleItems { get; private set; }

        [Inject] private InventoryModel _inventory;
        [Inject] private TimerPanel _timerPanel;
        [Inject] private ObjectivesPanel _objectivesPanel;
        [Inject] private SignalBus _signalBus;

        private RoundGenerator _generator;
        private RoundObjectiveChecker _checker;
        private RoundInventoryProcessor _inventoryProcessor;
        private RoundTimer _timer;

        private RoundObjectives _current;
        private int _roundIndex = 1;
        private bool _roundEnded = false;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            signalBus.Subscribe<NextRoundRequestedSignal>(NextRound);
            signalBus.Subscribe<RestartGameSignal>(RestartGame);
        }

        void Awake()
        {
            _generator = new RoundGenerator(PossibleItems);
            _checker = new RoundObjectiveChecker(_inventory);
            _inventoryProcessor = new RoundInventoryProcessor(_inventory);
            _timer = new RoundTimer();
        }

        void Start() => NextRound();

        void Update()
        {
            if (_current == null || _roundEnded)
                return;

            _timer.Tick(Time.deltaTime);
            _timerPanel.UpdateTimer(_timer.TimeLeft);

            if (_timer.IsExpired)
            {
                OnLose();
                return;
            }

            if (_checker.AllObjectivesCompleted(_current))
            {
                OnWin();
            }
        }

        private void NextRound()
        {
            _roundEnded = false;
            _current = _generator.Generate(_roundIndex);

            _objectivesPanel.Open();
            _objectivesPanel.Initialize(_current);

            _timer.Start(_current.TimeLimit);
            _timerPanel.Open();
            _timerPanel.Initialize(_current.TimeLimit);
        }

        private void OnWin()
        {
            if (_roundEnded) return;
            _roundEnded = true;

            ClosePanels();

            _inventoryProcessor.ConsumeItems(_current);

            _signalBus.Fire(new RoundCompletedSignal { RoundIndex = _roundIndex });

            _roundIndex++;
        }

        private void OnLose()
        {
            if (_roundEnded) return;
            _roundEnded = true;

            ClosePanels();
            _signalBus.Fire(new RoundFailedSignal { RoundIndex = _roundIndex });
        }

        private void RestartGame()
        {
            _roundIndex = 0;
            _current = null;
            _inventory.ResetToStartingItems();
            NextRound();
        }

        private void ClosePanels()
        {
            _objectivesPanel.Close();
            _timerPanel.Close();
        }
    }
}
