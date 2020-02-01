using Leopotam.Ecs;

namespace Modules.ViewHub.Interfaces
{
    public interface IUViewElement
    {
        void Allocate(EcsEntity entity, EcsWorld world);
        string ID { get; }
    }
}