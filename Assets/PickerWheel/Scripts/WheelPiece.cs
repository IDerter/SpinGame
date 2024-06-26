using UnityEngine ;

namespace EasyUI.PickerWheelUI 
{
    public enum TypePiece
    {
        MoneyAdd,
        Spin,
        MoneySteal
    }

    [System.Serializable]
    public class WheelPiece 
    {
        [SerializeField] private Sprite _icon;
        public Sprite Icon => _icon;

        [SerializeField] private string _label;
        public string Label => _label;

        [SerializeField] private TypePiece _typePiece;
        public TypePiece TypePiece => _typePiece;

        [Tooltip ("Reward amount")] [SerializeField] private int _amount;
        public int Amount => _amount;

        [Tooltip ("Probability in %")] 
        [Range (0f, 100f)]
        [SerializeField] private float _chance = 100f;
        public float Chance => _chance;

        [HideInInspector] [SerializeField] private int _index;
        public int Index { get { return _index; } set { _index = value; } }

        [HideInInspector] public double _weight = 0f;
    }
}
