using Leopotam.Ecs;
using UnityEngine;

namespace Modules.CoreGame
{
    public class ExplosionRoot : IEcsAutoReset
    {
        public ParticleSystem Explosion;
        public void Reset()
        {
            this.Explosion = null;
        }
    }
}