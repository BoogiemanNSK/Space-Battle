using Leopotam.Ecs;

namespace Modules.ViewHub
{
    public sealed class ViewAllocatorSystem : IEcsRunSystem
    {
        // auto injected fields
        private EcsFilter<AllocateView> _filter;
        private EcsWorld _world;
        private ViewHub _hub;
        
        public ViewAllocatorSystem(ViewHub hub)
        {
            _hub = hub;
        }
        
        public void Run()
        {
            foreach (var i in _filter)
            {
                _hub.Allocate(_filter.Get1[i].id, _filter.Entities[i], _world);
            }
        }
    }
}