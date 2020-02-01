using Leopotam.Ecs;
using UnityEngine;

namespace Modules.CoreGame
{
    public class CameraComponent : IEcsAutoReset
    {
        public Camera MainCamera;
        public void Reset()
        {
            this.MainCamera = null;
        }
    }
}