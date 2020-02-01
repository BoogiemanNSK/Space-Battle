using Leopotam.Ecs;

namespace Modules.Positioning.Components
{
    /// <summary>
    /// flag to not update view position every frame
    /// </summary>
    public class LazyPosition : IEcsIgnoreInFilter{}
}