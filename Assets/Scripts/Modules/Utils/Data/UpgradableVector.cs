using System.Globalization;
using UnityEngine;

namespace Modules.Upgradables.Data
{
    [CreateAssetMenu(menuName = "Modules/Utils/Data/UpgradableVector")]
    public class UpgradableVector : ScriptableObject
    {
        [SerializeField] private Vector3[] _values;
        [SerializeField] private Vector3 _valueIncreaseRate;

        [SerializeField] private string _YValuesRange;
        [SerializeField] private string _ZValuesRange;


        public Vector3[] Values
        {
            get => _values;
            set => _values = value;
        }


        public Vector3 ValueIncreaseRate
        {
            get => _valueIncreaseRate;
            set => _valueIncreaseRate = value;
        }


        public Vector3 Value(int level)
        {
            if (_values.Length <= level)
            {
                return OneByOneMultiplication(_values[_values.Length-1],VectorPow(_valueIncreaseRate, level+1 - _values.Length));
            }

            return _values[level];
        }

        [ContextMenu("Parse values")]
        public void ValuesFromString()
        {
            float[] yRange = FloatsFromString(_YValuesRange);
            float[] zRange = FloatsFromString(_ZValuesRange);
            _values = new Vector3[Mathf.Max(yRange.Length, zRange.Length)];
            for (int i = 0; i < _values.Length; i++)
            {
                float y = i < yRange.Length  ? yRange[i] : yRange[yRange.Length - 1];
                float z = i < zRange.Length  ? zRange[i] : zRange[zRange.Length - 1];
                _values[i] = new Vector3(0.0f, y,z);
            }
        }

        private float[] FloatsFromString(string input)
        {
            string[] split = input.Split();
            float[] values = new float[split.Length];
            var clone = (CultureInfo) CultureInfo.InvariantCulture.Clone();
            clone.NumberFormat.NumberDecimalSeparator = ",";
            clone.NumberFormat.NumberGroupSeparator = ".";
            for (int i = 0; i < split.Length; i++)
            {
                values[i] = float.Parse(split[i], clone);
            }

            return values;
        }

        private Vector3 VectorPow(Vector3 vector, float degree)
        {
            return new Vector3(Mathf.Pow(vector.x,degree), Mathf.Pow(vector.y, degree), Mathf.Pow(vector.z, degree));
        }

        private Vector3 OneByOneMultiplication(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x*b.x, a.y*b.y, a.z*b.z);
        }
    }
}