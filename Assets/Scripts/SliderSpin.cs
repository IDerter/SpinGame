using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGame
{
    public class SliderSpin : MonoBehaviour
    {
        [SerializeField] private Image spinSlider; // Ссылка на слайдер в вашем UI
        [SerializeField] private float maxSpins; // Максимальное количество спинов
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private Slider _sliderParticleTransform;

        private float _destroyTime = 5f;

        private void Start()
        {
            InitSlider();
            Spin.Instance.OnUpdateSpinCount += OnUpdateSpinCount;
        }

        private void OnDestroy()
        {
            Spin.Instance.OnUpdateSpinCount -= OnUpdateSpinCount;
        }

        private void OnUpdateSpinCount(int spinCount)
        {
            spinSlider.fillAmount = (float) (spinCount / maxSpins);
            _sliderParticleTransform.value = spinSlider.fillAmount;

            var particle = Instantiate(_particles, _sliderParticleTransform.handleRect);
            particle.Play();
            Destroy(particle, _destroyTime);
        }

        private void InitSlider()
        {
            maxSpins = GameInfo.Instance.CountSpinAvailable;
            spinSlider.fillAmount = maxSpins / maxSpins;
        }

        public void ResetSpins()
        {
            InitSlider();
        }
    }
}