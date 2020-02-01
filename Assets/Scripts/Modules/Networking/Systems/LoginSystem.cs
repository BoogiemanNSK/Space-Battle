using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class ConnectSystem : IEcsRunSystem
    {
        readonly EcsFilter<LoginActionTag> _filter;
        readonly EcsWorld _world;
        readonly PlayerApi _playerApi;

        public ConnectSystem(PlayerApi playerApi)
        {
            _playerApi = playerApi;
        }

        public void Run()
        {
            if(_filter.IsEmpty())
                return;

            _playerApi.Login(_world, _filter.Get1[0].PlayerName);
        }
    }
}