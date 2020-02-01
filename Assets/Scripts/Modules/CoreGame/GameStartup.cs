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
                .Add(new SyncWorldSystem(_worldApi))

                // object rotating system
                .Add(new ObjectRotationSystem());

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
                .Add(new UICoreECS.ScreenSwitchSystem(_screens, _uiRoot));

            _systems
                .OneFrame<Positioning.Components.LazyPositionUpdate>();

            _viewSystems
                .OneFrame<AllocateView>()
                .OneFrame<Positioning.Components.LazyPositionUpdate>();

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