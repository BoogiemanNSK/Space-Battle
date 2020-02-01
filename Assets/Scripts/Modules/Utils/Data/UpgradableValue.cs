using System.Globalization;
using UnityEngine;

namespace Modules.Upgradables.Data
{
    [CreateAssetMenu(menuName = "Modules/Utils/Data/UpgradableValue")]
    public class UpgradableValue : ScriptableObject
    {
        [SerializeField] private float[] _values;
        [SerializeField] private float _valueIncreaseRate;

        [SerializeField] private string _valuesRange;


        public float[] Values
        {
            get => _values;
            set => _values = value;
        }


        public float ValueIncreaseRate
        {
            get => _valueIncreaseRate;
            set => _valueIncreaseRate = value;
        }

        public float Value(int level)
        {
            if (_values.Length <= level)
            {
                return _values[_values.Length-1]* Mathf.Pow(_valueIncreaseRate, level+1 - _values.Length);
            }

            return _values[level];
        }
        
        [ContextMenu("Parse values")]
        public void ValuesFromString()
        {
            string[] split = _valuesRange.Split();
            _values = new float[split.Length];
            var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            clone.NumberFormat.NumberDecimalSeparator = ",";
            clone.NumberFormat.NumberGroupSeparator = ".";
            for (int i = 0; i < split.Length; i++)
            {
                _values[i] = float.Parse(split[i], clone);
            }
        }

    }
}