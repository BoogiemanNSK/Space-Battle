using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class UpdateOwnersTag : IEcsIgnoreInFilter { }

    public class UpdateOwnersProcessing : IEcsRunSystem, IEcsInitSystem
    {
        // auto injected fields
        readonly EcsFilter<UpdateOwnersTag> _filter;
        readonly EcsFilter<WorldPoint> _points;
        readonly EcsWorld _world;

        readonly WorldApi _worldApi;

        private float _updateTime;

        public UpdateOwnersProcessing(WorldApi worldApi)
        {
            _worldApi = worldApi;
        }

        public void Init()
        {
            _worldApi.UpdateOwned(_world);
            _updateTime = UnityEngine.Time.unscaledTime;
        }

        public void Run()
        {
            if(UnityEngine.Time.unscaledTime - _updateTime > 1.0f)
            {
                _worldApi.UpdateOwned(_world);
                _updateTime = UnityEngine.Time.unscaledTime;
            }

            if(_filter.IsEmpty())
                return;

            foreach (var i in _points)
            {
                if(_worldApi.OwnedData.TryGetValue(_points.Get1[i].PointID, out string owner))
                {
                    _points.Entities[i].Set<PointOwner>().OwnerID = owner;
                }else
                {
                    _points.Entities[i].Unset<PointOwner>();
                }
            }
        }
    }
}