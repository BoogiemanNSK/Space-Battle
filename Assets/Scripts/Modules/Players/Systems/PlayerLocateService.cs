using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class UpdatePlayerPointTag : IEcsIgnoreInFilter { }

    public class UpdatePlayerPointProcessing : IEcsRunSystem
    {
        // auto injected fields
        readonly EcsFilter<UpdatePlayerPointTag, Player, Positioning.Components.Position> _filter;
        readonly EcsFilter<WorldPoint, Positioning.Components.Position> _points;

        public void Run()
        {
            if(_filter.IsEmpty())
                return;

            foreach (var i in _filter)
            {
                foreach (var point in _points)
                {
                    if(_points.Get1[point].PointID == _filter.Get2[i].Location)
                    {
                        _filter.Entities[i].Set<TargetPoint>().Point = _points.Get2[point].Point;
                        _filter.Entities[i].Unset<UpdatePlayerPointTag>();
                    }
                }
            }
        }
    }
}