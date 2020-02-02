using UnityEngine;
using UnityEngine.UI;
using Leopotam.Ecs;
using TMPro;

namespace Modules.CoreGame
{
    public class UISelectedPlanetPanel : UICoreECS.AUIEntity
    {
        [SerializeField] private TextMeshProUGUI _owner;
        [SerializeField] private Button _move;

        private EcsWorld _world;

        private void Awake()
        {    
            _move.onClick.AddListener(Move);
        }

        public override void Init(EcsWorld world)
        {
            UISelectedPlanetPanelComponent view = world.NewEntity().Set<UISelectedPlanetPanelComponent>();
            view.Owner = _owner;
            view.Move = _move;
            
            _world = world;
        }

        public void Move()
        {
            _world.NewEntity().Set<MoveActionTag>();
        }
        
    }

    public class UISelectedPlanetPanelComponent : IEcsAutoReset
    {
        public TextMeshProUGUI Owner;
        public Button Move;

        public void Reset()
        {
            Owner = null;
            Move = null;
        }
    }

    public class UIRemotePointTarget : IEcsIgnoreInFilter {}

    public class UISelectedPlanetPanelDrawer : IEcsRunSystem
    {
        readonly EcsFilter<UIUpdate> _uiUpdate;
        readonly EcsFilter<UISelectedPlanetPanelComponent> _view;
        readonly EcsFilter<UIRemotePointTarget, WorldPoint> _current;
        readonly EcsFilter<UserPlayer, Player> _playerPoint;
        readonly EcsFilter<PointerClicked, WorldPoint> _new;
        readonly PlayerApi _playerApi;
        readonly EcsWorld _world;

        public UISelectedPlanetPanelDrawer(PlayerApi playerApi)
        {
            _playerApi = playerApi;
        }

        public void Run()
        {
            if (!_new.IsEmpty())
            {
                foreach (var i in _current)
                {
                    _current.Entities[i].Unset<UIRemotePointTarget>();
                }
                bool isPlayerPoint = false;
                foreach (var i in _new)
                {
                    if(_new.Get2[i].PointID == _playerPoint.Get2[0].Location)
                    {
                        isPlayerPoint = true;
                    }
                    _new.Entities[i].Set<UIRemotePointTarget>();
                }

                UICoreECS.ShowScreenTag screen = _world.NewEntity().Set<UICoreECS.ShowScreenTag>();
                screen.ID = isPlayerPoint ? 1 : 2;
                screen.Layer = 1;

                _world.NewEntity().Set<UIUpdate>();
            }

            if (!_uiUpdate.IsEmpty())
            {
                PointOwner owner;
                foreach (var i in _view)
                {
                    foreach (var j in _current)
                    {
                        owner = _current.Entities[j].Get<PointOwner>();
                        if (owner == null)
                        {
                            _view.Get1[i].Owner.text = "Destroyed";
                        }
                        else
                        {
                            _view.Get1[i].Owner.text = "Owner: " + owner.OwnerID;
                        }
                    }
                }
            }


        }
    }
}