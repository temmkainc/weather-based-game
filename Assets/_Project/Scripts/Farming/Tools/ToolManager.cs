using System.Collections.Generic;
using UnityEngine;

namespace Farming.Tools
{
    public class ToolManager : MonoBehaviour
    {
        [SerializeField] private ToolManagerSceneContainer _sceneContainer;
        [SerializeField] private List<ToolData> _toolsData;
        private int _currentIndex;

        public ToolData CurrentTool => _toolsData.Count > 0 ? _toolsData[_currentIndex] : null;

        private void Awake()
        {
            _currentIndex = 0;
            UpdateToolUI();
        }

        private void SetTool(int index)
        {
            if (index < 0 || index >= _toolsData.Count) return;
            if (_currentIndex == index) return;

            _currentIndex = index;
            UpdateToolUI();
        }

        public void SetTool(ToolData tool)
        {
            if (tool == null) return;
            int index = _toolsData.IndexOf(tool);
            if (index == -1) return;

            SetTool(index);
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
