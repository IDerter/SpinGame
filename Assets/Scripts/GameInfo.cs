using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpinGame
{
    public class GameInfo : SingletonBase<GameInfo>
    {
        [SerializeField] private int _countSpinAvailable = 10;
        public int CountSpinAvailable { get { return _countSpinAvailable; } set { _countSpinAvailable = value; } }
    }
}