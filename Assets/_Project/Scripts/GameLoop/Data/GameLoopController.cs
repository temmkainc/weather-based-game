using UnityEngine;
using System.Collections.Generic;
using Inventory;
using System;
using Zenject;
using Common;

namespace GameLoop
{
    public class GameLoopController : MonoBehaviour
    {
        [field: SerializeField] public List<ItemData> PossibleItems {  get; private set; }

        [Inject] private InventoryModel _inventoryModel;

        [Inject] private TimerView _timerView;
        [Inject] private ObjectivesView _objectivesView;
        [Inject] private SignalBus _signalBus;

        private RoundObjectives _currentRound;
        private float _roundTimer;
        private int _roundIndex = 1;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            signalBus.Subscribe<NextRoundRequestedSignal>(OnNextRoundRequested);
        }

        void Start()
        {
            StartNextRound();
        }

        void Update()
        {
            if (_currentRound == null) return;

            _roundTimer -= Time.deltaTime;
            _timerView?.UpdateTimer(_roundTimer);

            if (_roundTimer <= 0)
            {
                HandleLoss();
                return;
            }

            if (ObjectivesCompleted())
            {
                HandleWin();
            }
        }

        private void StartNextRound()
        {
            _currentRound = GenerateObjectives(_roundIndex);
            _objectivesView.Initialize(_currentRound);

            _roundTimer = _currentRound.TimeLimit;
            _timerView?.Initialize(_roundTimer);
        }

        private RoundObjectives GenerateObjectives(int round)
        {
            int objectiveCount = Mathf.Clamp(1 + round / 2, 1, 5);

            var roundObj = new RoundObjectives
            {
                TimeLimit = 30 + round * 5
            };

            List<ItemData> available = new List<ItemData>(PossibleItems);

            for (int i = 0; i < objectiveCount; i++)
            {
                if (available.Count == 0)
                    break;

                int index = DeterministicRandom.Next(0, available.Count);
                var item = available[index];
                available.RemoveAt(index);

                int amount = DeterministicRandom.Next(1, 2 + round);

                roundObj.Goals.Add(new Objective
                {
                    Item = item,
                    AmountRequired = amount
                });
            }

            return roundObj;
        }

        private bool ObjectivesCompleted()
        {
            foreach (var goal in _currentRound.Goals)
            {
                int totalCollected = 0;
                foreach (var slot in _inventoryModel.Items)
                {
                    if (slot != null && slot.Data == goal.Item)
                    {
                        totalCollected += Math.Min(slot.Quantity, goal.AmountRequired - totalCollected);
                    }
                }

                if (totalCollected < goal.AmountRequired)
                    return false;
            }

            return true;
        }

        private void HandleWin()
        {
            foreach (var goal in _currentRound.Goals)
            {
                int remaining = goal.AmountRequired;
                for (int i = 0; i < _inventoryModel.Items.Count; i++)
                {
                    var slot = _inventoryModel.Items[i];
                    if (slot == null) continue;
                    if (slot.Data != goal.Item) continue;

                    int removeAmount = Math.Min(remaining, slot.Quantity);
                    _inventoryModel.RemoveItemAtSlot(i, removeAmount);
                    remaining -= removeAmount;
                    if (remaining <= 0) break;
                }
            }

            _signalBus.Fire(new RoundCompletedSignal { RoundIndex = _roundIndex });
            _roundIndex++;
        }

        private void HandleLoss()
        {
            _signalBus.Fire(new RoundFailedSignal { RoundIndex = _roundIndex });
        }
        private void OnNextRoundRequested()
        {
            StartNextRound();
        }
    }
}
