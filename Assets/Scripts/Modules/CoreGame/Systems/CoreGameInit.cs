using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class CoreGameInit : IEcsInitSystem
    {
        // auto injected
        EcsWorld _world;

        public void Init()
        {
            // todo core init logic
            UICoreECS.ShowScreenTag _login = _world.NewEntity().Set<UICoreECS.ShowScreenTag>();
            _login.ID = 0;
            _login.Layer = 0;
        }
    }
}