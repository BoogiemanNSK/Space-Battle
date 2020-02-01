using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub.Interfaces;
using UnityEngine;

namespace Modules.ViewHub
{
    [CreateAssetMenu(menuName = "Modules/ViewHub/Hub")]
    public class ViewHub : ScriptableObject
    {
        private Dictionary<string, IUViewElement> _views;
        private Dictionary<string, ViewElementGroup> _viewElementGroups;
        [SerializeField] private List<MonoBehaviour> _viewElements;
        [SerializeField] private List<ViewElementGroup> _viewGroups;

        public void Init()
        {
            _views = new Dictionary<string, IUViewElement>();
            _viewElementGroups = new Dictionary<string, ViewElementGroup>();
            foreach (var view in _viewElements )
            {
                IUViewElement viewElement = view as IUViewElement;
                _views.Add(viewElement.ID, viewElement);
            }

            foreach (var view in _viewGroups)
            {
                _viewElementGroups.Add(view.id, view);
                view.Init();
                foreach (var item in view.Items)
                {
                    _views.Add(item.ID, item);
                }
            }
        }

        public void Allocate(string key, EcsEntity entity, EcsWorld world)
        {
            _views[key].Allocate(entity, world);
        }

        public IUViewElement View(string key)
        {
            return _views[key];
        }

        public ViewElementGroup Group(string key)
        {
            return _viewElementGroups[key];
        }
    
    
    }
}