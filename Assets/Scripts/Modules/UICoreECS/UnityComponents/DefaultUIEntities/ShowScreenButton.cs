using UnityEngine;
using Leopotam.Ecs;

namespace UICoreECS.DefaultEntities
{
    public class ShowScreenButton : AUIEntity 
    {
        [Header("Scene refs")]
        [SerializeField] private UnityEngine.UI.Button _button;
        [Header("Target screen")]
        [SerializeField] private int _targetID;
        [SerializeField] private int _targetLayer;
        private EcsWorld _world;

        public override void Init(EcsWorld world)
        {
            _world = world;
        }

        private void Awake() 
        {
            _button.onClick.AddListener(Action);
        }

        private void Action()
        {
            if(_world == null)
                return;

            ShowScreenTag tag = _world.NewEntity().Set<ShowScreenTag>();
            tag.ID = _targetID;
            tag.Layer = _targetLayer;
        }

    }
}