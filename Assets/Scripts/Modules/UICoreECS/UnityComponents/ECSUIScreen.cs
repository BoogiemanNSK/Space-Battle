using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Leopotam.Ecs;

namespace UICoreECS
{
    /// <summary>
    /// unity side thin monobehaviour
    /// </summary>
    public class ECSUIScreen : MonoBehaviour
    {
        #region SerializedFields
        [SerializeField] public int ID;
        [SerializeField] public int Layer;
        [SerializeField] public AUIEntity[] _extensions;
        [SerializeField, HideInInspector] private List<AScreenSwitcher> _showSwitchersChain = new List<AScreenSwitcher>();
        [SerializeField, HideInInspector] private List<AScreenSwitcher> _hideSwitchersChain = new List<AScreenSwitcher>();
        [SerializeField] private UnityEvent _onShow = new UnityEvent();
        [SerializeField] private UnityEvent _onHide = new UnityEvent();
        #endregion

        #region Privates
        private AScreenSwitcher _lock = null;
        private EcsWorld _world;
        #endregion

        #region Getters

        public List<AScreenSwitcher> ShowSwitchersChain
        {
            get => _showSwitchersChain;
            set => _showSwitchersChain = value;
        }

        public List<AScreenSwitcher> HideSwitchersChain
        {
            get => _hideSwitchersChain;
            set => _hideSwitchersChain = value;
        }

        public UnityEvent OnShow
        {
            get { return _onShow; }
            set { _onShow = value; }
        }

        public UnityEvent OnHide
        {
            get { return _onHide; }
            set { _onHide = value; }
        }

        #endregion

        #region BaseMethods
        public virtual ECSUIScreen Init(EcsWorld world, int id, int layer)
        {   
            InitChain(_showSwitchersChain);
            InitChain(_hideSwitchersChain);
            _world = world;
            ID = id;
            Layer = layer;
            for (int i = 0; i < _extensions.Length; i++)
            {
                _extensions[i].Init(world);
            }
            RectTransform rect = GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = Vector2.one * 0.5f;
            return this;
        }
        
        public virtual void Show()
        {
            Enable();
            OnShow.Invoke();
        }

        public virtual void Hide()
        {
            OnHide.Invoke();
            Disable();
        }

        public void ShowSelf()
        {
            // todo showself
            if(_world != null)
            {
                ShowScreenTag show = _world.NewEntity().Set<ShowScreenTag>();
                show.ID = ID;
                show.Layer = Layer;
            }
        }

        public void Disable()
        {
            UnlockSwithcer(true);
            // in case of only 1 swithcer at switch chain
            if (_hideSwitchersChain.Count > 0)
            {
                _hideSwitchersChain[0].Hide();
            }
            else if(_showSwitchersChain.Count > 0)
            {
                _showSwitchersChain[0].Hide();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void Enable()
        {
            
            UnlockSwithcer(true);
            if(_showSwitchersChain.Count >0)
                _showSwitchersChain[0].Show();
            else
                gameObject.SetActive(true);
            
        }
        


        #endregion

        #region Switchers

        private void InitChain(List<AScreenSwitcher> _chain)
        {
            for (int i = 0; i < _chain.Count; i++)
            {
                _chain[i].Init(this);
            }
        }

        public void LockSwithcer(AScreenSwitcher switcher)
        {
            if (_lock != null)
            {
                _lock.StopChain();
            }
            _lock = switcher;
        }
        
        public void UnlockSwithcer(bool checkLock = false)
        {
            if (checkLock && _lock != null)
            {
                _lock.StopChain();
            }
            _lock = null;
        }

        #endregion

        #region Extensions

        public AScreenSwitcher Next(List<AScreenSwitcher> chain, int current)
        {
            return chain.Count > current+1 ? chain[current+1] : null;
        }
        #endregion

#if UNITY_EDITOR
        private void Reset()
        {
        }
#endif
        
        
    }
}