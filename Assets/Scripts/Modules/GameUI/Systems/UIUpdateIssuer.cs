using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class UIUpadteIssuer : IEcsInitSystem, IEcsRunSystem
    {
        readonly EcsWorld _world;
        readonly Utils.TimeService _t;
        readonly float _tick;
        private float _updateTime;

        public UIUpadteIssuer(float tick)
        {
            _tick = tick;
        }

        public void Init()
        {
            _updateTime = _t.UnscaledTime;
            _world.NewEntity().Set<UIUpdate>();
        }
        public void Run()
        {
            if(_t.UnscaledTime - _updateTime > _tick)
            {
                _updateTime = _t.UnscaledTime;
                _world.NewEntity().Set<UIUpdate>();
            }
        }
    }
}