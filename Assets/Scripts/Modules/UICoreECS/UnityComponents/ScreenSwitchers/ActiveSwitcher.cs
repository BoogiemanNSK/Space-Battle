using System;
using UnityEditor;
using UnityEngine;

namespace UICoreECS
{
    [Serializable]
    public class ActiveSwitcher : AScreenSwitcher
    {
        [Header("ActiveSwitcher refs")]
        [SerializeField] private GameObject _screen;
        
        public override void Show(int current)
        {
            _screen.SetActive(true);
            base.Show(current);
        }

        public override void Hide(int current)
        {
            _screen.SetActive(false);
            base.Hide(current);
        }

    }
}