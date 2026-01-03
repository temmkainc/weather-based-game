using Common;
using Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop
{
    public class RoundGenerator
    {
        private const float ROUND_TIME_BASE = 50;
        private const float ROUND_TIME_INCREMENT_VALUE = 5;

        private readonly List<ItemData> _possibleItems;

        public RoundGenerator(List<ItemData> possibleItems)
        {
            _possibleItems = possibleItems;
        }

        public RoundObjectives Generate(int round)
        {
            int objectiveCount = Mathf.Clamp(1 + round / 2, 1, 5);

            var result = new RoundObjectives
            {
                TimeLimit = ROUND_TIME_BASE + round * ROUND_TIME_INCREMENT_VALUE
            };

            List<ItemData> available = new List<ItemData>(_possibleItems);

            for (int i = 0; i < objectiveCount; i++)
            {
                if (available.Count == 0)
                    break;

                int index = DeterministicRandom.Next(0, available.Count);
                ItemData item = available[index];
                available.RemoveAt(index);

                int amount = DeterministicRandom.Next(1, 2 + round);

                result.Goals.Add(new Objective
                {
                    Item = item,
                    AmountRequired = amount
                });
            }

            return result;
        }
    }
}
