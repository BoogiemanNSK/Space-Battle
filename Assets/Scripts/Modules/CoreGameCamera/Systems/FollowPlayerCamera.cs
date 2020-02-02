using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class FollowPlayerTag : IEcsIgnoreInFilter {}
    public class StopFollowingTag : IEcsIgnoreInFilter {}

    public class FollowPlayerCamera : IEcsRunSystem
    {
        readonly EcsFilter<UserPlayer, Positioning.Components.Position> _target;
        readonly EcsFilter<FollowPlayerTag> _followPlayer;
        readonly EcsFilter<StopFollowingTag> _stopTag;
        readonly EcsFilter<CameraComponent, Positioning.Components.Position> _camera;
        readonly Utils.TimeService _t;

        public void Run()
        {
            if(!_followPlayer.IsEmpty())
            {
                foreach (var i in _target)
                {
                    foreach (var j in _camera)
                    {
                        _camera.Get2[j].Point.Set(UnityEngine.Mathf.Lerp(_camera.Get2[j].Point.x, _target.Get2[i].Point.x, 3f* _t.DeltaTime), _camera.Get2[j].Point.y, UnityEngine.Mathf.Lerp(_camera.Get2[j].Point.z, _target.Get2[i].Point.z-60.0f, 3f* _t.DeltaTime));
                    }
                }
            }   

            if(!_stopTag.IsEmpty())
            {
                foreach (var i in _followPlayer)
                {
                    _followPlayer.Entities[i].Unset<FollowPlayerTag>();
                }

                foreach (var i in _stopTag)
                {
                    _stopTag.Entities[i].Unset<StopFollowingTag>();
                }
            }

        }
    }
}