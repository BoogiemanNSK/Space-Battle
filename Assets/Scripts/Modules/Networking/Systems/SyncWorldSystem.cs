using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class SyncWorldSystem : IEcsRunSystem, IEcsInitSystem
    {
        // auto injected
        readonly EcsWorld _world;

        readonly WorldApi _worldApi;

        public SyncWorldSystem(WorldApi worldApi)
        {
            _worldApi = worldApi;
        }

        public void Init()
        {
            _worldApi.GetWorldData(_world);
        }

        public void Run()
        {
            
        }
    }
}