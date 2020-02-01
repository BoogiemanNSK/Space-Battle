using Leopotam.Ecs;
using UnityEngine;

namespace Modules.ViewHub
{
    public abstract class ViewComponent : MonoBehaviour
    {
        public abstract void EntityInit(EcsEntity ecsEntity,EcsWorld ecsWorld, bool parentOnScene);
    }
}