using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class PlayersUpdateSystem : IEcsRunSystem
    {
        readonly EcsFilter<PlayersUpdateTag> _tag;
        readonly EcsFilter<Player> _players;
        readonly EcsFilter<UserPlayer, Player> _user;
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
                }else
                {
                    _players.Entities[i].Destroy();
                }
            }

            foreach (var i in _user)
            {
                if(_user.Get2[i].HP <= 0)
                {
                    UICoreECS.ShowScreenTag screen = _world.NewEntity().Set<UICoreECS.ShowScreenTag>();
                    screen.ID = 2;
                    screen.Layer = 0;
                    UnityEngine.PlayerPrefs.DeleteKey("token");
                    UnityEngine.PlayerPrefs.DeleteKey("username");
                    UnityEngine.PlayerPrefs.Save();
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