using System;
using UnityEngine;

namespace Farming
{
    public class CropBase : MonoBehaviour
    {
        public event Action GrowUpEvent;
        public event Action DieEvent;

        [Header("References")]
        [SerializeField] private CropData _data;
        [SerializeField] private CropSceneContainer _sceneContainer;
        [SerializeField] private SpriteRenderer _renderer;

        [Header("Water / Drying")]
        [SerializeField] private float _waterLevel;
        [SerializeField] private float _waterDrainRate = 0.1f;

        [Header("Growth")]
        [SerializeField] private float _elapsedGrowthTime = 0f;
        private int _currentStage = -1;
        private bool _isDead = false;
        private bool _isReady = false;
        private bool _isPlanted = false;

        public void Plant(CropData data)
        {
            _data = data;
            _waterLevel = 1f;
            _isPlanted = true;
            UpdateStage();
        }

        private void Update()
        {
            if (_isDead || _isReady || !_isPlanted) return;

            _waterLevel -= _waterDrainRate * Time.deltaTime;
            _waterLevel = Mathf.Clamp01(_waterLevel);

            if (_waterLevel <= 0f)
            {
                Die();
                return;
            }

            if (_waterLevel >= _data.WaterNeededToGrow)
            {
                _elapsedGrowthTime += Time.deltaTime;
            }

            if (_elapsedGrowthTime >= _data.GrowthStages[_currentStage].TimeToNextStage)
            {
                UpdateStage();
            }

            UpdateUI();
        }

        private void UpdateStage()
        {
            _currentStage++;

            if (_currentStage >= _data.GrowthStages.Length)
            {
                Grow();
                return;
            }

            _renderer.sprite = _data.GrowthStages[_currentStage].Sprite;
            _elapsedGrowthTime = 0f;

            if (_currentStage == _data.GrowthStages.Length - 1)
            {
                Grow();
            }
        }

        private void UpdateUI()
        {
            _sceneContainer.WaterBar.fillAmount = _waterLevel;
            _sceneContainer.GrowthBar.fillAmount = _elapsedGrowthTime / _data.GrowthStages[_currentStage].TimeToNextStage;
        }

        private void Grow()
        {
            _isReady = true;
            _sceneContainer.CropUI.SetActive(false);

            GrowUpEvent?.Invoke();
        }

        public void Water(float amount = 0.5f)
        {
            if (_isDead) return;
            _waterLevel = Mathf.Clamp01(_waterLevel + amount);
            _renderer.color = Color.white;
        }

        public void Harvest()
        {
            if (!_isReady) return;
            Debug.Log($"Harvested {_data.CropName}");
            Destroy(gameObject);
        }

        private void Die()
        {
            _isDead = true;
            DieEvent?.Invoke();
            Debug.Log($"{_data.CropName} died.");
        }
    }
}
