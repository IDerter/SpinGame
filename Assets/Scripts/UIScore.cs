using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using EasyUI.PickerWheelUI;

namespace SpinGame
{
    public class UIScore : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textCoins;
        [SerializeField] private int _score;
        [SerializeField] private CollectorCoins _collector;

        private float animationDuration = 0.5f; 

        public void RenderCount(int count)
        { 
            _textCoins.text = count.ToString();
        }

        private void Start()
        {
            _textCoins.text = _score.ToString();
            Spin.Instance.OnUpdateScore += OnUpdateScore;
        }

        private void OnDestroy()
        {
            Spin.Instance.OnUpdateScore -= OnUpdateScore;
        }

        private void OnUpdateScore(int addScore, WheelPiece wheelPiece)
        {
            if (wheelPiece.TypePiece == TypePiece.MoneyAdd || wheelPiece.TypePiece == TypePiece.MoneySteal)
            {
                AnimateScore(_score, _score + addScore);
                _collector.SetAmountCoins(addScore / 10000 + 5);
                _collector.CollectCoins();
                _score += addScore;
            }
        }

        private void AnimateScore(int currentScore, int finalScore)
        {
            StartCoroutine(AnimateScoreCoroutine(currentScore, finalScore));
        }

        private IEnumerator AnimateScoreCoroutine(int currentScore, int finalScore)
        {
            float timer = 0;
            while (timer < animationDuration)
            {
                int scoreToDisplay = (int)Mathf.Lerp(currentScore, finalScore, timer / animationDuration);
                _textCoins.text = scoreToDisplay.ToString("N0"); // Форматирование с разделителями групп разрядов
                timer += Time.deltaTime;
                yield return null;
            }
            _textCoins.text = finalScore.ToString("N0"); 
        }
    }
}