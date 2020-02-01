using Leopotam.Ecs;
using UnityEngine;

namespace Modules.Positioning.Components
{
    public class URotationRoot : IEcsAutoReset
    {
        public Transform Transform;
        public void Reset()
        {
            this.Transform = null;
        }
    }
}