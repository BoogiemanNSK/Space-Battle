using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public sealed class MarkersControlSystem : IEcsRunSystem
    {
        // Auto-injected
        private EcsFilter<PointOwner, MarkerRoot> _filter;
        private EcsFilter<MarkerRoot>.Exclude<PointOwner> _neutralFilter;

        readonly PlayerApi _playerApi;

        public MarkersControlSystem(PlayerApi playerApi) {
            _playerApi = playerApi;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                if (_filter.Get1[i].OwnerID == _playerApi.PlayerName) {
                    _filter.Get2[i].FriendlyMarker.SetActive(true);
                    _filter.Get2[i].EnemyMarker.SetActive(false);
                } else if (_filter.Get1[i].OwnerID != "") {
                    _filter.Get2[i].EnemyMarker.SetActive(true);
                    _filter.Get2[i].FriendlyMarker.SetActive(false);
                }
            }
            foreach (var i in _neutralFilter) {
                _neutralFilter.Get1[i].FriendlyMarker.SetActive(false);
                _neutralFilter.Get1[i].EnemyMarker.SetActive(false);
            }
        }
    }
}