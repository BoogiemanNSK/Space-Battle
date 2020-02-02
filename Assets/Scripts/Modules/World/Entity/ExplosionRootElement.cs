using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.CoreGame
{
    public class ExplosionRootElement : ViewComponent
    {
        [SerializeField] private ParticleSystem _explosion;
        
        public override void EntityInit(EcsEntity ecsEntity, EcsWorld world, bool OnScene)
        {
            ExplosionRoot eRoot = ecsEntity.Set<ExplosionRoot>();
            eRoot.Explosion = this._explosion;
            
            if(OnScene)
                Destroy(this);
        }
    }
}