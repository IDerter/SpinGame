using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpinGame {

    public class CollectorCoins : MonoBehaviour
    {
        [SerializeField] private GameObject _coinPrefab;

        [SerializeField] private Transform _coinParent;
        [SerializeField] private Transform _spawnLocation;
        [SerializeField] private Transform _endPosition;
        [SerializeField] private Transform _endPositionToPunch;

        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private float _duration;
        [SerializeField] private int _coinAmount;

        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;

        [SerializeField] private int _coinStart;
        [SerializeField] private int _coinEnd;
        [SerializeField] private int _needToSum;

        List<GameObject> coins = new List<GameObject>();

        private Tween _coinReactionTween;
        private const string _audioNameCoins = "CollectCoins";

        public void SetAmountCoins(int amountCoins)
        {
            _coinAmount = amountCoins;
        }

        [Button()]
        public async void CollectCoins()
        {
            Debug.Log("StartCollectCoins");
            // Reset

            for (int i = 0; i < coins.Count; i++)
            {
                Destroy(coins[i]);
            }
            coins.Clear();
            // Spawn the coin to a specific location with random value

            List<UniTask> spawnCoinTaskList = new List<UniTask>();
            for (int i = 0; i < _coinAmount; i++)
            {
                GameObject coinInstance = Instantiate(_coinPrefab, _coinParent);
                float xPosition = _spawnLocation.position.x + Random.Range(minX, maxX);
                float yPosition = _spawnLocation.position.y + Random.Range(minY, maxY);

                coinInstance.transform.position = new Vector3(xPosition, yPosition);
                spawnCoinTaskList.Add(coinInstance.transform.DOPunchPosition(new Vector3(0, 30, 0), Random.Range(0, 1f)).SetEase(Ease.InOutElastic)
                    .ToUniTask());
                coins.Add(coinInstance);
                await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
            }

            AudioManager.Instance.PlaySound(_audioNameCoins);
            await UniTask.WhenAll(spawnCoinTaskList);
            // Move all the coins to the coin label
            await MoveCoinsTask();
            // Animation the reaction when collecting coin
        }

        private void SetCoin(int value)
        {
            _coinStart += value;
            _coinText.text = _coinStart.ToString();
        }

        private async UniTask MoveCoinsTask()
        {
            List<UniTask> moveCoinTask = new List<UniTask>();
            for (int i = coins.Count - 1; i >= 0; i--)
            {
                moveCoinTask.Add(MoveCoinTask(coins[i]));

                await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
            }
        }

        private async UniTask MoveCoinTask(GameObject coinInstance)
        {
            await coinInstance.transform.DOMove(_endPosition.position, _duration).SetEase(Ease.InBack).ToUniTask();

            GameObject temp = coinInstance;
            coins.Remove(coinInstance);
            Destroy(temp);

            await ReactToCollectionCoin();
            SetCoin(_needToSum / _coinAmount);
        }

        private async UniTask ReactToCollectionCoin()
        {
            if (_coinReactionTween == null)
            {
                _coinReactionTween = _endPositionToPunch.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f).SetEase(Ease.InOutElastic);
                await _coinReactionTween.ToUniTask();
                _coinReactionTween = null;
            }
        }
    }
}