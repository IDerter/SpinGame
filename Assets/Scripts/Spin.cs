using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

namespace SpinGame
{
	public class Spin : SingletonBase<Spin>
	{
		public event Action<int, WheelPiece> OnUpdateScore;
		public event Action<int> OnUpdateSpinCount;

		[SerializeField] private Button _uiSpinButton;
		[SerializeField] private TextMeshProUGUI _uiSpinButtonText;
		[SerializeField] private TextMeshProUGUI _uiSpinReplenishText;

		[SerializeField] private PickerWheel _pickerWheel;
		[SerializeField] private GameInfo _gameInfo;

		[SerializeField] private int _countSpins;
		[SerializeField] private float _replenishRate = 20f;
		[SerializeField] private bool isCoroutineRunning = false;

		private void Start()
		{
			SpinAmount.Instance.OnUpdateSpinCount += OnUpdateMaxSpinCount;

			_countSpins = _gameInfo.CountSpinAvailable;



			_uiSpinButton.onClick.AddListener(() =>
			{
				StartCoroutine(ReplenishSpins());
				if (_countSpins > 0)
				{
					_uiSpinButton.interactable = false;
					_uiSpinButtonText.text = "Spinning";
					_countSpins--;
					OnUpdateSpinCount?.Invoke(_countSpins);

					_pickerWheel.OnSpinStart(() =>
					{
						Debug.Log("Spin started!");
					});

					_pickerWheel.OnSpinEnd(wheelPiece =>
					{
						SpinEnd(wheelPiece);
					});

					_pickerWheel.Spin();
				}
			});

		}

		private void SpinEnd(WheelPiece wheelPiece)
		{
			_uiSpinButton.interactable = true;
			Debug.Log("Spin ended : Label: " + wheelPiece.Label + " , Amount:" + wheelPiece.Amount);
			if (wheelPiece.Amount > 0)
				OnUpdateScore.Invoke(wheelPiece.Amount, wheelPiece);

			_uiSpinButtonText.text = "Spin";
			if (_countSpins <= 0)
			{
				_uiSpinButton.interactable = false;
				_uiSpinButtonText.text = "Zero Mana";
			}
		}

		private void OnDestroy()
		{
			SpinAmount.Instance.OnUpdateSpinCount -= OnUpdateMaxSpinCount;
		}

		private void OnUpdateMaxSpinCount(int addSpin)
		{
			if (_countSpins + addSpin <= _gameInfo.CountSpinAvailable)
				_countSpins += addSpin;
			else
				_countSpins = _gameInfo.CountSpinAvailable;
			OnUpdateSpinCount?.Invoke(_countSpins);
		}

		private IEnumerator ReplenishSpins()
		{
			while (true)
			{
				
				if (_countSpins < _gameInfo.CountSpinAvailable)
				{
					if (!isCoroutineRunning)
						StartCoroutine(AnimateTimeReplenish());
					// ќжидание времени восполнени€
					yield return new WaitForSeconds(_replenishRate);
					

					if (_countSpins == 0)
					{
						_uiSpinButton.interactable = true;
						_uiSpinButtonText.text = "Spin";
					}
				}
				else
				{
					// ќжидание, пока количество спинов не станет меньше максимума
					yield return new WaitUntil(() => _countSpins < _gameInfo.CountSpinAvailable);
				}
			}
		}

		private IEnumerator AnimateTimeReplenish()
		{
			if (!isCoroutineRunning)
            {
				isCoroutineRunning = true;
				float timer = _replenishRate;
				while (timer > 0)
				{
					timer -= Time.deltaTime;
					_uiSpinReplenishText.text = "1 spin in " + ((int)timer).ToString() + " sec";
					yield return null;
				}
				isCoroutineRunning = false;

				if (_countSpins < _gameInfo.CountSpinAvailable)
				{
					_countSpins++;
					// ќбновление UI или других компонентов игры
					OnUpdateSpinCount?.Invoke(_countSpins);
				}
			}
		}
	}
}