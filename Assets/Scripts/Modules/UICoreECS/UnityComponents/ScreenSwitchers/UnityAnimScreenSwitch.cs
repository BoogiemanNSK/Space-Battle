using System.Collections;
using UnityEngine;

namespace UICoreECS
{
    public class UnityAnimScreenSwitch : AScreenSwitcher
    {
        [SerializeField] private Animator _anim;
        [SerializeField] private string _showAnimKeyword;
        [SerializeField] private string _hideAnimKeyword;
        [SerializeField] private float _animTime;
        [SerializeField] private float _hideTime;
        [SerializeField] private bool _hideAnim;
         
        private int _current;
        private IEnumerator _enumerator;
        
        public override void Show(int current)
        {
            _current = current;
            _enumerator = AnimRoutine(true);
            StartCoroutine(_enumerator);
        }

        public override void Hide(int current)
        {
            _current = current;
            _enumerator = AnimRoutine(false);
            StartCoroutine(_enumerator);
        }

        public override void StopChain()
        {
            if (_enumerator != null)
            {
                StopCoroutine(_enumerator);
            }

            _anim.enabled = false;
        }

        IEnumerator AnimRoutine(bool show)
        {
            if (!show && !_hideAnim)
            {
                HideNext(_current);
                yield break;
            }

            LockChain();
            _anim.enabled = true;
            _anim.Play(show ? _showAnimKeyword : _hideAnimKeyword, 0, 0.0f);
            
            if(_anim.updateMode == AnimatorUpdateMode.UnscaledTime)
                yield return new WaitForSecondsRealtime(show ? _animTime : _hideTime);
            else
                yield return new WaitForSeconds(show ? _animTime : _hideTime);
            
            _anim.Play(show ? _showAnimKeyword : _hideAnimKeyword, 0, 1.0f);
            yield return null;
            _anim.enabled = false;
            UnlockChain();
            
            if(show)
                ShowNext(_current);
            else
                HideNext(_current);
            
        }
    }
}