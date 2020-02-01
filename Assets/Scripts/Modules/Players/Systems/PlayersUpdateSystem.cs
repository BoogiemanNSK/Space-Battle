using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class PlayersUpdateSystem : IEcsRunSystem
    {
        readonly EcsFilter<PlayersUpdateTag> _tag;
        readonly EcsFilter<Player> _players;
        readonly EcsWorld _world;

        readonly PlayerApi _playerApi;

        public PlayersUpdateSystem(PlayerApi playerApi)
        {
            _playerApi = playerApi;
        }

        public void Run()
        {
            if(_tag.IsEmpty())
                return;

            foreach (var i in _players)
            {
                if(_playerApi.PlayerData.TryGetValue(_players.Get1[i].Name, out Player p))
                {
                    _players.Get1[i].HP = p.HP;
                    _players.Get1[i].Power = p.Power;
                    if (_players.Get1[i].Location != p.Location)
                    {
                        _players.Get1[i].Location = p.Location;
                        _players.Entities[i].Set<UpdatePlayerPointTag>();
                    }
                }
            }
        }
    }

    public class PlayersUpdateIssuingSystem : IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsWorld _world;

        readonly PlayerApi _playerApi;

        private float _updateTime;

        public PlayersUpdateIssuingSystem(PlayerApi playerApi)
        {
            _playerApi = playerApi;
        }

        public void Init()
        {
            _playerApi.UpdatePlayerData(_world);
            _updateTime = UnityEngine.Time.unscaledTime;
        }

        public void Run()
        {
            if(UnityEngine.Time.unscaledTime - _updateTime > 1.0f)
            {
                _playerApi.UpdatePlayerData(_world);
                _updateTime = UnityEngine.Time.unscaledTime;
            }
        }
    }
}