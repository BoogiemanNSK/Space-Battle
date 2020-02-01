using UnityEngine;

namespace Modules.ViewHub.Data
{
    [CreateAssetMenu(menuName = "Modules/ViewHub/ViewIDCollection")]
    public class UViewIDCollection : ScriptableObject
    {
        public string[] IDS;
    }
}