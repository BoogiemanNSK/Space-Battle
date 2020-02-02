using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class UpdateUserPlanetTag : IEcsIgnoreInFilter { }

    public class UpdateUserPlanetProcessing : IEcsRunSystem
    {
        // auto injected fields
        readonly EcsFilter<UpdateUserPlanetTag> _filter;
        readonly EcsFilter<UserPlayer, UpdatePlayerPointTag> _playerMove;
        readonly EcsFilter<UserPlayer, Player> _player;
        readonly EcsFilter<CurrentPoint> _current;
        readonly EcsFilter<WorldPoint> _points;

        public void Run()
        {
            if(!_filter.IsEmpty() || !_playerMove.IsEmpty())
            {
                foreach (var i in _current)
                {
                    _current.Entities[i].Unset<CurrentPoint>();
                }

                foreach (var i in _points)
                {
                    foreach (var j in _player)
                    {
                        if(_player.Get2[j].Location == _points.Get1[i].PointID)
                        {
                            _points.Entities[i].Set<CurrentPoint>();
                        }
                    }
                }
            }
        }
    }
}