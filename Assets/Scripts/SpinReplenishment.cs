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
                    yield return new WaitForSeconds(replenishRate);
                    currentSpins++;
                    UpdateSpinCountUI();
                }
                else
                {
                    // ќжидание, пока количество спинов не станет меньше максимума
                    yield return new WaitUntil(() => currentSpins < maxSpins);
                }
            }
        }

        void UpdateSpinCountUI()
        {
            // «десь должен быть код дл€ обновлени€ UI, отображающего количество спинов
            Debug.Log("“екущее количество спинов: " + currentSpins);
        }

        // ¬ызовите этот метод, когда игрок использует спин
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