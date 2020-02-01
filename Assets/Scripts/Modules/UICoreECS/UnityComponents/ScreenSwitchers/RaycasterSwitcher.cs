using UnityEngine;
using UnityEngine.UI;

namespace UICoreECS
{
    public class RaycasterSwitcher : AScreenSwitcher
    {
        [SerializeField] private GraphicRaycaster _raycaster;

        public override void Show(int current)
        {
            _raycaster.enabled = true;
            base.Show(current);
        }

        public override void Hide(int current)
        {
            _raycaster.enabled = false;
            base.Hide(current);
        }
    }
}