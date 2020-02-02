using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.CoreGame
{
    public class MarkerRootElement : ViewComponent
    {
        [SerializeField] private GameObject _friendlyMarker, _enemyMarker;
        
        public override void EntityInit(EcsEntity ecsEntity, EcsWorld world, bool OnScene)
        {
            MarkerRoot mRoot = ecsEntity.Set<MarkerRoot>();
            mRoot.FriendlyMarker = this._friendlyMarker;
            mRoot.EnemyMarker = this._enemyMarker;
            
            if(OnScene)
                Destroy(this);
        }
    }
}