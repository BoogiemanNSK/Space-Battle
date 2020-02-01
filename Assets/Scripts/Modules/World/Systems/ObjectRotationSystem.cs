using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public sealed class ObjectRotationSystem : IEcsRunSystem
    {
        private EcsFilter<RotatingObject, Positioning.Components.Position> _filter;
        
        public void Run()
        {
            foreach (var i in _filter)
            {
                RotatingObject rt = _filter.Get1[i];
                _filter.Get2[i].EulerRotation.x = (_filter.Get2[i].EulerRotation.x + rt.RotatingSpeedX) % 360.0f;
                _filter.Get2[i].EulerRotation.y = (_filter.Get2[i].EulerRotation.y + rt.RotatingSpeedY) % 360.0f;
                _filter.Get2[i].EulerRotation.z = (_filter.Get2[i].EulerRotation.z + rt.RotatingSpeedZ) % 360.0f;
            }
        }
    }
}