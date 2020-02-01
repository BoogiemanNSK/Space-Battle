using System.Collections;
using UnityEngine;

namespace UICoreECS
{
    public class MoveScreenSwitch : AScreenSwitcher
    {
        [Header("MoveSwitch refs")]
        [SerializeField] private Transform _hidepoint;
        [SerializeField] private Transform _showPoint;
        [SerializeField] private Transform _screenTransform;
        [SerializeField] private float _transitionTime = 1.0f;

        private int _current;
        private IEnumerator _enumerator;
        
        public override void Show(int current)
        {
            _current = current;
            _enumerator = ShowMove();
            StartCoroutine(_enumerator);
        }

        public override void Hide(int current)
        {
            _current = current;
            _enumerator = HideMove();
            StartCoroutine(_enumerator);
        }

        public override void StopChain()
        {
            if (_enumerator != null)
            {
                StopCoroutine(_enumerator);
            }
        }

        private IEnumerator HideMove()
        {
            LockChain();
            float start = Time.time;
            
            Vector3 initPos = _showPoint.position;
            while (Time.time - start < _transitionTime )
            {
               _screenTransform.position = Vector3.Lerp(initPos, _hidepoint.position, ((Time.time - start)/_transitionTime));
               yield return null;
            }

            _screenTransform.position = _hidepoint.position;
            UnlockChain();
            base.Hide(_current);
            
        }
        
        private IEnumerator ShowMove()
        {
            LockChain();
            float start = Time.time;
            Vector3 initPos = _hidepoint.position;
            while (Time.time - start < _transitionTime )
            {
                _screenTransform.position = Vector3.Lerp(initPos, _showPoint.position, ((Time.time - start)/_transitionTime));
                yield return null;
            }

            _screenTransform.position = _showPoint.position;
            UnlockChain();
            base.Show(_current);
        }
    }
}