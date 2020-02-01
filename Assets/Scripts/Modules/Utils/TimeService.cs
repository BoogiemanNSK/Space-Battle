using Leopotam.Ecs;
using UnityEngine;

namespace Modules.Utils
{
    public sealed class TimeService
    {
        public float Time;
        public float DeltaTime;
    }
    
    public sealed class UnityTimeSystem : IEcsRunSystem {
        // auto-injected fields.
        readonly TimeService _time = null;

        public void Run () {
            _time.Time = Time.time;
            _time.DeltaTime = Time.deltaTime;
        }
    }
}