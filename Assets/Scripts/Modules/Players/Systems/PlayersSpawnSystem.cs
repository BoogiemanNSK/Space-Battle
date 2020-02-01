using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class SpawnPlayerTag : IEcsAutoReset
    {
        public Player Player;

        public void Reset()
        {
            Player = null;
        }
    }
    
    public class SpawnPlayerProcessing : IEcsRunSystem
    {
        // auto injected fields
        readonly EcsFilter<SpawnPlayerTag> _filter;

        readonly PlayerApi _playerApi;

        public SpawnPlayerProcessing(PlayerApi playerApi)
        {
            _playerApi = playerApi;
        }

        public void Run()
        {
            if(_filter.IsEmpty())
                return;

            Player p;

            foreach (var i in _filter)
            {
                p = _filter.Entities[i].Set<Player>();
                p.Name = _filter.Get1[i].Player.Name;
                p.Location = _filter.Get1[i].Player.Location;
                p.Power = _filter.Get1[i].Player.Power;
                p.HP = _filter.Get1[i].Player.HP;

                _filter.Entities[i].Set<ViewHub.AllocateView>().id = "Player";
                _filter.Entities[i].Set<Positioning.Components.Position>();
                _filter.Entities[i].Set<UpdatePlayerPointTag>();
                if(p.Name.Equals(_playerApi.PlayerName))
                {
                    _filter.Entities[i].Set<UserPlayer>();
                }
            }
        }
    }
}