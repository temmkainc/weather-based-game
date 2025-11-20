using UnityEngine;
using UnityEngine.UI;

namespace GameLoop
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;

        private float _maxTime;

        public void Initialize(float maxTime)
        {
            _maxTime = maxTime;
            UpdateFill(_maxTime);
        }

        public void UpdateTimer(float currentTime)
        {
            UpdateFill(currentTime);
        }

        private void UpdateFill(float currentTime)
        {
            if (_fillImage == null) return;

            float fill = Mathf.Clamp01(currentTime / _maxTime);
            _fillImage.fillAmount = fill;
        }
    }
}
