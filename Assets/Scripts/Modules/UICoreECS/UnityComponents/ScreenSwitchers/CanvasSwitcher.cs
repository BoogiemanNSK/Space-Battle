using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UICoreECS
{
    [Serializable]
    public class CanvasSwitcher :  AScreenSwitcher
    {
        [Header("CanvasSwitcher refs")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GraphicRaycaster _raycaster;

        public Canvas Canvas
        {
            get => _canvas;
            set => _canvas = value;
        }


        public override void Show(int current)
        {
            _canvas.enabled = true;
            _raycaster.enabled = true;
            base.Show(current);
        }

        public override void Hide(int current)
        {
            _canvas.enabled = false;
            _raycaster.enabled = false;
            base.Hide(current);
        }
        
    }
    
}