using UnityEngine;
using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class TargetPoint : IEcsAutoReset
    {
        public Vector3 Point;
        public bool FirstSpawn;

        public void Reset() 
        {
            FirstSpawn = true;
        }
    }

    public class MoveTargetSys : IEcsRunSystem
    {
        readonly EcsFilter<TargetPoint, Positioning.Components.Position> _filter;
        readonly Utils.TimeService _t;

        public void Run()
        {
            foreach (var i in _filter)
            {
                if(_filter.Get1[i].FirstSpawn)
                {
                    _filter.Get1[i].FirstSpawn = false;
                    _filter.Get2[i].Point = _filter.Get1[i].Point;
                }else
                {
                    _filter.Get2[i].Point = Vector3.Lerp(_filter.Get2[i].Point, _filter.Get1[i].Point, 3.0f * _t.DeltaTime);
                }
            }
        }
    }
}