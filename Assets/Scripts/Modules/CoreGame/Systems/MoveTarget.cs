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
                    if ((_filter.Get1[i].Point - _filter.Get2[i].Point).magnitude > 1.0f)
                    {
                        // Smoothly rotate towards the target point.
                        _filter.Get2[i].EulerRotation = Quaternion.Slerp(Quaternion.Euler(_filter.Get2[i].EulerRotation), Quaternion.LookRotation(_filter.Get1[i].Point - _filter.Get2[i].Point), 4.5f * Time.deltaTime).eulerAngles;
                    }

                    _filter.Get2[i].Point = Vector3.Lerp(_filter.Get2[i].Point, _filter.Get1[i].Point, 3.0f * _t.DeltaTime);
                }
            }
        }
    }
}