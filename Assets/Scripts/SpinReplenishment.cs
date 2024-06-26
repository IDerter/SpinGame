using UnityEngine;
using System.Collections;

namespace SpinGame
{
    public class SpinReplenishment : MonoBehaviour
    {
        public int maxSpins = 5;
        private int currentSpins;
        private float replenishRate = 60f;

        private void Start()
        {
            currentSpins = maxSpins;
            StartCoroutine(ReplenishSpins());
        }

        private IEnumerator ReplenishSpins()
        {
            while (true)
            {
                if (currentSpins < maxSpins)
                {
                    // Ожидание времени восполнения
                    yield return new WaitForSeconds(replenishRate);
                    currentSpins++;
                    // Обновление UI или других компонентов игры
                    UpdateSpinCountUI();
                }
                else
                {
                    // Ожидание, пока количество спинов не станет меньше максимума
                    yield return new WaitUntil(() => currentSpins < maxSpins);
                }
            }
        }

        void UpdateSpinCountUI()
        {
            // Здесь должен быть код для обновления UI, отображающего количество спинов
            Debug.Log("Текущее количество спинов: " + currentSpins);
        }

        // Вызовите этот метод, когда игрок использует спин
        public void UseSpin()
        {
            if (currentSpins > 0)
            {
                currentSpins--;
                UpdateSpinCountUI();
            }
        }
    }
}