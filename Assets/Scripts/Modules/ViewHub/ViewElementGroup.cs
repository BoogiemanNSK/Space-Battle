using System.Collections.Generic;
using Modules.ViewHub.Interfaces;
using UnityEngine;

namespace Modules.ViewHub
{
    [CreateAssetMenu(menuName = "Modules/ViewHub/ViewElementGroup")]
    public class ViewElementGroup : ScriptableObject
    {
        public string id;
        public List<IUViewElement> Items;
        public List<MonoBehaviour> Views;

        public void Init()
        {
            Items = new List<IUViewElement>();
            foreach (var i in Views)
            {
                IUViewElement element = (IUViewElement) i;
                if(element != null)
                    Items.Add(element);
            }
        }
    }
}