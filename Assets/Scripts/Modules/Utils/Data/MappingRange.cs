using System.Linq;
using UnityEngine;

namespace Modules.Utils.Data
{
    [CreateAssetMenu(menuName = "Modules/Utils/Data/MappingRange")]
    public class MappingRange : ScriptableObject
    {
        [SerializeField] private MappingRangeEntry[] _entries;
        
        public MappingRangeEntry GetRange(float value)
        {
            MappingRangeEntry entry = _entries[0];
            for (int i = 0; i < _entries.Length; i++)
            {
                if (_entries[i].Min < value)
                    entry = _entries[i];
            }
            return entry;
        }

        public float GetRangedValue(float value)
        {
            return GetRange(value).GetRangedValue(value);
        }

        [ContextMenu(nameof(Sort))]
        private void Sort()
        {
            if(_entries != null && _entries.Length >0)
                _entries = _entries.OrderBy((value) => value.Min).ToArray();
        }

#if UNITY_EDITOR
        [ContextMenu("Test ranges")]
        private void TestRanges()
        {
            float step = 1.0f/20.0f;
            string result = "";
            for (int i = 0; i <= 20; i++)
            {
                float input = step * i;
                float output = GetRange(input).GetRangedValue(input);
                result +=
                    $"<color=blue>ID: {GetRange(input).Id}</color> <color=green>Input: {input}</color> <color=teal>Output:{output}</color>\n";
            }
            Debug.Log(result);
        }
#endif

    }

    [System.Serializable]
    public class MappingRangeEntry
    {   
        [SerializeField] public string Id;
        [SerializeField] public float Min;
        [SerializeField] public float Max;
        [SerializeField] public float MinValue;
        [SerializeField] public float MaxValue;

        public float GetRangedValue(float result)
        {
            result = Mathf.Max(Min, result);
            result = Mathf.Min(result, Max);
            float x = (MaxValue - MinValue) / (Max - Min);
            return MinValue+(result - Min)*x;
        }
    }
}