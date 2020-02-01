using UnityEngine;
using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class UIPlayerStats : UICoreECS.AUIEntity 
    {
        [SerializeField] private TMPro.TextMeshProUGUI HP;
        [SerializeField] private TMPro.TextMeshProUGUI Power;

        public override void Init(EcsWorld world)
        {
            UIPlayerStatsComponent stats = world.NewEntity().Set<UIPlayerStatsComponent>();
            stats.HP = HP;
            stats.Power = Power;
        }
    }

    public class UIPlayerStatsComponent : IEcsAutoReset
    {
        public TMPro.TextMeshProUGUI HP;
        public TMPro.TextMeshProUGUI Power;

        public void Reset()
        {
            HP = null;
            Power = null;
        }
        
    }

    public class UIPlayerStatsUpdateSystem : IEcsRunSystem
    {
        readonly EcsFilter<UIPlayerStatsComponent> _view;
        readonly EcsFilter<UserPlayer, Player> _player;
        readonly EcsFilter<UIUpdate> _event;

        public void Run()
        {
            if(_event.IsEmpty())
                return;

            foreach (var i in _view)
            {
                foreach (var j in _player)
                {
                    _view.Get1[i].HP.text = "HP: " + _player.Get2[i].HP.ToString();
                    _view.Get1[i].Power.text = "Power: " + _player.Get2[i].Power.ToString();
                }
            }
        }
    }
}