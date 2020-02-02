using UnityEngine;
using UICoreECS;
using Leopotam.Ecs;


namespace Modules.CoreGame
{
    
    public class FollowPlayerButton : AUIEntity 
    {
        [SerializeField] private UnityEngine.UI.Button _button;
        private bool _following;
        private EcsWorld _world;

        private void Awake() 
        {
            _button.onClick.AddListener(Switch);
        }

        public override void Init(EcsWorld world)
        {
            _world = world;
        }

        public void Switch()
        {
            if(_following)
            {
                _world.NewEntity().Set<StopFollowingTag>();
                _following = false;
            }else
            {
                _world.NewEntity().Set<FollowPlayerTag>();
                _following = true;
            }
        }
        
    }
}