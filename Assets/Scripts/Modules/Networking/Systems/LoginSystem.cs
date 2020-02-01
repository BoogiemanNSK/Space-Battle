using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class ConnectSystem : IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsFilter<LoginActionTag> _loginFilter;
        readonly EcsFilter<LoggedInTag> _loggedIn;
        readonly EcsWorld _world;
        readonly PlayerApi _playerApi;

        public ConnectSystem(PlayerApi playerApi)
        {
            _playerApi = playerApi;
        }

        public void Init()
        {
            _playerApi.ValideConnection(_world);
        }

        public void Run()
        {
            if (!_loginFilter.IsEmpty())
            {
                _playerApi.Connect(_world, _loginFilter.Get1[0].PlayerName);
            }
            
            if(!_loggedIn.IsEmpty())
            {
                UICoreECS.ShowScreenTag screen = _world.NewEntity().Set<UICoreECS.ShowScreenTag>();
                screen.ID = 1;
                screen.Layer = 0;
            }
        }
    }
}