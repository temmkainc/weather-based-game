using System.Collections.Generic;
using UnityEngine;

namespace Farming.Tools
{
    public class ToolManager : MonoBehaviour
    {
        [SerializeField] private ToolManagerSceneContainer _sceneContainer;
        [SerializeField] private List<ToolData> _tools;
        private int _currentIndex;

        public ToolData CurrentTool => _tools.Count > 0 ? _tools[_currentIndex] : null;

        private void Awake()
        {
            _currentIndex = 0;
            UpdateToolUI();
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
            int nextIndex = (_currentIndex + 1) % _tools.Count;
            SetTool(nextIndex);
        }

        private void PreviousTool()
        {
            if (_tools.Count == 0) return;
            int prevIndex = (_currentIndex - 1 + _tools.Count) % _tools.Count;
            SetTool(prevIndex);
        }

        public void SetTool(int index)
        {
            if (index < 0 || index >= _tools.Count) return;
            if (_currentIndex == index) return;

            _currentIndex = index;
            UpdateToolUI();
        }

        private void UpdateToolUI()
        {
            if (_sceneContainer == null || CurrentTool is not ToolData toolData)
                return;

            _sceneContainer.CurrentToolImage.sprite = toolData.Icon;

            Debug.Log($"Selected Tool: {CurrentTool.Name}");
        }
    }
}
