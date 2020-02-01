using System.Collections;
using UnityEngine;

namespace UICoreECS
{
    public class SleepSwithcer : AScreenSwitcher
    {
        [SerializeField] private float _sleepTime = 0.5f;
        [SerializeField] private bool _unscaledTime = true;

        private int _current;
        private IEnumerator _enumerator;
        
        public override void Show(int current)
        {
            _current = current;
            _enumerator = SleepRoutine(true);
            StartCoroutine(_enumerator);
        }

        public override void Hide(int current)
        {
            _current = current;
            _enumerator = SleepRoutine(false);
            StartCoroutine(_enumerator);
        }

        public override void StopChain()
        {
            if (_enumerator != null)
            {
                StopCoroutine(_enumerator);
            }
        }

        IEnumerator SleepRoutine(bool show)
        {
            LockChain();
            
            if(_unscaledTime)
                yield return new WaitForSecondsRealtime(_sleepTime);
            else
                yield return new WaitForSeconds(_sleepTime);
            UnlockChain();
            if(show)
                ShowNext(_current);
            else
                HideNext(_current);
            
        }
    }
}