using Leopotam.Ecs;

namespace UICoreECS
{
    public abstract class AUIEntity : UnityEngine.MonoBehaviour
    {
        public abstract void Init(EcsWorld world);
    }
}