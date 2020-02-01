using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub.Interfaces;
using UnityEngine;

namespace Modules.ViewHub
{
    public class SubViewElement : ViewComponent, IUViewElement
    {
        public string id;        

        public List<ViewComponent> _components;

        public string ID => id;

        public void Allocate(EcsEntity entity, EcsWorld world)
        {
            GameObject.Instantiate(this, transform.parent).Spawn(entity, world);
        }

        public override void EntityInit(EcsEntity entity, EcsWorld world, bool parentOnScene)
        {
            Spawn(world.NewEntity(), world);
        }

        public void Spawn(EcsEntity entity, EcsWorld world)
        {
            OnSpawn(entity);
            _components.ForEach((component => component.EntityInit(entity, world, true)));
            Destroy(this);
        }

        public virtual void OnSpawn(EcsEntity entity)
        {
            entity.Set<UnityView>();
            UnityView view = entity.Get<UnityView>();
            view.GameObject = this.gameObject;
            view.id = id;
            view.Transform = transform;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            id = gameObject.name;
        }

        [ContextMenu("Collect components")]
        private void CollectComponents()
        {
            _components = new List<ViewComponent>(transform.GetComponentsInChildren<ViewComponent>());
            for (int i = _components.Count-1; i >= 0; i--)
            {
                if(_components[i] == this)
                {
                    _components.RemoveAt(i);
                }
            }
        }
#endif
    }
}