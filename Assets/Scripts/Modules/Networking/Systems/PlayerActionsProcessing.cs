using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class BuyNetSystem : IEcsRunSystem
    {
        readonly EcsFilter<BuyActionTag> _filter;
        readonly EcsWorld _world;
        readonly PlayerApi _playerApi;

        public BuyNetSystem(PlayerApi playerApi)
        {
            _playerApi = playerApi;
        }

        public void Run()
        {
            if(_filter.IsEmpty())
                return;

            _playerApi.BuyAction(_world);
        }
    }

        public class DestroyNetSystem : IEcsRunSystem
    {
        readonly EcsFilter<DestroyActionTag> _filter;
        readonly EcsWorld _world;
        readonly PlayerApi _playerApi;

        public DestroyNetSystem(PlayerApi playerApi)
        {
            _playerApi = playerApi;
        }

        public void Run()
        {
            if(_filter.IsEmpty())
                return;

            _playerApi.BuyAction(_world);
        }
    }
}