using Leopotam.Ecs;
using UnityEngine;

namespace Modules.ViewHub
{
    public class UnityView : IEcsAutoReset
    {
        public string id;
        public GameObject GameObject;
        public Transform Transform;
        
        public void Reset()
        {
            id = null;
            if(this.GameObject != null)
                Object.Destroy(this.GameObject);
            Transform = null;
            GameObject = null;
        }
    }
}