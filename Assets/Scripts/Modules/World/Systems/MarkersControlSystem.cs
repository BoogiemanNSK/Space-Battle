using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public sealed class MarkersControlSystem : IEcsRunSystem
    {
        // Auto-injected
        private EcsFilter<PointOwner, MarkerRoot> _filter;
        readonly PlayerApi _playerApi;

        public MarkersControlSystem(PlayerApi playerApi) {
            _playerApi = playerApi;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                _filter.Get2[i].FriendlyMarker.SetActive(false);
                _filter.Get2[i].EnemyMarker.SetActive(false);

                if (_filter.Get1[i].OwnerID == _playerApi.PlayerName) {
                    _filter.Get2[i].FriendlyMarker.SetActive(true);
                } else if (_filter.Get1[i].OwnerID != "") {
                    _filter.Get2[i].EnemyMarker.SetActive(true);
                }
            }
        }
    }
}