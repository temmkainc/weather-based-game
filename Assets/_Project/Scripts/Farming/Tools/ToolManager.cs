using System.Collections.Generic;
using UnityEngine;

namespace Farming.Tools
{
    public class ToolManager : MonoBehaviour
    {
        [SerializeField] private ToolManagerSceneContainer _sceneContainer;
        [SerializeField] private List<ToolData> _toolsData;
        private int _currentIndex;

        public ToolData CurrentTool => _toolsData.Count > 0 && _currentIndex != -1 ? _toolsData[_currentIndex] : null;

        private void Awake()
        {
            _currentIndex = 0;
            UpdateToolUI();
        }

        private void SetTool(int index)
        {
            if (index < -1 || index >= _toolsData.Count) return;
            if (_currentIndex == index) return;

            _currentIndex = index;
            UpdateToolUI();
        }

        public void SetTool(ToolData tool)
        {
            if (tool == null)
            {
                SetTool(-1);
                return;
            }
            SetTool(_toolsData.IndexOf(tool));
        }

        private void UpdateToolUI()
        {
            if (_currentIndex == -1)
            {
                _sceneContainer.CurrentToolImage.sprite = null;
                _sceneContainer.CurrentToolImage.color = Color.clear;
                Debug.Log($"Selected Tool: None");
                return;
            }

            if (_sceneContainer == null || CurrentTool is not ToolData toolData)
                return;

            _sceneContainer.CurrentToolImage.sprite = toolData.Icon;
            _sceneContainer.CurrentToolImage.color = Color.white;

            Debug.Log($"Selected Tool: {CurrentTool.Name}");
        }
    }
}
