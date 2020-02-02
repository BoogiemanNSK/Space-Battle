using UnityEngine;
using Modules.ViewHub;
using Leopotam.Ecs;
using UnityEngine.EventSystems;

namespace Modules.CoreGame
{
    public class PointerClicked : IEcsIgnoreInFilter {}

    public class UPointer : ViewComponent , IPointerClickHandler
    {
        private EcsEntity _entity;

        public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
        {
            _entity = ecsEntity;
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            _entity.Set<PointerClicked>();
        }
    }
}