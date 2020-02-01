using Leopotam.Ecs;
using Modules.Positioning.Components;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.Positioning.UnityComponents
{
    public class RotationRootElement : ViewComponent
    {
        [SerializeField] private Transform _root;
        
        public override void EntityInit(EcsEntity ecsEntity, EcsWorld world, bool OnScene)
        {
            ecsEntity.Set<URotationRoot>().Transform = this._root;
            if(OnScene)
                Destroy(this);
        }
    }
}