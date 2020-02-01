using Leopotam.Ecs;
using UnityEngine;

namespace Modules.Positioning.Components
{
    public class Position : IEcsAutoReset
    {
        public Vector3 Point;
        public Vector3 EulerRotation;
        public void Reset()
        {
            Point = Vector3.zero;
            EulerRotation = Vector3.zero;
        }
    }
}