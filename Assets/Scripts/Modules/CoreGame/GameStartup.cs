using Leopotam.Ecs;
using UnityEngine;
using Modules.ViewHub;

namespace Modules.CoreGame
{
    /// <summary>
    /// composition root/entry point for gameplay logic
    /// </summary>
    sealed class GameStartup : MonoBehaviour
    {
        
        #region SerializedFields

        [Header("Scene refs")]
        [SerializeField] private Transform _uiRoot;
        [SerializeField] private WorldApi _worldApi;
        [SerializeField] private PlayerApi _playerApi;

        [Header("Data refs")]
        [SerializeField] private ViewHub.ViewHub _gameView;
        [SerializeField] private UICoreECS.ScreensCollection _screens;
        #endregion

        #region Privates

        EcsWorld _world;
        EcsSystems _systems;
        EcsSystems _viewSystems;

        #endregion
        
        #region CompositionRoot

        void OnEnable()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _viewSystems = new EcsSystems(_world, "UnityView");

            Application.targetFrameRate = 60;
            _gameView.Init();

            UnityIntegration();

            _systems
                .Add(new CoreGameInit())
                .Add(new Utils.UnityTimeSystem())
                // user input
                .Add(new UserInput.TapTrackerSystem())
                
                // networking
                .Add(new PlayersUpdateIssuingSystem(_playerApi))
                .Add(new SyncWorldSystem(_worldApi))
                .Add(new ConnectSystem(_playerApi))
                .Add(new PlayersUpdateSystem(_playerApi))
                .Add(new DestroyNetSystem(_playerApi))
                .Add(new BuyNetSystem(_playerApi))
                .Add(new MoveNetSystem(_playerApi))
                .Add(new AttackNetSystem(_playerApi))
                .Add(new HealNetSystem(_playerApi))

                // stuff
                .Add(new SpawnPlayerProcessing(_playerApi))
                .Add(new UpdateUserPlanetProcessing())
                .Add(new UpdatePlayerPointProcessing())
                .Add(new UpdateOwnersProcessing(_worldApi))
                .Add(new MoveTargetSys())

                // object rotating system
                .Add(new ObjectRotationSystem())

                // camera
                .Add(new CameraSystem())
                .Add(new FollowPlayerCamera())

                // space objects markers
                .Add(new MarkersControlSystem(_playerApi));

            _viewSystems
                // view allocations
                .Add(new ViewAllocatorSystem(_gameView))

                // view sync
                .Add(new Positioning.Systems.UPositionSyncSystem())
                .Add(new Positioning.Systems.URotationsSyncSystem())

                // rendering
                .Add(new UMeshRenderer.Systems.MatrixPositionUpdate())
                .Add(new UMeshRenderer.Systems.UMeshRenderer())
                
                //ui
                .Add(new UIUpadteIssuer(0.5f))
                .Add(new UICoreECS.ScreenSwitchSystem(_screens, _uiRoot))
                .Add(new UIPlayerStatsUpdateSystem())
                .Add(new UICurrentPlanetPanelDrawer(_playerApi))
                .Add(new InfoPopUpProcessing())
                .Add(new UISelectedPlanetPanelDrawer(_playerApi));

            _systems
                .OneFrame<Positioning.Components.LazyPositionUpdate>()
                .OneFrame<LoginActionTag>()
                .OneFrame<LoggedInTag>()
                .OneFrame<PlayersUpdateTag>()
                .OneFrame<SpawnPlayerTag>()
                .OneFrame<UpdateOwnersTag>()
                .OneFrame<BuyActionTag>()
                .OneFrame<DestroyActionTag>()
                .OneFrame<MoveActionTag>()
                .OneFrame<PointerClicked>()
                .OneFrame<AttackActionTag>()
                .OneFrame<HealActionTag>();

            _viewSystems
                .OneFrame<AllocateView>()
                .OneFrame<Positioning.Components.LazyPositionUpdate>()
                .OneFrame<UIUpdate>();

            _systems.Add(_viewSystems);

            _systems.Inject(new Utils.TimeService());
            _systems.Init();
        } 
        
        
        #endregion

        #region RunGroup

        void Update()
        {
            _systems.Run();
        }

        #endregion

        #region DisposalEvents

        private void OnDisable()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
                _world.Destroy();
                _world = null;
            }
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
                _world.Destroy();
                _world = null;
            }
        }

        #endregion

        #region Integrations

        private void UnityIntegration()
        {
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif
        }

        #endregion

    }
    
}