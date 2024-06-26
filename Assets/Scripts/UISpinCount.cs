using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SpinGame
{
    public class UISpinCount : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _spinCount;
        [SerializeField] private int _spinStartCount;

        private void Start()
        {
            _spinStartCount = GameInfo.Instance.CountSpinAvailable;
            OnUpdateSpinCount(_spinStartCount);

            Spin.Instance.OnUpdateSpinCount += OnUpdateSpinCount;
        }

        private void OnUpdateSpinCount(int spinCount)
        {
            _spinCount.text = spinCount.ToString() + "/" + _spinStartCount.ToString();
        }
    }
}