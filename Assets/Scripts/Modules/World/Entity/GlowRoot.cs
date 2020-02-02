using Leopotam.Ecs;
using UnityEngine;

namespace Modules.CoreGame
{
    public class GlowRoot : IEcsAutoReset
    {
        public GameObject FriendlyGlow, EnemyGlow;
        public void Reset()
        {
            this.FriendlyGlow = null;
            this.EnemyGlow = null;
        }
    }
}