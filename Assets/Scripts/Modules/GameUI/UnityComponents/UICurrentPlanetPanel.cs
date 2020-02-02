using UnityEngine;
using UnityEngine.UI;
using Leopotam.Ecs;
using TMPro;

namespace Modules.CoreGame
{
    public class UICurrentPlanetPanel : UICoreECS.AUIEntity
    {
        [SerializeField] private TextMeshProUGUI _owner;
        [SerializeField] private Button _buy;
        [SerializeField] private Button _destroy;

        private EcsWorld _world;

        private void Awake()
        {    
            _buy.onClick.AddListener(Buy);
            _destroy.onClick.AddListener(Destroy);
        }

        public override void Init(EcsWorld world)
        {
            UICurrentPlanetPanelComponent view = world.NewEntity().Set<UICurrentPlanetPanelComponent>();
            view.Owner = _owner;
            view.Buy = _buy;
            view.Destroy = _destroy;

            _world = world;
        }

        public void Buy()
        {
            _world.NewEntity().Set<BuyActionTag>();
        }

        public void Destroy()
        {
            _world.NewEntity().Set<DestroyActionTag>();
        }
        
    }

    public class UICurrentPlanetPanelComponent : IEcsAutoReset
    {
        public TextMeshProUGUI Owner;
        public Button Buy;
        public Button Destroy;

        public void Reset()
        {
            Owner = null;
            Buy = null;
            Destroy = null;
        }
    } 

    public class UICurrentPlanetPanelDrawer : IEcsRunSystem
    {
        readonly EcsFilter<UIUpdate> _uiUpdate;
        readonly EcsFilter<UICurrentPlanetPanelComponent> _view;
        readonly EcsFilter<CurrentPoint, WorldPoint> _current;
        readonly PlayerApi _playerApi;

        public UICurrentPlanetPanelDrawer(PlayerApi playerApi)
        {
            _playerApi = playerApi;
        }

        public void Run()
        {
            if(_uiUpdate.IsEmpty())
                return;

            PointOwner owner;
            foreach (var i in _view)
            {
                foreach (var j in _current)
                {
                    owner = _current.Entities[j].Get<PointOwner>();
                    if(owner == null)
                    {
                        _view.Get1[i].Owner.text = "Destroyed";
                        _view.Get1[i].Destroy.gameObject.SetActive(false);
                        _view.Get1[i].Buy.gameObject.SetActive(true);
                    }else
                    {
                        _view.Get1[i].Owner.text = "Owner: " + owner.OwnerID;
                        if(owner.OwnerID.Equals(_playerApi.PlayerName))
                        {
                            _view.Get1[i].Destroy.gameObject.SetActive(false);
                        }else
                        {
                            _view.Get1[i].Destroy.gameObject.SetActive(true);
                        }

                        _view.Get1[i].Buy.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}