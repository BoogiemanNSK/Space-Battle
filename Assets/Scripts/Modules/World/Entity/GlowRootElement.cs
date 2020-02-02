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
            GlowRoot gRoot = ecsEntity.Set<GlowRoot>();
            gRoot.FriendlyGlow = this._friendlyGlow;
            gRoot.EnemyGlow = this._enemyGlow;

            if (ecsEntity.Get<UserPlayer>() == null) {
                _friendlyGlow.SetActive(false);
                _enemyGlow.SetActive(true);
            } else {
                _friendlyGlow.SetActive(true);
                _enemyGlow.SetActive(false);
            }
            
            if(OnScene)
                Destroy(this);
        }
    }
}