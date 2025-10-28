using System.Collections.Generic;
using UnityEngine;

namespace Farming.Tools
{
    public class ToolManager : MonoBehaviour
    {
        [SerializeField] private List<ToolData> _toolsData;
        private int _currentIndex;

        public ToolData CurrentTool => _toolsData.Count > 0 && _currentIndex != -1 ? _toolsData[_currentIndex] : null;

        private void Awake()
        {
            _currentIndex = 0;
        }

        private void SetTool(int index)
        {
            if (index < -1 || index >= _toolsData.Count) return;
            if (_currentIndex == index) return;

            _currentIndex = index;
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
    }
}
