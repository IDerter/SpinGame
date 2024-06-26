using EasyUI.PickerWheelUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpinGame
{
    public class SpinAmount : SingletonBase<SpinAmount>
    {
        public event Action<int> OnUpdateSpinCount;

        private void Start()
        {
            Spin.Instance.OnUpdateScore += OnUpdateScore;
        }

        private void OnDestroy()
        {
            Spin.Instance.OnUpdateScore -= OnUpdateScore;
        }

        private void OnUpdateScore(int addScore, WheelPiece wheelPiece)
        {
            if (wheelPiece.TypePiece == TypePiece.Spin)
            {
                if (wheelPiece.Amount > 0)
                    OnUpdateSpinCount?.Invoke(wheelPiece.Amount);
            }
        }
    }
}