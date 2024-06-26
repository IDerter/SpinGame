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
                    // ��������, ���� ���������� ������ �� ������ ������ ���������
                    yield return new WaitUntil(() => currentSpins < maxSpins);
                }
            }
        }

        void UpdateSpinCountUI()
        {
            // ����� ������ ���� ��� ��� ���������� UI, ������������� ���������� ������
            Debug.Log("������� ���������� ������: " + currentSpins);
        }

        // �������� ���� �����, ����� ����� ���������� ����
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