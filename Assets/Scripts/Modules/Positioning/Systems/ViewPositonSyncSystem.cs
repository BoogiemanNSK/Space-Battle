using Leopotam.Ecs;
using Modules.Positioning.Components;
using Modules.ViewHub;

namespace Modules.Positioning.Systems
{
    public sealed class UPositionSyncSystem : IEcsRunSystem
    {
        private EcsFilter<Position, UnityView>.Exclude<LazyPosition> _filter;
        private EcsFilter<Position, UnityView, LazyPositionUpdate> _lazyObjects;
        
        public void Run()
        {
            foreach (var i in _filter)
            {
                _filter.Get2[i].Transform.position = _filter.Get1[i].Point;
            }

            if(_lazyObjects.IsEmpty())
                return;

            foreach (var i in _lazyObjects)
            {
                _lazyObjects.Get2[i].Transform.localPosition = _lazyObjects.Get1[i].Point;
            }
        }
    }
}