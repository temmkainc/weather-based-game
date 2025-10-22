using System.Collections.Generic;
using UnityEngine;

namespace Farming.Tools
{
    public class ToolManager : MonoBehaviour
    {
        [SerializeField] private List<ITool> _tools = new();
        private int _currentIndex;

        public ITool CurrentTool => _tools.Count > 0 ? _tools[_currentIndex] : null;

        private void Awake()
        {
            if (_tools.Count == 0)
            {
                _tools.Add(new ShovelTool());
                _tools.Add(new WateringCanTool());
            }

            _currentIndex = 0;
        }

        private void Update()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0)
                NextTool();
            else if (scroll < 0)
                PreviousTool();
        }

        private void NextTool()
        {
            if (_tools.Count == 0) return;

            _currentIndex = (_currentIndex + 1) % _tools.Count;
            Debug.Log($"Selected Tool: {CurrentTool.Name}");
        }

        private void PreviousTool()
        {
            if (_tools.Count == 0) return;

            _currentIndex = (_currentIndex - 1 + _tools.Count) % _tools.Count;
            Debug.Log($"Selected Tool: {CurrentTool.Name}");
        }

        public void SetTool(int index)
        {
            if (index < 0 || index >= _tools.Count) return;
            if (_currentIndex == index) return;

            _currentIndex = index;
        }
    }
}
