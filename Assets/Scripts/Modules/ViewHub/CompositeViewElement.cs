using UnityEngine;
using System.Collections.Generic;
using Modules.ViewHub.Interfaces;
using Leopotam.Ecs;

namespace Modules.ViewHub
{

    public class CompositeViewElement : MonoBehaviour, IUViewElement
    {
        public string id;
        public ViewElement _dynamic;
        public UMeshRenderer.UnityComponents.MeshView _static;

        public void Allocate(EcsEntity entity, EcsWorld world)
        {
            _dynamic.Allocate(entity, world);
            _static.Allocate(entity, world);
        }

        public string ID => id;

#if UNITY_EDITOR
        private void Reset()
        {
            id = gameObject.name;
        }
#endif
    }

}