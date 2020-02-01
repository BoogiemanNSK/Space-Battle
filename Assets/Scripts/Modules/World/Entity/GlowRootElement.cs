using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.CoreGame
{
    public class GlowRootElement : ViewComponent
    {
        [SerializeField] private GameObject _friendlyGlow, _enemyGlow;
        
        public override void EntityInit(EcsEntity ecsEntity, EcsWorld world, bool OnScene)
        {
            if (ecsEntity.Get<UserPlayer>() == null) {
                _friendlyGlow.SetActive(false);
                _enemyGlow.SetActive(true);
            } else {
                _friendlyGlow.SetActive(true);
                _enemyGlow.SetActive(false);
            }
            
            ecsEntity.Set<GlowRoot>().FriendlyGlow = this._friendlyGlow;
            ecsEntity.Set<GlowRoot>().EnemyGlow = this._enemyGlow;
            if(OnScene)
                Destroy(this);
        }
    }
}