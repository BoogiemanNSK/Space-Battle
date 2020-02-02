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

            _playerApi.DestroyAction(_world);
        }
    }


    public class MoveNetSystem : IEcsRunSystem
    {
        readonly EcsFilter<MoveActionTag> _filter;
        readonly EcsFilter<UIRemotePointTarget, WorldPoint> _target;
        readonly EcsWorld _world;
        readonly PlayerApi _playerApi;

        public MoveNetSystem(PlayerApi playerApi)
        {
            _playerApi = playerApi;
        }

        public void Run()
        {
            if(_filter.IsEmpty() || _target.IsEmpty())
                return;

            _playerApi.MoveAction(_world, _target.Get2[0].PointID);
        }
    }

    public class HealNetSystem : IEcsRunSystem
    {
        readonly EcsFilter<HealActionTag> _filter;
        readonly EcsFilter<UIRemotePointTarget, WorldPoint> _target;
        readonly EcsWorld _world;
        readonly PlayerApi _playerApi;

        public HealNetSystem(PlayerApi playerApi)
        {
            _playerApi = playerApi;
        }

        public void Run()
        {
            if(_filter.IsEmpty())
                return;

            _playerApi.HealAction(_world);
        }
    }

    public class AttackNetSystem : IEcsRunSystem
    {
        readonly EcsFilter<AttackActionTag> _filter;
        readonly EcsFilter<UserPlayer, Player> _target;
        readonly EcsFilter<Player> _players;
        readonly EcsWorld _world;
        readonly PlayerApi _playerApi;

        public AttackNetSystem(PlayerApi playerApi)
        {
            _playerApi = playerApi;
        }

        public void Run()
        {
            if(_filter.IsEmpty() || _target.IsEmpty())
                return;

            foreach (var i in _players)
            {
                if(_players.Get1[i].Location == _target.Get2[0].Location && !_players.Get1[i].Name.Equals(_playerApi.PlayerName))
                {
                    _playerApi.AttackAction(_world, _players.Get1[i].Name);
                }
            }
        }
    }

    
}